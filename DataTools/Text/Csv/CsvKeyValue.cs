
using System.ComponentModel;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;

using DataTools.Observable;

namespace DataTools.Text.Csv
{

    public class CsvKeyValue : ObservableBase
    {
        private string key;
        private string value;
        
        private CsvRow row;

        private int index;
        private bool updateParentRow;

        /// <summary>
        /// Specify whether or not to update the row data of the row object
        /// that owns this key/value pair if the value is changed.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool UpdateParentRow
        {
            get => updateParentRow;
            set
            {
                SetProperty(ref updateParentRow, value);
            }
        }

        public CsvRow ParentRow
        {
            get => row;
            internal set
            {
                SetProperty(ref row, value);
            }
        }

        /// <summary>
        /// The index of this column.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Index
        {
            get => index;
            internal set
            {
                SetProperty(ref index, value);
            }
        }

        /// <summary>
        /// The name of this column.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Key
        {
            get => key;
            set
            {
                SetProperty(ref key, value);
            }
        }

        /// <summary>
        /// The value of this column.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Value
        {
            get => value;
            set
            {
                if (SetProperty(ref this.value, value))
                {
                    if (updateParentRow)
                    {
                        row.ColumnList[index] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Find the index of a key given its name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int FindKeyNumber(string key)
        {
            string[] s = row.Parent.ColumnNames;
            int i;

            int c = s.Count();

            for (i = 0; i < c; i++)
            {
                if ((key ?? "") == (s[i] ?? ""))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Find the name of a key given its index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FindKeyName(int index)
        {
            return row?.Parent?.ColumnNames?.ElementAtOrDefault(index);
        }

        /// <summary>
        /// Initialize a new Key/Value pair based on key name and value.
        /// 0 based column index will be determined automatically.
        /// </summary>
        /// <param name="key">Column name.</param>
        /// <param name="value">Value of column.</param>
        /// <remarks></remarks>
        public CsvKeyValue(string key, string value, CsvRow parentRow = null)
        {
            ParentRow = parentRow;

            Index = FindKeyNumber(key);
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Initialize a new Key/Value pair based on column index and value.
        /// Column name will be determined automatically.
        /// </summary>
        /// <param name="index">0 Based column index.</param>
        /// <param name="value">Value of column.</param>
        /// <remarks></remarks>
        public CsvKeyValue(int index, string value, CsvRow parentRow = null)
        {
            ParentRow = parentRow;

            Key = FindKeyNumber(index.ToString()).ToString();
            Index = index;
            Value = value;
        }

    }

}
