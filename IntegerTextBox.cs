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
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Data;

namespace FancyPrimitives
{
    //**************************************************************************************************************************************************
    public class IntegerTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[-]?[0-9]+$");
        private int initialValue;
        //----------------------------------------------------------------------------------------------------------------------------------
        public IntegerTextBox()
        {
            PreviewKeyDown += OnPreviewKeyDown;
            AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent,
                      new System.Windows.Controls.TextChangedEventHandler(OnTextChanged));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            initialValue = Convert.ToInt32(Text);
            base.OnGotKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (Text.Trim() == "")
                Text = "0";
            base.OnLostKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;

            int.TryParse(new_str, out int newIntValue);
            if (newIntValue < MinValue || newIntValue > MaxValue)
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected void OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            IntegerTextBox txtBox = sender as IntegerTextBox;

            if (args.Key == Key.Escape)
            {
                args.Handled = true;
                txtBox.Text = initialValue.ToString();
                Keyboard.ClearFocus();
            }
            else if (args.Key == Key.Enter)
            {
                args.Handled = true;
                //txtBox.Text = lastValidValue.ToString();
                Keyboard.ClearFocus();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IntegerTextBox txtBox = sender as IntegerTextBox;

            if (Text.Trim() == "") return;

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
            DependencyProperty.Register("Command", typeof(ICommand), typeof(IntegerTextBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Object), typeof(IntegerTextBox), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(int.MinValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(IntegerTextBox), new PropertyMetadata(int.MaxValue));
        //----------------------------------------------------------------------------------------------------------------------------------
        private RelayCommand incrementValueCommand;
        public RelayCommand IncrementValueCommand
        {
            get
            {
                return incrementValueCommand ??
                  (incrementValueCommand = new RelayCommand(obj =>
                  {
                      int.TryParse(Text, out int intValue);
                      if (intValue < MaxValue)
                          SetCurrentValue(TextProperty, (intValue + 1).ToString());
                  }));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private RelayCommand decrementValueCommand;
        public RelayCommand DecrementValueCommand
        {
            get
            {
                return decrementValueCommand ??
                  (decrementValueCommand = new RelayCommand(obj =>
                  {
                      int.TryParse(Text, out int intValue);
                      if (intValue > MinValue)
                          SetCurrentValue(TextProperty, (intValue - 1).ToString());
                  }));
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
