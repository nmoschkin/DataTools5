// '
// ' Hugely useful, ultra-secret CSV Wrappers.
// ' Copyright (C) 2012-2015 Nathaniel Moschkin.
// ' All Rights Reserved.

// ' Commercial use prohibited without express authorization from Nathaniel Moschkin.

using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using DataTools.Text.ByteOrderMark;
using DataTools.Standard.Memory.StringBlob;
using DataTools.Observable;

namespace DataTools.Text.Csv
{
    
  
    /// <summary>
    /// Encapsulates an entire CSV file, including all headers and record data, loading and saving data, and sorting and searching data.
    /// </summary>
    /// <remarks></remarks>
    public class CsvWrapper : IEnumerable<CsvRow>, ICollection<CsvRow>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public delegate void CollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void NotifyPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

        internal string[] _Lines;
        internal CsvRow cols;

        
        public bool ImportCollection<T>(ICollection<T> col, ImportFlags flags = ImportFlags.Browsable | ImportFlags.Descriptions)
        {
            try
            {
                // ' will contain column names
                var scol = new List<string>();

                // ' collection of property info
                PropertyInfo[] mi;

                // ' valid properties will wind up here.
                var ml = new List<PropertyInfo>();

                // ' check for public and non public orientations
                if (0 != (flags & ImportFlags.NonPublic))
                {
                    mi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                }
                else
                {
                    mi = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                }

                bool dm;
                bool br;

                var lfin = new List<PropertyInfo>();

                string s;

                foreach (var x in mi)
                {
                    br = x.GetType().GetCustomAttribute(typeof(BrowsableAttribute)) != null;
                    dm = x.GetType().GetCustomAttribute(typeof(System.Runtime.Serialization.DataMemberAttribute)) != null;

                    // ' check for browsable requirement
                    // ' check for datamember requirement
                    if (((flags & ImportFlags.Browsable) == 0 || br == true) && ((flags & ImportFlags.DataMember) == 0 || dm == true))
                    {
                        ml.Add(x);
                    }
                }

                if (0 != (flags & ImportFlags.Descriptions))
                {

                    // ' with the Description directive, where this is no description, those columns are excluded.
                    // ' to include columns without a description directive, do not specify the Description flag.
                    foreach (var x in ml)
                    {

                        var da = (DescriptionAttribute)x.GetType().GetCustomAttribute(typeof(DescriptionAttribute));
                        if (da != null)
                        {
                            s = da.Description;
                            scol.Add(s);
                            lfin.Add(x);
                        }
                    }
                }
                else
                {
                    foreach (var x in ml)
                    {
                        scol.Add(x.Name);
                        lfin.Add(x);
                    }
                }

                // ' all set, let's make the data.
                Clear();

                cols = new CsvRow(this);
                cols.Columns = scol.ToArray();

                scol.Clear();

                object o;

                foreach (var rec in col)
                {
                    foreach (var p in lfin)
                    {
                        o = p.GetValue(rec);

                        if (o is object)
                        {
                            scol.Add(o.ToString());
                        }
                        else
                        {
                            scol.Add("");
                        }
                    }

                    Add(new CsvRow(this, scol.ToArray()));

                    scol.Clear();
                }
            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

        
        
        public string FileName { get; set; }

        /// <summary>
        /// Open the CSV document named in the Filename property.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool OpenDocument()
        {
            if (string.IsNullOrWhiteSpace(FileName) || File.Exists(FileName) == false)
                return false;

            return OpenDocument(FileName);
        }

        /// <summary>
        /// Open a CSV document.
        /// </summary>
        /// <param name="fileName">Full path of the file to read.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool OpenDocument(string fileName)
        {
            if (File.Exists(fileName) == false)
                return false;
            
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (OpenDocument(fs))
                    {
                        FileName = fileName;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return false;
        }

        /// <summary>
        /// Open a CSV document from a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool OpenDocument(Stream stream)
        {
            byte[] b;
            try
            {
                b = new byte[((int)stream.Length)];
                stream.Read(b, 0, b.Length);
                
                Lines = FixBadCsv(Encoding.UTF8.GetString(b)).Split("\n");
            }
            catch 
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save a CSV document using the current Filename property.
        /// </summary>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveDocument()
        {
            if (string.IsNullOrWhiteSpace(FileName))
                return false;

            return SaveDocument(FileName);
        }

        /// <summary>
        /// Save a CSV document to the specified file.
        /// </summary>
        /// <param name="fileName">Full path of the file to save.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveDocument(string fileName)
        {
            bool ret;

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                ret = SaveDocument(fs, false);

                if (ret)
                {
                    FileName = fileName;
                }
            }

            return ret;
        }

        /// <summary>
        /// Save a CSV document to a stream.
        /// </summary>
        /// <param name="stream">Stream to receive the document.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveDocument(Stream stream)
        {
            return SaveDocument(stream, true);
        }

        /// <summary>
        /// Save a CSV document to a stream.
        /// </summary>
        /// <param name="stream">Stream to receive the document.</param>
        /// <param name="closeStream">True to close the stream after writing.</param>
        /// <returns>True if successful.</returns>
        /// <remarks></remarks>
        public bool SaveDocument(Stream stream, bool closeStream)
        {
            try
            {
                var b = Encoding.UTF8.GetBytes(((StringBlob)Lines).ToFormattedString(StringBlobFormats.CrLf));

                stream.Write(b, 0, b.Length);

                if (closeStream)
                    stream.Close();
            }
            catch //(Exception ex)
            {
                return false;
            }

            return true;
        }

        
        
        /// <summary>
        /// Returns the names of all the columns in this csv document.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CsvRow ColumnNames
        {
            get => cols;
            set
            {
                cols = value;
                OnPropertyChanged(nameof(ColumnNames));

                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Gets or sets the text lines of a CSV file.
        /// For setting, column names must be present.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string[] Lines
        {
            get
            {
                var l = new List<string>();

                if (cols is object)
                {
                    l.Add(cols.Text);
                }

                l.AddRange(_Lines);
                return l.ToArray();
            }

            set
            {
                int i;
                int c = value.Length;

                _Lines = new string[c - 1];

                for (i = 1; i < c; i++)
                    _Lines[i - 1] = value[i];

                cols = value[0];

                OnPropertyChanged(nameof(Lines));
                OnPropertyChanged(nameof(ColumnNames));
            
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Add a raw CSV record to the CSV file.
        /// </summary>
        /// <param name="text"></param>
        /// <remarks></remarks>
        public void AddLine(string text)
        {
            text = text?.Replace("\r", "").Replace("\n", "").Trim() ?? "";

            if (_Lines is null)
            {
                _Lines = new string[1];
                _Lines[0] = text;

                return;
            }

            int c = _Lines.Count();
            Array.Resize(ref _Lines, c + 1);

            _Lines[c] = text;

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }


        /// <summary>
        /// Gets or sets a CsvRow object to the specified row index.
        /// </summary>
        /// <param name="index">Index at which to get or set the object.</param>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public CsvRow this[int index]
        {
            get
            {
                return new CsvRow(this, _Lines[index]);
            }

            set
            {
                var newitem = value.Text;
                var olditem = _Lines[index];

                if (newitem == olditem) return;

                _Lines[index] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, olditem, index));
            }
        }

        /// <summary>
        /// Returns the entire, formatted text string for a CSV file.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Text
        {
            get
            {
                return ((StringBlob)Lines).ToFormattedString(StringBlobFormats.CrLf);
            }

            set
            {
                Lines = FixBadCsv(value).Split("\n");

                OnPropertyChanged(nameof(Text));
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }


        protected string FixBadCsv(string value)
        {
            value = value.Replace("\r\n", "\n");
            Regex r = new Regex("\"\n(?!\")");
            return r.Replace(value, "\"");
        }

        /// <summary>
        /// Gets the maximum number of columns based on the current raw string record set.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected int MaxCols()
        {
            int cc = 0;
            foreach (CsvRow c in _Lines)
            {
                if (c.Count() > cc)
                    cc = c.Count();
            }

            if (cols.Count() > cc)
                cc = cols.Count();
            return cc;
        }

        /// <summary>
        /// Truncate the CSV record set to the specified index.
        /// </summary>
        /// <param name="lastIndex">Last item index to preserve.</param>
        /// <remarks></remarks>
        public void Truncate(int lastIndex)
        {
            var a = new List<string>();
            int i;
            lastIndex = Math.Min(lastIndex, _Lines.Count() - 1);
            var loopTo = lastIndex;
            for (i = 0; i <= loopTo; i++)
                a.Add(_Lines[i]);
            _Lines = a.ToArray();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
        }

        
        
        /// <summary>
        /// Add a CsvRow object to the CSV file.
        /// </summary>
        /// <param name="row"></param>
        /// <remarks></remarks>
        public void Add(CsvRow row)
        {
            AddLine(row.Text);
        }

        public void AddRow(CsvRow row) => Add(row);

        /// <summary>
        /// Copies the contents of this record set to an array of CsvRows
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index within the array at which to start the copying.</param>
        /// <remarks></remarks>
        public void CopyTo(CsvRow[] array, int arrayIndex)
        {
            int c = arrayIndex;
            int d = this.Count - 1;
            for (int i = 0, loopTo = d; i <= loopTo; i++)
            {
                array[c] = this[i];
                c += 1;
            }
        }

        /// <summary>
        /// Gets the number of rows in the current record set.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Count
        {
            get
            {
                return _Lines.Count();
            }
        }

        /// <summary>
        /// Gets a value indicating that the list is read-only.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Clear the entire record set, excluding column names.
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            _Lines = Array.Empty<string>();
        }


        /// <summary>
        /// Determines whether or not the CSV file contains the specified CsvRow object.
        /// The exact text of a rendered CSV record must match the exact text of the rendered item for this function to succeed.
        /// </summary>
        /// <param name="item">A CsvRow object to scan for.</param>
        /// <returns>True if the row is found.</returns>
        /// <remarks></remarks>
        public bool Contains(CsvRow item)
        {
            int i;
            int c = _Lines.Count() - 1;
            string t = item.Text;
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if ((_Lines[i] ?? "") == (t ?? ""))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove a CsvRow item from the current record set.
        /// The exact text of a rendered CSV record must match the exact text of the rendered item for this function to succeed.
        /// </summary>
        /// <param name="item">A CsvRow object to scan for.</param>
        /// <returns>True if a row was removed.</returns>
        /// <remarks></remarks>
        public bool Remove(CsvRow item)
        {
            int i;
            int c = _Lines.Count() - 1;
            bool b = false;
            var ln = new List<string>();
            var loopTo = c;
            for (i = 0; i <= loopTo; i++)
            {
                if ((item.Text ?? "") != (_Lines[i] ?? ""))
                {
                    ln.Add(_Lines[i]);
                }
                else
                {
                    b = true;
                }
            }

            _Lines = ln.ToArray();
            if (b)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return b;
        }

        
        
        /// <summary>
        /// Sort the entire record-set by a specific column.
        /// </summary>
        /// <param name="columnIndex">The index of the columns to sort by.</param>
        /// <param name="descending">Set to True to sort in descending order.</param>
        /// <param name="columnType"></param>
        /// <remarks></remarks>
        public void SortByColumn(int columnIndex, bool descending, ColumnType columnType)
        {
            var c = new RowComparer();
            if (columnType == ColumnType.None)
            {
                if (ColumnNames.ElementAtOrDefault(columnIndex).IndexOf("Price") != -1)
                    columnType = ColumnType.Number;
            }

            c.ColumnType = columnType;
            c.Descending = descending;
            
            var l = new List<string>();
            
            l.AddRange(_Lines);
            l.Sort(c);

            _Lines = l.ToArray();
        }

        /// <summary>
        /// Saves an index from a sort.
        /// </summary>
        /// <remarks></remarks>
        public class IndexSaverObject
        {
            public string Text { get; set; }
            public int Index { get; set; }

            internal IndexSaverObject()
            {
            }
        }

        /// <summary>
        /// Sorts the entire CSV record set by the specified column, in ascending order.
        /// To sort in another order, or in a method other than a string comparison, pass an IComparer object.
        /// </summary>
        /// <param name="columnIndex">The column index to sort by.</param>
        /// <param name="comparer">The optional IComparer object to use for comparisons.</param>
        /// <returns>True if successful.  False may indicate there are no records or the columnIndex value is greater than the greatest available column index for this record set or less than zero.</returns>
        /// <remarks></remarks>
        public bool SortByColumn(int columnIndex, IComparer<IndexSaverObject> comparer = null)
        {
            if (_Lines is null || _Lines.Count() == 0)
                return false;
            
            if (ColumnNames is null || columnIndex >= ColumnNames.Count() || columnIndex < 0)
                return false;

            var cl = new List<IndexSaverObject>();
            IndexSaverObject ll;
            int e;
            var loopTo = this.Count - 1;
            for (e = 0; e <= loopTo; e++)
            {
                ll = new IndexSaverObject();
                ll.Text = this[e].Columns[columnIndex];
                ll.Index = e;
                cl.Add(ll);
                ll = null;
            }

            if (comparer is null)
            {
                cl.Sort(new Comparison<IndexSaverObject>((x, y) => string.Compare(x.Text, y.Text)));
            }
            else
            {
                cl.Sort(comparer);
            }

            var lns = new List<string>();
            foreach (var cc in cl)
                lns.Add(_Lines[cc.Index]);
            _Lines = lns.ToArray();
            GC.Collect(0);
            return true;
        }

        /// <summary>
        /// Searches a sorted record set by columnIndex for the specified value using a binary search algorithm.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="columnIndex">The column to search.</param>
        /// <param name="noPreSort">Specifies that the binary searcher will not attempt to pre-sort the list by the specified column.</param>
        /// <param name="comparer">The optional IComparer object to use for comparisons during sort.</param>
        /// <returns>The index of the found record or -1 if no record was found.</returns>
        /// <remarks></remarks>
        public int BinarySearchByColumn(string value, int columnIndex, bool noPreSort = true, IComparer<IndexSaverObject> comparer = null)
        {
            if (this.Count <= 0)
                return -1;
            if (this.Count <= 4)
            {
                int x = 0;
                foreach (CsvRow r in this)
                {
                    if ((r.Columns[columnIndex] ?? "") == (value ?? ""))
                        return x;
                }
            }

            if (!noPreSort)
            {
                SortByColumn(columnIndex, comparer);
            }

            int count = this.Count;

            int currentIdx = (int)Math.Round(count / 2d);
            int startPos = 0;

            int endPos = count - 1;

            int valDiff;
            int nextCenter;

            string propVal;

            do
            {
                propVal = this[currentIdx].Columns[columnIndex];
                valDiff = string.Compare(value, propVal);
                if (valDiff == 0)
                {
                    return currentIdx;
                }

                if (valDiff < 0)
                {
                    endPos = currentIdx - 1;
                    if (endPos < startPos)
                        break;
                    nextCenter = (int)Math.Round((endPos - startPos) / 2d);
                    if (nextCenter == currentIdx)
                        break;
                    currentIdx = nextCenter;
                }
                else
                {
                    startPos = currentIdx + 1;
                    if (startPos >= count)
                        break;
                    nextCenter = (int)Math.Round(startPos + (endPos - startPos) / 2d);
                    if (nextCenter == currentIdx)
                        break;
                    currentIdx = nextCenter;
                }
            }
            while (true);

            return -1;
        }

        
        
        public IEnumerator<CsvRow> GetEnumerator()
        {
            return new CsvRowEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CsvRowEnumerator(this);
        }

        protected void OnPropertyChanged(string e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e));
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

    }


}

