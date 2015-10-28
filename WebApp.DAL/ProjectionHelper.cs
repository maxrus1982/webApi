using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

using WebApp.Domain.Interface;

namespace WebApp.DAL
{
    public static class ProjectionHelper
    {
        /// <summary>
        /// Проверка ИД на пустое значение, вместо проверки объекта
        /// </summary>
        public static Boolean IDNullEqualityComparison = false;
        private static readonly ConcurrentDictionary<String, Expression> _readyProjections = new ConcurrentDictionary<string, Expression>();

        private static object __lockObj = new object();

        private static MethodInfo _methodSelect;
        private static MethodInfo MethodSelect
        {
            get
            {
                return _methodSelect ??
                       (_methodSelect = typeof(Enumerable)
                                            .GetMethods()
                                            .FirstOrDefault(
                                                x =>
                                                x.Name == "Select" &&
                                                x.GetParameters().Count() == 2));
            }
        }

        private static MethodInfo _methodProjection;
        private static MethodInfo MethodProjection
        {
            get
            {
                return _methodProjection ??
                       (_methodProjection = typeof(ProjectionHelper)
                                            .GetMethods()
                                            .FirstOrDefault(
                                                x =>
                                                x.Name == "GetProjection" &&
                                                x.GetParameters().Count() == 2));
            }
        }

        /// <summary>
        /// Создает projection по всем полям ViewModel - тип TTo
        /// </summary>
        /// <typeparam name="TFrom">BO</typeparam>
        /// <typeparam name="TTo">ViewModel</typeparam>
        /// <returns></returns>
        public static Expression<Func<TFrom, TTo>> GetProjection<TFrom, TTo>(Boolean withEnumerables = false, Boolean objectQuery = false)
            where TFrom : IDocument//Hive.BOL.DomainObject
            where TTo : new()
        {
            //var __type = typeof(TFrom).FullName + typeof(TTo).FullName + withEnumerables.ToString();
            //Expression __result = null;

            //    if (!_readyProjections.TryGetValue(__type, out __result))
            //    {
            //        __result = GetProjection<TFrom, TTo>(typeof(TTo).GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance).Where(y => y.SetMethod != null).Select(x => x.Name).ToArray(), withEnumerables);
            //        _readyProjections.TryAdd(__type, __result);
            //    }           
            //return __result as Expression<Func<TFrom, TTo>>;            
            return GetProjection<TFrom, TTo>(null, withEnumerables, objectQuery);
        }

        public static Expression<Func<TFrom, TTo>> GetProjection<TFrom, TTo>(Expression<Func<TFrom, TTo>> mergeExpression, Boolean withEnumerables = false, Boolean objectQuery = false)
            where TFrom : IDocument//Hive.BOL.DomainObject
            where TTo : new()
        {
            var __type = typeof(TFrom).FullName + typeof(TTo).FullName + withEnumerables.ToString() +
                objectQuery.ToString() + (mergeExpression != null ? mergeExpression.ToString() : String.Empty);
            Expression __result = null;

            if (!_readyProjections.TryGetValue(__type, out __result))
            {
                __result = GetProjection<TFrom, TTo>(typeof(TTo).GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance).Where(y => y.GetSetMethod() != null).Select(x => x.Name).ToArray(), mergeExpression, withEnumerables, objectQuery);
                _readyProjections.TryAdd(__type, __result);
            }
            return __result as Expression<Func<TFrom, TTo>>;
        }

