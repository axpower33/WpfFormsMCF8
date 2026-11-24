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
    /// Логика взаимодействия для Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        Entities context = new Entities();
        CollectionViewSource tableDocViewSource;
        Window4 _mainwindow; 

        public Window4()
        {
            InitializeComponent();
            tableDocViewSource = ((CollectionViewSource)(FindResource("tableDocViewSource")));

            DataContext = this;
            _mainwindow = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource tableDocViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableDocViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableDocViewSource.Source = [универсальный источник данных]
            context.TableDocs.Load();

            tableDocViewSource.Source = context.TableDocs.Local;
            tableDocViewSource.View.MoveCurrentToFirst();

            System.Windows.Data.CollectionViewSource kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();

        }

        internal void Button_Click(object sender, RoutedEventArgs e)
        {
            TableDoc currentTableDoc = (TableDoc)tableDocViewSource.View.CurrentItem;

            if (currentTableDoc != null)
            {
                TableDoc newTableDoc = new TableDoc
                {
                    Id = currentTableDoc.Id,
                    DataDoc = currentTableDoc.DataDoc,
                    NomerDoc = currentTableDoc.NomerDoc,
                    Kontragent = currentTableDoc.Kontragent,
                    uid = currentTableDoc.uid
                };

                context.SaveChanges();

                context.TableDocs.Load();
                tableDocViewSource.Source = context.TableDocs.Local;

                tableDocViewSource.View.MoveCurrentToPrevious();
                tableDocViewSource.View.MoveCurrentToLast();


            }
            else
            {
                context.TableDocs.Load();
                tableDocViewSource.Source = context.TableDocs.Local;
                tableDocViewSource.View.MoveCurrentToLast();
            }
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)tableDocDataGrid;
            var selIt = grid.SelectedItems.Cast<TableDoc>().ToArray();

            if (grid.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " TableDocs?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (TableDoc item in selIt)
                    {
                        context.TableDocs.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " TableDocs have being deleted!");
                }
            }

        }

        private void tableDocDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            int tId = ((TableDoc)tableDocViewSource.View.CurrentItem).Id;
             
            Window5 x = new Window5(tId, _mainwindow);
            x.Show();
        }
    }
}
