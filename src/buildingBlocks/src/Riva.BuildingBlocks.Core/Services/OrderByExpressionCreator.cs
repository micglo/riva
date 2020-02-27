using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Riva.BuildingBlocks.Core.Services
{
    public class OrderByExpressionCreator<T> : IOrderByExpressionCreator<T> where T : class
    {
        public Func<IQueryable<T>, IOrderedQueryable<T>> CreateExpression(string sort)
        {
            var propertyName = GetPropertyName(sort);
            var direction = GetSortDirection(sort);
            var queryableParameterExpression = Expression.Parameter(typeof(IQueryable<T>), "o");

            var orderByMethodCallExpression = GetOrderByMethodCallExpression(propertyName, direction, queryableParameterExpression);
            var resultExpression = Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(orderByMethodCallExpression, queryableParameterExpression);

            return resultExpression.Compile();
        }

        private static string GetPropertyName(string sort)
        {
            var colonIndex = sort.IndexOf(":", StringComparison.Ordinal);
            var propertyName = sort.Substring(0, colonIndex);
            return propertyName;
        }

        private static SortDirection GetSortDirection(string sort)
        {
            var colonIndex = sort.IndexOf(":", StringComparison.Ordinal);
            var directionString = sort.Substring(colonIndex + 1).ToLower();
            var direction = directionString.Equals("asc") ? SortDirection.Asc : SortDirection.Desc;
            return direction;
        }

        private static MethodCallExpression GetOrderByMethodCallExpression(string propertyName, SortDirection direction, Expression parameterExpression)
        {
            try
            {
                var entityParameterExpression = Expression.Parameter(typeof(T), "x");
                Expression member = Expression.Property(entityParameterExpression, propertyName);
                var orderByMethodInfo = GetOrderByMethodInfo(propertyName, direction);

                return Expression.Call(orderByMethodInfo, parameterExpression, Expression.Lambda(member, entityParameterExpression));
            }
            catch (ArgumentException)
            {
                throw new ArgumentException($"Property {propertyName} not found.");
            }
        }

        private static MethodInfo GetOrderByMethodInfo(string propertyName, SortDirection direction)
        {
            var entityType = typeof(T);
            var propertyType = GetPropertyType(entityType, propertyName);

            var queryableType = typeof(Queryable);
            static bool GenericPredicate(MethodInfo m) => m.IsGenericMethodDefinition && m.GetParameters().Length == 2;

            var orderByMethodInfo = direction == SortDirection.Desc
                ? queryableType.GetMethods().Where(m => m.Name == "OrderByDescending").Where(GenericPredicate).Single()
                : queryableType.GetMethods().Where(m => m.Name == "OrderBy").Where(GenericPredicate).Single();

            return orderByMethodInfo.MakeGenericMethod(entityType, propertyType);
        }

        private static Type GetPropertyType(Type classType, string propertyName)
        {
            return classType.GetProperties().SingleOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.CurrentCultureIgnoreCase))?.PropertyType;
        }

        private enum SortDirection
        {
            Asc,
            Desc
        }
    }
}