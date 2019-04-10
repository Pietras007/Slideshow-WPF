using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace OpacityEffect
{
    public class OpacityEffect : ISlideshowEffect.ISlideshowEffect
    {
        public string Name => "Opacity Effect";

        public void JustEffect(Image imageIn, Image imageOut)
        {
            Storyboard Story1 = new Storyboard‫‎‮‏‏‎‎⁫⁪⁮⁮‫‬⁭‌⁮‭​‍‭‬‬⁮‬‪‮⁪⁮‌⁯‍‬⁯‫‌‏⁯‬⁫‮();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(Animation1, imageIn);


            Storyboard Story2 = new Storyboard‫‎‮‏‏‎‎⁫⁪⁮⁮‫‬⁭‌⁮‭​‍‭‬‬⁮‬‪‮⁪⁮‌⁯‍‬⁯‫‌‏⁯‬⁫‮();
            DoubleAnimation Animation2 = new DoubleAnimation(1, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(UIElement.OpacityProperty));
            Storyboard.SetTarget(Animation2, imageOut);

            Story1.Children.Add(Animation1);
            Story1.Begin();

            Story2.Children.Add(Animation2);
            Story2.Begin();
        }
    }
}
