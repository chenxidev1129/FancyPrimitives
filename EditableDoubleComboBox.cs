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
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Xml;
using System.Globalization;
using System.Windows;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FancyPrimitives
{
    public class EditableDoubleComboBox : ComboBox
    {
        private static Regex regex = new Regex("^[-]?[0-9]*[,.]?[0-9]*$"); // Positive float value
        private double lastValidValue;
        private double initialValue;
        //----------------------------------------------------------------------------------------------------------------------------------
        public EditableDoubleComboBox()
        {
            IsEditable = true;
            IsReadOnly = false;
            SelectedValuePath = "Content";
            PreviewKeyDown += OnKeyDownHandler;
            AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
                      new System.Windows.Controls.TextChangedEventHandler(OnTextChanged));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (MyUtility.ParseStringAsDouble(Text, out double d))
                initialValue = lastValidValue = d;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (!MyUtility.ParseStringAsDouble(Text, out double d))
                Text = lastValidValue.ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            TextBox txtBox = (TextBox)Template.FindName("PART_EditableTextBox", this);
            string new_str = txtBox.Text.Substring(0, txtBox.SelectionStart) + e.Text + txtBox.Text.Substring(txtBox.SelectionStart + txtBox.SelectionLength,
                                            txtBox.Text.Length - txtBox.SelectionStart - txtBox.SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;

            double.TryParse(new_str, out double newDoubleValue);
            if (newDoubleValue < MinValue || newDoubleValue > MaxValue)
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected void OnKeyDownHandler(object sender, KeyEventArgs args)
        {
            ComboBox combo = sender as ComboBox;

            if (args.Key == Key.Escape)
            {
                args.Handled = true;
                combo.Text = initialValue.ToString();
                Keyboard.ClearFocus();
            }
            else if (args.Key == Key.Enter)
            {
                args.Handled = true;
                combo.Text = lastValidValue.ToString();
                Keyboard.ClearFocus();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            if (MyUtility.ParseStringAsDouble(combo.Text, out double d) && d != lastValidValue)
            {
                BindingExpressionBase expr = BindingOperations.GetBindingExpressionBase(combo, CommandParameterProperty);
                if (expr != null)
                    expr.UpdateTarget();

                lastValidValue = d;
                //Debug.WriteLine($"Send command ({d})");
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(EditableDoubleComboBox), new PropertyMetadata(double.MinValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(EditableDoubleComboBox), new PropertyMetadata(double.MaxValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EditableDoubleComboBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Object), typeof(EditableDoubleComboBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
