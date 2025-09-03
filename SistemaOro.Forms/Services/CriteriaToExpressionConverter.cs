using System;
using DevExpress.Data.Filtering;
using System.Linq.Expressions;
using DevExpress.DataProcessing;
using SistemaOro.Data.Entities;

namespace SistemaOro.Forms.Services;

public static class CriteriaToExpressionConverter
{
    public static Expression<Func<ComprasCliente, bool>> Convert(CriteriaOperator criteria)
    {
        // Implementación básica - puedes expandir según necesites
        var parameter = Expression.Parameter(typeof(ComprasCliente), "x");
        var expression = ConvertCriteria(criteria, parameter);
        return Expression.Lambda<Func<ComprasCliente, bool>>(expression, parameter);
    }

    private static Expression ConvertCriteria(CriteriaOperator criteria, ParameterExpression parameter)
    {
        // Implementación simplificada
        if (criteria is BinaryOperator binaryOp)
        {
            var left = ConvertOperand(binaryOp.LeftOperand, parameter);
            var right = ConvertOperand(binaryOp.RightOperand, parameter);
            
            return binaryOp.OperatorType switch
            {
                BinaryOperatorType.Equal => Expression.Equal(left, right),
                BinaryOperatorType.NotEqual => Expression.NotEqual(left, right),
                BinaryOperatorType.Greater => Expression.GreaterThan(left, right),
                BinaryOperatorType.GreaterOrEqual => Expression.GreaterThanOrEqual(left, right),
                BinaryOperatorType.Less => Expression.LessThan(left, right),
                BinaryOperatorType.LessOrEqual => Expression.LessThanOrEqual(left, right),
                BinaryOperatorType.Like => Expression.Call(left, "Contains", null, right),
                _ => throw new NotSupportedException()
            };
        }
        
        throw new NotSupportedException("Tipo de criterio no soportado");
    }

    private static Expression ConvertOperand(CriteriaOperator operand, ParameterExpression parameter)
    {
        return Expression.Property(parameter, operand.PropertyName());
    }

    private static Expression ConvertOperand(OperandValue operand, ParameterExpression parameter)
    {
        return Expression.Constant(operand.Value);
    }
}