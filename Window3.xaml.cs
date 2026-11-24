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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
 
    public partial class Window3 : Window
    {
        Entities context = new Entities();
        CollectionViewSource nomenklaturaViewSource;

        public Window3()
        {
            InitializeComponent();
            nomenklaturaViewSource = ((CollectionViewSource)(FindResource("nomenklaturaViewSource")));

            DataContext = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            nomenklaturaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nomenklaturaViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // nomenklaturaViewSource.Source = [универсальный источник данных]
            context.Nomenklaturas.Load();

            nomenklaturaViewSource.Source = context.Nomenklaturas.Local;
            nomenklaturaViewSource.View.MoveCurrentToFirst();

            System.Windows.Data.CollectionViewSource kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();


            System.Windows.Data.CollectionViewSource edIzmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("edIzmViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // edIzmViewSource.Source = [универсальный источник данных]
            context.EdIzms.Load();

            edIzmViewSource.Source = context.EdIzms.Local;
            edIzmViewSource.View.MoveCurrentToFirst();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Nomenklatura currentNomenklatura = (Nomenklatura)nomenklaturaViewSource.View.CurrentItem;

            if (currentNomenklatura != null)
            {
                Nomenklatura newNomenklatura = new Nomenklatura
                {
                    Id = currentNomenklatura.Id,
                    Naimenovanie = currentNomenklatura.Naimenovanie,
                    EdIzm = currentNomenklatura.EdIzm,
                    Kontragent = currentNomenklatura.Kontragent
                };

                context.SaveChanges();

                context.Nomenklaturas.Load();
                nomenklaturaViewSource.Source = context.Nomenklaturas.Local;

                nomenklaturaViewSource.View.MoveCurrentToPrevious();
                nomenklaturaViewSource.View.MoveCurrentToLast();


            }
            else
            {
                context.EdIzms.Load();
                nomenklaturaViewSource.Source = context.Nomenklaturas.Local;
                nomenklaturaViewSource.View.MoveCurrentToLast();
            }
            this.Close();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)nomenklaturaDataGrid;
            var selIt = grid.SelectedItems.Cast<Nomenklatura>().ToArray();

            if (grid.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " Nomenklaturas?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (Nomenklatura item in selIt)
                    {
                        context.Nomenklaturas.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " Nomenklaturas have being deleted!");
                }
            }

        }
    }
}
