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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices; // [CallerMemberName]
using System.Reflection;
using System.Text.RegularExpressions;

namespace FancyPrimitives
{
    public static class MyUtility
    {
        //----------------------------------------------------------------------------------------------------------------------------------
        public static bool ParseStringAsDouble(string s, out double d, bool isStrictDouble = true)
        {
            s = s.Replace(",", ".").Trim();

            Regex strictDoubleRegex = new Regex("(^[-]?[0-9]*$)|(^[-]?[0-9]*[.]{1}[0-9]*[1-9]{1}$)");
            Regex nonStrictDoubleRegex = new Regex("(^[-]?[0-9]+[.]?[0-9]*$)|(^[-]?[0-9]*[.]?[0-9]+$)");
            Regex regex = isStrictDouble ? strictDoubleRegex : nonStrictDoubleRegex;
            if (!regex.IsMatch(s))
            {
                d = 0;
                return false;
            }

            NumberStyles style = NumberStyles.Float;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");
            return Double.TryParse(s, style, culture, out d);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public static bool SetProperty(DependencyObject propertyOwner, string propertyName, object propertyValue, out object old_value)
        {
            DependencyPropertyDescriptor dpDescriptor = DependencyPropertyDescriptor.FromName(propertyName, propertyOwner.GetType(), propertyOwner.GetType());
            if (dpDescriptor != null) // Dependency property
            {
                DependencyProperty dp = dpDescriptor.DependencyProperty;
                old_value = propertyOwner.GetValue(dp);
                bool isNewValueEqualsToOldValue = ((old_value != null) && old_value.GetType().IsValueType) ?
                                                    old_value.Equals(propertyValue) : (old_value == propertyValue);

                if (isNewValueEqualsToOldValue) return false;

                propertyOwner.SetCurrentValue(dp, propertyValue);
                return true;
            }
            else // Regular property
            {
                PropertyInfo propInfo = propertyOwner.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null && propInfo.CanWrite)
                {
                    old_value = propInfo.GetValue(propertyOwner);
                    bool isNewValueEqualsToOldValue = ((old_value != null) && old_value.GetType().IsValueType) ?
                                                        old_value.Equals(propertyValue) : (old_value == propertyValue);

                    if (isNewValueEqualsToOldValue) return false;

                    propInfo.SetValue(propertyOwner, Convert.ChangeType(propertyValue, propInfo.PropertyType), null);
                    return true;
                }
            }
            old_value = null;
            return false;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public static Size MeasureString(this TextBox thisTextBox, string stringToMeasure)
        {
            var formattedText = new FormattedText(
                stringToMeasure,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(thisTextBox.FontFamily, thisTextBox.FontStyle, thisTextBox.FontWeight, thisTextBox.FontStretch),
                thisTextBox.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                1);

            return new Size(formattedText.Width, formattedText.Height);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
