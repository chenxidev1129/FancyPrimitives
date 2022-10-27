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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FancyPrimitives
{
    /// <summary>Interaction logic for PenSelector.xaml</summary>
    public partial class PenSelector : UserControl
    {
        public PenSelector()
        {
            InitializeComponent();
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public static Pen DefaultSelectedPen = (Pen)(new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)), 1)).GetAsFrozen();

        public Pen SelectedPen
        {
            get { return (Pen)GetValue(SelectedPenProperty); }
            set { SetValue(SelectedPenProperty, value); }
        }
        public static readonly DependencyProperty SelectedPenProperty =
            DependencyProperty.Register("SelectedPen", typeof(Pen), typeof(PenSelector), new PropertyMetadata(DefaultSelectedPen, OnSelectedPenChanged));

        static void OnSelectedPenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            /*PenSelector thisControl = obj as PenSelector;
            if (thisControl == null) return;

            Pen newSelectedPen = (Pen)e.NewValue;

            if (thisControl.FindStandardColor(newSelectedPen) == -1)
            {
                Color col = Color.FromArgb(255, newSelectedPen.R, newSelectedPen.G, newSelectedPen.B);
                thisControl.StandardColors[thisControl.StandardColors.Length - 1] = col;
                thisControl.comboBoxStandardColor.SelectedValue = col;
            }
            else
                thisControl.comboBoxStandardColor.SelectedValue = newSelectedPen;

            thisControl.sliderAlphaChannel.Value = newSelectedPen.A;

            if (e.NewValue != null && thisControl.IsLoaded)
            {
                BindingExpressionBase expr = BindingOperations.GetBindingExpressionBase(thisControl, PenSelector.CommandParameterProperty);
                if (expr != null)
                    expr.UpdateTarget();

                if (thisControl.Command != null && thisControl.Command.CanExecute(thisControl.CommandParameter))
                    thisControl.Command.Execute(thisControl.CommandParameter);
            }

            */
        }

        private void OnMainButtonClick(object sender, RoutedEventArgs e)
        {
            Pen oldSelectedPen = SelectedPen.Clone();

            PenSelectorWindow popup = new PenSelectorWindow(this);
            popup.Owner = Window.GetWindow(this);
            if (popup.ShowDialog() == true)
            {
            }
            else
                SelectedPen = oldSelectedPen;
        }
    }
}
