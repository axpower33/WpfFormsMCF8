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
    /// Логика взаимодействия для Window6.xaml
    /// </summary>
    public partial class Window6 : Window
    {
        Entities context = new Entities();
        CollectionViewSource tableDoc2ViewSource;
        CollectionViewSource kontragentViewSource;
        Window6 _mainwindow;
        public Window6()
        {
            InitializeComponent();
            tableDoc2ViewSource = ((CollectionViewSource)(FindResource("tableDoc2ViewSource")));

            DataContext = this;
            _mainwindow = this;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            tableDoc2ViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableDoc2ViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableDoc2ViewSource.Source = [универсальный источник данных]
            context.TableDoc2.Load();

            tableDoc2ViewSource.Source = context.TableDoc2.Local;
            tableDoc2ViewSource.View.MoveCurrentToFirst();
            kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();
        }

        internal void Button_Click(object sender, RoutedEventArgs e)
        {
            TableDoc2 currentTableDoc2 = (TableDoc2)tableDoc2ViewSource.View.CurrentItem;

            if (currentTableDoc2 != null)
            {
                TableDoc2 newTableDoc2 = new TableDoc2
                {
                    Id = currentTableDoc2.Id,
                    DataDoc = currentTableDoc2.DataDoc,
                    NomerDoc = currentTableDoc2.NomerDoc,
                    Kontragent = currentTableDoc2.Kontragent,
                    uid = currentTableDoc2.uid
                };

                //tableDoc2ViewSource.View.Refresh();

                context.SaveChanges();
                
                context.TableDoc2.Load();
                tableDoc2ViewSource.Source = context.TableDoc2.Local;

                tableDoc2ViewSource.View.MoveCurrentToPrevious();
                tableDoc2ViewSource.View.MoveCurrentToLast();
                
            }
            else
            {
                context.TableDoc2.Load();
                tableDoc2ViewSource.Source = context.TableDoc2.Local;
                tableDoc2ViewSource.View.MoveCurrentToLast();
            }
            
            this.Close();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid)tableDoc2DataGrid;
            var selIt = grid.SelectedItems.Cast<TableDoc2>().ToArray();

            if (grid.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid.SelectedItems.Count + " TableDoc2s?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (TableDoc2 item in selIt)
                    {
                        context.TableDoc2.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " TableDoc2s have being deleted!");
                }
            }

        }

        private void tableDoc2DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int tId = ((TableDoc2)tableDoc2ViewSource.View.CurrentItem).Id;
            
            Window7 x = new Window7(tId, _mainwindow);
            x.Show();
            
        }
    }
}
