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
using System.Globalization;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;

namespace FancyPrimitives
{
    public class DoubleTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[-]?[0-9]*[,.]?[0-9]*$");
        private double initialValue;
        private double lastNonStrictValidValue;
        //----------------------------------------------------------------------------------------------------------------------------------
        public DoubleTextBox()
        {
            PreviewKeyDown += OnKeyDownHandler;
            AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
                      new System.Windows.Controls.TextChangedEventHandler(OnTextChanged));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (FancyPrimitives.MyUtility.ParseStringAsDouble(Text, out lastNonStrictValidValue, false))
                initialValue = lastNonStrictValidValue;
            else
                initialValue = lastNonStrictValidValue = 0;

            base.OnGotKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (Text.Trim() == "")
                Text = "0";
            else
              Text = lastNonStrictValidValue.ToString();

            base.OnLostKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(double.MinValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(DoubleTextBox), new PropertyMetadata(double.MaxValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);

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
            if (args.Key == Key.Escape)
            {
                args.Handled = true;
                Text = initialValue.ToString();
                Keyboard.ClearFocus();
            }
            else if (args.Key == Key.Enter)
            {
                args.Handled = true;
                Text = lastNonStrictValidValue.ToString();
                Keyboard.ClearFocus();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            DoubleTextBox txtBox = sender as DoubleTextBox;

            if (Text.Trim() == "") return;

            if (FancyPrimitives.MyUtility.ParseStringAsDouble(Text.Trim(), out double d, false))
                lastNonStrictValidValue = d;

            if (Command != null)
            {
                BindingExpressionBase expr = BindingOperations.GetBindingExpressionBase(txtBox, CommandParameterProperty);
                if (expr != null)
                    expr.UpdateTarget();

                //Debug.WriteLine($"Send command ({d})");
                if (Command != null && Command.CanExecute(CommandParameter))
                    Command.Execute(CommandParameter);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(DoubleTextBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Object), typeof(DoubleTextBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
