using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Configuration;
using System.Collections.ObjectModel;

namespace ImageSlideshow
{
    public partial class Window1 : Window
    {
        private DispatcherTimer timerImageChange;
        private Image[] ImageControls;
        private List<ImageSource> Images = new List<ImageSource>();

        private int CurrentSourceIndex, EffectIndex, IntervalTimer;
        ObservableCollection<WpfApp1.Image_> images = new ObservableCollection<WpfApp1.Image_>();
        bool is_first = true;
        Type[] Types;

        public Window1(ObservableCollection<WpfApp1.Image_> imag, int type, Type[] Type)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Types = Type;
            int i = 0;
            foreach (var a in WpfApp1.MainWindow.Combo_Item)
            {
                string[] pierwszy = a.Split(' ');
                i++;
            }
            EffectIndex = type;
            images = imag;
            IntervalTimer = 5;
            ImageControls = new[] { myImage, myImage2 };
            LoadImages();
            timerImageChange = new DispatcherTimer();
            timerImageChange.Interval = new TimeSpan(0, 0, IntervalTimer);
            timerImageChange.Tick += new EventHandler(timerImageChange_Tick);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PlaySlideShow();
            timerImageChange.IsEnabled = true;
        }

        private void LoadImages()
        {
            Images.Clear();
            foreach (var a in images)
            {
                Images.Add(CreateImageSource(a.Path));
            }
        }

        private ImageSource CreateImageSource(string file)
        {
            var src = new BitmapImage(new Uri(file, UriKind.Absolute));
            src.Freeze();
            return src;
        }

        private void timerImageChange_Tick(object sender, EventArgs e)
        {
            PlaySlideShow();
        }

        private void PlaySlideShow()
        {
            try
            {
                if (Images.Count == 0)
                {
                    return;
                }
                ImageSource lastSource = Images[CurrentSourceIndex];
                if (is_first == false)
                {
                    CurrentSourceIndex = (CurrentSourceIndex + 1) % Images.Count;
                }

                Image imgLast = ImageControls[0];
                Image imgNow = ImageControls[1];
                ImageSource newSource = Images[CurrentSourceIndex];
                if (is_first == false)
                {
                    imgLast.Source = lastSource;
                }
                is_first = false;
                imgNow.Source = newSource;

                object instanceOfMyType = Activator.CreateInstance(Types[EffectIndex]);
                var EFEKT = instanceOfMyType as ISlideshowEffect.ISlideshowEffect;
                EFEKT.JustEffect(imgNow, imgLast);
            }
            catch (Exception ex) { }
        }

        public void Click_PlayPause(object obj, RoutedEventArgs e)
        {
            if (timerImageChange.IsEnabled == true)
            {
                timerImageChange.IsEnabled = false;
            }
            else
            {
                timerImageChange.IsEnabled = true;
            }
        }

        public void Click_Stop(object obj, RoutedEventArgs e)
        {
            Close();
        }
    }
}

