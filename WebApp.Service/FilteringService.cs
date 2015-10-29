using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using WebApp.Domain.Interface;
using WebApp.Service.Interface;
using WebApp.DAL;
using WebApp.Core;

namespace WebApp.Service
{
    public static class FilteringService
    {
        public static IQueryable<TDocumentDTO> ByRequest<TDocumentDTO, TRequest>(IQueryable<TDocumentDTO> expr, TRequest request)
            where TDocumentDTO : IDocumentDTO, new()
            where TRequest : IRequest, new()
        {
            var __isFinalized = false;

            if (request.SearchData != null && request.SearchData.Fields.Count != 0)
            {
                if (HasSearchFieldsWithProjectionIgnore<TDocumentDTO>(request.SearchData))
                {
                    expr = expr.ToList().AsQueryable();
                    __isFinalized = true;
                }

                expr = FilterByRequest(expr, request.SearchData);
            }

            if (request.Filter != null && request.Filter.Filters.Count != 0)
            {
                expr = FilterByRequest(expr, request.Filter, false);

                if (HasFieldsWithProjectionIgnore<TDocumentDTO>(request.Filter))
                {
                    if (!__isFinalized)
                        expr = expr.ToList().AsQueryable();
                    __isFinalized = true;
                    expr = FilterByRequest(expr, request.Filter, true);
                }
            }

            if (HasSortWithProjectionIgnore<TDocumentDTO>(request.Sort) && !__isFinalized)
                expr = expr.ToList().AsQueryable();

            if (request.Sort != null && request.Sort.Count>0)
            {
                expr = request.Sort
                   .Where(sortDescriptor => request.Sort.Count(x => x.Member != sortDescriptor.Member) == 0 && !String.IsNullOrWhiteSpace(sortDescriptor.Field))
                   .Aggregate(expr, (current, sortDescriptor) => OrderByRequest(current, sortDescriptor, sortDescriptor == request.Sort.First()));
            }

            request.TotalRows = expr.Count();
            expr = expr.Skip(request.Take * (request.Page - 1)).Take(request.Take);
            return expr;
        }

        private static IQueryable<TDocumentDTO> FilterByRequest<TDocumentDTO>(IQueryable<TDocumentDTO> expr, IFilterData filterDescriptor, Boolean hasProjectionIgnoreAttr = false)
            where TDocumentDTO : new()
        {
            var __type = typeof(TDocumentDTO);
            var __arg = Expression.Parameter(__type, "x");

            var __expr = filterDescriptor is Filter ?
                ParseFilter<TDocumentDTO>(__arg, filterDescriptor as Filter, hasProjectionIgnoreAttr)
                : ParseSearchData<TDocumentDTO>(__arg, filterDescriptor as SearchData);
            var __propertyType = typeof(bool);

            var __delegateType = typeof(Func<,>).MakeGenericType(typeof(TDocumentDTO), __propertyType);
            var __lambda = Expression.Lambda(__delegateType, __expr, __arg);

            var __methodName = "Where";

            var __result = (IQueryable<TDocumentDTO>)typeof(Queryable).GetMethods().Single(
                method => method.Name == __methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 1
                        && method.GetParameters().Length == 2
                        && ((method.GetParameters()[1]).ParameterType).GenericTypeArguments[0].GenericTypeArguments.Count() == 2 //эту строчку придумал не я
                        )
                .MakeGenericMethod(__type)
                .Invoke(null, new object[] { expr, __lambda });

            return __result;
        }

        private static Expression ParseSearchData<TDocumentDTO>(ParameterExpression parameterExpression,
            SearchData searchData)
            where TDocumentDTO : new()
        {
            var __result = Expression.Equal(Expression.Constant(true), Expression.Constant(false));

            var __containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (var field in searchData.Fields)
            {
                var __propertyInfo = typeof(TDocumentDTO).GetProperty(field);
                var __propertyType = __propertyInfo.PropertyType;

                var __left = Expression.Property(parameterExpression, __propertyInfo);
                var __right = Expression.Constant(searchData.Search, __propertyType);

                var __toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

                var __fieldExpr = Expression.AndAlso(
                        Expression.NotEqual(__left, Expression.Constant(null, __left.Type)),
                        Expression.Call(Expression.Call(__left, __toLowerMethod), __containsMethod, Expression.Call(__right, __toLowerMethod))
                    );

                __result = Expression.OrElse(__result, __fieldExpr);
            }

            return __result;
        }

        private static Expression ParseFilter<TDocumentDTO>(ParameterExpression parameterExpression,
            Filter filterDescriptor,
            Boolean hasProjectionIgnoreAttr = false)
            where TDocumentDTO : new()
        {
            if (filterDescriptor.Filters.Any())
            {
                if (filterDescriptor.Logic.ToLower() == "and")
                {
                    var __andResult = Expression.Equal(Expression.Constant(true), Expression.Constant(true));
                    return filterDescriptor.Filters.Aggregate(__andResult, (current, filter) => Expression.AndAlso(current, ParseFilter<TDocumentDTO>(parameterExpression, filter, hasProjectionIgnoreAttr)));
                }
                else
                {
                    var __orResult = Expression.Equal(Expression.Constant(true), Expression.Constant(false));
                    return filterDescriptor.Filters.Aggregate(__orResult, (current, filter) => Expression.OrElse(current, ParseFilter<TDocumentDTO>(parameterExpression, filter, hasProjectionIgnoreAttr)));
                }
            }

            var __propertyInfo = typeof(TDocumentDTO).GetProperty(filterDescriptor.Field);
            var __propertyType = __propertyInfo.PropertyType;

            if (HasProjectionIgnoreAttibute(__propertyInfo) != hasProjectionIgnoreAttr)
                return Expression.Constant(true);

            var __filterValue = TypeDescriptor.GetConverter(__propertyType).ConvertFrom(filterDescriptor.Value);

            var __left = Expression.Property(parameterExpression, __propertyInfo);
            var __right = Expression.Constant(__filterValue, __propertyType);

            return ExpressionResolver(__left, __right, filterDescriptor.Operator);
        }

