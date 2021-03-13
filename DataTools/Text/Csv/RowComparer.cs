using System;
using System.Collections.Generic;
using System.Text;

namespace DataTools.Text.Csv
{

    public class RowComparer : IComparer<string>
    {
        public int CompareColumn { get; set; } = 0;
        public ColumnType ColumnType { get; set; }
        public bool Descending { get; set; } = false;

        public int Compare(string x, string y)
        {
            int ret;
            var xc = TextTools.Split(x, ",", true, true, '"', '"', true);
            var yc = TextTools.Split(y, ",", true, true, '"', '"', true);

            if (xc is null && yc is null)
                return 0;

            string xcmp;
            string ycmp;

            if (xc is null)
                xc = new[] { "" };

            if (yc is null)
                yc = new[] { "" };

            int cc = CompareColumn < xc.Length && CompareColumn < yc.Length ? CompareColumn : 0;

            xcmp = xc[cc].Trim();
            ycmp = yc[cc].Trim();

            var ct = ColumnType;

            if (ct == ColumnType.None)
            {
                if (TextTools.IsNumber(xc[cc]) && TextTools.IsNumber(yc[cc]))
                {
                    ct = ColumnType.Number;
                }
                else
                {
                    ct = ColumnType.Text;
                }
            }

            if (ct == ColumnType.Number)
            {
                try
                {
                    string[] argvalues = null;
                    xcmp = TextTools.JustNumbers(xcmp, values: ref argvalues);

                    string[] argvalues1 = null;
                    ycmp = TextTools.JustNumbers(ycmp, values: ref argvalues1);

                    double xd = double.Parse(xcmp, System.Globalization.NumberStyles.Any);
                    double yd = double.Parse(ycmp, System.Globalization.NumberStyles.Any);

                    if (xd > yd)
                    {
                        ret = 1;
                    }
                    else if (yd > xd)
                    {
                        ret = -1;
                    }
                    else
                    {
                        ret = 0;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                ret = string.Compare(xcmp, ycmp);
            }

            if (Descending)
                ret = -ret;

            return ret;
        }
    }
}