        /// <summary>
        /// Создает projection по перечисленным полям ViewModel - тип TTo
        /// </summary>
        /// <typeparam name="TFrom">BO</typeparam>
        /// <typeparam name="TTo">ViewModel</typeparam>
        /// <returns></returns>
        public static Expression<Func<TFrom, TTo>> GetProjection<TFrom, TTo>(String[] fields, Expression<Func<TFrom, TTo>> mergeExpression = null, Boolean withEnumerables = false, Boolean objectQuery = false)
            where TFrom : IDocument//Hive.BOL.DomainObject
            where TTo : new()
        {
            var __new = Expression.New(typeof(TTo));
            var __bindings = new List<MemberBinding>();

            ParameterExpression fromParam = Expression.Parameter(typeof(TFrom), "from");

            if (mergeExpression != null && mergeExpression.Body as MemberInitExpression != null)
            {
                var __expr = mergeExpression.Body as MemberInitExpression;
                // этот кусок заменяет во всех внутренних выражениях параметр на один
                ParameterReplacer replacer = new ParameterReplacer(fromParam);
                var __replaced = replacer.Visit(__expr);
                /////       

                __bindings.AddRange(((MemberInitExpression)__replaced).Bindings);
            }


            foreach (var f in fields)
            {
                if (!__bindings.Any(x => x.Member.Name.Equals(f, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var __binding = GetMemberBinding<TFrom, TTo>(f, fromParam, withEnumerables, objectQuery);
                    if (__binding != null)
                        __bindings.Add(__binding);
                }
            }
            var init = Expression.MemberInit(__new, __bindings);
            return Expression.Lambda<Func<TFrom, TTo>>(init, fromParam);

        }
        static MemberBinding GetMemberBinding<TFrom, TTo>(string propertyName, ParameterExpression param, Boolean withEnumerables, Boolean objectQuery = false)
        {
            try
            {
                PropertyInfo memberInfo = typeof(TTo).GetProperty(propertyName);
                if (memberInfo.HasAttribute<ProjectionIgnore>())
                    return null;
                var __proptype = memberInfo.PropertyType;
                var __projectionAttribute = memberInfo.GetPropertyAttribute<ProjectionAttribute>();
                String[] __splitted;
                if (__projectionAttribute != null)
                {
                    __splitted = __projectionAttribute.PropertyPath.Split('.');
                }
                else if (!ParseCamelPropertyPath(typeof(TFrom), propertyName, out __splitted))
                {
                    __splitted = propertyName.Split('_');
                    //throw new Exception("Не смогли распарсить поле для Projection" + propertyName);
                }
                var __isEnumerableTarget = __proptype.IsGenericType && __proptype.GetInterface("IEnumerable", true) != null;

                if (__splitted.Count() > 1 && !__isEnumerableTarget)
                {
                    MemberExpression[] __accessors = new MemberExpression[__splitted.Count()];
                    __accessors[0] = LambdaExpression.PropertyOrField(param, __splitted[0]);
                    for (int i = 1; i < __splitted.Count(); i++)
                    {
                        __accessors[i] = LambdaExpression.PropertyOrField(__accessors[i - 1], __splitted[i]);
                    }

                    ConstantExpression __defValue;
                    if (__proptype.Equals(typeof(String)))
                        __defValue = Expression.Constant(String.Empty);
                    else if (__proptype.IsGenericType && __proptype.GetGenericTypeDefinition() == typeof(Nullable<>))
                        __defValue = Expression.Constant(null, __proptype);
                    else
                        __defValue = Expression.Constant(__proptype.GetDefaultValue());

                    Expression __iif;
                    if (!objectQuery && IDNullEqualityComparison && typeof(IDocument).IsAssignableFrom(__accessors[0].Type))
                    {
                        __iif = Expression.Equal(Expression.Constant(null), Expression.Convert(LambdaExpression.PropertyOrField(__accessors[0], "ID"), typeof(Guid?)));
                    }
                    else
                    {
                        __iif = Expression.ReferenceEqual(Expression.Constant(null), __accessors[0]);
                    }
                    for (int i = 1; i < __splitted.Count() - 1; i++)
                    {
                        BinaryExpression __compare;
                        if (!objectQuery && IDNullEqualityComparison && typeof(IDocument).IsAssignableFrom(__accessors[i].Type))
                        {
                            __compare = Expression.Equal(Expression.Constant(null), Expression.Convert(LambdaExpression.PropertyOrField(__accessors[i], "ID"), typeof(Guid?)));
                        }
                        else
                        {
                            __compare = Expression.ReferenceEqual(Expression.Constant(null), __accessors[i]);
                        }
                        __iif = Expression.OrElse(__iif, __compare);
                    }
                    Expression __accExpr = __accessors[__splitted.Count() - 1];
                    var __accessorType = __accExpr.Type;
                    if (__accessorType != __proptype)
                    {
                        if (__proptype.IsGenericType && __proptype.GetGenericTypeDefinition() == typeof(Nullable<>)
                            && __proptype.GenericTypeArguments[0] == __accessorType)
                        {
                            __accExpr = Expression.Convert(__accExpr, __proptype);
                        }
                        else
                        {
                            throw new ArgumentException(String.Format("Отличаются типы свойства модели и бизнес-объекта. ViewModel: {0}.{1}({2}),BO: {3}.{4}({5})",
                                    typeof(TTo).Name, memberInfo.Name, __proptype, typeof(TFrom).Name, __accExpr, __accessorType));
                        }
                    }
                    var __accessor = Expression.Condition(__iif, __defValue, __accExpr);
                    return System.Linq.Expressions.Expression.Bind(memberInfo, __accessor);
                }
                else
                {
                    var __fromProp = typeof(TFrom).GetProperty(__splitted[0]);
                    if (__fromProp == null)
                        return null;
                    var __fromPropType = __fromProp.PropertyType;

                    //var __isEnumerableTarget = __proptype.IsGenericType && __proptype.GetInterface("IEnumerable", true) != null;
                    var __isEnumerableSource = __fromPropType.IsGenericType && __fromPropType.GetInterface("IEnumerable", true) != null;

                    if (__isEnumerableSource ^ __isEnumerableTarget)
                        return null;

                    if ((__isEnumerableSource || __isEnumerableTarget) && !withEnumerables)
                        return null;

                    MemberExpression memberExpression = LambdaExpression.PropertyOrField(param, __splitted[0]);
                    if (__isEnumerableSource || __isEnumerableTarget)
                    //IEnumerablePath
                    {
                        var __fromUnderType = __fromPropType.GetGenericArguments()[0];
                        var __toUnderType = __proptype.GetGenericArguments()[0];
                        var __genericSelect = MethodSelect.MakeGenericMethod(__fromUnderType, __toUnderType);
                        var __genericProjection = MethodProjection.MakeGenericMethod(__fromUnderType, __toUnderType);
                        //var __projectionCall = Expression.Call(__genericProjection, Expression.Constant(true));
                        var t = __genericProjection.Invoke(null, new object[] { true, true }) as Expression;
                        var __selectCall = Expression.Call(__genericSelect, memberExpression, t);
                        return System.Linq.Expressions.Expression.Bind(memberInfo, __selectCall);
                    }

                    //Common path                
                    return System.Linq.Expressions.Expression.Bind(memberInfo, memberExpression);
                }
            }
            catch (System.Reflection.AmbiguousMatchException __ex)
            {
                throw new InvalidOperationException(String.Format("Ambiguous match found for property '{0}'", propertyName ?? "<null>"));
            }
        }

        public static Boolean ParseCamelPropertyPath(Type TFrom, String propertyPath, out String[] splitted)
        {
            //var __camelSplitted = Regex.Matches(propertyPath, "[A-Z][0-9a-z_]+").OfType<Match>().Select(match => match.Value);
            var __camelSplitted = Regex.Matches(propertyPath, "(?x)( [A-Z][a-z0-9]+ | [A-Z]+(?![a-z]) )").OfType<Match>().Select(match => match.Value);

            if (__camelSplitted.Count() == 0)
            {
                splitted = new String[] { propertyPath };
                return true;
            }
            if (__camelSplitted.Count() == 1)
            {
                splitted = __camelSplitted.ToArray();
                return true;
            }
            var __wordcount = __camelSplitted.Count();
            for (int i = 0; i < __wordcount; i++)
            {
                var __fromProp = TFrom.GetProperty(__camelSplitted.Take(__wordcount - i).Aggregate((s1, s2) => s1 + s2));
                if (__fromProp != null)
                {
                    if (i == 0)
                    {
                        splitted = new String[] { propertyPath };
                        return true;
                    }
                    String[] __childSplitted;
                    if (ParseCamelPropertyPath(__fromProp.PropertyType, __camelSplitted.Skip(__wordcount - i).Take(i).Aggregate((s1, s2) => s1 + s2), out __childSplitted))
                    {
                        splitted = new String[] { __camelSplitted.Take(__wordcount - i).Aggregate((s1, s2) => s1 + s2) }.Concat(__childSplitted).ToArray();
                        return true;
                    }
                }
            }
            splitted = null;
            return false;
        }

        class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression parameter;

            internal ParameterReplacer(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }

            protected override Expression VisitParameter
                (ParameterExpression node)
            {
                if (node.Type == parameter.Type)
                    return parameter;
                return node;
            }
        }

        public static IQueryable<TResult> Map<TSource, TResult>(this IQueryable<TSource> source, Boolean withEnumerables = false, Boolean objectQuery = false)
            where TSource : IDocument//Hive.BOL.DomainObject
            where TResult : new()
        {
            return source.Select(GetProjection<TSource, TResult>(withEnumerables, objectQuery));
        }

        public static IQueryable<TResult> Map<TSource, TResult>(this IQueryable<TSource> source, Expression<Func<TSource, TResult>> customExpression, Boolean withEnumerables = false, Boolean objectQuery = false)
            where TSource : IDocument//Hive.BOL.DomainObject
            where TResult : new()
        {
            return source.Select(GetProjection<TSource, TResult>(customExpression, withEnumerables, objectQuery));
        }

    }
    public static class TypeExtension
    {
        //a thread-safe way to hold default instances created at run-time
        private static ConcurrentDictionary<Type, object> typeDefaults = new ConcurrentDictionary<Type, object>();

        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? typeDefaults.GetOrAdd(type, t => Activator.CreateInstance(t)) : null;
        }
    }
}
