using System;
using System.Linq.Expressions;

namespace ExpressionTreeIssue.Controllers
{
    public static class Extensions
    {
        public static string GetElementName<T>(this T model, Expression<Func<T, object>> expression) where T : IModel {
            var visitor = new Parser<T>(expression);
            var steps = visitor.GetCallSteps();
            return string.Join(".", steps);
        }

        public static TEntity Get<TEntity>(this EntityCollection<TEntity> collection, TEntity entity) where TEntity : class, IEntity {
            return entity;
        }
    }
}