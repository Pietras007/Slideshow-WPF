using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Vertical
{
    public class Vertical : ISlideshowEffect.ISlideshowEffect
    {
        public string Name => "Vertical Effect";

        public void JustEffect(System.Windows.Controls.Image imageIn, System.Windows.Controls.Image imageOut)
        {
            Storyboard Story1 = new Storyboard‫‎‮‏‏‎‎⁫⁪⁮⁮‫‬⁭‌⁮‭​‍‭‬‬⁮‬‪‮⁪⁮‌⁯‍‬⁯‫‌‏⁯‬⁫‮();
            DoubleAnimation Animation1 = new DoubleAnimation(0, 768, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation1, new PropertyPath(FrameworkElement.HeightProperty));

            ThicknessAnimation Thickness1 = new ThicknessAnimation(new Thickness(0, 768, 0, 0), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 1, 250), 0);
            Storyboard.SetTargetProperty(Thickness1, new PropertyPath(FrameworkElement.MarginProperty));

            Storyboard.SetTarget(Thickness1, imageIn);
            Storyboard.SetTarget(Animation1, imageIn);

            Storyboard Story2 = new Storyboard‫‎‮‏‏‎‎⁫⁪⁮⁮‫‬⁭‌⁮‭​‍‭‬‬⁮‬‪‮⁪⁮‌⁯‍‬⁯‫‌‏⁯‬⁫‮();
            DoubleAnimation Animation2 = new DoubleAnimation(768, 0, new TimeSpan(0, 0, 0, 1, 250));
            Storyboard.SetTargetProperty(Animation2, new PropertyPath(FrameworkElement.HeightProperty));
            Storyboard.SetTarget(Animation2, imageOut);

            Story1.Children.Add(Animation1);
            Story1.Children.Add(Thickness1);
            Story1.Begin();

            Story2.Children.Add(Animation2);
            Story2.Begin();
        }
    }
}
