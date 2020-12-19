using DataTools.Text;
using System.Collections;
using System.Collections.ObjectModel;

using System;
using System.Runtime.InteropServices;

namespace DataTools.MathTools
{
    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    public class MathExpressionParser
    {
        private string m_Expression = "";
        private Collection<object> m_Col = new Collection<object>();
        private string m_Error = "";
        private double m_Value = 0.0d;

        public double Value
        {
            get
            {
                return m_Value;
            }
        }

        public string ErrorText
        {
            get
            {
                return m_Error;
            }

            internal set
            {
                m_Error = value;
            }
        }

        public string Expression
        {
            get
            {
                return m_Expression;
            }

            set
            {
                m_Expression = value;
            }
        }

        public Collection<object> Operations
        {
            get
            {
                return m_Col;
            }
        }

        public bool ParseOperations(string value = null)
        {
            string[] st = null;
            int i;
            int c;
            int n = 0;
            int b = 0;
            string sLeft = "";
            string sFar = "";
            MathExpressionParser objOp = null;
            if (value == null)
            {
                value = m_Expression;
            }

            value = TextTools.OneSpace(TextTools.SpaceOperators(value.Trim()).ToLower());
            m_Expression = value;
            c = 0;
            i = value.IndexOf("(");
            if (i == -1)
            {
                i = value.IndexOf(")");
                if (i != -1)
                {
                    m_Error = "Unexpected closing parenthesis at column " + i;
                    return false;
                }

                m_Value = ParseExpression(value, ref m_Error);
                if (m_Value == default)
                    return false;
                return true;
            }

            while (c < value.Length)
            {
                i = value.IndexOf("(", c);
                if (i == -1)
                {
                    Array.Resize(ref st, n + 1);
                    st[n] = "@" + value.Substring(c);
                    n += 1;
                    break;
                }

                Array.Resize(ref st, n + 1);
                if (i - c > 0)
                {
                    st[n] = "@" + value.Substring(c, i - c);
                    n += 1;
                }

                sFar = TextTools.Bracket(value, ref i, ref c);
                Array.Resize(ref st, n + 1);
                st[n] = sFar;
                n += 1;
                i = value.IndexOf("(", c);
            }

            n -= 1;
            sFar = "";
            m_Col.Clear();
            var loopTo = n;
            for (i = 0; i <= loopTo; i++)
            {
                ErrorText = "";
                if (st[i].Substring(0, 1) == "@")
                {
                    sFar += st[i].Substring(1);
                    objOp = new MathExpressionParser();
                    string argErrorText = null;
                    objOp.ParseExpression(sFar, ErrorText: ref argErrorText);
                    m_Col.Add(objOp);
                }
                else
                {
                    objOp = new MathExpressionParser();
                    objOp.ParseOperations(st[i]);
                    if (!string.IsNullOrEmpty(objOp.ErrorText))
                    {
                        ErrorText = objOp.ErrorText;
                        return false;
                    }

                    m_Col.Add(objOp);
                    sFar += " " + objOp.Value;
                }
            }

            sFar = TextTools.OneSpace(TextTools.SpaceOperators(sFar.Trim()).ToLower());
            double localParseExpression() { string argErrorText = ErrorText; var ret = ParseExpression(sFar, ref argErrorText); ErrorText = argErrorText; return ret; }

            m_Value = localParseExpression();
            if (!string.IsNullOrEmpty(ErrorText))
                return false;
            return true;
        }

        public double ParseExpression(string value, [Optional, DefaultParameterValue(null)] ref string ErrorText)
        {
            string[] s;
            double dOut = 0.0d;
            int i = 0;
            int j = 0;
            int c;
            int cc = 0;
            int n = 0;
            int m = 0;
            double? valUnit;
            long a;
            long b;
            double d;
            double e;
            object[] ops = null;
            object[] ops2;

            // functions come first!
            var orderOps = new[] { "log", "log10", "sin", "cos", "tan", "asin", "acos", "atan", "^", "exp", "abs", "sqrt", "*", "/", @"\", "mod", "%", "+", "-" };
            s = TextTools.Words(TextTools.OneSpace(TextTools.SpaceOperators(value).Trim()));
            ops2 = new object[s.Length * 2 + 1];
            ops = new object[s.Length * 2 + 1];
            c = s.Length - 1;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                valUnit = TextTools.FVal(s[i]);
                if (valUnit is null)
                {
                    if (i == 0)
                    {
                        switch (s[i] ?? "")
                        {
                            case "abs":
                            case "sqrt":
                            case "log":
                            case "log10":
                            case "sin":
                            case "cos":
                            case "tan":
                            case "asin":
                            case "acos":
                            case "atan":
                                {
                                    break;
                                }

                            default:
                                {
                                    ErrorText = "Unexpected identifier '" + s[i] + "' at column " + (cc + 1);
                                    return default;
                                }
                        }
                    }

                    switch (s[i] ?? "")
                    {
                        case "^":
                        case "exp":
                        case "abs":
                        case "sqrt":
                        case "*":
                        case "/":
                        case @"\":
                        case "mod":
                        case "%":
                        case "+":
                        case "-":
                        case "log":
                        case "log10":
                        case "sin":
                        case "cos":
                        case "tan":
                        case "asin":
                        case "acos":
                        case "atan":
                            {
                                ops[n] = s[i];
                                n += 1;
                                break;
                            }

                        default:
                            {
                                ErrorText = "Unexpected identifier '" + s[i] + "' at column " + (cc + 1);
                                return default;
                            }
                    }
                }
                else
                {
                    if (s[i].Substring(0, 1) == "+")
                    {
                        ops[n] = "un";
                        n += 1;
                    }

                    ops[n] = valUnit;
                    n += 1;
                }

                // just to keep track of the character position in case there's an error
                cc += s[i].Length;
            }

            // Now, perform the operations!

            c = orderOps.Length - 1;
            n -= 1;
            try
            {
                m = 0;
                var loopTo1 = n;
                for (j = 0; j <= loopTo1; j++)
                {
                    if (j + 3 <= n)
                    {
                        if (TextTools.IsNumber(ops[j]) & TextTools.IsNumber(ops[j + 1]))
                        {
                            if (ops[j + 2].ToString() == "/" & TextTools.IsNumber(ops[j + 3]))
                            {
                                ops2[m] = (double)(ops[j]) + (double)(ops[j + 1]) / (double)(ops[j + 3]);
                                m += 1;
                                j += 3;
                            }

                            continue;
                        }
                    }

                    Array.Resize(ref ops2, m);
                    ops2[m] = ops[j];
                    m += 1;
                }

                ops = ops2;
                ops2 = null;
                n = m - 1;
                var loopTo2 = c;
                for (i = 0; i <= loopTo2; i++)
                {
                    var loopTo3 = n;
                    for (j = 0; j <= loopTo3; j++)
                    {
                        if (!(ops[j] is string))
                        {
                            ops2[m] = ops[j];
                            m += 1;
                            continue;
                        }

                        if ((ops[j].ToString() ?? "") != (orderOps[i] ?? ""))
                        {
                            ops2[m] = ops[j];
                            m += 1;
                            continue;
                        }

                        switch (orderOps[i] ?? "")
                        {
                            case "^":
                            case "exp":
                                {
                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = (double)(ops[j + 1]);
                                    if (d < 0.0d)
                                    {
                                        d *= -1;
                                        d = Math.Pow(d, e);
                                        d *= -1;
                                    }
                                    else if (j - 1 > 0)
                                    {
                                        if (ops[j - 2].ToString() == "un")
                                        {
                                            d = Math.Pow(d, e);
                                            d = Math.Abs(d);
                                            m -= 1;
                                        }
                                        else
                                        {
                                            d = Math.Pow(d, e);
                                        }
                                    }
                                    else
                                    {
                                        d = Math.Pow(d, e);
                                    }

                                    ops2[m] = d;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "abs":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = Math.Abs((double)(ops[j + 1]));
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "sqrt":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = Math.Sqrt((double)(ops[j + 1]));
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "*":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = (double)(ops[j + 1]);
                                    ops2[m] = d * e;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "/":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = (double)(ops[j + 1]);
                                    ops2[m] = d / e;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case @"\":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    a = (long)(ops[j - 1]);
                                    b = (long)(ops[j + 1]);
                                    ops2[m] = a / b;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "mod":
                            case "%":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = j + 1;
                                    ops2[m] = d % e;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "+":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = j + 1;
                                    ops2[m] = d + e;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "-":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == 0)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    m -= 1;
                                    d = (double)(ops[j - 1]);
                                    e = (double)(ops[j + 1]);
                                    ops2[m] = d - e;
                                    ops[j + 1] = ops2[m];
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "log":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Log(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "log10":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Log10(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "sin":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Sin(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "cos":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Cos(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "tan":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Tan(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "asin":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Asin(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "acos":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Acos(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }

                            case "atan":
                                {
                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    if (j == n)
                                    {
                                        ErrorText = "Unexpected identifier '" + s[i];
                                        return default;
                                    }

                                    d = (double)(ops[j + 1]);
                                    d = Math.Atan(d);
                                    ops2[m] = d;
                                    m += 1;
                                    j += 1;
                                    continue;
                                }
                        }
                    }

                    Array.Resize(ref ops2, m);
                    ops = ops2;
                    n = ops2.Length - 1;
                    m = 0;
                    ops2 = null;
                }

                // numbers that occur together add

                n = ops.Length - 1;
                e = 0.0d;
                var loopTo4 = n;
                for (j = 0; j <= loopTo4; j++)
                {
                    if (TextTools.IsNumber(ops[j]))
                        e += (double)(ops[j]);
                }
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
                return default;
            }

            m_Error = "";
            m_Value = e;
            return e;
        }
    }
}

/* TODO ERROR: Skipped EndRegionDirectiveTrivia */