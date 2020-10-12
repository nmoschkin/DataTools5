using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DataTools.Desktop.Unified;
using DataTools.MathTools;
using DataTools.Desktop.Structures;


namespace DataTools.Shell.TextMapping
{
    public class CalculatedEventArgs : EventArgs
    {
        private DateTime _Start;

        public void StartTimer()
        {
            _Start = DateTime.Now;
        }

        public void StopTimer()
        {
            _Stop = DateTime.Now;
        }

        public TimeSpan Elapsed
        {
            get
            {
                return _Stop - _Start;
            }
        }

        public DateTime Start
        {
            get
            {
                return _Start;
            }
        }

        private DateTime _Stop;

        public DateTime Stop
        {
            get
            {
                return _Stop;
            }

            internal set
            {
                _Stop = value;
            }
        }

        private List<UniRect> _Regions;

        public List<UniRect> Regions
        {
            get
            {
                return _Regions;
            }

            internal set
            {
                _Regions = value;
            }
        }

        private int _TotalCalculations;

        public int TotalCalculations
        {
            get
            {
                return _TotalCalculations;
            }

            internal set
            {
                _TotalCalculations = value;
            }
        }

        internal CalculatedEventArgs(List<UniRect> r, int t, DateTime b, DateTime e)
        {
            _Start = b == DateTime.Parse("1901-01-01") ? DateTime.Now : b;
            _Stop = e == DateTime.Parse("1901-01-01") ? DateTime.Now : e;
            _Regions = r;
            _TotalCalculations = t;
        }
    }

    /// <summary>
    /// Maps blank areas of an image that could be used to render text.
    /// </summary>
    /// <remarks></remarks>
    public class GraphicalTextMapper : System.Collections.ObjectModel.ObservableCollection<LineSegments>
    {
        private LineSegments _rawData;
        private Bitmap _img;
        private Bitmap _rendered;
        private double _Scale;
        private int _maxX = 800;
        private int _maxY = 800;
        private System.Windows.Size _calcSize;
        private double _minUsableX = 0.5d;
        private double _minUsableY = 0.25d;
        private bool _purgeOverlaps = true;
        private List<UniRect> _Regions = new List<UniRect>();

        public event RenderedEventHandler Rendered;

        public delegate void RenderedEventHandler(object sender, EventArgs e);

        public event CalculatedEventHandler Calculated;

        public delegate void CalculatedEventHandler(object sender, CalculatedEventArgs e);

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Returns all of the regions that were discovered during the last run.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<UniRect> Regions
        {
            get
            {
                return _Regions;
            }
        }

        /// <summary>
        /// The scaled, rendered image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Bitmap RenderedImage
        {
            get
            {
                return _rendered;
            }
        }

        /// <summary>
        /// Returns the raw, unprocessed data from the image scan as a LineSegments object.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public LineSegments RawOutput
        {
            get
            {
                return _rawData;
            }
        }

        /// <summary>
        /// The size of the scaled image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Windows.Size ScaleSize
        {
            get
            {
                return _calcSize;
            }
        }

