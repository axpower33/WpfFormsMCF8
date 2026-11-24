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
    /// Логика взаимодействия для Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        Entities context = new Entities();
        CollectionViewSource tableDocViewSource;
        CollectionViewSource tableTableChastViewSource;
        CollectionViewSource edIzmViewSource;
        CollectionViewSource kontragentViewSource;
        CollectionViewSource nomenklaturaViewSource;
        int pId;
        Window4 _mainwindow;

        public Window5(int iId, Window4 mainwindow)
        {
            InitializeComponent();
            tableDocViewSource = ((CollectionViewSource)(FindResource("tableDocViewSource")));
            tableTableChastViewSource = ((CollectionViewSource)(FindResource("tableTableChastViewSource")));
            edIzmViewSource = ((CollectionViewSource)(FindResource("edIzmViewSource")));
            kontragentViewSource = ((CollectionViewSource)(FindResource("kontragentViewSource")));
            nomenklaturaViewSource = ((CollectionViewSource)(FindResource("nomenklaturaViewSource")));

            DataContext = this;
            _mainwindow = mainwindow;
            pId = iId;


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            tableDocViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableDocViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableDocViewSource.Source = [универсальный источник данных]
            context.TableDocs.Load();

            tableDocViewSource.Source = context.TableDocs.Local;
     

            tableTableChastViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableTableChastViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableTableChastViewSource.Source = [универсальный источник данных]
            context.TableTableChasts.Load();

            tableTableChastViewSource.Source = context.TableTableChasts.Local;
            
            tableTableChastViewSource.View.Filter = new Predicate<object>(TableTableChast_Filter);
            
            edIzmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("edIzmViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // edIzmViewSource.Source = [универсальный источник данных]
            context.EdIzms.Load();

            edIzmViewSource.Source = context.EdIzms.Local;
            edIzmViewSource.View.MoveCurrentToFirst();

            kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();
            foreach (TableDoc item in tableDocViewSource.View.SourceCollection)
            {
                if (item.Id == pId)
                {
                    tableDocViewSource.View.MoveCurrentTo(item);
                    _mainwindow.Button_Click(sender, e);
                    foreach (TableTableChast item2 in tableTableChastViewSource.View.SourceCollection)
                        if (item2.Id == pId)
                            tableTableChastViewSource.View.MoveCurrentTo(item2);

                    int _ind = Convert.ToInt32(kontragentComboBox.SelectedValue);
                    foreach (Kontragent itkont in kontragentViewSource.View.SourceCollection)
                    {
                        if (itkont.Id == _ind)
                        {
                            kontragentViewSource.View.MoveCurrentTo(itkont);
                        }
                    }
                }
            }
            context.SaveChanges();
            nomenklaturaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nomenklaturaViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // nomenklaturaViewSource.Source = [универсальный источник данных]
            context.Nomenklaturas.Load();

            nomenklaturaViewSource.Source = context.Nomenklaturas.Local;
            nomenklaturaViewSource.View.MoveCurrentToFirst();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TableDoc currentTableDoc = (TableDoc)tableDocViewSource.View.CurrentItem;

            if (currentTableDoc != null)
            {
                TableDoc newTableDoc = new TableDoc
                {
                    Id = currentTableDoc.Id,
                    DataDoc = dataDocDatePicker.DisplayDate,
                    NomerDoc = nomerDocTextBox.Text,
                    Kontragent = Convert.ToInt32(kontragentComboBox.SelectedValue),
                    uid = currentTableDoc.uid
                };

                context.SaveChanges();

                context.TableDocs.Load();
                tableDocViewSource.Source = context.TableDocs.Local;

                tableDocViewSource.View.MoveCurrentToPrevious();
                tableDocViewSource.View.MoveCurrentToLast();

            }

            try
            {

                TableTableChast currentTableTableChast = (TableTableChast)tableTableChastViewSource.View.CurrentItem;

                if (currentTableTableChast != null)
                {
                    TableTableChast newTableTableChast = new TableTableChast
                    {
                        Id = currentTableTableChast.Id,
                        Nomenklatura = currentTableTableChast.Nomenklatura,
                        EdIzm = currentTableTableChast.EdIzm,
                        Kolichestvo = currentTableTableChast.Kolichestvo,
                        Cena = currentTableTableChast.Cena,
                        Summa = currentTableTableChast.Summa,
                        uid = currentTableTableChast.uid
                    };

                    //context.TableTableChasts.Add(newTableTableChast);
                    //tableTableChastViewSource.View.MoveCurrentTo(newTableTableChast);
                    //tableTableChastViewSource.View.Refresh();

                    context.TableTableChasts.Load();
                    tableTableChastViewSource.Source = context.TableTableChasts.Local;

                    context.SaveChanges();

                    tableTableChastViewSource.View.MoveCurrentToPrevious();
                    tableTableChastViewSource.View.MoveCurrentToLast();

                }
                else
                {
                    context.TableTableChasts.Load();
                    tableTableChastViewSource.Source = context.TableTableChasts.Local;
                    tableTableChastViewSource.View.MoveCurrentToLast();
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
            var grid2 = (DataGrid)tableTableChastDataGrid;
            var selIt2 = grid2.SelectedItems.Cast<TableTableChast>().ToArray();

            if (grid2.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid2.SelectedItems.Count + " TableTableChasts?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (TableTableChast item in selIt2)
                    {
                        context.TableTableChasts.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " TableTableChasts have being deleted!");
                }
            }

            tableDocViewSource.View.MoveCurrentToLast();
            tableTableChastViewSource.View.MoveCurrentToLast();

        }

        private void tableTableChastDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (tableTableChastDataGrid.CurrentColumn == null) return;
            int idx = tableTableChastDataGrid.CurrentColumn.DisplayIndex;
            if ((idx == 0) || (idx == 1) || (idx == 2) || (idx == 5) || (idx == 6)) return;
            if (((TableTableChast)tableTableChastViewSource.View.CurrentItem).Id == 0) return;
            if ((idx == 3) || (idx == 4))
            {
                decimal pSumma = ((TableTableChast)tableTableChastViewSource.View.CurrentItem).Kolichestvo * ((TableTableChast)tableTableChastViewSource.View.CurrentItem).Cena;
                ((TableTableChast)tableTableChastViewSource.View.CurrentItem).Summa = pSumma;
                //context.TableTableChasts.Load();
                //tableTableChastViewSource.Source = context.TableTableChasts.Local;

            }
            context.SaveChanges();
            tableTableChastViewSource.View.Refresh();

        }
        public bool TableTableChast_Filter(object de)
        {   
            TableTableChast order = de as TableTableChast;
            return (order.Id == pId);
        }
    }
}