        private static Expression ExpressionResolver(Expression left, Expression right, String filterOperator)
        {
            var __containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var __startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var __endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var __stringEmpty = Expression.Constant(String.Empty, typeof(string));

            var __toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            var __leftLower = left.Type == typeof(string) ? Expression.Call(left, __toLowerMethod) : null;
            var __rightLower = right.Type == typeof(string) ? Expression.Call(right, __toLowerMethod) : null;

            switch (filterOperator.ToLower())
            {
                case "eq":
                    return Expression.Equal(left, right);
                case "neq":
                    return Expression.NotEqual(left, right);
                case "gt":
                    return Expression.GreaterThan(left, right);
                case "lt":
                    return Expression.LessThan(left, right);
                case "gte":
                    return Expression.GreaterThanOrEqual(left, right);
                case "lte":
                    return Expression.LessThanOrEqual(left, right);
                case "contains":
                    return
                        Expression.OrElse(
                            Expression.AndAlso(Expression.NotEqual(left, Expression.Constant(null, left.Type)), Expression.Call(__leftLower, __containsMethod, __rightLower)),
                            Expression.Equal(right, __stringEmpty)
                        );
                case "doesnotcontain":
                    return
                        Expression.OrElse(
                            Expression.AndAlso(Expression.NotEqual(left, Expression.Constant(null, left.Type)), Expression.Not(Expression.Call(__leftLower, __containsMethod, __rightLower))),
                            Expression.Equal(left, Expression.Constant(null, left.Type))
                        );
                case "startswith":
                    return
                        Expression.OrElse(
                            Expression.AndAlso(Expression.NotEqual(left, Expression.Constant(null, left.Type)), Expression.Call(__leftLower, __startsWithMethod, __rightLower)),
                            Expression.Equal(right, __stringEmpty)
                        );
                case "endswith":
                    return
                        Expression.OrElse(
                        Expression.AndAlso(Expression.NotEqual(left, Expression.Constant(null, left.Type)), Expression.Call(__leftLower, __endsWithMethod, __rightLower)),
                            Expression.Equal(right, __stringEmpty)
                        );
                default:
                    return Expression.Equal(left, right);
            }
        }

        private static IQueryable<TDocumentDTO> OrderByRequest<TDocumentDTO>(IQueryable<TDocumentDTO> expr, Sort sortDescriptor, Boolean isFirstSort = true)
            where TDocumentDTO : new()
        {
            var __type = typeof(TDocumentDTO);
            var __arg = Expression.Parameter(__type, "x");

            var __propertyInfo = __type.GetProperty(sortDescriptor.Member);

            var __expr = Expression.Property(__arg, __propertyInfo);
            var __propertyType = __propertyInfo.PropertyType;

            var __delegateType = typeof(Func<,>).MakeGenericType(typeof(TDocumentDTO), __propertyType);
            var __lambda = Expression.Lambda(__delegateType, __expr, __arg);

            var __methodName = isFirstSort
                ? sortDescriptor.SortDirection == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending"
                : sortDescriptor.SortDirection == ListSortDirection.Ascending ? "ThenBy" : "ThenByDescending";

            var __result = (IQueryable<TDocumentDTO>)typeof(Queryable).GetMethods().Single(
                method => method.Name == __methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(__type, __propertyType)
                .Invoke(null, new object[] { expr, __lambda });

            return __result;
        }

        private static Boolean HasProjectionIgnoreAttibute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetPropertyAttribute<ProjectionIgnore>() != null;
        }

        private static Boolean HasFieldsWithProjectionIgnore<TDocumentDTO>(Filter filter) where TDocumentDTO : new()
        {
            if (filter.Filters.Any())
            {
                return filter.Filters.Aggregate(false, (current, __filter) => current || HasFieldsWithProjectionIgnore<TDocumentDTO>(__filter));
            }

            var __propertyInfo = typeof(TDocumentDTO).GetProperty(filter.Field);
            return HasProjectionIgnoreAttibute(__propertyInfo);
        }

        private static Boolean HasSortWithProjectionIgnore<TDocumentDTO>(IEnumerable<Sort> sorts)
            where TDocumentDTO : new()
        {
            return sorts.Select(sort => typeof(TDocumentDTO).GetProperty(sort.Field)).Any(__propertyInfo => HasProjectionIgnoreAttibute(__propertyInfo));
        }

        private static Boolean HasSearchFieldsWithProjectionIgnore<TDocumentDTO>(SearchData searchData)
            where TDocumentDTO : new()
        {
            return searchData.Fields.Select(field => typeof(TDocumentDTO).GetProperty(field)).Any(__propertyInfo => HasProjectionIgnoreAttibute(__propertyInfo));
        }
    }
}
