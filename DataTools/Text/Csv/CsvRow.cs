
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataTools.Observable;

namespace DataTools.Text.Csv
{
    public class CsvRow : ObservableBase, ICsvRow, IEnumerable<string>, ICloneable, IComparable<string>, IComparable<CsvRow>
    {
        protected List<string> fields = new List<string>();
        protected CsvWrapper parent;

        /// <summary>
        /// Gets the parent CSV file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CsvWrapper Parent
        {
            get => parent;
            internal set
            {
                SetProperty(ref parent, value);
            }
        }

        /// <summary>
        /// Get the CSV-formatted text string for the entire row.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Text
        {
            get
            {
                var sb = new StringBuilder();
                int c = 0;

                foreach (var f in fields)
                {
                    var s = f;

                    if (c > 0)
                        sb.Append(",");

                    sb.Append("\"");

                    s = s.Replace("\"", "\"\"");

                    sb.Append(s);
                    sb.Append("\"");

                    c++;
                }

                return sb.ToString();
            }
            set
            {
                fields.Clear();

                if (string.IsNullOrWhiteSpace(value))
                    return;

                value = value.Replace("\n", "").Replace("\r", "");
                var sa = TextTools.Split(value, ",", true, true, '"', '"', true);

                int c = sa.Length;

                for (var i = 0; i < c; i++)
                {
                    sa[i] = sa[i].Replace("\t", " ").Trim();
                }

                fields.AddRange(sa);

                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Get all values for this record as a list of strings.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<string> ColumnList
        {
            get => fields;
            set
            {
                SetProperty(ref fields, value);
            }
        }

        /// <summary>
        /// Get all values for this record.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] Columns
        {
            get => fields.ToArray();
            set
            {
                fields.Clear();

                if (value != null && value.Length > 0)
                {
                    
                    foreach (var s in value)
                    {
                        fields.Add(s.Replace("\t", " ").Trim());
                    }
                }

                OnPropertyChanged(nameof(Columns));
            }
        }

        /// <summary>
        /// Get all values for this record.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] GetValues() => Columns;

        /// <summary>
        /// Set all values for this record.
        /// </summary>
        /// <param name="vals"></param>
        /// <remarks></remarks>
        public void SetValues(string[] vals)
        {
            Columns = vals;
        }

        /// <summary>
        /// Returns the column index from the given name.
        /// The CsvRow object must be associated with a parent CsvWrapper with a list of column names for this function to succeed.
        /// </summary>
        /// <param name="name">Case-insensitive name of the column to retrieve the index for.</param>
        /// <returns>Column index, or -1 if not found.</returns>
        /// <remarks></remarks>
        public int GetColumnIndex(string name)
        {

            CsvRow cn;

            if (parent is null)
                cn = this;
            else
                cn = parent.ColumnNames;

            int i = 0;
            string tv = name?.Trim().ToUpper() ?? "";

            foreach (var s in cn)
            {
                string av = (s?.Trim().ToUpper() ?? "");

                if (string.Compare(tv, av) == 0)
                {
                    return i;
                }

                i++;
            }

            return -1;
        }

        /// <summary>
        /// Gets or sets the value of the data in this row at the specified column matched by name.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns></returns>
        public string this[string name]
        {
            get
            {
                return fields[GetColumnIndex(name)];
            }
            set
            {
                var idx = GetColumnIndex(name);

                while (idx >= fields.Count)
                {
                    fields.Add("");
                }

                fields[idx] = value?.Replace("\t", " ").Trim() ?? "";
            }
        }

        /// <summary>
        /// Returns the value for the given column name.
        /// The CsvRow object must be associated with a parent CsvWrapper with a list of column names for this function to succeed.
        /// </summary>
        /// <param name="name">Case-insensitive name of the column to retrieve the data for.</param>
        /// <returns>Data, or null if not found.</returns>
        /// <remarks></remarks>
        public string GetColumnData(string name)
        {
            return this[name];
        }

        /// <summary>
        /// Get key/value pairs for all contents of this record.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public CsvKeyValue[] GetKeyValues()
        {
            int i;
            int c = fields.Count;

            var lout = new List<CsvKeyValue>();

            for (i = 0; i < c; i++)
                lout.Add(new CsvKeyValue(i, fields[i], this));

            return lout.ToArray();
        }

        /// <summary>
        /// Normalize the data to number of columns (or the number of columns in the parent)
        /// </summary>
        /// <param name="c">Optional number of columns.</param>
        /// <remarks></remarks>
        public void Normalize(int c = 0)
        {
            int d;
            if (c <= 0)
            {
                if (parent is object)
                {
                    c = parent.ColumnNames.Count();
                }
                else
                {
                    return;
                }
            }

            if (fields.Count < c)
            {
                d = fields.Count;
                while (d != c)
                {
                    fields.Add("");
                    d++;
                }
            }
            else
            {
                while (fields.Count != c)
                    fields.RemoveAt(fields.Count - 1);
            }
        }

        /// <summary>
        /// Create a new row.
        /// </summary>
        /// <remarks></remarks>
        public CsvRow()
        {
            fields = new List<string>();
        }

        /// <summary>
        /// Create a new row with the specified parent.
        /// </summary>
        /// <param name="parent">Parent CsvWrapper object.</param>
        /// <remarks></remarks>
        public CsvRow(CsvWrapper parent)
        {
            fields = new List<string>();
            this.parent = parent;
        }

        /// <summary>
        /// Create a new row with the specified raw CSV row text.
        /// </summary>
        /// <param name="text">Raw CSV row text.</param>
        /// <remarks></remarks>
        public CsvRow(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Create a new row with the specified parent and raw CSV row text.
        /// </summary>
        /// <param name="parent">The parent CsvWrapper object.</param>
        /// <param name="text">Raw CSV row text.</param>
        /// <remarks></remarks>
        public CsvRow(CsvWrapper parent, string text)
        {
            Text = text;
            this.parent = parent;

            Normalize();
        }

        /// <summary>
        /// Create a new row with the specified parent and column values.
        /// </summary>
        /// <param name="parent">The parent CsvWrapper object.</param>
        /// <param name="values">A list of string values to fill the content of columns.</param>
        /// <remarks></remarks>
        public CsvRow(CsvWrapper parent, params string[] values)
        {
            this.parent = parent;
            var l = new List<string>();
        
            foreach (var v in values)
                l.Add(v?.Replace("\t", " ").Trim() ?? "");
            
            fields = l;
            
            Normalize();
        }

        /// <summary>
        /// Create a new row with the specified column values.
        /// </summary>
        /// <param name="values">A list of string values to fill the content of columns.</param>
        /// <remarks></remarks>
        public CsvRow(params string[] values)
        {
            var l = new List<string>();

            foreach (var v in values)
                l.Add(v?.Replace("\t", " ").Trim() ?? "");

            fields = l;

            Normalize();
        }

        public object Clone() => Clone(parent);
   
        public CsvRow Clone(CsvWrapper parent)
        {
            var r = new CsvRow();

            foreach (var s in fields)
                r.fields.Add(s);

            r.parent = parent;

            return r;
        }


        public IEnumerator<string> GetEnumerator()
        {
            return new StringEnumerator(fields);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new StringEnumerator(fields);
        }

        public override string ToString()
        {
            return Text;
        }

        public override int GetHashCode()
        {
            return (int)Streams.Crc32.Calculate(Encoding.UTF8.GetBytes(Text));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (obj.GetType() == typeof(string))
            {
                return Text.Equals((string)obj);
            }

            if (obj.GetType() != this.GetType()) return false;

            var fields2 = ((CsvRow)obj).fields;

            if (fields2.Count != fields.Count) return false;

            int i, c = fields.Count;

            for (i = 0; i < c; i++)
            {
                if (!fields[i].Equals(fields2[i])) return false;
            }

            return true;
        }

        public int CompareTo(CsvRow other)
        {
            var c = fields.Count;

            if (other.fields.Count != c)
            {
                return c - other.fields.Count;
            }

            for (int i = 0; i < c; i++)
            {
                int d = string.Compare(fields[i], other.fields[i]);
                if (d != 0) return d;
            }

            return 0;
        }

        public int CompareTo(string other)
        {
            return string.Compare(this, other);
        }

        public static bool operator ==(CsvRow val1, string val2)
        {
            if (!(val1 is object) && !(val2 is object)) return true;
            if (!(val1 is object) && (val2 is object)) return false;
            if ((val1 is object) && !(val2 is object)) return false;


            return val1.Equals(val2);
        }

        public static bool operator !=(CsvRow val1, string val2)
        {
            if (!(val1 is object) && !(val2 is object)) return false;
            if (!(val1 is object) && (val2 is object)) return true;
            if ((val1 is object) && !(val2 is object)) return true;

            return !val1.Equals(val2);
        }

        public static bool operator ==(string val1, CsvRow val2)
        {
            if (!(val1 is object) && !(val2 is object)) return true;
            if (!(val1 is object) && (val2 is object)) return false;
            if ((val1 is object) && !(val2 is object)) return false;


            return val1.Equals(val2);
        }

        public static bool operator !=(string val1, CsvRow val2)
        {
            if (!(val1 is object) && !(val2 is object)) return false;
            if (!(val1 is object) && (val2 is object)) return true;
            if ((val1 is object) && !(val2 is object)) return true;

            return !val1.Equals(val2);
        }


        public static bool operator ==(CsvRow val1, CsvRow val2)
        {
            if (!(val1 is object) && !(val2 is object)) return true;
            if (!(val1 is object) && (val2 is object)) return false;
            if ((val1 is object) && !(val2 is object)) return false;


            return val1.Equals(val2);
        }

        public static bool operator !=(CsvRow val1, CsvRow val2)
        {
            if (!(val1 is object) && !(val2 is object)) return false;
            if (!(val1 is object) && (val2 is object)) return true;
            if ((val1 is object) && !(val2 is object)) return true;

            return !val1.Equals(val2);
        }


        public static implicit operator CsvRow(string operand)
        {
            return new CsvRow(operand);
        }

        public static implicit operator string(CsvRow operand)
        {
            return operand.Text;
        }

        public static implicit operator string[](CsvRow operand)
        {
            return operand.GetValues();
        }

        public static implicit operator CsvRow(string[] operand)
        {
            var c = new CsvRow();
            c.fields.AddRange(operand);
            return c;
        }

    }




}
