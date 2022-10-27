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
using System.Globalization;

namespace FancyPrimitives
{

    public class Hex8DigitTextBox : TextBox
    {
        private static readonly Regex regex = new Regex("^[0-9a-fA-F]{0,8}$");
        private string initialValue;
        //----------------------------------------------------------------------------------------------------------------------------------
        public Hex8DigitTextBox()
        {
            PreviewKeyDown += OnPreviewKeyDown;
            TextChanged += new TextChangedEventHandler(OnTextChanged);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            initialValue = Text;
            base.OnGotKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs args)
        {
            if (Text.Trim() == "" || Text.Trim().Length != 8)
                Text = initialValue;

            base.OnLostKeyboardFocus(args);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string new_str = Text.Substring(0, SelectionStart) + e.Text + Text.Substring(SelectionStart + SelectionLength, Text.Length - SelectionStart - SelectionLength);
            if (!regex.IsMatch(new_str))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        protected void OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            Hex8DigitTextBox txtBox = sender as Hex8DigitTextBox;

            if (args.Key == Key.Escape)
            {
                args.Handled = true;
                txtBox.Text = initialValue;
                Keyboard.ClearFocus();
            }
            else if (args.Key == Key.Enter)
            {
                args.Handled = true;

                if (txtBox.Text.Trim().Length != 8)
                    txtBox.Text = initialValue;

                Keyboard.ClearFocus();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        static void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //HexNumberTextBox thisTextBox = sender as HexNumberTextBox;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
