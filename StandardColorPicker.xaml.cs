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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Reflection;
using System.Diagnostics;

namespace FancyPrimitives
{
    /// <summary>
    /// Логика взаимодействия для StandardColorPicker.xaml
    /// </summary>
    public partial class StandardColorPicker : UserControl
    {
        //----------------------------------------------------------------------------------------------------------------------------------
        public StandardColorPicker()
        {
            PropertyInfo[] props = typeof(Colors).GetProperties();
            List<Color> arr = props.Select(p => (Color)p.GetValue(null)).ToList();
            arr.Add(Color.FromArgb(0, 0, 0, 0));
            StandardColors = arr.ToArray();

            InitializeComponent();

            comboBoxStandardColor.SelectedValue = SelectedColor;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public Color[] StandardColors { get; private set; }

        private int FindStandardColor(Color color)
        {
            for (int i = 0; i < StandardColors.Length; i++)
                if (StandardColors[i] == color) return i;
            return -1;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public static Color DefaultSelectedColor { get { return Colors.Black; } }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(StandardColorPicker), new PropertyMetadata(DefaultSelectedColor, OnSelectedColorChanged));

        static void OnSelectedColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            StandardColorPicker thisControl = obj as StandardColorPicker;
            if (thisControl == null) return;

            Color newSelectedColor = (Color)e.NewValue;

            if (thisControl.FindStandardColor(newSelectedColor) == -1)
                thisControl.StandardColors[thisControl.StandardColors.Length - 1] = newSelectedColor;

            thisControl.comboBoxStandardColor.SelectedValue = newSelectedColor;

            if (e.NewValue != null && thisControl.IsLoaded)
            {
                BindingExpressionBase expr = BindingOperations.GetBindingExpressionBase(thisControl, StandardColorPicker.CommandParameterProperty);
                if (expr != null)
                    expr.UpdateTarget();

                if (thisControl.Command != null && thisControl.Command.CanExecute(thisControl.CommandParameter))
                    thisControl.Command.Execute(thisControl.CommandParameter);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void OnStandardColorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color newSelectedColor = (Color)e.AddedItems[0];
            SetCurrentValue(SelectedColorProperty, newSelectedColor);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(StandardColorPicker), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public Object CommandParameter
        {
            get { return (Object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(Object), typeof(StandardColorPicker), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
