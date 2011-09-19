#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using System.Data.OleDb;
using Sage.Common.Expressions;
using System.Diagnostics;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class QueryFilterBuilder
    {
        #region Properties

        public IEntityQueryWrapper EntityQueryWrapper { get; private set; }
        private ExpressionParser _expressionParser;
        private OrderByParser _orderByParser;

        #endregion

        #region Ctor.

        public QueryFilterBuilder(IEntityQueryWrapper entityQueryWrapper)
        {
            this.EntityQueryWrapper = entityQueryWrapper;
            _expressionParser = new ExpressionParser(this.EntityQueryWrapper);
            _orderByParser = new OrderByParser(this.EntityQueryWrapper);
        }

        #endregion


        public void BuildSqlStatement(RequestContext requestContext, out string additionalClause, out OleDbParameter[] oleDbParameters)
        {
            oleDbParameters = null;
            additionalClause = string.Empty;
            List<OleDbParameter> oleDbParameterList = new List<OleDbParameter>();

            if (requestContext.SdataUri.WhereExpression != null)
            {
                string expression = _expressionParser.GetExpressionDispatch(requestContext.SdataUri.WhereExpression, ref oleDbParameterList);
                if (!string.IsNullOrEmpty(expression))
                {
                    additionalClause = string.Format("where ( {0} )", expression);
                    oleDbParameters = oleDbParameterList.ToArray();
                }
            }
            if (!String.IsNullOrEmpty(requestContext.SdataUri.OrderBy))
            {
                additionalClause += " " + _orderByParser.Get(requestContext.SdataUri.OrderBy);
            }
        }

        #region CLASS: ExpressionParser

        private class ExpressionParser
        {
            private readonly IEntityQueryWrapper _entityQueryWrapper;

            public ExpressionParser(IEntityQueryWrapper entityQueryWrapper)
            {
                _entityQueryWrapper = entityQueryWrapper;
            }

            //public string GetCollectionPredicateExpression(RequestContext requestContext, ref List<OleDbParameter> parameters)
            //{
            //    string resourceKey = requestContext.ResourceKey;
            //    if (null == resourceKey)
            //        throw new InvalidOperationException();

            //    string scrmIdentifierName = _entityQueryWrapper.GetSageCrmIdentifier();
            //    string dbIdentifierName = _entityQueryWrapper.GetDbFieldName(scrmIdentifierName);
                
            //    parameters.Add(_entityQueryWrapper.GetOleDbParameter(dbf
            //    Type dbType = _entityQueryWrapper.GetDbFieldType(dbIdentifierName);

            //    if (dbType == typeof(string))
            //        return " ([" + dbIdentifierName + "] = '" + requestContext.ResourceKey + "') ";
            //    else
            //        return " ([" + dbIdentifierName + "] = " + requestContext.ResourceKey + ") ";
                
            //}
            public string GetExpressionDispatch(IExpression expression, ref List<OleDbParameter> parameters)
            {
                if (expression is ShortcutConditionalOperatorExpression)
                {
                    return GetExpression(expression as ShortcutConditionalOperatorExpression, ref parameters);
                }

                if (expression is ComparisonOperatorExpression)
                {
                    return GetExpression(expression as ComparisonOperatorExpression, ref parameters);
                }

                if (expression is LikeExpression)
                {
                    return GetExpression(expression as LikeExpression, ref parameters);
                }
                if (expression is DivideExpression)
                {
                    return GetExpression(expression as DivideExpression, ref parameters);
                }
                if (expression is MultiplyExpression)
                {
                    return GetExpression(expression as MultiplyExpression, ref parameters);
                }
                if (expression is MinusExpression)
                {
                    return GetExpression(expression as MinusExpression, ref parameters);
                }
                if (expression is PlusExpression)
                {
                    return GetExpression(expression as PlusExpression, ref parameters);
                }
                if (expression is LiteralExpression) // 'alfki' 1 ....
                {
                    return GetExpression(expression as LiteralExpression, ref parameters);
                }
                if (expression is FieldExpression) // customerid ....
                {
                    return GetExpression(expression as FieldExpression, ref parameters);
                }
                if (expression is BetweenExpression) // between
                {
                    return GetExpression(expression as BetweenExpression, ref parameters);
                }
                if (expression is UnaryOperatorExpression)  //Not 
                {
                    return GetExpression(expression as UnaryOperatorExpression, ref parameters);
                }
                else
                    return expression.ToString();
            }

            private string GetExpression(UnaryOperatorExpression expression, ref List<OleDbParameter> parameters)
            {
                if (expression is NotExpression)
                {
                    return GetExpression(expression as NotExpression, ref parameters);
                }

                if (expression is ParenthesesExpression)
                {
                    return GetExpression(expression as ParenthesesExpression, ref parameters);
                }
                else

                    return expression.ToString();
            }

            private string GetExpression(ParenthesesExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " ) ";
            }

            private string GetExpression(NotExpression expression, ref List<OleDbParameter> parameters)
            {
                return " (NOT( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " )) ";
            }

            private string GetExpression(BetweenExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " BETWEEN " +
                    GetExpressionDispatch(expression.Children[1], ref parameters) + " AND " +
                    GetExpressionDispatch(expression.Children[2], ref parameters) + " ) ";
            }

            public string GetExpression(FieldExpression expression, ref List<OleDbParameter> parameters)
            {
                return " " + _entityQueryWrapper.GetDbFieldName(expression.FieldName) + " ";
            }

            public string GetExpression(LiteralExpression expression, ref List<OleDbParameter> parameters)
            {
                string name = "p" + parameters.Count.ToString();
                OleDbParameter parameter = new OleDbParameter(name, expression.Value);
                parameters.Add(parameter);
                return " ? ";
            }

            private string GetExpression(IBinaryOperatorExpression expression, ref List<OleDbParameter> parameters)
            {
                if (expression is ShortcutConditionalOperatorExpression)
                {
                    return GetExpression(expression as ShortcutConditionalOperatorExpression, ref parameters);
                }

                if (expression is ComparisonOperatorExpression)
                {
                    return GetExpression(expression as ComparisonOperatorExpression, ref parameters);
                }

                if (expression is LikeExpression)
                {
                    return GetExpression(expression as LikeExpression, ref parameters);
                }
                if (expression is DivideExpression)
                {
                    return GetExpression(expression as DivideExpression, ref parameters);
                }
                if (expression is MultiplyExpression)
                {
                    return GetExpression(expression as MultiplyExpression, ref parameters);
                }
                if (expression is MinusExpression)
                {
                    return GetExpression(expression as MinusExpression, ref parameters);
                }
                if (expression is PlusExpression)
                {
                    return GetExpression(expression as PlusExpression, ref parameters);
                }

                else
                    return expression.ToString();
            }

            public string GetExpression(ComparisonOperatorExpression expression, ref List<OleDbParameter> parameters)
            {
                if (expression is EqualsExpression)
                {
                    return GetExpression(expression as EqualsExpression, ref parameters);
                }
                if (expression is NotEqualExpression)
                {
                    return GetExpression(expression as NotEqualExpression, ref parameters);
                }
                if (expression is GreaterThanExpression)
                {
                    return GetExpression(expression as GreaterThanExpression, ref parameters);
                }
                if (expression is GreaterThanOrEqualExpression)
                {
                    return GetExpression(expression as GreaterThanOrEqualExpression, ref parameters);
                }
                if (expression is LessThanExpression)
                {
                    return GetExpression(expression as LessThanExpression, ref parameters);
                }
                if (expression is LessThanOrEqualExpression)
                {
                    return GetExpression(expression as LessThanOrEqualExpression, ref parameters);
                }

                else
                    return expression.ToString();

            }

            private string GetExpression(ShortcutConditionalOperatorExpression expression, ref List<OleDbParameter> parameters)
            {
                if (expression is AndExpression)
                {
                    return GetExpression(expression as AndExpression, ref parameters);
                }

                if (expression is OrExpression)
                {
                    return GetExpression(expression as OrExpression, ref parameters);
                }

                else
                    return expression.ToString();

            }

            private string GetExpression(OrExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " OR " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(NotEqualExpression expression, ref List<OleDbParameter> parameters)
            {
                if ((expression.Children[1] is LiteralExpression) && ((expression.Children[1] as LiteralExpression).Value == null))
                    return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " is not null ) ";

                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " <> " +
                            GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(DivideExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " / " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }
            private string GetExpression(PlusExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " + " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(MultiplyExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " * " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(MinusExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " - " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(LikeExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " Like " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(LessThanOrEqualExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " <= " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(LessThanExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " < " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(GreaterThanOrEqualExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " >= " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(GreaterThanExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " > " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            public string GetExpression(EqualsExpression expression, ref List<OleDbParameter> parameters)
            {
                if ((expression.Children[1] is LiteralExpression) && ((expression.Children[1] as LiteralExpression).Value == null))
                    return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " is null ) ";

                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " = " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }

            private string GetExpression(AndExpression expression, ref List<OleDbParameter> parameters)
            {
                return " ( " + GetExpressionDispatch(expression.Children[0], ref parameters) + " AND " +
                  GetExpressionDispatch(expression.Children[1], ref parameters) + " ) ";
            }
        }
        
        #endregion

        #region CLASS: OrderByParser

        private class OrderByParser
        {
            private readonly IEntityQueryWrapper _entityQueryWrapper;

            public OrderByParser(IEntityQueryWrapper entitxQueryWrapper)
            {
                _entityQueryWrapper = entitxQueryWrapper;
            }

            public string Get(string orderBy)
            {
                string parsedExpression = string.Empty;

                string[] commaParts = orderBy.Split(',');

                foreach (string commaSplit in commaParts)
                {
                    string workingString = commaSplit.Trim();
                    if (string.IsNullOrEmpty(workingString))
                        throw new ArgumentException("Invalid ordering criteria. Code: 1");

                    workingString = workingString.Trim(',');
                    if (string.IsNullOrEmpty(workingString))
                        throw new ArgumentException("Invalid ordering criteria. Code: 2");

                    string[] sortPair = workingString.Split(' ');

                    if (sortPair.Length != 2)
                        throw new ArgumentException("Invalid ordering criteria. Code: 3");

                    if (! (sortPair[1] == "asc" || sortPair[1] == "desc"))
                        throw new ArgumentException("Invalid ordering criteria. Code: 4");

                    string parameterName = sortPair[0].Trim();
                    if (string.IsNullOrEmpty(parameterName))
                        throw new ArgumentException("Invalid ordering criteria. Code: 5");

                    string dbFieldName = _entityQueryWrapper.GetDbFieldName(parameterName);
                    if (string.IsNullOrEmpty(parameterName))
                        throw new ArgumentException("Invalid ordering criteria. Code: 6");

                    parsedExpression += string.Format(" {0} {1},", dbFieldName, sortPair[1]);
                }


                if (!string.IsNullOrEmpty(parsedExpression))
                    parsedExpression = "order by" + parsedExpression.TrimEnd(',');

                return parsedExpression;
            }
        }

        #endregion
    }
}
