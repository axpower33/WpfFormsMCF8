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
using System.Collections.ObjectModel;



namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для Window7.xaml
    /// </summary>
    public partial class Window7 : Window
    {
        Entities context = new Entities();
        CollectionViewSource tableDoc2ViewSource;
        CollectionViewSource tableTableChast2ViewSource;
        CollectionViewSource edIzmViewSource;
        CollectionViewSource kontragentViewSource;
        CollectionViewSource nomenklaturaViewSource;
        int pId;
        Window6 _mainwindow;

        public Window7(int iId, Window6 mainwindow)
        {
            InitializeComponent();
            tableDoc2ViewSource = ((CollectionViewSource)(FindResource("tableDoc2ViewSource")));
            tableTableChast2ViewSource = ((CollectionViewSource)(FindResource("tableTableChast2ViewSource")));
            edIzmViewSource = ((CollectionViewSource)(FindResource("edIzmViewSource")));
            kontragentViewSource = ((CollectionViewSource)(FindResource("kontragentViewSource")));
            nomenklaturaViewSource = ((CollectionViewSource)(FindResource("nomenklaturaViewSource")));

            DataContext = this;
            
            pId = iId;
            _mainwindow = mainwindow;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            tableTableChast2ViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableTableChast2ViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableTableChast2ViewSource.Source = [универсальный источник данных]
            context.TableTableChast2.Load();

            tableTableChast2ViewSource.Source = context.TableTableChast2.Local;

            tableTableChast2ViewSource.View.Filter = new Predicate<object>(TableTableChast_Filter);

            tableDoc2ViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tableDoc2ViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // tableDoc2ViewSource.Source = [универсальный источник данных]
            context.TableDoc2.Load();

            tableDoc2ViewSource.Source = context.TableDoc2.Local;
            
            edIzmViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("edIzmViewSource")));
            context.EdIzms.Load();

            edIzmViewSource.Source = context.EdIzms.Local;
            edIzmViewSource.View.MoveCurrentToFirst();

            kontragentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("kontragentViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // kontragentViewSource.Source = [универсальный источник данных]
            context.Kontragents.Load();

            kontragentViewSource.Source = context.Kontragents.Local;
            kontragentViewSource.View.MoveCurrentToFirst();

            foreach (TableDoc2 item in tableDoc2ViewSource.View.SourceCollection)
            {
                if (item.Id == pId)
                {
                    tableDoc2ViewSource.View.MoveCurrentTo(item);
                    _mainwindow.Button_Click(sender, e);
                    foreach (TableTableChast2 item2 in tableTableChast2ViewSource.View.SourceCollection)
                        if (item2.Id==pId)
                            tableTableChast2ViewSource.View.MoveCurrentTo(item2);
                    
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

            nomenklaturaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("nomenklaturaViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // nomenklaturaViewSource.Source = [универсальный источник данных]
            context.Nomenklaturas.Load();

            nomenklaturaViewSource.Source = context.Nomenklaturas.Local;
            nomenklaturaViewSource.View.MoveCurrentToFirst();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TableDoc2 currentTableDoc2 = (TableDoc2)tableDoc2ViewSource.View.CurrentItem;

            if (currentTableDoc2 != null)
            {
                TableDoc2 newTableDoc2 = new TableDoc2
                {
                    Id = currentTableDoc2.Id,
                    DataDoc = dataDocDatePicker.DisplayDate,
                    NomerDoc = nomerDocTextBox.Text,
                    Kontragent = Convert.ToInt32(kontragentComboBox.SelectedValue),
                    uid = currentTableDoc2.uid
                };

                context.SaveChanges();

                context.TableDoc2.Load();
                tableDoc2ViewSource.Source = context.TableDoc2.Local;

                tableDoc2ViewSource.View.MoveCurrentToPrevious();
                tableDoc2ViewSource.View.MoveCurrentToLast();
            }

            try
            {

                TableTableChast2 currentTableTableChast2 = (TableTableChast2)tableTableChast2ViewSource.View.CurrentItem;

                if (currentTableTableChast2 != null)
                {
                    TableTableChast2 newTableTableChast2 = new TableTableChast2
                    {
                        Id = currentTableTableChast2.Id,
                        Nomenklatura = currentTableTableChast2.Nomenklatura,
                        EdIzm = currentTableTableChast2.EdIzm,
                        Kolichestvo = currentTableTableChast2.Kolichestvo,
                        Cena = currentTableTableChast2.Cena,
                        Summa = currentTableTableChast2.Summa,
                        uid = currentTableTableChast2.uid
                    };

                    //context.TableTableChasts.Add(newTableTableChast);
                    //tableTableChastViewSource.View.MoveCurrentTo(newTableTableChast);
                    //tableTableChastViewSource.View.Refresh();

                    context.TableTableChast2.Load();
                    tableTableChast2ViewSource.Source = context.TableTableChast2.Local;

                    context.SaveChanges();

                    tableTableChast2ViewSource.View.MoveCurrentToPrevious();
                    tableTableChast2ViewSource.View.MoveCurrentToLast();

                }
                else
                {
                    context.TableTableChast2.Load();
                    tableTableChast2ViewSource.Source = context.TableTableChast2.Local;
                    tableTableChast2ViewSource.View.MoveCurrentToLast();
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
            var grid2 = (DataGrid)tableTableChast2DataGrid;
            var selIt2 = grid2.SelectedItems.Cast<TableTableChast2>().ToArray();

            if (grid2.SelectedItems.Count > 0)
            {
                var Res = MessageBox.Show("Are you sure you want to delete " + grid2.SelectedItems.Count + " TableTableChast2s?", "Deleting Records", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (Res == MessageBoxResult.Yes)
                {
                    int aa = 0;
                    foreach (TableTableChast2 item in selIt2)
                    {
                        context.TableTableChast2.Remove(item);
                        aa++;
                    }

                    context.SaveChanges();
                    MessageBox.Show(aa.ToString() + " TableTableChast2s have being deleted!");
                }
            }

            tableTableChast2ViewSource.View.MoveCurrentToLast();
        }

        private void tableTableChast2DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (tableTableChast2DataGrid.CurrentColumn == null) return;
            int idx = tableTableChast2DataGrid.CurrentColumn.DisplayIndex;
            if ((idx == 0) || (idx == 1) || (idx == 2) || (idx == 5) || (idx == 6)) return;
            if (((TableTableChast2)tableTableChast2ViewSource.View.CurrentItem).Id == 0) return;
            if ((idx == 3) || (idx == 4))
            {
                decimal pSumma = ((TableTableChast2)tableTableChast2ViewSource.View.CurrentItem).Kolichestvo * ((TableTableChast2)tableTableChast2ViewSource.View.CurrentItem).Cena;
                ((TableTableChast2)tableTableChast2ViewSource.View.CurrentItem).Summa = pSumma;
                //context.TableTableChast2.Load();
                //tableTableChast2ViewSource.Source = context.TableTableChast2.Local;

            }
            context.SaveChanges();
            tableTableChast2ViewSource.View.Refresh();

        }
        public bool TableTableChast_Filter(object de)
        {   
            TableTableChast2 order = de as TableTableChast2;         
            return (order.Id == pId);
        }

    }
}
