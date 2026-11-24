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

namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window1 x = new Window1();
            x.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 x = new Window2();
            x.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Window3 x = new Window3();
            x.Show();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Window4 x = new Window4();
            x.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            Window6 x = new Window6();
            x.Show();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Window8 x = new Window8();
            x.Show();

        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            Window9 x = new Window9();
            x.Show();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            Window10 x = new Window10();
            x.Show();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            Window11 x = new Window11();
            x.Show();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            Window12 x = new Window12();
            x.Show();
        }
    }
}
