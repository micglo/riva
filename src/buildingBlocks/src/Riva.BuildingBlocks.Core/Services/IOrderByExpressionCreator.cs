using System;
using System.Linq;

namespace Riva.BuildingBlocks.Core.Services
{
    public interface IOrderByExpressionCreator<T> where T : class
    {
        Func<IQueryable<T>, IOrderedQueryable<T>> CreateExpression(string sort);
    }
}