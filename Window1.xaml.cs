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
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;


namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Entities context = new Entities();
        CollectionViewSource kontragentViewSource;

        public Window1()
        {
            InitializeComponent();
            kontragentViewSource = ((CollectionViewSource)(FindResource("kontragentViewSource")));

            DataContext = this;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                Kontragent currentKontragent = (Kontragent)kontragentViewSource.View.CurrentItem;

                if (currentKontragent != null)
                {
                    Kontragent newTableDoc = new Kontragent
                    {
                        Id = currentKontragent.Id,
                        Naimenovanie = currentKontragent.Naimenovanie,
                    };

                    context.SaveChanges();

                    context.Kontragents.Load();
                    kontragentViewSource.Source = context.Kontragents.Local;

                    kontragentViewSource.View.MoveCurrentToPrevious();
                    kontragentViewSource.View.MoveCurrentToLast();


                }
                else
                {
                    context.Kontragents.Load();
                    kontragentViewSource.Source = context.Kontragents.Local;
                    kontragentViewSource.View.MoveCurrentToLast();
                }

            }
            catch (Exception exMessage)
            {
                MessageBox.Show(exMessage.Message);
            }
            this.Close();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)kontragentDataGrid;
            var selIt = grid.SelectedItems.Cast<Kontragent>().ToArray();

            if (grid.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " Kontragents?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (Kontragent item in selIt)
                    {
                        context.Kontragents.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " Kontragents have being deleted!");
                }
            }

        }
    }
}
