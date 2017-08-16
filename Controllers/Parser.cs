using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreeIssue.Controllers
{
    public class Parser<T>
    {
        private readonly Expression<Func<T, object>> _expression;
        private List<Segment> _segments;

        public Parser(Expression<Func<T, object>> expression) {
            _expression = expression;
        }

        public List<Segment> GetCallSteps() {
            _segments = new List<Segment>();
            VisitNode(_expression.Body, _segments);
            _segments.Reverse();
            return _segments;
        }

        private void VisitNode(Expression body, List<Segment> steps) {
            switch (body.NodeType) {
                case ExpressionType.Convert:
                    var unary = body as UnaryExpression;
                    
                    VisitNode(unary.Operand, steps);
                    
                    break;
                case ExpressionType.MemberAccess:
                    var property = body as MemberExpression;
                    
                    VisitProperty(property, steps);
                    VisitNode(property.Expression, steps);
                    
                    break;
                case ExpressionType.Call:
                    var method = body as MethodCallExpression;
                    var caller = method.Arguments[0] as MemberExpression;
                    var arg = method.Arguments[1] as MemberExpression;
                    
                    VisitMethod(caller, arg, steps);
                    
                    if (caller.Expression.NodeType == ExpressionType.Call)
                        VisitNode(caller.Expression, steps);

                    break;
            }
        }

        private void VisitProperty(MemberExpression property, List<Segment> segments) {
            segments.Add(Segment.Property((PropertyInfo)property.Member));
        }

        private void VisitMethod(MemberExpression caller, MemberExpression arg, List<Segment> segments) {
            var converted = Expression.Convert(arg, typeof(IEntity));
            var lambda = Expression.Lambda<Func<IEntity>>(converted);
            var entity = lambda.Compile()();
            
            segments.Add(Segment.Collection((PropertyInfo)caller.Member, entity.Id));
        }
    }
}