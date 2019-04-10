using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Windows.Threading;
using System.Threading;
using System.Reflection;

namespace WpfApp1
{
    public class File_Info
    {
        public string Name { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Size { get; set; }
    }

    public struct Image_
    {
        public string Path { get; set; }
        public string File_Name { get; set; }
    }

    public partial class MainWindow : Window
    {
        public static string[] Combo_Item = { };
        public static Type[] Typy = { };
        public static ObservableCollection<Image_> images = new ObservableCollection<Image_>() { };

        private struct MainWindowData
        {
            public ObservableCollection<Image_> Images { get; set; }
            public File_Info Info_file { get; set; }
            public string[] Combo_Items { get; set; }
            public string[] List_Items { get; set; }
        }

        File_Info info = new File_Info()
        {
        };

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Load_DLL_Files();
            List<string> sciezki = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
                trvStructure.Items.Add(CreateTreeItem(driveInfo));

            Visibility_Stack_Panel1.Visibility = Visibility.Collapsed;
            Visibility_Stack_Panel2.Visibility = Visibility.Visible;

            var DataForBinding = new MainWindowData();
            DataForBinding.Images = images;
            DataForBinding.Info_file = info;
            DataForBinding.Combo_Items = Combo_Item;
            this.DataContext = DataForBinding;
        }

        void Load_DLL_Files()
        {
            Assembly[] assembly = { };
            string[] paths = Directory.GetFiles(".");
            for (int i = 0; i < paths.Length; i++)
            {
                int dlugosc = paths[i].Length;
                if (paths[i][dlugosc - 1] == 'l' && paths[i][dlugosc - 2] == 'l' && paths[i][dlugosc - 3] == 'd')
                {
                    int newsize = assembly.Length + 1;
                    Array.Resize(ref assembly, newsize);
                    assembly[newsize - 1] = Assembly.LoadFrom(paths[i]);
                }
            }

            List<Type> xyz = new List<Type>();
            foreach (var x in assembly)
            {
                xyz.AddRange(x.GetExportedTypes());
            }

            Type[] types = xyz.ToArray();
            foreach(var a in types)
            {
                try
                {
                    object instanceOfMyType = Activator.CreateInstance(a);
                    if ((instanceOfMyType as ISlideshowEffect.ISlideshowEffect) != null)
                    {
                        var abcd = instanceOfMyType as ISlideshowEffect.ISlideshowEffect;
                        int newsize = Combo_Item.Length + 1;
                        Array.Resize(ref Combo_Item, newsize);
                        Combo_Item[newsize - 1] = abcd.Name;
                        int newsize2 = Typy.Length + 1;
                        Array.Resize(ref Typy, newsize2);
                        Typy[newsize2 - 1] = a;
                    }
                }
                catch (System.MissingMethodException)
                {
                }
            }
        }

        public void Object_Click(object obj, RoutedEventArgs e)
        {
            string myValue = ((StackPanel)obj).Tag as string;
            FileInfo image = new FileInfo(myValue);
            info.Name = image.Name;
            string size = (image.Length / 1024.0).ToString();
            int i = 0;
            string result = "";
            while((size[i] != '.') && (size[i] !=',') && i<size.Length)
            {
                result += size[i];
                i++;
            }

            for(int x=0;x<3 && i < size.Length; x++, i++)
            {
                result += size[i];
                i++;
            }

            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(myValue, UriKind.Absolute);
                bitmap.EndInit();
            }
            catch (NotSupportedException)
            {
            }

            info.Width = bitmap.PixelWidth.ToString();
            info.Height = bitmap.PixelHeight.ToString();

            info.Size = result;
            BindingExpression be1 = File_Name_BOX.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression be2 = Size_BOX.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression be3 = Width_BOX.GetBindingExpression(TextBlock.TextProperty);
            BindingExpression be4 = Height_BOX.GetBindingExpression(TextBlock.TextProperty);
            be1.UpdateTarget();
            be2.UpdateTarget();
            be3.UpdateTarget();
            be4.UpdateTarget();

