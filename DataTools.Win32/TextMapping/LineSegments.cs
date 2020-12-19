using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataTools.Shell.TextMapping
{

    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Line-segment sorting options.
    /// </summary>
    /// <remarks></remarks>
    public enum LineSegmentCompareTypes
    {
        /// <summary>
        /// Sorts by line number (Y coordinate)
        /// </summary>
        /// <remarks></remarks>
        ByIndex,

        /// <summary>
        /// Sorts by Origin (Left X coordinate)
        /// </summary>
        /// <remarks></remarks>
        ByOrigin,

        /// <summary>
        /// Sorts by length of segment.
        /// </summary>
        /// <remarks></remarks>
        ByLength,

        /// <summary>
        /// Sorts by EndPoint (Right X coordinate)
        /// </summary>
        /// <remarks></remarks>
        ByEndPoint
    }

    /// <summary>
    /// Describes a line segment.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct LineSegment
    {

        /// <summary>
        /// The X-Origin of the line.
        /// </summary>
        /// <remarks></remarks>
        public int Origin;

        /// <summary>
        /// The length of the line.
        /// </summary>
        /// <remarks></remarks>
        public int Length;

        /// <summary>
        /// The index of the line.
        /// </summary>
        /// <remarks></remarks>
        public int Index;

        public int X1
        {
            get
            {
                return Origin;
            }
        }

        public int EndPoint
        {
            get
            {
                return Origin + Length - 1;
            }
        }

        public LineSegment(int Origin, int length, int index = 0)
        {
            this.Origin = Origin;
            Length = length;
            Index = index;
        }

        public override string ToString()
        {
            return "ORIGIN=" + Origin + ", LENGTH=" + Length + (Index != 0 ? ", LINE INDEX=" + Index : "");
        }
    }

    public class LineSegmentsComparer : IComparer<LineSegment>
    {

        /// <summary>
        /// Specifies whether to sort in descending order.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Descending { get; set; }
        public LineSegmentCompareTypes SortType { get; set; }

        /// <summary>
        /// Compare two line segments according to the configured criteria.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Compare(LineSegment x, LineSegment y)
        {
            int c = 0;
            switch (SortType)
            {
                case LineSegmentCompareTypes.ByIndex:
                    {
                        c = x.Index - y.Index;
                        break;
                    }

                case LineSegmentCompareTypes.ByLength:
                    {
                        c = x.Length - y.Length;
                        break;
                    }

                case LineSegmentCompareTypes.ByOrigin:
                    {
                        c = x.Origin - y.Origin;
                        break;
                    }

                case LineSegmentCompareTypes.ByEndPoint:
                    {
                        c = x.EndPoint - y.EndPoint;
                        break;
                    }
            }

            if (Descending)
                c = -c;
            return c;
        }

        /// <summary>
        /// Creats a new LineSegmentComparer
        /// </summary>
        /// <param name="descending">Whether to sort descending</param>
        /// <remarks></remarks>
        public LineSegmentsComparer(bool descending = false, LineSegmentCompareTypes sortType = LineSegmentCompareTypes.ByIndex)
        {
            Descending = descending;
            SortType = sortType;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Describes a region of the page in terms of a collection of line segments.
    /// </summary>
    /// <remarks></remarks>
    public class LineSegments : System.Collections.ObjectModel.Collection<LineSegment>
    {
        private bool _isContiguous = false;
        private double _Area = double.NaN;
        private int _lct = 140;
        private int _oct = 100;

        /// <summary>
        /// Retrieve the calculated bounding rectangle from the specified configuration options.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Rectangle Bounds
        {
            get
            {
                int y = Count - 1;
                if (y < 1)
                    return default;
                return new Rectangle(MaxOrigin, this[0].Index, MinEndPoint - MaxOrigin, y);
            }
        }

        /// <summary>
        /// Gets the area of the rectangle.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Area
        {
            get
            {
                if (double.IsNaN(_Area))
                {
                    _Area = (MinEndPoint - MaxOrigin) * Count;
                }

                return _Area;
            }

            internal set
            {
                _Area = value;
            }
        }

        /// <summary>
        /// The maxmimum distance in length between two lines that could trigger a discontinuity.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int LengthContinuityThreshold
        {
            get
            {
                return _lct;
            }

            set
            {
                _lct = value;
                TestContiguous();
            }
        }

        /// <summary>
        /// The maxmimum distance in origin between two lines that could trigger a discontinuity.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int OriginContinuityThreshold
        {
            get
            {
                return _oct;
            }

            set
            {
                _oct = value;
                TestContiguous();
            }
        }

        /// <summary>
        /// Returns the minimum screen coordinate value for the right-hand boundary.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MinEndPoint
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.EndPoint < x || x == -1)
                    {
                        x = p.EndPoint;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the maximum screen coordinate value for the right-hand boundary.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MaxEndPoint
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.EndPoint > x || x == -1)
                    {
                        x = p.EndPoint;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the average EndPoint
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double AverageEndPoint
        {
            get
            {
                return (MaxEndPoint + MinEndPoint) / 2d;
            }
        }

        /// <summary>
        /// Returns the median EndPoint, where half the values are greater, and half the values are less.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegment MedianEndPoint
        {
            get
            {
                if (Count == 0)
                    return default;
                else
                {
                    if (Count < 3)
                        return this[0];
                }
                var l = base.Items;

                var xl = new List<LineSegment>(l.ToArray());
                
                xl.Sort((new LineSegmentsComparer(false, LineSegmentCompareTypes.ByEndPoint)));

                int c = xl.Count;

                if ((c & 1) == 1)
                    c += 1;

                c = (int)(c / 2d) - 1;

                return xl[c];
            }
        }

        /// <summary>
        /// Returns the minimum x-coordinate Origin of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MinOrigin
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.Origin < x || x == -1)
                    {
                        x = p.Origin;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the maximum x-coordinate Origin of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MaxOrigin
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.Origin > x || x == -1)
                    {
                        x = p.Origin;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the average X-coordinate Origin of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float AverageOrigin
        {
            get
            {
                float d = 0.0f;
                foreach (var p in this)
                    d += p.Origin;
                d /= Count;
                return d;
            }
        }

        /// <summary>
        /// Returns the median Origin, where half the values are greater, and half the values are less.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegment MedianOrigin
        {
            get
            {
                if (Count == 0)
                    return default;
                else
                {
                    if (Count < 3)
                        return this[0];
                }

                var l = new List<LineSegment>(base.Items);

                l.Sort(new LineSegmentsComparer(false, LineSegmentCompareTypes.ByOrigin));
                int c = l.Count;
                if ((c & 1) == 1)
                    c += 1;
                c = (int)(c / 2d) - 1;
                return l[c];
            }
        }

        /// <summary>
        /// Returns the minimum x-coordinate length of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MinLength
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.Length < x || x == -1)
                    {
                        x = p.Length;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the maximum x-coordinate length of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MaxLength
        {
            get
            {
                int x = -1;
                if (Count == 0)
                    return -1;
                foreach (var p in this)
                {
                    if (p.Length > x || x == -1)
                    {
                        x = p.Length;
                    }
                }

                return x;
            }
        }

        /// <summary>
        /// Returns the average X-coordinate length of a set of lines.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public float AverageLength
        {
            get
            {
                float d = 0.0f;
                foreach (var p in this)
                    d += p.Length;
                d /= Count;
                return d;
            }
        }

        /// <summary>
        /// Returns the median length, where half the values are greater, and half the values are less.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegment MedianLength
        {
            get
            {
                if (Count == 0)
                    return default;
                else
                {
                    if (Count < 3)
                        return this[0];
                }

                var l = new List<LineSegment>(base.Items);
                l.Sort(new LineSegmentsComparer(false, LineSegmentCompareTypes.ByLength));
                int c = l.Count;
                if ((c & 1) == 1)
                    c += 1;
                c = (int)(c / 2d) - 1;
                return l[c];
            }
        }

        /// <summary>
        /// Returns true of this data set represents a single continguous region.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsContiguous
        {
            get
            {
                return _isContiguous;
            }
        }

        /// <summary>
        /// test the collection for linear continuity.
        /// Normally, this should not need to be called by the consumer, since continuity is tested whenever the colleciton is modified.
        /// </summary>
        /// <param name="startFrom"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool TestContiguous(int startFrom = 0)
        {
            if (Count == 0)
                return false;
            else
            {
                if (Count == 1)
                    return true;
            }

            int idx = this[startFrom].Index;
            int i = startFrom;

            int c = Count - 2;

            if (startFrom != 0)
                startFrom -= 1;

            for (i = startFrom; i <= c; i++)
            {
                if (this[i].Index != this[i + 1].Index - 1 || Math.Abs(this[i].EndPoint - this[i + 1].EndPoint) >= _lct || Math.Abs(this[i].Origin - this[i + 1].Origin) >= _oct)

                {
                    _isContiguous = false;
                    return false;
                }
            }

            _isContiguous = true;
            return true;
        }

        /// <summary>
        /// Returns an array of all contiguous segments in a non-contiguous collection.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegments[] FindAllContiguousRegions()
        {
            var l = new List<LineSegments>();
            LineSegments pps;
            int y = 0;
            do
            {
                try
                {
                    pps = GetContiguousRegion(y, ref y);
                    if (pps is object)
                        l.Add(pps);
                    y += 1;
                }
                catch 
                {
                    break;
                }
            }
            while (true);
            return l.ToArray();
        }

        /// <summary>
        /// Gets a contiguous region starting at the specified Y coordinate.
        /// </summary>
        /// <param name="startY">The starting Y coordinate.</param>
        /// <param name="stopY">Receives the stopping Y coordinate.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegments GetContiguousRegion(int startY, [Optional, DefaultParameterValue(0)] ref int stopY)
        {
            int i = 0;
            int c = Count;
            bool inCont = false;
            var newPairs = new LineSegments();
            LineSegment pp;
            int y = startY;
            int len = 0;
            int org = 0;
            newPairs._isContiguous = true;
            if (startY > c || startY < 0)
                throw new ArgumentOutOfRangeException("startY");
            do
            {
                pp = this[i];
                if (!inCont)
                {
                    if (pp.Index >= startY)
                    {
                        inCont = true;
                        newPairs.Add(pp);
                        y = pp.Index;
                        len = pp.EndPoint;
                        org = pp.Origin;
                    }
                }
                else if ((pp.Index == y + 1 || pp.Index == y) && Math.Abs(pp.EndPoint - len) < _lct && Math.Abs(pp.Origin - org) < _oct)
                {
                    y = pp.Index;
                    len = pp.EndPoint;
                    org = pp.Origin;
                    newPairs.Add(pp);
                }
                else
                {
                    break;
                }

                i += 1;
            }
            while (i != c);
            stopY = y;
            return newPairs;
        }

        /// <summary>
        /// Finds a set of lines who all meet the specified criteria.
        /// </summary>
        /// <param name="containsX1">The line must contain the first x coordinate.</param>
        /// <param name="containsX2">The line must contain the second x coordinate.</param>
        /// <param name="startY">Optional starting line.</param>
        /// <param name="makeContiguous">If set to true, all items will have the same X1 and X2 and the collection will be marked contiguous.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegments FindSet(int containsX1, int containsX2, int startY = 0, bool makeContiguous = false, bool reverseScan = false)
        {
            int i;
            int c = Count;
            LineSegment pp;
            bool inCont = false;
            int startIdx = -1;
            int stopIdx = -1;

            // Dim tCol As New List(Of LineSegment)

            // For i = 0 To Me.Count - 1
            // tCol.Add(MergeSegments(GetLine(Me(i).Index)))
            // Next
            var tcol = this;
            c = tcol.Count;
            if (c < 2)
                return null;
            if (reverseScan)
            {
                i = c - 1;
                do
                {
                    pp = tcol[i];
                    if (inCont == false)
                    {
                        if (pp.Index <= startY)
                        {
                            if (pp.Origin <= containsX1 && pp.EndPoint >= containsX1 && pp.EndPoint >= containsX2 && pp.Origin <= containsX2)
                            {
                                inCont = true;
                                startIdx = i;
                                stopIdx = i;
                            }
                        }
                    }
                    else if (pp.Origin <= containsX1 && pp.EndPoint >= containsX1 && pp.EndPoint >= containsX2 && pp.Origin <= containsX2)
                    {
                        stopIdx = i;
                    }
                    else
                    {
                        break;
                    }

                    i -= 1;
                }
                while (i != -1);
                i = stopIdx;
                stopIdx = startIdx;
                startIdx = i;
            }
            else
            {
                i = 0;
                do
                {
                    pp = tcol[i];
                    if (inCont == false)
                    {
                        if (pp.Index >= startY)
                        {
                            if (pp.Origin <= containsX1 && pp.EndPoint >= containsX1 && pp.EndPoint >= containsX2 && pp.Origin <= containsX2)
                            {
                                inCont = true;
                                startIdx = i;
                                stopIdx = i;
                            }
                        }
                    }
                    else if (pp.Origin <= containsX1 && pp.EndPoint >= containsX1 && pp.EndPoint >= containsX2 && pp.Origin <= containsX2)
                    {
                        stopIdx = i;
                    }
                    else
                    {
                        break;
                    }

                    i += 1;
                }
                while (i != c);
            }

            if (stopIdx >= 0 && startIdx >= 0 && stopIdx != startIdx)
            {
                var p = new LineSegments();

                for (i = startIdx; i <= stopIdx; i++)
                {
                    pp = tcol[i];
                    if (makeContiguous)
                    {
                        pp.Origin = containsX1;
                        pp.Length = containsX2 - pp.Origin;
                    }

                    p.Add(pp);
                }

                if (makeContiguous)
                    p._isContiguous = true;
                return p;
            }

            return null;
        }

        /// <summary>
        /// Sort the collection according the the specified criteria.
        /// </summary>
        /// <param name="descending">Order descending.</param>
        /// <param name="sortType">Ordering mode.</param>
        /// <remarks></remarks>
        public void Sort(bool descending = false, LineSegmentCompareTypes sortType = LineSegmentCompareTypes.ByIndex)
        {
            var l = this.ToList();
            l.Sort(new LineSegmentsComparer(descending, sortType));
            int i = 0;
            int c = Count;
            if (c == 0)
                return;
            do
            {
                this[i] = l[i];
                i += 1;
            }
            while (i != c);
            return;
        }

        /// <summary>
        /// Returns all positions in a line.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private LineSegment[] GetLine(int y, bool sortFirst = false)
        {
            var l = this;
            var lOut = new List<LineSegment>();
            if (sortFirst)
                l.Sort();
            int i;
            int c = l.Count;

            for (i = 0; i < c; i++)
            {
                if (l[i].Index == y)
                {
                    while (l[i].Index == y)
                    {
                        lOut.Add(l[i]);

                        i += 1;

                        if (i >= c)
                            break;
                    }

                    return lOut.ToArray();
                }
            }

            return null;
        }

        public static LineSegments MergeSegments(LineSegments ls1, LineSegments ls2)
        {
            var n = new LineSegments();
            foreach (var l in ls1)
                n.Add(l);
            foreach (var l in ls2)
                n.Add(l);
            n.Sort();
            return n;
        }

        private LineSegment MergeSegments(LineSegment[] segments)
        {
            var l = new LineSegment();
            int i;
            int c = segments.Count();
            int o = -1;
            int ep = -1;

            for (i = 0; i < c; i++)
            {
                if (segments[i].Origin < o || o == -1)
                {
                    o = segments[i].Origin;
                }

                if (segments[i].EndPoint > ep || ep == -1)
                {
                    ep = segments[i].EndPoint;
                }
            }

            l.Origin = o;
            l.Length = ep - o;
            l.Index = segments[0].Index;
            return l;
        }

        protected override void InsertItem(int index, LineSegment item)
        {
            base.InsertItem(index, item);
            _Area = double.NaN;
            TestContiguous(_isContiguous ? index : 0);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _Area = double.NaN;
            TestContiguous();
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            _Area = double.NaN;
            _isContiguous = false;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
}