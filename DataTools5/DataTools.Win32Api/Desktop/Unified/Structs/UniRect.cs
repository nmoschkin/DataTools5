using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.Win32Api;

namespace DataTools.Desktop.Unified
{
    /// <summary>
    /// Unified rectangle structure for WinForms, WPF and the Win32 API.
    /// </summary>
    /// <remarks></remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UniRect : INotifyPropertyChanged
    {
        private double _Left;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public double Left
        {
            get
            {
                return _Left;
            }

            set
            {
                _Left = value;
                OnPropertyChanged("Left");
                OnPropertyChanged("X");
            }
        }

        private double _Top;

        public double Top
        {
            get
            {
                return _Top;
            }

            set
            {
                _Top = value;
                OnPropertyChanged("Top");
                OnPropertyChanged("Y");
            }
        }

        private double _Width;

        public double Width
        {
            get
            {
                return _Width;
            }

            set
            {
                _Width = value;
                OnPropertyChanged("Width");
                OnPropertyChanged("Right");
                OnPropertyChanged("CX");
            }
        }

        private double _Height;

        public double Height
        {
            get
            {
                return _Height;
            }

            set
            {
                _Height = value;
                OnPropertyChanged("Height");
                OnPropertyChanged("Bottom");
                OnPropertyChanged("CY");
            }
        }

        public double Right
        {
            get
            {
                return _Width - _Left - 1d;
            }

            set
            {
                _Width = value - _Left + 1d;
                OnPropertyChanged("Height");
                OnPropertyChanged("Bottom");
                OnPropertyChanged("CY");
            }
        }

        public double Bottom
        {
            get
            {
                return _Height - _Top - 1d;
            }

            set
            {
                _Height = value - _Top + 1d;
                OnPropertyChanged("Width");
                OnPropertyChanged("Right");
                OnPropertyChanged("CX");
            }
        }

        public double X
        {
            get
            {
                return _Left;
            }
        }

        public double Y
        {
            get
            {
                return _Top;
            }
        }

        public double CX
        {
            get
            {
                return _Width;
            }
        }

        public double CY
        {
            get
            {
                return _Height;
            }
        }

        public UniRect(double x, double y, double width, double height)
        {
            _Left = x;
            _Top = y;
            _Width = width;
            _Height = height;

            PropertyChanged = null;
        }

        public UniRect(System.Windows.Point location, System.Windows.Size size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(Point location, Size size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(PointF location, SizeF size)
        {
            _Left = location.X;
            _Top = location.Y;
            _Width = size.Width;
            _Height = size.Height;
            PropertyChanged = null;
        }

        public UniRect(W32RECT rectStruct)
        {
            _Left = rectStruct.left;
            _Top = rectStruct.top;

            _Width = (rectStruct.right - rectStruct.left) + 1;
            _Height = (rectStruct.bottom - rectStruct.top) + 1;

            PropertyChanged = null;
        }

        public UniRect(Rectangle rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(RectangleF rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(System.Windows.Rect rectStruct)
        {
            _Left = rectStruct.Left;
            _Top = rectStruct.Top;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        public UniRect(System.Windows.Int32Rect rectStruct)
        {
            _Left = rectStruct.X;
            _Top = rectStruct.Y;
            _Width = rectStruct.Width;
            _Height = rectStruct.Height;
            PropertyChanged = null;
        }

        event System.ComponentModel.PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}; {2}x{3}", _Left, _Top, _Width, _Height);
        }

        public static implicit operator System.Windows.Rect(UniRect operand)
        {
            return new System.Windows.Rect(operand._Left, operand._Top, operand._Width, operand._Height);
        }

        public static implicit operator UniRect(System.Windows.Rect operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator RectangleF(UniRect operand)
        {
            return new RectangleF((float)operand._Left, (float)operand._Top, (float)operand._Width, (float)operand._Height);
        }

        public static implicit operator UniRect(RectangleF operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator Rectangle(UniRect operand)
        {
            return new Rectangle((int)operand._Left, (int)operand._Top, (int)operand._Width, (int)operand._Height);
        }

        public static implicit operator UniRect(Rectangle operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator W32RECT(UniRect operand)
        {
            return new W32RECT() { left = (int)operand._Left, top = (int)operand._Top, right = (int)operand.Right, bottom = (int)operand.Bottom };
        }

        public static implicit operator UniRect(W32RECT operand)
        {
            return new UniRect(operand);
        }

        public static explicit operator System.Windows.Int32Rect(UniRect operand)
        {
            return new System.Windows.Int32Rect((int)operand.Left, (int)operand.Top, (int)operand.Width, (int)operand.Height);
        }

        public static implicit operator UniRect(System.Windows.Int32Rect operand)
        {
            return new UniRect(operand);
        }

        public static implicit operator double[](UniRect operand)
        {
            return new[] { operand.Left, operand.Top, operand.Right, operand.Bottom };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
    }
}