            Visibility_Stack_Panel1.Visibility = Visibility.Visible;
            Visibility_Stack_Panel2.Visibility = Visibility.Collapsed;
        }

        public void OpenFile_Click(object obj, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            DialogResult a = dlg.ShowDialog();
            if (a == System.Windows.Forms.DialogResult.OK)
            {
                Take_Files(dlg.SelectedPath);
            }
            Visibility_Stack_Panel1.Visibility = Visibility.Collapsed;
            Visibility_Stack_Panel2.Visibility = Visibility.Visible;
        }

        public void Take_Files(string folder_n)
        {
            images.Clear();
            string folderName = folder_n;
            string[] paths = Directory.GetFiles(folderName);
            for (int i = 0; i < paths.Length; i++)
            {
                int dlugosc = paths[i].Length;
                if (paths[i][dlugosc - 1] == 'g' && paths[i][dlugosc - 2] == 'p' && paths[i][dlugosc - 3] == 'j')
                {
                    string[] tab = paths[i].Split('\\');
                    images.Add(new Image_() { Path = paths[i], File_Name = tab[tab.Length - 1] });
                }
            }
        }

        public void About_Click(object obj, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("This is a simple image slideshow application.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Exit_Click(object obj, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void Start_Slideshow(object obj, RoutedEventArgs e)
        {
            if (images.Count > 0)
            {
                System.Windows.Controls.Button item = e.Source as System.Windows.Controls.Button;
                ImageSlideshow.Window1 w1 = new ImageSlideshow.Window1(images, (int)item.Tag, Typy);
                w1.ShowDialog();
            }
            else
            { 
                System.Windows.MessageBox.Show("The selected folder does not contain any image to start slideshow!", "An error occured", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void Effect_Click(object obj, RoutedEventArgs e)
        {
            if (images.Count > 0)
            {
                System.Windows.Controls.MenuItem item = e.OriginalSource as System.Windows.Controls.MenuItem;
                string nazwa = item.Header as string;
                int i = 0;
                foreach(var a in Combo_Item)
                {
                    if(a == nazwa)
                    {
                        break;
                    }
                    i++;
                }
                ImageSlideshow.Window1 w1 = new ImageSlideshow.Window1(images, i, Typy);
                w1.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("The selected folder does not contain any image to start slideshow!", "An error occured", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void Tree_Click(object obj, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            DirectoryInfo expandedDir = null;
            if (item.Tag is DriveInfo)
                expandedDir = (item.Tag as DriveInfo).RootDirectory;
            if (item.Tag is DirectoryInfo)
                expandedDir = (item.Tag as DirectoryInfo);

            string fullpath = expandedDir.FullName;
            string folderName = fullpath;
            images.Clear();
            try
            {
                string[] paths = Directory.GetFiles(folderName);
                for (int i = 0; i < paths.Length; i++)
                {
                    int dlugosc = paths[i].Length;
                    if (paths[i][dlugosc - 1] == 'g' && paths[i][dlugosc - 2] == 'p' && paths[i][dlugosc - 3] == 'j')
                    {
                        string[] tab = paths[i].Split('\\');
                        images.Add(new Image_() { Path = paths[i], File_Name = tab[tab.Length - 1] });
                    }
                }
            }
            catch
            {
            }
            Visibility_Stack_Panel1.Visibility = Visibility.Collapsed;
            Visibility_Stack_Panel2.Visibility = Visibility.Visible;
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();

                DirectoryInfo expandedDir = null;
                if (item.Tag is DriveInfo)
                    expandedDir = (item.Tag as DriveInfo).RootDirectory;
                if (item.Tag is DirectoryInfo)
                    expandedDir = (item.Tag as DirectoryInfo);

                try
                {
                    foreach (DirectoryInfo subDir in expandedDir.GetDirectories())
                        item.Items.Add(CreateTreeItem(subDir));
                }
                catch { }
            }
        }

        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = o.ToString();
            item.Tag = o;
            item.Items.Add("Loading...");
            return item;
        }
    }
}
