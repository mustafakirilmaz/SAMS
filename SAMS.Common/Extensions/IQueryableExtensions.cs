using SAMS.Common.Helpers;
using SAMS.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SAMS.Common.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, GridFilter gridFilter)
        {
            if (!string.IsNullOrWhiteSpace(gridFilter.SortBy))
            {
                if (gridFilter.IsSortAscending)
                {
                    query = query.OrderBy(GetPropertyExpression<T>(char.ToUpperInvariant(gridFilter.SortBy[0]) + gridFilter.SortBy.Substring(1)));
                }
                else
                {
                    query = query.OrderByDescending(GetPropertyExpression<T>(char.ToUpperInvariant(gridFilter.SortBy[0]) + gridFilter.SortBy.Substring(1)));
                }
            }
            return query;
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, GridFilter gridFilter)
        {
            //if (gridFilter.PageSize <= 0)
            //    gridFilter.PageSize = 10;
            //}

            return query.Skip(gridFilter.PageFirstIndex).Take(gridFilter.PageSize);
        }

        private static Dictionary<string, Expression<Func<T, object>>> CreateColumnMap<T>(List<string> columNameList)
        {
            var dictionary = new Dictionary<string, Expression<Func<T, object>>>();
            foreach (var columnName in columNameList)
            {
                dictionary[columnName] = GetPropertyExpression<T>(columnName);
            }
            return dictionary;
        }

        /// <summary>
        /// Grid'in kendi filtreleri haricinde ek filtreler uygulanacağında, sıralama veya paging yapılacaksa kullanılır. QueryFilter'ın PageSize'ı sıfır olduğu durumda paging yapılmaz.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSearchFilter"></typeparam>
        /// <param name="query">Çalıştırılacak sorgu</param>
        /// <param name="queryFilter">Query'ye eklenecek filtreler</param>
        /// <returns></returns>
        public static async Task<GridResult<List<T>>> GetGridResult<T, TSearchFilter>(this IQueryable<T> query, QueryFilter<TSearchFilter> queryFilter) where TSearchFilter : class
        {
            query = query.Convert();
            var mappedQueryFilter = ReflectionHelper.CloneObject<QueryFilter<TSearchFilter>, GridFilter>(queryFilter);
            if (queryFilter.PageSize > 0)
            {
                int totalCount = await query.CountAsync();
                var result = await query.ApplyOrdering(mappedQueryFilter).ApplyPaging(mappedQueryFilter).ToListAsync();
                return new GridResult<List<T>> { Result = result, TotalCount = totalCount };
            }
            else
            {
                var result = await query.ApplyOrdering(mappedQueryFilter).ToListAsync();
                return new GridResult<List<T>> { Result = result, TotalCount = result.Count() };
            }
        }

        /// <summary>
        ///  Grid'in kendi filtreleri haricinde herhangi bir ek filtre uygulanmayacaksa, fakat sıralama veya paging yapılacaksa kullanılır. QueryFilter'ın PageSize'ı sıfır olduğu durumda paging yapılmaz.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Çalıştırılacak sorgu</param>
        /// <param name="queryFilter">Query'ye eklenecek filtreler</param>
        /// <returns></returns>
        public static async Task<GridResult<List<T>>> GetGridResult<T>(this IQueryable<T> query, QueryFilter queryFilter)
        {
            query = query.Convert();
            int totalCount = await query.CountAsync();
            var mappedQueryFilter = ReflectionHelper.CloneObject<QueryFilter, GridFilter>(queryFilter);

            if (queryFilter.PageSize > 0)
            {
                var result = await query.ApplyOrdering(mappedQueryFilter).ApplyPaging(mappedQueryFilter).ToListAsync();
                return new GridResult<List<T>> { Result = result, TotalCount = totalCount };
            }
            else
            {
                var result = await query.ApplyOrdering(mappedQueryFilter).ToListAsync();
                return new GridResult<List<T>> { Result = result, TotalCount = result.Count() };
            }
        }

        #region Link'in oluşturduğu sorguyu görme
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            using var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }
        #region private
        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        #endregion

        #endregion

        private static Expression<Func<TModel, object>> GetPropertyExpression<TModel>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(TModel), "p");
            var property = Expression.Property(parameter, propertyName);

            var expression = Expression.Lambda<Func<TModel, object>>(Expression.Convert(Expression.Property(parameter, propertyName), typeof(Object)), parameter);
            return expression;
        }

        /// <summary>
        /// Listelerde enum descriptionlara göre sıralama yapar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<T> Convert<T>(this IQueryable<T> source)
        {
            var expression = new ExpressionConverter().Visit(source.Expression);
            if (expression == source.Expression) return source;
            return source.Provider.CreateQuery<T>(expression);
        }
    }
}
