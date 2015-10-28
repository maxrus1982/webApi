using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Service
{
    public static class IListExtension
    {
        /// <summary>
        /// Добавляет список элементов в коллекцию
        /// </summary>
        public static ICollection<T> AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");

            var __items = items.ToList(); // no side-effects
            if (__items.Count == 0)
                return list;

            foreach (T item in __items)
            {
                list.Add(item);
            }

            return list;
        }

        public static ICollection<T> AddRange<T>(this ICollection<T> list, params T[] items)
        {
            return list.AddRange(items.AsEnumerable());
        }

        public static ICollection<T> RemoveRange<T>(this ICollection<T> list, IEnumerable<T> items)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");

            //List<T> __items = items is List<T> ? (items as List<T>) : items.ToList(); // no side-effects
            List<T> __items = items.ToList();

            if (__items.Count == 0)
                return list;

            foreach (T item in __items)
            {
                list.Remove(item);
            }

            return list;
        }

        /// <summary>
        /// Добавляет элемент в список, при отсутствии элемента в списке
        /// Проверка отсутствия выполняется по ключу
        /// </summary>
        public static void AddNotExists<TEntity, TKey>(this ICollection<TEntity> list, TEntity item, Func<TEntity, TKey> keySelector)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (item == null) throw new ArgumentNullException("item");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            if (!list.Any(o => keySelector(o).Equals(keySelector(item))))
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Добавляет элементы в список, при отсутствии элементов в списке
        /// Проверка отсутствия выполняется по ключу
        /// </summary>
        public static void AddNotExists<TEntity, TKey>(this ICollection<TEntity> list, IEnumerable<TEntity> items, Func<TEntity, TKey> keySelector)
        {
            if (list == null) throw new ArgumentNullException("list");
            if (items == null) throw new ArgumentNullException("items");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            foreach (var __item in items)
            {
                if (!list.Any(o => keySelector(o).Equals(keySelector(__item))))
                {
                    list.Add(__item);
                }
            }
        }

        /// <summary>
        /// Проверяет что в списке есть любой из перечисленных элементов
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static bool HasAny<TEntity>(this IEnumerable<TEntity> list, params TEntity[] entities)
        {
            if (list == null)
                return false;

            return list.Any(o => entities.Contains(o));
        }

        public static TKey With<TEntity, TKey>(this TEntity entity, Func<TEntity, TKey> action) where TEntity : class
        {
            if (entity == null)
            {
                return default(TKey);
            }

            return action(entity);
        }

        public static TKey Return<TEntity, TKey>(this TEntity entity, Func<TEntity, TKey> action)
        {
            if (entity == null)
            {
                return default(TKey);
            }

            return action(entity);
        }

        public static TKey Return<TEntity, TKey>(this TEntity entity, Func<TEntity, TKey> action, TKey defaultValue) where TEntity : class
        {
            if (entity == null)
            {
                return defaultValue;
            }

            return action(entity);
        }

        public static TKey ReturnValue<TEntity, TKey>(this TEntity? entity, Func<TEntity, TKey> action, TKey defaultValue) where TEntity : struct
        {
            if (!entity.HasValue)
            {
                return defaultValue;
            }

            return action(entity.Value);
        }

        public static TEntity ReturnValue<TEntity>(this TEntity? entity, TEntity defaultValue) where TEntity : struct
        {
            if (!entity.HasValue)
            {
                return defaultValue;
            }

            return entity.Value;
        }

        public static void Do<TEntity>(this TEntity entity, Action<TEntity> action)
        {
            if (entity != null)
            {
                action(entity);
            }
        }

        public static IEnumerable<TEntity> DoAll<TEntity>(this IEnumerable<TEntity> entities, Action<TEntity> action)
        {
            if (entities != null)
            {
                foreach (var __entity in entities)
                {
                    if (__entity != null)
                    {
                        action(__entity);
                    }
                }
            }

            return entities;
        }

        public static Int32 CountWith<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> children)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (children == null) throw new ArgumentNullException("children");

            Int32 __result = items.Count();

            foreach (var __item in items)
            {
                __result += children(__item)
                    .Return(o => o.CountWith(children), 0);
            }

            return __result;
        }

        public static Int32 CountLeafs<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> children)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (children == null) throw new ArgumentNullException("children");

            Int32 __result = 0;

            foreach (var __item in items)
            {
                var __childs = children(__item);
                if (__childs == null || __childs.Count() == 0)
                    __result++;
                else
                    __result += __childs
                        .Return(o => o.CountLeafs(children), 0);
            }

            return __result;
        }

        public static void ForEach<T>(this IEnumerable<T> query, Action<T> method)
        {
            foreach (T item in query)
            {
                method(item);
            }
        }

        public static IEnumerable<T[]> Split<T>(this IEnumerable<T> source, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length");

            var __section = new List<T>(length);
            foreach (var __item in source)
            {
                __section.Add(__item);
                if (__section.Count == length)
                {
                    yield return __section.ToArray();
                    __section.Clear();
                }
            }
            if (__section.Count > 0)
                yield return __section.ToArray();
        }

        public static IEnumerable<T> ForEachReturn<T>(this IEnumerable<T> query, Action<T> method)
        {
            foreach (T item in query)
            {
                method(item);
                yield return item;
            }
        }
    }
}
