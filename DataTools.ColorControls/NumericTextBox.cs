using DataTools.Text;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents.DocumentStructures;
using System.Windows.Input;
using System.Windows.Media;

namespace DataTools.ColorControls
{
    public class NumericTextBox : TextBox, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool error = false;
        NumberStyles numStyle;

        public NumericTextBox() : base()
        {

        }

        /// <summary>
        /// Gets or sets the numeric value of the control.
        /// </summary>
        public double NumericValue
        {
            get { return (double)GetValue(NumericValueProperty); }
            set { SetValue(NumericValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumericValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericValueProperty =
            DependencyProperty.Register("NumericValue", typeof(double), typeof(NumericTextBox), new PropertyMetadata(0.0d, OnNeedFormat));

        /// <summary>
        /// Gets or sets the numeric value of the control.
        /// </summary>
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.MinValue, OnNeedValidation));


        /// <summary>
        /// Gets or sets the numeric value of the control.
        /// </summary>
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericTextBox), new PropertyMetadata(double.MaxValue, OnNeedValidation));


        /// <summary>
        /// Gets or sets the numeric format.
        /// </summary>
        public string NumericFormat
        {
            get { return (string)GetValue(NumericFormatProperty); }
            set { SetValue(NumericFormatProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumericFormat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericFormatProperty =
            DependencyProperty.Register("NumericFormat", typeof(string), typeof(NumericTextBox), new PropertyMetadata(null, OnNeedFormat));

        /// <summary>
        /// Gets or sets the valid parsing <see cref="NumberStyles"/>.
        /// </summary>
        [TypeConverter(typeof(EnumConverter))] //yeah, this line
        public EditorNumberStyle NumberStyle
        {
            get { return (EditorNumberStyle)GetValue(NumberStyleProperty); }
            set { SetValue(NumberStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NumberStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumberStyleProperty =
            DependencyProperty.Register("NumberStyle", typeof(EditorNumberStyle), typeof(NumericTextBox), new PropertyMetadata(EditorNumberStyle.Float, OnNeedValidation));

        new public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderBrush.  This enables animation, styling, binding, etc...
        new public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(NumericTextBox), new PropertyMetadata(SystemColors.ActiveBorderBrush, OnBrushChanged));

        public Brush ErrorBorderBrush
        {
            get { return (Brush)GetValue(ErrorBorderBrushProperty); }
            set { SetValue(ErrorBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBorderBrushProperty =
            DependencyProperty.Register("ErrorBorderBrush", typeof(Brush), typeof(NumericTextBox), new PropertyMetadata(new SolidColorBrush(Colors.Red), OnBrushChanged));

        private static void OnBrushChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue && sender is NumericTextBox ctrl)
            {
                if (ctrl is TextBox tb)
                {
                    if (tb.BorderBrush != ctrl.BorderBrush)
                        tb.BorderBrush = ctrl.BorderBrush;
                }
            }
        }

        private static void OnNeedValidation(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if (sender is NumericTextBox ctrl)
            {

                if (string.Compare((string)e.NewValue, (string)e.OldValue) != 0)
                    ctrl.ValidateValue();
            }
        }

        private static void OnNeedFormat(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is NumericTextBox ctrl)
            {
                if (!e.OldValue.Equals(e.NewValue))
                    ctrl.FormatValue();
            }
        }

        /// <summary>
        /// Gets a value indicating that the text box is in an error state.
        /// </summary>
        public bool IsErrorState
        {
            get => error;
            protected set
            {
                if (error != value)
                {
                    error = value;
                    if (error)
                    {
                        base.BorderBrush = ErrorBorderBrush;
                    }
                    else
                    {
                        base.BorderBrush = BorderBrush;
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsErrorState)));
                }
            }
        }

        private void FormatValue()
        {
            string s = null;

            if (!string.IsNullOrEmpty(NumericFormat))
            {
                try
                {
                    s = NumericValue.ToString(NumericFormat);
                    var d = double.Parse(s);
                }
                catch
                {
                    IsErrorState = true;
                }
            }
            else
            {
                s = NumericValue.ToString();
            }

            if (Text != s)
            {
                Text = s;
            }
        }

        private NumberStyles ComputeNumberStyle()
        {
                if (NumberStyle == EditorNumberStyle.Float)
                    return NumberStyles.Float;

                else if (NumberStyle == EditorNumberStyle.Integer)
                    return NumberStyles.Integer;

                else if (NumberStyle == EditorNumberStyle.FloatWithThousands)
                    return NumberStyles.Float | NumberStyles.AllowThousands;

                else if (NumberStyle == EditorNumberStyle.IntegerWithThousands)
                    return NumberStyles.Integer | NumberStyles.AllowThousands;

                else
                    return NumberStyles.Number;

        }

        private bool ValidateValue()
        {
            if (string.IsNullOrEmpty(Text))
            {
                NumericValue = double.NaN;
                IsErrorState = false;

                return true;
            }

            numStyle = ComputeNumberStyle();
            var b = double.TryParse(Text, numStyle, CultureInfo.InvariantCulture, out double val);

            if (b)
            {
                NumericValue = val;
                IsErrorState = false;
            }
            else
            {
                NumericValue = double.NaN;
                IsErrorState = true;
            }

            return b;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            ValidateValue();
        }
    }


    //public struct EditorNumberStyle
    //{
    //    readonly int value;

    //    // public int Value => value;

    //    public static readonly EditorNumberStyle Float = new EditorNumberStyle(0);
    //    public static readonly EditorNumberStyle FloatWithThousands = new EditorNumberStyle(1);
    //    public static readonly EditorNumberStyle Integer = new EditorNumberStyle(2);
    //    public static readonly EditorNumberStyle IntegerWithThousands = new EditorNumberStyle(3);

    //    static Dictionary<int, string> styles = new Dictionary<int, string>();

    //    private EditorNumberStyle(int value)
    //    {
    //        this.value = value; 
    //    }

    //    static EditorNumberStyle()
    //    {
    //        var fld = typeof(EditorNumberStyle).GetFields(BindingFlags.Static | BindingFlags.Public);

    //        foreach (var f in fld)
    //        {
    //            if (f.GetValue(null) is EditorNumberStyle ens)
    //            {
    //                styles.Add(ens.value, f.Name);
    //            }
    //        }
    //    }
    //    public override string ToString()
    //    {
    //        return styles[value];
    //    }

    //    public static explicit operator int(EditorNumberStyle value)
    //    {
    //        return value.value;
    //    }

    //    public static explicit operator EditorNumberStyle(int value)
    //    {
    //        if (!styles.ContainsKey(value)) styles.Add(value, value.ToString());
    //        return new EditorNumberStyle(value);
    //    }

    //    public static explicit operator string(EditorNumberStyle value)
    //    {
    //        return styles[value.value];
    //    }

    //    public static explicit operator EditorNumberStyle(string value)
    //    {
    //        var f = typeof(EditorNumberStyle).GetField(value);

    //        if (f != null && f.GetValue(null) is EditorNumberStyle ens)
    //        {
    //            return ens;
    //        }
    //        else
    //        {
    //            var x = styles.Keys.Max() + 1;
    //            styles.Add(x, value.ToString());
    //            return new EditorNumberStyle(x);
    //        }
    //    }

    //    public override bool Equals([NotNullWhen(true)] object obj)
    //    {
    //        if (obj is EditorNumberStyle ens)
    //        {
    //            return ens.value == value;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }

    //    public override int GetHashCode()
    //    {
    //        return value.GetHashCode();
    //    }

    //    public static bool operator ==(EditorNumberStyle val1, EditorNumberStyle val2)
    //    {
    //        return val1.value == val2.value;
    //    }
    //    public static bool operator !=(EditorNumberStyle val1, EditorNumberStyle val2)
    //    {
    //        return val1.value != val2.value;
    //    }

    //}

    public enum EditorNumberStyle
    {
        Float,
        Integer,
        FloatWithThousands,
        IntegerWithThousands
    }

}
