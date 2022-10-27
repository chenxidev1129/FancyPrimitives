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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Diagnostics;

namespace FancyPrimitives
{
    internal partial class PenSelectorWindow : Window
    {
        private PenSelector penSelector;
        //----------------------------------------------------------------------------------------------------------------------------------
        public PenSelectorWindow()
        {
            InitializeComponent();
        }

        public PenSelectorWindow(PenSelector penSelector)
        {
            this.penSelector = penSelector;
            DataContext = penSelector;

            PropertyInfo[] props = typeof(Colors).GetProperties();
            List<Color> arr = props.Select(p => (Color)p.GetValue(null)).ToList();

            Color currentColor = (penSelector.SelectedPen.Brush as SolidColorBrush).Color;
            Byte currentAlpha = currentColor.A;
            currentColor = Color.FromArgb(255, currentColor.R, currentColor.G, currentColor.B);
            if (FindStandardColor(arr, currentColor) == -1)
                arr.Add(currentColor);
            else
                arr.Add(Color.FromArgb(255, 11, 222, 33));

            StandardColors = arr.ToArray();

            InitializeComponent();

            listBoxStdColors.SelectedValue = currentColor;
            txtBoxThickness.Text = ((int)penSelector.SelectedPen.Thickness).ToString();
            sliderAlphaChannel.Value = currentAlpha;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public Color[] StandardColors { get; private set; }

        private int FindStandardColor(IList<Color> stdColors, Color color)
        {
            for (int i = 0; i < stdColors.Count; i++)
                if (stdColors[i] == color) return i;
            return -1;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            DataContext = null;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            DataContext = null;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void listBoxStdColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Byte alpha = (Byte)sliderAlphaChannel.Value;
            Color newColor = (Color)e.AddedItems[0];
            newColor = Color.FromArgb(alpha, newColor.R, newColor.G, newColor.B);
            penSelector.SetCurrentValue(PenSelector.SelectedPenProperty, new Pen(new SolidColorBrush(newColor), penSelector.SelectedPen.Thickness));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void sliderAlphaChannel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Byte alpha = (Byte)sliderAlphaChannel.Value;
            Color oldColor = (penSelector.SelectedPen.Brush as SolidColorBrush).Color;
            Color newColor = Color.FromArgb(alpha, oldColor.R, oldColor.G, oldColor.B);
            penSelector.SetCurrentValue(PenSelector.SelectedPenProperty, new Pen(new SolidColorBrush(newColor), penSelector.SelectedPen.Thickness));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void txtBoxThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            double.TryParse(txtBoxThickness.Text, out double th);
            penSelector.SetCurrentValue(PenSelector.SelectedPenProperty, new Pen(penSelector.SelectedPen.Brush, th));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void WindowTitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
