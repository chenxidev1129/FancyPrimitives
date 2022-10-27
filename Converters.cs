/*
MIT License

Copyright(c) 2019 Dennis Geller

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FancyPrimitives
{
    //**************************************************************************************************************************************************
    public class SymStringThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        private object SymmetricalConvert(object value, Type targetType)
        {
            if (value is Thickness t)
            {
                if (targetType == typeof(string))
                    return t.Left.ToString() + " " + t.Top.ToString() + " " + t.Right.ToString() + " " + t.Bottom.ToString();
                else
                    return null;
            }
            else if (value is string s)
            {
                if (targetType == typeof(Thickness))
                {
                    Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
                    s = regex.Replace(s, " ");

                    string[] numbers = s.Split(' ');
                    if (numbers.Length == 4)
                    {
                        if (double.TryParse(numbers[0], out double left)
                            && double.TryParse(numbers[1], out double top)
                            && double.TryParse(numbers[2], out double right)
                            && double.TryParse(numbers[3], out double bottom))
                            return new Thickness(left, top, right, bottom);
                        else 
                            return null;
                    }
                    else return null;
                }
                else
                    return value;
            }
            else
                return null;
        }
    }
    //**************************************************************************************************************************************************
    [ValueConversion(typeof(object), typeof(string))]
    public class StringFormatConverter : IValueConverter
    {
        public string StringFormat { get; set; }

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(StringFormat)) return "";
            return string.Format(StringFormat, value);
        }


        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
    //**************************************************************************************************************************************************
    public class FadeDownColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object byteAlpha_parameter, CultureInfo culture)
        {
            Byte byteAlpha = 30;
            if (byteAlpha_parameter!=null)
                Byte.TryParse(byteAlpha_parameter as string, out byteAlpha);

            if (value is SolidColorBrush b)
            {
                Color col = b.Color;
                col.A = byteAlpha;
                if (targetType == typeof(Color))
                    return col;
                else
                    return new SolidColorBrush(col);
            }
            else if (value is Color col)
            {
                col.A = byteAlpha;
                if (targetType == typeof(Color))
                    return col;
                else
                    return new SolidColorBrush(col);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //**************************************************************************************************************************************************
    public class SymStringToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        private object SymmetricalConvert(object value, Type targetType)
        {
            if (value is double d)
            {
                if (targetType == typeof(string))
                    return d.ToString();
                else if (targetType == typeof(int))
                    return (int)d;
                else
                    return value;
            }
            else if (value is string s)
            {
                if (targetType == typeof(double))
                {
                    if (FancyPrimitives.MyUtility.ParseStringAsDouble(s, out d))
                        return d;
                    else
                        return null;
                }
                else if (targetType == typeof(int))
                {
                    int.TryParse(s, out int i);
                    return i;
                }
                else 
                    return value;
            }
            else
                return null;
        }
    }
    //**************************************************************************************************************************************************
    public class SymColorBrushStringConverter : IValueConverter
    {
        private static readonly Regex hexColorRegex = new Regex("^[0-9a-fA-F]{8}$");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return SymmetricalConvert(value, targetType);
        }

        private static string IfOneHexSymbolThenTwoHexSymbols(string hexString)
        {
            return (hexString.Length == 1) ? ("0" + hexString) : hexString;
        }

        private object SymmetricalConvert(object value, Type targetType)
        {
            if (value is Color color)
            {
                if (targetType == typeof(Color))
                    return color;
                else if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
                    return new SolidColorBrush(color);
                else if (targetType == typeof(string))
                    return IfOneHexSymbolThenTwoHexSymbols(color.A.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(color.R.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(color.G.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(color.B.ToString("X"));
                else
                    return Binding.DoNothing;
            }
            else if (value is SolidColorBrush solidBrush)
            {
                if (targetType == typeof(Color))
                    return solidBrush.Color;
                else if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
                    return solidBrush;
                else if (targetType == typeof(string))
                    return IfOneHexSymbolThenTwoHexSymbols(solidBrush.Color.A.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(solidBrush.Color.R.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(solidBrush.Color.G.ToString("X")) + IfOneHexSymbolThenTwoHexSymbols(solidBrush.Color.B.ToString("X"));
                else
                    return Binding.DoNothing;
            }
            else if (value is string hexStringColor)
            {
                hexStringColor = hexStringColor.Trim();
                if (!hexColorRegex.IsMatch(hexStringColor))
                    return -1; // deliberately incorrect value

                if (targetType == typeof(Color))
                    return (Color)ColorConverter.ConvertFromString("#" + hexStringColor);
                else if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#" + hexStringColor));
                else return Binding.DoNothing;
            }
            else if (((value is Brush || value.GetType().IsSubclassOf(typeof(Brush))) && targetType == typeof(Brush))
                      || value?.GetType() == targetType)
                return value;
            else
                return Binding.DoNothing;
        }
    }
    //**************************************************************************************************************************************************
    [ValueConversion(typeof(Color), typeof(LinearGradientBrush))]
    public class ColorToLinearGradientBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Transparent;

            Color color = (Color)value;
            return new LinearGradientBrush(Colors.Transparent, color, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //*******************************************************************************************************************************************************************
    public class ColorAndAlphaToColorConverter : IMultiValueConverter
    {
        // values[0] - Color or SolidColorBrush - non-transparent constituent
        // values[1] - Double or String - alpha channel (0-255)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;
            if (values[0] is Color)
                color = (Color)values[0];
            else if (values[0] is SolidColorBrush)
                color = ((SolidColorBrush)values[0]).Color;
            else return null;

            double doubleAlpha;
            if (values[1] is string)
                double.TryParse(values[1] as string, out doubleAlpha);
            else if (values[1] is double || values[1] is int)
                doubleAlpha = (double)values[1];
            else return null;

            Color newСolor = Color.FromArgb((Byte)doubleAlpha, color.R, color.G, color.B);
            if (targetType == typeof(Color))
                return newСolor;
            else if (targetType == typeof(Brush) || targetType == typeof(SolidColorBrush))
                return new SolidColorBrush(newСolor);
            else
                return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color color;
            if (value is Color)
                color = (Color)value;
            else if (value is SolidColorBrush)
                color = ((SolidColorBrush)value).Color;
            else return null;

            object[] arr = new object[2];

            if (targetTypes[0] == typeof(Color))
                arr[0] = Color.FromArgb(255, color.R, color.G, color.B);
            else if (targetTypes[0] == typeof(Brush) || targetTypes[0] == typeof(SolidColorBrush))
                arr[0] = new SolidColorBrush(Color.FromArgb(255, color.R, color.G, color.B));
            else return null;

            arr[1] = (double)color.A;

            return arr;
        }
    }
    //**************************************************************************************************************************************************
    //**************************************************************************************************************************************************
    //**************************************************************************************************************************************************
}
