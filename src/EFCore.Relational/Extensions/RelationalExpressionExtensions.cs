// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.Internal
{
    /// <summary>
    ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public static class RelationalExpressionExtensions
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static bool IsSimpleExpression([NotNull] this Expression expression)
        {
            Check.NotNull(expression, nameof(expression));

            var unwrappedExpression = expression.RemoveConvert();

            return unwrappedExpression is ConstantExpression
                   || unwrappedExpression is ColumnExpression
                   || unwrappedExpression is ParameterExpression
                   || unwrappedExpression is ColumnReferenceExpression
                   || unwrappedExpression is AliasExpression;
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static ColumnReferenceExpression LiftExpressionFromSubquery([NotNull] this Expression expression, [NotNull] TableExpressionBase table)
        {
            Check.NotNull(expression, nameof(expression));
            Check.NotNull(table, nameof(table));

            switch (expression)
            {
                case ColumnExpression columnExpression:
                    return new ColumnReferenceExpression(columnExpression, table);
                case AliasExpression aliasExpression:
                    return new ColumnReferenceExpression(aliasExpression, table);
                case ColumnReferenceExpression columnReferenceExpression:
                    return new ColumnReferenceExpression(columnReferenceExpression, table);
            }

            return null;
        }
    }
}