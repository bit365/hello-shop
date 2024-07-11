// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace MultiTenancySample.FieldIsolationService.EntityFrameworks
{
    public static class EntityTypeBuilderQueryFilterExtensions
    {
        /// <summary>
        /// Support multiple HasQueryFilter calls on same entity type
        /// https://github.com/dotnet/efcore/issues/10275
        /// </summary>
        public static void AddQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression)
        {
            ParameterExpression parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
            Expression expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

            LambdaExpression? currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
            if (currentQueryFilter is not null)
            {
                Expression currentExpressionFilter = ReplacingExpressionVisitor.Replace(currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }

            LambdaExpression lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
            entityTypeBuilder.HasQueryFilter(lambdaExpression);
        }
    }
}
