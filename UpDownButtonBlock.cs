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
using System.Windows.Controls.Primitives;

namespace FancyPrimitives
{
    [TemplatePart(Name = "PART_UpButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "PART_DownButton", Type = typeof(ButtonBase))]
    public class UpDownButtonBlock : Control
    {
        //----------------------------------------------------------------------------------------------------------------------------------
        static UpDownButtonBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UpDownButtonBlock), new FrameworkPropertyMetadata(typeof(UpDownButtonBlock)));
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public override void OnApplyTemplate()
        {
            ButtonBase upButton = GetTemplateChild("PART_UpButton") as ButtonBase;
            if (upButton != null)
                upButton.Click -= OnUpButtonClick;

            ButtonBase downButton = GetTemplateChild("PART_DownButton") as ButtonBase;
            if (downButton != null)
                downButton.Click -= OnDownButtonClick;


            base.OnApplyTemplate();

            upButton = GetTemplateChild("PART_UpButton") as ButtonBase;
            if (upButton != null)
                upButton.Click += OnUpButtonClick;

            downButton = GetTemplateChild("PART_DownButton") as ButtonBase;
            if (downButton != null)
                downButton.Click += OnDownButtonClick;
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void OnUpButtonClick(object sender, RoutedEventArgs e)
        {
            if (UpButtonPressedCommand != null && UpButtonPressedCommand.CanExecute(UpButtonPressedCommandParameter))
                UpButtonPressedCommand.Execute(UpButtonPressedCommandParameter);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        private void OnDownButtonClick(object sender, RoutedEventArgs e)
        {
            if (DownButtonPressedCommand != null && DownButtonPressedCommand.CanExecute(DownButtonPressedCommandParameter))
                DownButtonPressedCommand.Execute(DownButtonPressedCommandParameter);
        }
        //----------------------------------------------------------------------------------------------------------------------------------
        public ICommand UpButtonPressedCommand
        {
            get { return (ICommand)GetValue(UpButtonPressedCommandProperty); }
            set { SetValue(UpButtonPressedCommandProperty, value); }
        }
        public static readonly DependencyProperty UpButtonPressedCommandProperty =
            DependencyProperty.Register("UpButtonPressedCommand", typeof(ICommand), typeof(UpDownButtonBlock), new PropertyMetadata(null));

        public Object UpButtonPressedCommandParameter
        {
            get { return (Object)GetValue(UpButtonPressedCommandParameterProperty); }
            set { SetValue(UpButtonPressedCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty UpButtonPressedCommandParameterProperty =
            DependencyProperty.Register("UpButtonPressedCommandParameter", typeof(Object), typeof(UpDownButtonBlock), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        public ICommand DownButtonPressedCommand
        {
            get { return (ICommand)GetValue(DownButtonPressedCommandProperty); }
            set { SetValue(DownButtonPressedCommandProperty, value); }
        }
        public static readonly DependencyProperty DownButtonPressedCommandProperty =
            DependencyProperty.Register("DownButtonPressedCommand", typeof(ICommand), typeof(UpDownButtonBlock), new PropertyMetadata(null));

        public Object DownButtonPressedCommandParameter
        {
            get { return (Object)GetValue(DownButtonPressedCommandParameterProperty); }
            set { SetValue(DownButtonPressedCommandParameterProperty, value); }
        }
        public static readonly DependencyProperty DownButtonPressedCommandParameterProperty =
            DependencyProperty.Register("DownButtonPressedCommandParameter", typeof(Object), typeof(UpDownButtonBlock), new PropertyMetadata(null));
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------
    }
}