        /// <summary>
        /// The scale factor.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double RenderScale
        {
            get
            {
                return _Scale;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Specifies whether to purge overlapping rectangles from the final collection.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool PurgeOverlapping
        {
            get
            {
                return _purgeOverlaps;
            }

            set
            {
                _purgeOverlaps = value;
                OnPropertyChanged("PurgeOverlapping");
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the minimum usable width (in percentage of page size) to be considered when performing shape identification.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double MinUsableWidth
        {
            get
            {
                return _minUsableX;
            }

            set
            {
                _minUsableX = value;
                OnPropertyChanged("MinUsableWidth");
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the minimum usable height (in percentage of page size) to be considered when performing shape identification.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double MinUsableHeight
        {
            get
            {
                return _minUsableY;
            }

            set
            {
                _minUsableY = value;
                OnPropertyChanged("MinUsableHeight");
                Refresh();
            }
        }

        /// <summary>
        /// The maximum allowable X extent.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MaxX
        {
            get
            {
                return _maxX;
            }

            set
            {
                _maxX = value;
                OnPropertyChanged("MaxX");
                Refresh();
            }
        }

        /// <summary>
        /// The maximum allowable Y-extent.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int MaxY
        {
            get
            {
                return _maxY;
            }

            set
            {
                _maxY = value;
                OnPropertyChanged("MaxY");
                Refresh();
            }
        }

        /// <summary>
        /// The active processing image.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Bitmap Image
        {
            get
            {
                return _img;
            }

            set
            {
                _img = value;
                OnPropertyChanged("Image");
                Refresh();
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Recalculate and redraw all.
        /// </summary>
        /// <remarks></remarks>
        public void Refresh()
        {
            BuildRegions();
        }

        /// <summary>
        /// Create a new instance of the GraphicalTextMapper.
        /// </summary>
        /// <remarks></remarks>
        public GraphicalTextMapper()
        {
        }

        /// <summary>
        /// Create a new instance of the GraphicalTextMapper from the specified image.
        /// </summary>
        /// <param name="image">The image to use.</param>
        /// <param name="calcImmediately">Specify whether or not to process the image upon instantiation.</param>
        /// <remarks></remarks>
        public GraphicalTextMapper(Image image, bool calcImmediately = true)
        {
            _img = (Bitmap)image;
            if (calcImmediately)
                Refresh();
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Calculate the dimensions for rendering.
        /// </summary>
        /// <remarks></remarks>
        protected void CalcDims()
        {
            double x;
            double y;
            if (_img.Height > _img.Width)
            {
                _Scale = MaxY / (double)_img.Height;
                y = MaxY;
                x = _img.Width * _Scale;
            }
            else
            {
                _Scale = MaxX / (double)_img.Width;
                x = MaxX;
                y = _img.Height * _Scale;
            }

            _calcSize = new System.Windows.Size(x, y);
            OnPropertyChanged("RenderScale");
        }

        /// <summary>
        /// Builds all of the discovered open areas of an image.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool BuildRegions()
        {
            try
            {
                if (_img is null)
                    return false;
                Calculate();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Process the image and render the output.
        /// </summary>
        /// <param name="drawRectangles">Specifies whether to draw the discovered rectangles on the output image.</param>
        /// <remarks></remarks>
        public Bitmap RenderImage(bool drawRectangles = true)
        {
            if (_img is null)
                return null;
            CalcDims();
            if (_Regions is null || _Regions.Count == 0)
            {
                Calculate();
            }

            var rc = _Regions.ToArray();
            if (_rendered is object)
            {
                _rendered.Dispose();
            }

            var p = new Pen(Brushes.Black, 2f);
            double x;
            double y;
            var img2 = new Bitmap(_img.Width, _img.Height, PixelFormat.Format32bppArgb);
            Graphics g;
            var g2 = Graphics.FromImage(img2);
            g2.DrawImage(_img, 0, 0, _img.Width, _img.Height);
            if (drawRectangles)
            {
                foreach (var r in rc)
                    g2.DrawRectangle(p, (Rectangle)r);
            }

            p.Dispose();
            g2.Dispose();
            if (_img.Height > _img.Width)
            {
                y = MaxY;
                x = _img.Width * (MaxY / (double)_img.Height);
            }
            else
            {
                x = MaxX;
                y = _img.Height * (MaxX / (double)_img.Width);
            }

            _rendered = new Bitmap((int)x, (int)y, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(_rendered);
            g.DrawImage(img2, 0f, 0f, (float)x, (float)y);
            img2.Dispose();
            g.Dispose();
            OnPropertyChanged("RenderedImage");
            Rendered?.Invoke(this, new EventArgs());
            return _rendered;
        }

        /// <summary>
        /// Draws a grid onto an image.
        /// </summary>
        /// <param name="img">The image to work with.</param>
        /// <param name="GridX">The spacing of the X grid, in pixels.</param>
        /// <param name="GridY">The spacing of the Y grid, in pixels.</param>
        /// <param name="ColorX">The color of the X grid.</param>
        /// <param name="ColorY">The color of the Y grid.</param>
        /// <param name="Thickness">The line thickness in pixels.</param>
        /// <param name="GridType">The grid type.</param>
        /// <param name="DashPatternX">The X dash pattern.</param>
        /// <param name="DashPatternY">The Y dash pattern.</param>
        /// <remarks></remarks>
        public static void RenderGrid(Bitmap img, float GridX, float GridY, UniColor ColorX, UniColor ColorY, double Thickness = 1.0d, GridLinesType GridType = GridLinesType.Dotted, float[] DashPatternX = null, float[] DashPatternY = null)








        {
            var g = Graphics.FromImage(img);
            int x;
            int y;
            int cx;
            int cy;
            var dashValuesX = new float[] { 1f, 0f, 1f, 0f };
            var dashValuesY = new float[] { 1f, 0f, 1f, 0f };
            if (DashPatternX is object)
            {
                dashValuesX = DashPatternX;
            }

            if (DashPatternY is object)
            {
                dashValuesY = DashPatternY;
            }

            Pen pnX;
            Pen pnY;
            pnX = new Pen(ColorX, (float)Thickness);
            pnY = new Pen(ColorY, (float)Thickness);

            // g.FillRectangle(br2, 0, 0, Me.Width, Me.Height)

            switch (GridType)
            {
                case GridLinesType.Dashed:
                    {
                        dashValuesX[0] = Math.Max(1f, GridX / 4f - 2f);
                        dashValuesX[1] = 2f;
                        dashValuesX[2] = Math.Max(1f, GridX / 4f - 2f);
                        dashValuesX[3] = 2f;
                        dashValuesY[0] = Math.Max(1f, GridY / 4f - 2f);
                        dashValuesY[1] = 2f;
                        dashValuesY[2] = Math.Max(1f, GridY / 4f - 2f);
                        dashValuesY[3] = 2f;
                        pnX.DashPattern = dashValuesX;
                        pnY.DashPattern = dashValuesY;
                        break;
                    }

                case GridLinesType.Dotted:
                    {
                        dashValuesX[0] = 1f;
                        dashValuesX[1] = Math.Max(1f, GridX - 1f);
                        dashValuesX[2] = 1f;
                        dashValuesX[3] = Math.Max(1f, GridX - 1f);
                        dashValuesY[0] = 1f;
                        dashValuesY[1] = Math.Max(1f, GridY - 1f);
                        dashValuesY[2] = 1f;
                        dashValuesY[3] = Math.Max(1f, GridY - 1f);
                        pnX.DashPattern = dashValuesX;
                        pnY.DashPattern = dashValuesY;
                        break;
                    }
            }

            cx = img.Width;
            cy = img.Height;
            var loopTo = cx;
            for (x = (int)(GridX / 2f); (int)GridX >= 0 ? x <= loopTo : x >= loopTo; x += (int)GridX)
            {
                if (GridType == GridLinesType.Dotted)
                {
                    g.DrawLine(pnX, x, (int)Math.Floor(GridY / 2f), x, cy);
                }
                else
                {
                    // If (x = (GridX / 2)) Then Continue For
                    g.DrawLine(pnX, x, 0, x, cy);
                }
            }

            var loopTo1 = cy;
            for (y = (int)(GridY / 2f); (int)GridY >= 0 ? y <= loopTo1 : y >= loopTo1; y += (int)GridY)
            {
                if (GridType == GridLinesType.Dotted)
                {
                    g.DrawLine(pnX, (int)Math.Floor(GridX / 2f), y, cx, y);
                }
                else
                {
                    // If (y = (GridY / 2)) Then Continue For
                    g.DrawLine(pnX, 0, y, cx, y);
                }
            }

            pnX.Dispose();
            pnY.Dispose();
            g.Dispose();
        }

        /// <summary>
        /// RenderScale the given rectangle by the current scale factor
        /// </summary>
        /// <param name="rcIn"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected Rectangle ScaleRect(Rectangle rcIn)
        {
            return new Rectangle((int)(rcIn.X * _Scale), (int)(rcIn.Y * _Scale), (int)(rcIn.Width * _Scale), (int)(rcIn.Height * _Scale));
        }

        /// <summary>
        /// Perform all the calculations necessary to determine the
        /// usable text area from the given template and stationory image.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected Rectangle[] Calculate()
        {
            Rectangle[] CalculateRet = default;
            Bitmap bmp;

            // ' what we're going to do is map every possible rectangular region in the image.
            // ' we will accept a certain percentage of white space to add that rectangle to the image.

            double minX;
            double minY;
            int x;
            int y;
            int cx;
            int cy;
            bool bInUsable = false;
            var useStart = new PointF();
            var useStop = new PointF();
            int l;
            int c = 0;
            int d = 0;
            bool bPass = false;
            var lastUseStart = new PointF();
            var rcCol = new List<Rectangle>();
            var lineParts = new LineSegments();
            var hsv = new HSVDATA();
            var ceArgs = new CalculatedEventArgs( null, 0, DateTime.MinValue, DateTime.MinValue);
            ceArgs.StartTimer();

            // Dim sMult As Double = 1.0#
            // Dim maxImg As Integer = 2400

            bmp = _img;

            // If _img.Width > maxImg OrElse _img.Height > maxImg Then

            // If _img.Width > _img.Height Then
            // x = maxImg
            // y = _img.Height * (maxImg / _img.Width)
            // sMult = maxImg / _img.Width
            // Else
            // y = maxImg
            // x = _img.Width * (maxImg / _img.Height)
            // sMult = maxImg / _img.Height
            // End If

            // bmp = New Bitmap(x, y, PixelFormat.Format32bppArgb)
            // Dim g As Graphics = Graphics.FromImage(bmp)

            // g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
            // g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            // g.SmoothingMode = Drawing2D.SmoothingMode.None

            // g.DrawImage(_img, 0, 0, x, y)

            // g.Dispose()
            // _img = bmp
            // Else
            // bmp = _img
            // End If

            cx = bmp.Width;
            cy = bmp.Height;
            minX = _minUsableX * cx;
            minY = _minUsableY * cy;
            var bm = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            DataTools.Memory.MemPtr mm = bm.Scan0;
            ceArgs.TotalCalculations = cx * cy * 2;
            var loopTo = cy - 1;
            for (y = 0; y <= loopTo; y++)
            {
                c = -1;
                d = -1;
                var loopTo1 = cx - 1;
                for (x = 0; x <= loopTo1; x++)
                {
                    l = (y * cx + x) * 4;
                    if (mm.ByteAt(l) >= 250 && mm.ByteAt(l + 1) >= 250 && mm.ByteAt(l + 2) >= 250)

                    {
                        if (c == -1)
                        {
                            c = x;
                            d = 0;
                        }
                        else
                        {
                            d += 1;
                        }
                    }
                    else if (d != -1)
                    {
                        if (d > minX)
                        {
                            lineParts.Add(new LineSegment(c, d, y));
                        }

                        c = -1;
                        d = -1;
                    }
                }

                if (d > minX)
                {
                    lineParts.Add(new LineSegment(c, d, y));
                }
            }

            lineParts.Sort();
            _rawData = lineParts;
            var experiment = new List<LineSegments>();
            LineSegments temp;
            // Dim temp2 As LineSegments

            double minArea = minX * minY;
            minArea *= 0.5d;
            ceArgs.TotalCalculations += cx * cy;
            var loopTo2 = cx - 1;
            for (x = 0; (int)(cx * 0.0375d) >= 0 ? x <= loopTo2 : x >= loopTo2; x += (int)(cx * 0.0375d))
            {
                var loopTo3 = cy - 1;
                for (y = 0; (int)(cy * 0.0375d) >= 0 ? y <= loopTo3 : y >= loopTo3; y += (int)(cy * 0.0375d))
                {
                    l = (y * cx + x) * 4;
                    temp = _rawData.FindSet(x, cx - x, y, false);
                    if (temp is null || temp.Area < minArea)
                        continue;
                    for (l = temp.Count - 2; l >= 1; l -= 1)
                    {
                        if (temp[l].Index - temp[l - 1].Index > temp.OriginContinuityThreshold)
                        {
                            temp.RemoveAt(l - 1);
                        }
                    }

                    ceArgs.TotalCalculations += temp.Count + _rawData.Count;
                    experiment.Add(temp);
                }
            }

            experiment.Sort(new SortByArea());
            experiment.Reverse();
            LineSegments[] pps; // = lineParts.FindAllContiguousRegions
            pps = experiment.ToArray();
            Clear();
            bmp.UnlockBits(bm);
            ceArgs.TotalCalculations += pps.Count() + _rawData.Count;
            _Regions.Clear();
            foreach (var px in pps)
            {
                var rc = px.Bounds;
                temp = _rawData.FindSet(rc.Left, rc.Right, rc.Top, true, false);
                if (temp is null)
                    continue;
                rc = temp.Bounds;
                if (rc.Width < minX)
                    continue;
                if (rc.Height < minY)
                    continue;
                rc.Inflate(-20, -20);
                if (_purgeOverlaps)
                {
                refor:
                    ;
                    foreach (var rcChk in rcCol)
                    {
                        if (rcChk.IntersectsWith(rc))
                        {
                            if (rcChk.Width * rcChk.Height > rc.Width * rc.Height) // AndAlso (rcChk.Width > rc.Width) Then
                            {
                                rc = rcChk;
                            }
                            else if (TestSquareness(rcChk) < TestSquareness(rc)) // AndAlso (rc.Width >= rcChk.Width) Then
                            {
                                rcCol.Remove(rcChk);
                                goto refor;
                            }
                            else
                            {
                                rc = Rectangle.Empty;
                            }

                            break;
                        }
                    }
                }

                if (temp is object)
                    Add(temp);
                if (rc != Rectangle.Empty)
                {
                    // If CheckRect(rc, cx, cy, mm) = False Then Continue For
                    rcCol.Add(rc);
                    _Regions.Add(rc);
                }
            }

            ceArgs.StopTimer();
            CalculateRet = rcCol.ToArray();
            ceArgs.Regions = _Regions;
            OnPropertyChanged("Regions");
            OnPropertyChanged("RawOutput");
            Calculated?.Invoke(this, ceArgs);
            return CalculateRet;
        }

        protected bool CheckRect(Rectangle rc, int cx, int cy, DataTools.Memory.MemPtr mm)
        {
            int x;
            int y;
            int l;
            var loopTo = rc.Bottom;
            for (y = rc.Top; y <= loopTo; y++)
            {
                var loopTo1 = rc.Right;
                for (x = rc.Left; x <= loopTo1; x++)
                {
                    l = (y * cx + x) * 4;
                    if (mm.ByteAt(l) < 250 || mm.ByteAt(l + 1) < 250 || mm.ByteAt(l + 2) < 250)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected double TestSquareness(Rectangle rect)
        {
            try
            {
                double avg = (rect.Width + rect.Height) / 2d;
                double wpct = rect.Width / avg;
                double hpct = rect.Height / avg;
                return 1d - Math.Abs(wpct - hpct);
            }
            catch (Exception ex)
            {
                return double.NaN;
            }
        }

        protected void OnPropertyChanged(string name)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    public class SortByArea : IComparer<LineSegments>
    {
        public int Compare(LineSegments x, LineSegments y)
        {
            return (int)(x.Area - y.Area);
        }
    }

    public enum GridLinesType
    {
        None,
        Dotted,
        Dashed,
        Solid
    }
}