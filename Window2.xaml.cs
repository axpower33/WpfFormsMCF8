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
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        Entities context = new Entities();
        CollectionViewSource edIzmViewSource;

        public Window2()
        {
            InitializeComponent();
            edIzmViewSource = ((CollectionViewSource)(FindResource("edIzmViewSource")));

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            edIzmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("edIzmViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // edIzmViewSource.Source = [универсальный источник данных]
            context.EdIzms.Load();

            edIzmViewSource.Source = context.EdIzms.Local;
            edIzmViewSource.View.MoveCurrentToFirst();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                EdIzm currentEdIzm = (EdIzm)edIzmViewSource.View.CurrentItem;

                if (currentEdIzm != null)
                {
                    EdIzm newTableDoc = new EdIzm
                    {
                        Id = currentEdIzm.Id,
                        Naimenovanie = currentEdIzm.Naimenovanie,
                    };

                    context.SaveChanges();

                    context.EdIzms.Load();
                    edIzmViewSource.Source = context.EdIzms.Local;

                    edIzmViewSource.View.MoveCurrentToPrevious();
                    edIzmViewSource.View.MoveCurrentToLast();


                }
                else
                {
                    context.EdIzms.Load();
                    edIzmViewSource.Source = context.EdIzms.Local;
                    edIzmViewSource.View.MoveCurrentToLast();
                }

            }
            catch (Exception exMessage)
            {
                MessageBox.Show(exMessage.Message);
            }
            this.Close();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)edIzmDataGrid;
            var selIt = grid.SelectedItems.Cast<EdIzm>().ToArray();

            if (grid.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " EdIzms?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (EdIzm item in selIt)
                    {
                        context.EdIzms.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " EdIzms have being deleted!");
                }
            }


        }
    }
}
