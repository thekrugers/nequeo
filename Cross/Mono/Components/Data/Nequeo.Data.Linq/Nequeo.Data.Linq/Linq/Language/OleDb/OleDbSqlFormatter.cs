﻿/*  Company :       Nequeo Pty Ltd, http://www.nequeo.com.au/
 *  Copyright :     Copyright © Nequeo Pty Ltd 2010 http://www.nequeo.com.au/
 * 
 *  File :          
 *  Purpose :       
 * 
 */

#region Nequeo Pty Ltd License
/*
    Permission is hereby granted, free of charge, to any person
    obtaining a copy of this software and associated documentation
    files (the "Software"), to deal in the Software without
    restriction, including without limitation the rights to use,
    copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following
    conditions:

    The above copyright notice and this permission notice shall be
    included in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
    OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
    HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
    WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
    OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Nequeo.Data.Linq;
using Nequeo.Data.Linq.Common;
using Nequeo.Data.Linq.Language;
using Nequeo.Data.Linq.Provider;
using Nequeo.Data.Linq.Common.Expressions;
using Nequeo.Data.Linq.Common.Translation;

namespace Nequeo.Data.Linq.Language
{
    /// <summary>
    /// Formats a query expression into OleDbSQL language syntax
    /// </summary>
    public class OleDbSqlFormatter : DbExpressionVisitor
    {
        StringBuilder sb;
        int indent = 2;
        int depth;
        Dictionary<TableAlias, string> aliases;
        bool patternModified = false;
        PatternModifier patternModifier = PatternModifier.In;
        ReadOnlyCollection<ColumnDeclaration> selectColumns;

        private OleDbSqlFormatter()
        {
            this.sb = new StringBuilder();
            this.aliases = new Dictionary<TableAlias, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string Format(Expression expression)
        {
            OleDbSqlFormatter formatter = new OleDbSqlFormatter();
            formatter.Visit(expression);
            return formatter.sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        protected enum Indentation
        {
            /// <summary>
            /// 
            /// </summary>
            Same,
            /// <summary>
            /// 
            /// </summary>
            Inner,
            /// <summary>
            /// 
            /// </summary>
            Outer
        }

        /// <summary>
        /// 
        /// </summary>
        protected enum PatternModifier
        {
            /// <summary>
            /// 
            /// </summary>
            In,
            /// <summary>
            /// 
            /// </summary>
            NotIn
        }

        /// <summary>
        /// 
        /// </summary>
        internal int IndentationWidth
        {
            get { return this.indent; }
            set { this.indent = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        private void AppendNewLine(Indentation style)
        {
            sb.AppendLine();
            this.Indent(style);
            for (int i = 0, n = this.depth * this.indent; i < n; i++)
            {
                sb.Append(" ");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        private void Indent(Indentation style)
        {
            if (style == Indentation.Inner)
            {
                this.depth++;
            }
            else if (style == Indentation.Outer)
            {
                this.depth--;
                System.Diagnostics.Debug.Assert(this.depth >= 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected override Expression Visit(Expression exp)
        {
            if (exp == null) return null;

            // check for supported node types first 
            // non-supported ones should not be visited (as they would produce bad SQL)
            switch (exp.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                case ExpressionType.Not:
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                case ExpressionType.UnaryPlus:
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                case ExpressionType.Divide:
                case ExpressionType.Modulo:
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.Coalesce:
                case ExpressionType.RightShift:
                case ExpressionType.LeftShift:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.Power:
                case ExpressionType.Conditional:
                case ExpressionType.Constant:
                case ExpressionType.MemberAccess:
                case ExpressionType.Call:
                case ExpressionType.New:
                case (ExpressionType)DbExpressionType.Table:
                case (ExpressionType)DbExpressionType.Column:
                case (ExpressionType)DbExpressionType.Select:
                case (ExpressionType)DbExpressionType.Join:
                case (ExpressionType)DbExpressionType.Aggregate:
                case (ExpressionType)DbExpressionType.Scalar:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.In:
                case (ExpressionType)DbExpressionType.AggregateSubquery:
                case (ExpressionType)DbExpressionType.IsNull:
                case (ExpressionType)DbExpressionType.Between:
                case (ExpressionType)DbExpressionType.RowCount:
                case (ExpressionType)DbExpressionType.Projection:
                case (ExpressionType)DbExpressionType.NamedValue:
                    return base.Visit(exp);

                case ExpressionType.ArrayLength:
                case ExpressionType.Quote:
                case ExpressionType.TypeAs:
                case ExpressionType.ArrayIndex:
                case ExpressionType.TypeIs:
                case ExpressionType.Parameter:
                case ExpressionType.Lambda:
                case ExpressionType.NewArrayInit:
                case ExpressionType.NewArrayBounds:
                case ExpressionType.Invoke:
                case ExpressionType.MemberInit:
                case ExpressionType.ListInit:
                default:
                    throw new Exception(string.Format("The LINQ expression node of type {0} is not supported", exp.NodeType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMemberAccess(MemberExpression m)
        {
            if (m.Member.DeclaringType == typeof(string))
            {
                switch (m.Member.Name)
                {
                    case "Length":
                        sb.Append("LEN(");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                }
            }
            else if (m.Member.DeclaringType == typeof(DateTime) || m.Member.DeclaringType == typeof(DateTimeOffset))
            {
                switch (m.Member.Name)
                {
                    case "Day":
                        sb.Append("DAY(");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Month":
                        sb.Append("MONTH(");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Year":
                        sb.Append("YEAR(");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Hour":
                        sb.Append("DATEPART(hour, ");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Minute":
                        sb.Append("DATEPART(minute, ");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Second":
                        sb.Append("DATEPART(second, ");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "Millisecond":
                        sb.Append("DATEPART(millisecond, ");
                        this.Visit(m.Expression);
                        sb.Append(")");
                        return m;
                    case "DayOfWeek":
                        sb.Append("(DATEPART(weekday, ");
                        this.Visit(m.Expression);
                        sb.Append(") - 1)");
                        return m;
                    case "DayOfYear":
                        sb.Append("(DATEPART(dayofyear, ");
                        this.Visit(m.Expression);
                        sb.Append(") - 1)");
                        return m;
                }
            }
            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(string))
            {
                switch (m.Method.Name)
                {
                    case "StartsWith":
                        sb.Append("(");
                        this.Visit(m.Object);
                        sb.Append(" LIKE ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" + '%')");
                        return m;
                    case "EndsWith":
                        sb.Append("(");
                        this.Visit(m.Object);
                        sb.Append(" LIKE '%' + ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(")");
                        return m;
                    case "Contains":
                        sb.Append("(");
                        this.Visit(m.Object);
                        sb.Append(" LIKE '%' + ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" + '%')");
                        return m;
                    case "Concat":
                        IList<Expression> args = m.Arguments;
                        if (args.Count == 1 && args[0].NodeType == ExpressionType.NewArrayInit)
                        {
                            args = ((NewArrayExpression)args[0]).Expressions;
                        }
                        for (int i = 0, n = args.Count; i < n; i++)
                        {
                            if (i > 0) sb.Append(" + ");
                            this.Visit(args[i]);
                        }
                        return m;
                    case "IsNullOrEmpty":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" IS NULL OR ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" = '')");
                        return m;
                    case "ToUpper":
                        sb.Append("UPPER(");
                        this.Visit(m.Object);
                        sb.Append(")");
                        return m;
                    case "ToLower":
                        sb.Append("LOWER(");
                        this.Visit(m.Object);
                        sb.Append(")");
                        return m;
                    case "Replace":
                        sb.Append("REPLACE(");
                        this.Visit(m.Object);
                        sb.Append(", ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "Substring":
                        sb.Append("SUBSTRING(");
                        this.Visit(m.Object);
                        sb.Append(", ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" + 1, ");
                        if (m.Arguments.Count == 2)
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        else
                        {
                            sb.Append("8000");
                        }
                        sb.Append(")");
                        return m;
                    case "Remove":
                        sb.Append("STUFF(");
                        this.Visit(m.Object);
                        sb.Append(", ");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" + 1, ");
                        if (m.Arguments.Count == 2)
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        else
                        {
                            sb.Append("8000");
                        }
                        sb.Append(", '')");
                        return m;
                    case "IndexOf":
                        sb.Append("(CHARINDEX(");
                        this.Visit(m.Object);
                        sb.Append(", ");
                        this.Visit(m.Arguments[0]);
                        if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                        {
                            sb.Append(", ");
                            this.Visit(m.Arguments[1]);
                        }
                        sb.Append(") - 1)");
                        return m;
                    case "Trim":
                        sb.Append("RTRIM(LTRIM(");
                        this.Visit(m.Object);
                        sb.Append("))");
                        return m;
                }
            }
            else if (m.Method.DeclaringType == typeof(DateTime))
            {
                switch (m.Method.Name)
                {
                    case "op_Subtract":
                        if (m.Arguments[1].Type == typeof(DateTime))
                        {
                            sb.Append("DATEDIFF(");
                            this.Visit(m.Arguments[0]);
                            sb.Append(", ");
                            this.Visit(m.Arguments[1]);
                            sb.Append(")");
                            return m;
                        }
                        break;
                }
            }
            else if (m.Method.DeclaringType == typeof(Decimal))
            {
                switch (m.Method.Name)
                {
                    case "Add":
                    case "Subtract":
                    case "Multiply":
                    case "Divide":
                    case "Remainder":
                        sb.Append("(");
                        this.VisitValue(m.Arguments[0]);
                        sb.Append(" ");
                        sb.Append(GetOperator(m.Method.Name));
                        sb.Append(" ");
                        this.VisitValue(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "Negate":
                        sb.Append("-");
                        this.Visit(m.Arguments[0]);
                        sb.Append("");
                        return m;
                    case "Ceiling":
                    case "Floor":
                        sb.Append(m.Method.Name.ToUpper());
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(")");
                        return m;
                    case "Round":
                        if (m.Arguments.Count == 1)
                        {
                            sb.Append("ROUND(");
                            this.Visit(m.Arguments[0]);
                            sb.Append(", 0)");
                            return m;
                        }
                        else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                        {
                            sb.Append("ROUND(");
                            this.Visit(m.Arguments[0]);
                            sb.Append(", ");
                            this.Visit(m.Arguments[1]);
                            sb.Append(")");
                            return m;
                        }
                        break;
                    case "Truncate":
                        sb.Append("ROUND(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", 0, 1)");
                        return m;
                }
            }
            else if (m.Method.DeclaringType == typeof(System.Math))
            {
                switch (m.Method.Name)
                {
                    case "Abs":
                    case "Acos":
                    case "Asin":
                    case "Atan":
                    case "Cos":
                    case "Exp":
                    case "Log10":
                    case "Sin":
                    case "Tan":
                    case "Sqrt":
                    case "Sign":
                    case "Ceiling":
                    case "Floor":
                        sb.Append(m.Method.Name.ToUpper());
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(")");
                        return m;
                    case "Atan2":
                        sb.Append("ATN2(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "Log":
                        if (m.Arguments.Count == 1)
                        {
                            goto case "Log10";
                        }
                        break;
                    case "Pow":
                        sb.Append("POWER(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "Round":
                        if (m.Arguments.Count == 1)
                        {
                            sb.Append("ROUND(");
                            this.Visit(m.Arguments[0]);
                            sb.Append(", 0)");
                            return m;
                        }
                        else if (m.Arguments.Count == 2 && m.Arguments[1].Type == typeof(int))
                        {
                            sb.Append("ROUND(");
                            this.Visit(m.Arguments[0]);
                            sb.Append(", ");
                            this.Visit(m.Arguments[1]);
                            sb.Append(")");
                            return m;
                        }
                        break;
                    case "Truncate":
                        sb.Append("ROUND(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", 0, 1)");
                        return m;
                }
            }
            else if (m.Method.DeclaringType == typeof(Nequeo.Data.TypeExtenders.SqlQueryMethods))
            {
                switch (m.Method.Name)
                {
                    case "Like":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" LIKE ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "NotLike":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" NOT LIKE ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(")");
                        return m;
                    case "LikeEscape":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" LIKE ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(" ESCAPE ");
                        this.Visit(m.Arguments[2]);
                        sb.Append(")");
                        return m;
                    case "NotLikeEscape":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" NOT LIKE ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(" ESCAPE ");
                        this.Visit(m.Arguments[2]);
                        sb.Append(")");
                        return m;
                    case "Between":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" BETWEEN ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(" AND ");
                        this.Visit(m.Arguments[2]);
                        sb.Append(")");
                        return m;
                    case "NotBetween":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" NOT BETWEEN ");
                        this.Visit(m.Arguments[1]);
                        sb.Append(" AND ");
                        this.Visit(m.Arguments[2]);
                        sb.Append(")");
                        return m;
                    case "IsNull":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" IS NULL ");
                        sb.Append(")");
                        return m;
                    case "IsNotNull":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" IS NOT NULL ");
                        sb.Append(")");
                        return m;
                    case "Contains":
                        sb.Append("(CONTAINS(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(", ");
                        this.Visit(m.Arguments[1]);
                        sb.Append("))");
                        return m;
                    case "In":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" IN (");
                        if (m.Arguments.Count > 2)
                        {
                            int inNumber = m.Arguments.Count;
                            for (int inCount = 1; inCount < inNumber; inCount++)
                            {
                                if (inCount < inNumber - 1)
                                {
                                    this.Visit(m.Arguments[inCount]);
                                    sb.Append(", ");
                                }
                                else
                                {
                                    this.Visit(m.Arguments[inCount]);
                                }
                            }
                        }
                        else
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        sb.Append("))");
                        return m;
                    case "NotIn":
                        sb.Append("(");
                        this.Visit(m.Arguments[0]);
                        sb.Append(" NOT IN (");
                        if (m.Arguments.Count > 2)
                        {
                            int notInNumber = m.Arguments.Count;
                            for (int notInCount = 1; notInCount < notInNumber; notInCount++)
                            {
                                if (notInCount < notInNumber - 1)
                                {
                                    this.Visit(m.Arguments[notInCount]);
                                    sb.Append(", ");
                                }
                                else
                                {
                                    this.Visit(m.Arguments[notInCount]);
                                }
                            }
                        }
                        else
                        {
                            this.Visit(m.Arguments[1]);
                        }
                        sb.Append("))");
                        return m;
                }
            }
            if (m.Method.Name == "ToString")
            {
                if (m.Object.Type == typeof(string))
                {
                    this.Visit(m.Object);  // no op
                }
                else
                {
                    sb.Append("CONVERT(VARCHAR, ");
                    this.Visit(m.Object);
                    sb.Append(")");
                }
                return m;
            }
            else if (m.Method.Name == "Equals")
            {
                if (m.Method.IsStatic && m.Method.DeclaringType == typeof(object))
                {
                    sb.Append("(");
                    this.Visit(m.Arguments[0]);
                    sb.Append(" = ");
                    this.Visit(m.Arguments[1]);
                    sb.Append(")");
                    return m;
                }
                else if (!m.Method.IsStatic && m.Arguments.Count == 1 && m.Arguments[0].Type == m.Object.Type)
                {
                    sb.Append("(");
                    this.Visit(m.Object);
                    sb.Append(" = ");
                    this.Visit(m.Arguments[0]);
                    sb.Append(")");
                    return m;
                }
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nex"></param>
        /// <returns></returns>
        protected override NewExpression VisitNew(NewExpression nex)
        {
            if (nex.Constructor.DeclaringType == typeof(DateTime))
            {
                if (nex.Arguments.Count == 3)
                {
                    sb.Append("DATEADD(year, ");
                    this.Visit(nex.Arguments[0]);
                    sb.Append(", DATEADD(month, ");
                    this.Visit(nex.Arguments[1]);
                    sb.Append(", DATEADD(day, ");
                    this.Visit(nex.Arguments[2]);
                    sb.Append(", 0)))");
                    return nex;
                }
                else if (nex.Arguments.Count == 6)
                {
                    sb.Append("DATEADD(year, ");
                    this.Visit(nex.Arguments[0]);
                    sb.Append(", DATEADD(month, ");
                    this.Visit(nex.Arguments[1]);
                    sb.Append(", DATEADD(day, ");
                    this.Visit(nex.Arguments[2]);
                    sb.Append(", DATEADD(hour, ");
                    this.Visit(nex.Arguments[3]);
                    sb.Append(", DATEADD(minute, ");
                    this.Visit(nex.Arguments[4]);
                    sb.Append(", DATEADD(second, ");
                    this.Visit(nex.Arguments[5]);
                    sb.Append(", 0))))))");
                    return nex;
                }
            }

            throw new NotSupportedException(string.Format("The construtor '{0}' is not supported", nex.Constructor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression u)
        {
            string op = this.GetOperator(u);
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    if (IsBoolean(u.Operand.Type))
                    {
                        sb.Append(op);
                        sb.Append(" ");
                        this.VisitPredicate(u.Operand);
                    }
                    else
                    {
                        sb.Append(op);
                        this.VisitValue(u.Operand);
                    }
                    break;
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    sb.Append(op);
                    this.VisitValue(u.Operand);
                    break;
                case ExpressionType.UnaryPlus:
                    this.VisitValue(u.Operand);
                    break;
                case ExpressionType.Convert:
                    // ignore conversions for now
                    this.Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression b)
        {
            string op = this.GetOperator(b);
            Expression left = b.Left;
            Expression right = b.Right;

            if (b.NodeType == ExpressionType.Power)
            {
                sb.Append("POWER(");
                this.VisitValue(left);
                sb.Append(", ");
                this.VisitValue(right);
                sb.Append(")");
                return b;
            }
            else if (b.NodeType == ExpressionType.Coalesce)
            {
                sb.Append("COALESCE(");
                this.VisitValue(left);
                sb.Append(", ");
                while (right.NodeType == ExpressionType.Coalesce)
                {
                    BinaryExpression rb = (BinaryExpression)right;
                    this.VisitValue(rb.Left);
                    sb.Append(", ");
                    right = rb.Right;
                }
                this.VisitValue(right);
                sb.Append(")");
                return b;
            }
            else
            {
                sb.Append("(");
                switch (b.NodeType)
                {
                    case ExpressionType.And:
                    case ExpressionType.AndAlso:
                    case ExpressionType.Or:
                    case ExpressionType.OrElse:
                        if (IsBoolean(left.Type))
                        {
                            this.VisitPredicate(left);
                            sb.Append(" ");
                            sb.Append(op);
                            sb.Append(" ");
                            this.VisitPredicate(right);
                        }
                        else
                        {
                            this.VisitValue(left);
                            sb.Append(" ");
                            sb.Append(op);
                            sb.Append(" ");
                            this.VisitValue(right);
                        }
                        break;
                    case ExpressionType.Equal:
                    case ExpressionType.NotEqual:
                        if (right.NodeType == ExpressionType.Constant)
                        {
                            ConstantExpression ce = (ConstantExpression)right;
                            if (ce.Value == null)
                            {
                                this.Visit(left);
                                sb.Append(" IS NULL");
                                break;
                            }
                        }
                        else if (left.NodeType == ExpressionType.Constant)
                        {
                            ConstantExpression ce = (ConstantExpression)left;
                            if (ce.Value == null)
                            {
                                this.Visit(right);
                                sb.Append(" IS NULL");
                                break;
                            }
                        }
                        goto case ExpressionType.LessThan;
                    case ExpressionType.LessThan:
                    case ExpressionType.LessThanOrEqual:
                    case ExpressionType.GreaterThan:
                    case ExpressionType.GreaterThanOrEqual:
                        // check for special x.CompareTo(y) && type.Compare(x,y)
                        if (left.NodeType == ExpressionType.Call && right.NodeType == ExpressionType.Constant)
                        {
                            MethodCallExpression mc = (MethodCallExpression)left;
                            ConstantExpression ce = (ConstantExpression)right;
                            if (ce.Value != null && ce.Value.GetType() == typeof(int) && ((int)ce.Value) == 0)
                            {
                                if (mc.Method.Name == "CompareTo" && !mc.Method.IsStatic && mc.Arguments.Count == 1)
                                {
                                    left = mc.Object;
                                    right = mc.Arguments[0];
                                }
                                else if (
                                    (mc.Method.DeclaringType == typeof(string) || mc.Method.DeclaringType == typeof(decimal))
                                      && mc.Method.Name == "Compare" && mc.Method.IsStatic && mc.Arguments.Count == 2)
                                {
                                    left = mc.Arguments[0];
                                    right = mc.Arguments[1];
                                }
                            }
                        }
                        goto case ExpressionType.Add;
                    case ExpressionType.Add:
                    case ExpressionType.AddChecked:
                    case ExpressionType.Subtract:
                    case ExpressionType.SubtractChecked:
                    case ExpressionType.Multiply:
                    case ExpressionType.MultiplyChecked:
                    case ExpressionType.Divide:
                    case ExpressionType.Modulo:
                    case ExpressionType.ExclusiveOr:
                        this.VisitValue(left);
                        sb.Append(" ");
                        sb.Append(op);
                        sb.Append(" ");
                        this.VisitValue(right);
                        break;
                    case ExpressionType.RightShift:
                        this.VisitValue(left);
                        sb.Append(" / POWER(2, ");
                        this.VisitValue(right);
                        sb.Append(")");
                        break;
                    case ExpressionType.LeftShift:
                        this.VisitValue(left);
                        sb.Append(" * POWER(2, ");
                        this.VisitValue(right);
                        sb.Append(")");
                        break;
                    default:
                        throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
                }
                sb.Append(")");
            }
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetOperator(string methodName)
        {
            switch (methodName)
            {
                case "Add": return "+";
                case "Subtract": return "-";
                case "Multiply": return "*";
                case "Divide": return "/";
                case "Negate": return "-";
                case "Remainder": return "%";
                default: return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        private string GetOperator(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Negate:
                case ExpressionType.NegateChecked:
                    return "-";
                case ExpressionType.UnaryPlus:
                    return "+";
                case ExpressionType.Not:
                    return IsBoolean(u.Operand.Type) ? "NOT" : "~";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string GetOperator(BinaryExpression b)
        {
            switch (b.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    return (IsBoolean(b.Left.Type)) ? "AND" : "&";
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return (IsBoolean(b.Left.Type) ? "OR" : "|");
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Modulo:
                    return "%";
                case ExpressionType.ExclusiveOr:
                    return "^";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsBoolean(Type type)
        {
            return type == typeof(bool) || type == typeof(bool?);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        private bool IsPredicate(Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return IsBoolean(((BinaryExpression)expr).Type);
                case ExpressionType.Not:
                    return IsBoolean(((UnaryExpression)expr).Type);
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.GreaterThanOrEqual:
                case (ExpressionType)DbExpressionType.IsNull:
                case (ExpressionType)DbExpressionType.Between:
                case (ExpressionType)DbExpressionType.Exists:
                case (ExpressionType)DbExpressionType.In:
                    return true;
                case ExpressionType.Call:
                    return IsBoolean(((MethodCallExpression)expr).Type);
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        protected virtual Expression VisitPredicate(Expression expr)
        {
            this.Visit(expr);
            if (!IsPredicate(expr))
            {
                sb.Append(" <> 0");
            }
            return expr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        protected virtual Expression VisitValue(Expression expr)
        {
            if (IsPredicate(expr))
            {
                sb.Append("CASE WHEN (");
                this.Visit(expr);
                sb.Append(") THEN 1 ELSE 0 END");
            }
            else
            {
                this.Visit(expr);
            }
            return expr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConditional(ConditionalExpression c)
        {
            if (this.IsPredicate(c.Test))
            {
                sb.Append("(CASE WHEN ");
                this.VisitPredicate(c.Test);
                sb.Append(" THEN ");
                this.VisitValue(c.IfTrue);
                Expression ifFalse = c.IfFalse;
                while (ifFalse != null && ifFalse.NodeType == ExpressionType.Conditional)
                {
                    ConditionalExpression fc = (ConditionalExpression)ifFalse;
                    sb.Append(" WHEN ");
                    this.VisitPredicate(fc.Test);
                    sb.Append(" THEN ");
                    this.VisitValue(fc.IfTrue);
                    ifFalse = fc.IfFalse;
                }
                if (ifFalse != null)
                {
                    sb.Append(" ELSE ");
                    this.VisitValue(ifFalse);
                }
                sb.Append(" END)");
            }
            else
            {
                sb.Append("(CASE ");
                this.VisitValue(c.Test);
                sb.Append(" WHEN 0 THEN ");
                this.VisitValue(c.IfFalse);
                sb.Append(" ELSE ");
                this.VisitValue(c.IfTrue);
                sb.Append(" END)");
            }
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression c)
        {
            this.WriteValue(c.Value);
            return c;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void WriteValue(object value)
        {
            if (value == null)
            {
                sb.Append("NULL");
            }
            else
            {
                if (patternModified)
                {
                    switch (patternModifier)
                    {
                        case PatternModifier.In:
                            sb.Append(value);
                            break;
                        case PatternModifier.NotIn:
                            sb.Append(value);
                            break;
                    }
                    patternModified = false;
                }
                else
                {
                    switch (Type.GetTypeCode(value.GetType()))
                    {
                        case TypeCode.Boolean:
                            sb.Append(((bool)value) ? 1 : 0);
                            break;
                        case TypeCode.String:
                        case TypeCode.Char:
                            sb.Append("'");
                            sb.Append(value);
                            sb.Append("'");
                            break;
                        case TypeCode.DateTime:
                            DateTime itemDateTime = (DateTime)value;
                            sb.Append("'");
                            sb.Append(itemDateTime.Year);
                            sb.Append("-");
                            sb.Append(itemDateTime.Month);
                            sb.Append("-");
                            sb.Append(itemDateTime.Day);
                            sb.Append(" ");
                            sb.Append(itemDateTime.Hour);
                            sb.Append(":");
                            sb.Append(itemDateTime.Minute);
                            sb.Append(":");
                            sb.Append(itemDateTime.Second);
                            sb.Append("'");
                            break;
                        case TypeCode.Byte:
                            Byte[] itemByteArray = (Byte[])value;

                            // Foreach byte found convert the byte
                            // to an octet value string item.
                            int i = 0;
                            string[] octetArrayByte = new string[itemByteArray.Count()];
                            foreach (Byte item in itemByteArray)
                                octetArrayByte[i++] = item.ToString("X2");

                            // Create the octet string of bytes.
                            string octetValue = "0x" + String.Join("", octetArrayByte);
                            sb.Append(octetValue);
                            break;
                        case TypeCode.SByte:
                            Byte[] itemSByteArray = (Byte[])value;

                            // Foreach byte found convert the byte
                            // to an octet value string item.
                            int j = 0;
                            string[] octetSArrayByte = new string[itemSByteArray.Count()];
                            foreach (Byte item in itemSByteArray)
                                octetSArrayByte[j++] = item.ToString("X2");

                            // Create the octet string of bytes.
                            string octetSValue = "0x" + String.Join("", octetSArrayByte);
                            sb.Append(octetSValue);
                            break;
                        case TypeCode.Object:
                            if (value.GetType().IsEnum)
                            {
                                sb.Append(Convert.ChangeType(value, typeof(int)));
                            }
                            else
                            {
                                throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", value));
                            }
                            break;
                        default:
                            sb.Append(value);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        private string GetAliasName(TableAlias alias)
        {
            string name;
            if (!this.aliases.TryGetValue(alias, out name))
            {
                name = "t" + this.aliases.Count;
                this.aliases.Add(alias, name);
            }
            return name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected override Expression VisitColumn(ColumnExpression column)
        {
            if (column.Alias != null)
            {
                sb.Append(GetAliasName(column.Alias));
                sb.Append(".");
            }
            sb.Append(column.Name);
            return column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        protected override Expression VisitProjection(ProjectionExpression proj)
        {
            // treat these like scalar subqueries
            if (proj.Projector is ColumnExpression)
            {
                sb.Append("(");
                this.AppendNewLine(Indentation.Inner);
                this.Visit(proj.Source);
                sb.Append(")");
                this.Indent(Indentation.Outer);
            }
            else
            {
                throw new NotSupportedException("Non-scalar projections cannot be translated to SQL.");
            }
            return proj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        protected override Expression VisitSelect(SelectExpression select)
        {
            sb.Append("SELECT ");
            if (select.IsDistinct)
            {
                sb.Append("DISTINCT ");
            }
            if (select.Take != null)
            {
                switch (MappingController.ConnectionDataType)
                {
                    case Nequeo.Data.DataType.ConnectionContext.ConnectionDataType.AccessDataType:
                        sb.Append("TOP ");
                        this.Visit(select.Take);
                        sb.Append(" ");
                        break;

                    case Nequeo.Data.DataType.ConnectionContext.ConnectionDataType.ScxDataType:
                        sb.Append("TOP (");
                        this.Visit(select.Take);
                        sb.Append(") ");
                        break;

                    default:
                        sb.Append("(");
                        this.Visit(select.Take);
                        sb.Append(") ");
                        break;
                }
            }
            if (select.Columns.Count > 0)
            {
                selectColumns = select.Columns;
                for (int i = 0, n = select.Columns.Count; i < n; i++)
                {
                    ColumnDeclaration column = select.Columns[i];

                    if (!column.Name.Equals("UniqueBaseId") && !column.Name.Equals("ExtensionData"))
                    {
                        if (i > 0)
                        {
                            sb.Append(", ");
                        }
                        ColumnExpression c = this.VisitValue(column.Expression) as ColumnExpression;
                        if (!string.IsNullOrEmpty(column.Name) && (c == null || c.Name != column.Name))
                        {
                            sb.Append(" AS ");
                            sb.Append(column.Name);
                        }
                    }
                }
            }
            else
            {
                sb.Append("NULL ");
                if (this.isNested)
                {
                    sb.Append("AS tmp ");
                }
            }
            if (select.From != null)
            {
                this.AppendNewLine(Indentation.Same);
                sb.Append("FROM ");
                this.VisitSource(select.From);
            }
            if (select.Where != null)
            {
                this.AppendNewLine(Indentation.Same);
                sb.Append("WHERE ");
                this.VisitPredicate(select.Where);
            }
            if (select.GroupBy != null && select.GroupBy.Count > 0)
            {
                this.AppendNewLine(Indentation.Same);
                sb.Append("GROUP BY ");
                for (int i = 0, n = select.GroupBy.Count; i < n; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    this.VisitValue(select.GroupBy[i]);
                }
            }
            if (select.OrderBy != null && select.OrderBy.Count > 0)
            {
                this.AppendNewLine(Indentation.Same);
                sb.Append("ORDER BY ");
                for (int i = 0, n = select.OrderBy.Count; i < n; i++)
                {
                    OrderExpression exp = select.OrderBy[i];
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    this.VisitValue(exp.Expression);
                    if (exp.OrderType != OrderType.Ascending)
                    {
                        sb.Append(" DESC");
                    }
                }
            }
            return select;
        }

        bool isNested = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override Expression VisitSource(Expression source)
        {
            bool saveIsNested = this.isNested;
            this.isNested = true;
            switch ((DbExpressionType)source.NodeType)
            {
                case DbExpressionType.Table:
                    TableExpression table = (TableExpression)source;
                    sb.Append(table.Name);
                    sb.Append(" AS ");
                    sb.Append(GetAliasName(table.Alias));
                    break;
                case DbExpressionType.Select:
                    SelectExpression select = (SelectExpression)source;
                    sb.Append("(");
                    this.AppendNewLine(Indentation.Inner);
                    this.Visit(select);
                    this.AppendNewLine(Indentation.Same);
                    sb.Append(")");
                    sb.Append(" AS ");
                    sb.Append(GetAliasName(select.Alias));
                    this.Indent(Indentation.Outer);
                    break;
                case DbExpressionType.Join:
                    this.VisitJoin((JoinExpression)source);
                    break;
                default:
                    throw new InvalidOperationException("Select source is not valid type");
            }
            this.isNested = saveIsNested;
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="join"></param>
        /// <returns></returns>
        protected override Expression VisitJoin(JoinExpression join)
        {
            this.VisitSource(join.Left);
            this.AppendNewLine(Indentation.Same);
            switch (join.Join)
            {
                case JoinType.CrossJoin:
                    sb.Append("CROSS JOIN ");
                    break;
                case JoinType.InnerJoin:
                    sb.Append("INNER JOIN ");
                    break;
                case JoinType.CrossApply:
                    sb.Append("CROSS APPLY ");
                    break;
                case JoinType.OuterApply:
                    sb.Append("OUTER APPLY ");
                    break;
                case JoinType.LeftOuter:
                    sb.Append("LEFT OUTER JOIN ");
                    break;
            }
            this.VisitSource(join.Right);
            if (join.Condition != null)
            {
                this.AppendNewLine(Indentation.Inner);
                sb.Append("ON ");
                this.VisitPredicate(join.Condition);
                this.Indent(Indentation.Outer);
            }
            return join;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregateType"></param>
        /// <returns></returns>
        private string GetAggregateName(AggregateType aggregateType)
        {
            switch (aggregateType)
            {
                case AggregateType.Count: return "COUNT";
                case AggregateType.Min: return "MIN";
                case AggregateType.Max: return "MAX";
                case AggregateType.Sum: return "SUM";
                case AggregateType.Average: return "AVG";
                default: throw new Exception(string.Format("Unknown aggregate type: {0}", aggregateType));
            }
        }

        private bool RequiresAsteriskWhenNoArgument(AggregateType aggregateType)
        {
            return aggregateType == AggregateType.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aggregate"></param>
        /// <returns></returns>
        protected override Expression VisitAggregate(AggregateExpression aggregate)
        {
            sb.Append(GetAggregateName(aggregate.AggregateType));
            sb.Append("(");
            if (aggregate.IsDistinct)
            {
                sb.Append("DISTINCT ");
            }
            if (aggregate.Argument != null)
            {
                this.VisitValue(aggregate.Argument);
            }
            else if (RequiresAsteriskWhenNoArgument(aggregate.AggregateType))
            {
                sb.Append("*");
            }
            sb.Append(")");
            return aggregate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isnull"></param>
        /// <returns></returns>
        protected override Expression VisitIsNull(IsNullExpression isnull)
        {
            this.VisitValue(isnull.Expression);
            sb.Append(" IS NULL");
            return isnull;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="between"></param>
        /// <returns></returns>
        protected override Expression VisitBetween(BetweenExpression between)
        {
            this.VisitValue(between.Expression);
            sb.Append(" BETWEEN ");
            this.VisitValue(between.Lower);
            sb.Append(" AND ");
            this.VisitValue(between.Upper);
            return between;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        protected override Expression VisitRowNumber(RowNumberExpression rowNumber)
        {
            sb.Append("ROW_NUMBER() OVER(");
            if (rowNumber.OrderBy != null && rowNumber.OrderBy.Count > 0)
            {
                sb.Append("ORDER BY ");
                for (int i = 0, n = rowNumber.OrderBy.Count; i < n; i++)
                {
                    OrderExpression exp = rowNumber.OrderBy[i];
                    if (i > 0)
                    {
                        sb.Append(", ");
                    }
                    this.VisitValue(exp.Expression);
                    if (exp.OrderType != OrderType.Ascending)
                    {
                        sb.Append(" DESC");
                    }
                }
            }
            sb.Append(")");
            return rowNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subquery"></param>
        /// <returns></returns>
        protected override Expression VisitScalar(ScalarExpression subquery)
        {
            sb.Append("(");
            this.AppendNewLine(Indentation.Inner);
            this.Visit(subquery.Select);
            this.AppendNewLine(Indentation.Same);
            sb.Append(")");
            this.Indent(Indentation.Outer);
            return subquery;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exists"></param>
        /// <returns></returns>
        protected override Expression VisitExists(ExistsExpression exists)
        {
            sb.Append("EXISTS(");
            this.AppendNewLine(Indentation.Inner);
            this.Visit(exists.Select);
            this.AppendNewLine(Indentation.Same);
            sb.Append(")");
            this.Indent(Indentation.Outer);
            return exists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="in"></param>
        /// <returns></returns>
        protected override Expression VisitIn(InExpression @in)
        {
            this.VisitValue(@in.Expression);
            sb.Append(" IN (");
            if (@in.Select != null)
            {
                this.AppendNewLine(Indentation.Inner);
                this.Visit(@in.Select);
                this.AppendNewLine(Indentation.Same);
                sb.Append(")");
                this.Indent(Indentation.Outer);
            }
            else if (@in.Values != null)
            {
                for (int i = 0, n = @in.Values.Count; i < n; i++)
                {
                    if (i > 0) sb.Append(", ");
                    this.VisitValue(@in.Values[i]);
                }
                sb.Append(")");
            }
            return @in;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override Expression VisitNamedValue(NamedValueExpression value)
        {
            sb.Append("@" + value.Name);
            return value;
        }
    }
}