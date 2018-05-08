using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ImageEvents
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        float previousLevel;
        ScaleTransform scaleTransform;
        TranslateTransform translateTransform;
        TransformGroup transformGroup;
        bool isScale;
        int touchCount;
        int zoomLevel;
        double previousCumlativeScale;
        double previousTranslateX;
        double previousTranslateY;
        double previousXPosition;
        double previousYPosition;
        float currentZoomLevel;

        public MainPage()
        {
            this.InitializeComponent();
            zoomLevel = 1;
            scaleTransform = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
            translateTransform = new TranslateTransform() { X = 0, Y = 0 };
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            grid.ManipulationDelta += Grid_ManipulationDelta;
            grid.ManipulationStarted += Grid_ManipulationStarted;
            grid.PointerPressed += Grid_PointerPressed;
            grid.PointerExited += Grid_PointerExited;
            grid.PointerReleased += Grid_PointerReleased;
        }

        private void Grid_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            touchCount = 0;     
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
       
         
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
                touchCount++;
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            previousLevel = e.Cumulative.Scale;
            if (scaleTransform.ScaleX > 1)
                isScale = true;
        }



        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (previousCumlativeScale != e.Cumulative.Scale && touchCount > 1)
            {
                if ((e.Cumulative.Scale < 1 && zoomLevel > 1) || e.Cumulative.Scale > 1)
                {

                    if (isScale)
                    {
                        scaleTransform.ScaleX = previousCumlativeScale * e.Delta.Scale;
                        scaleTransform.ScaleY = previousCumlativeScale * e.Delta.Scale;
                        translateTransform.X = previousTranslateX + e.Delta.Translation.X;
                        translateTransform.Y = previousTranslateY + e.Delta.Translation.Y;
                        scaleTransform.CenterX = (previousXPosition + e.Position.X) / 2;
                        scaleTransform.CenterY = (previousYPosition + e.Position.Y) / 2;
                    }
                    else
                    {
                        scaleTransform.ScaleX = e.Cumulative.Scale;
                        scaleTransform.ScaleY = e.Cumulative.Scale;
                        translateTransform.X = e.Cumulative.Translation.X;
                        translateTransform.Y = e.Cumulative.Translation.Y;
                    }


                    scaleTransform.CenterX = e.Position.X;
                    scaleTransform.CenterY = e.Position.Y;

                    this.RenderTransform = transformGroup;
                }

                if (previousLevel != scaleTransform.ScaleX)
                {
                    currentZoomLevel = this.zoomLevel * (float)scaleTransform.ScaleX;
                      ChangeLevel(e.Position);
                }
                previousCumlativeScale = scaleTransform.ScaleX;
                previousTranslateX = translateTransform.X;
                previousTranslateY = translateTransform.Y;
                previousXPosition = scaleTransform.CenterX;
                previousYPosition = scaleTransform.CenterY;

            }
        }

        private void ChangeLevel(Point position)
        {
            if (Math.Abs((int)(currentZoomLevel) - (int)(zoomLevel)) >= 1)
            {
                    image.Source = new BitmapImage(new Uri("ms-appx:///Assets/image2.png"));
               
            }
        
        }
    }
}
