using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.BaseConnect
{
    public class RemoveCastsVisitor : ExpressionVisitor
    {
        private static readonly ExpressionVisitor Default = new RemoveCastsVisitor();

        private RemoveCastsVisitor()
        {
        }

        public new static Expression Visit(Expression node)
        {
            return RemoveCastsVisitor.Default.Visit(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Convert && node.Type.IsAssignableFrom(node.Operand.Type))
            {
                return base.Visit(node.Operand);
            }
            return base.VisitUnary(node);
        }
    }
}
