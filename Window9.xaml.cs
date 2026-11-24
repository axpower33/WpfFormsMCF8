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
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для Window9.xaml
    /// </summary>
    public partial class Window9 : Window
    {
        public Window9()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet MyDataSet = new DataSet();
            //string con2 = @"Provider=SQLOLEDB; Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True; Connect Timeout = 20";
            string con2 = @" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True;Connect Timeout = 20";
            string sSql = @" SELECT TableTableChast2.Id, Nomenklatura.Naimenovanie AS Nomenklatura, EdIzm.Naimenovanie AS EdIzm, TableTableChast2.Kolichestvo, TableTableChast2.Cena, TableTableChast2.Summa " +
                            " FROM TableTableChast2 INNER JOIN " +
                            " Nomenklatura ON TableTableChast2.Nomenklatura = Nomenklatura.Id INNER JOIN " +
                            " EdIzm ON TableTableChast2.EdIzm = EdIzm.Id ";
            using (SqlConnection oleCon = new SqlConnection(con2))
            {
                try
                {
                    oleCon.Open();
                    SqlCommand olecmd = new SqlCommand(sSql, oleCon);
                    SqlDataAdapter da = new SqlDataAdapter(olecmd);
                    da.Fill(MyDataSet);
                    oleCon.Close();
                }
                catch (Exception exMessage)
                {
                    MessageBox.Show(exMessage.Message);
                }
            }

            MyReportViewer.ProcessingMode = ProcessingMode.Local;
            MyReportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            MyReportViewer.LocalReport.ReportPath = @"Report2.rdlc";
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", MyDataSet.Tables[0]));
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", MyDataSet.Tables[0]));
            MyReportViewer.RefreshReport();

        }
    }
}
