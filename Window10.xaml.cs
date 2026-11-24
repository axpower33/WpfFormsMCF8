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
using System.ComponentModel;

namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для Window10.xaml
    /// </summary>
    public partial class Window10 : Window
    {
        public Window10()
        {
            InitializeComponent();
            fromDate.SelectedDate = new DateTime(2021, 01, 01);
            toDate.SelectedDate = new DateTime(2022, 12, 03);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet MyDataSet = new DataSet();
            //string con2 = @"Provider=SQLOLEDB; Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True; Connect Timeout = 20";
            string con2 = @" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True;Connect Timeout = 20";
            string sSql = @" SELECT  Kontragent.Naimenovanie AS Kontragent, Nomenklatura.Naimenovanie AS Nomenklatura, EdIzm.Naimenovanie AS EdIzm, SUM(TableTableChast.Kolichestvo) AS Kolichestvo, SUM(TableTableChast.Summa) " +
               " AS Summa " +
" FROM           TableTableChast LEFT OUTER JOIN " +
               " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id LEFT OUTER JOIN " +
               " EdIzm AS EdIzm ON TableTableChast.EdIzm = EdIzm.Id LEFT OUTER JOIN " +
               " TableDoc AS TableDoc ON TableDoc.Id = TableTableChast.Id LEFT OUTER JOIN " +
               " Kontragent AS Kontragent ON TableDoc.Kontragent = Kontragent.Id " +
" WHERE (TableDoc.DataDoc BETWEEN '" + fromDate.SelectedDate + "' AND '" + toDate.SelectedDate + "') " +
" GROUP BY Kontragent.Naimenovanie, Nomenklatura.Naimenovanie, EdIzm.Naimenovanie " +
" UNION " +
" SELECT        Kontragent_1.Naimenovanie AS Kontragent, Nomenklatura_1.Naimenovanie AS Nomenklatura, EdIzm_1.Naimenovanie AS EdIzm, SUM(- TableTableChast2.Kolichestvo) AS Kolichestvo, " +
"                         SUM(- TableTableChast2.Summa) AS Summa " +
" FROM            TableTableChast2 LEFT OUTER JOIN " +
               " Nomenklatura AS Nomenklatura_1 ON TableTableChast2.Nomenklatura = Nomenklatura_1.Id LEFT OUTER JOIN " +
               " EdIzm AS EdIzm_1 ON TableTableChast2.EdIzm = EdIzm_1.Id LEFT OUTER JOIN " +
               " TableDoc2 AS TableDoc2 ON TableDoc2.Id = TableTableChast2.Id LEFT OUTER JOIN " +
               " Kontragent AS Kontragent_1 ON TableDoc2.Kontragent = Kontragent_1.Id " +
  " WHERE(TableDoc2.DataDoc BETWEEN '" + fromDate.SelectedDate + "' AND '" + toDate.SelectedDate + "') " +
  " GROUP BY Kontragent_1.Naimenovanie, Nomenklatura_1.Naimenovanie, EdIzm_1.Naimenovanie ";


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
            ReportParameter[] p = new ReportParameter[]
                       {
                            new ReportParameter("ReportParameter1",fromDate.SelectedDate.ToString()),
                            new ReportParameter("ReportParameter2",toDate.SelectedDate.ToString())
                       };
            MyReportViewer.ProcessingMode = ProcessingMode.Local;
            MyReportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            MyReportViewer.LocalReport.ReportPath = @"Report4.rdlc";
            MyReportViewer.LocalReport.DataSources.Clear();
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", MyDataSet.Tables[0]));
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", MyDataSet.Tables[0]));
            MyReportViewer.LocalReport.SetParameters(p);
            MyReportViewer.RefreshReport();

        }

        private void MyReportViewer_ReportRefresh(object sender, CancelEventArgs e)
        {
            DataSet MyDataSet = new DataSet();
            //string con2 = @"Provider=SQLOLEDB; Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True; Connect Timeout = 20";
            string con2 = @" Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True;Connect Timeout = 20";
            string sSql = @" SELECT  Kontragent.Naimenovanie AS Kontragent, Nomenklatura.Naimenovanie AS Nomenklatura, EdIzm.Naimenovanie AS EdIzm, SUM(TableTableChast.Kolichestvo) AS Kolichestvo, SUM(TableTableChast.Summa) " +
               " AS Summa " +
" FROM           TableTableChast LEFT OUTER JOIN " +
               " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id LEFT OUTER JOIN " +
               " EdIzm AS EdIzm ON TableTableChast.EdIzm = EdIzm.Id LEFT OUTER JOIN " +
               " TableDoc AS TableDoc ON TableDoc.Id = TableTableChast.Id LEFT OUTER JOIN " +
               " Kontragent AS Kontragent ON TableDoc.Kontragent = Kontragent.Id " +
" WHERE (TableDoc.DataDoc BETWEEN '" + fromDate.SelectedDate + "' AND '" + toDate.SelectedDate + "') " +
" GROUP BY Kontragent.Naimenovanie, Nomenklatura.Naimenovanie, EdIzm.Naimenovanie " +
" UNION " +
" SELECT        Kontragent_1.Naimenovanie AS Kontragent, Nomenklatura_1.Naimenovanie AS Nomenklatura, EdIzm_1.Naimenovanie AS EdIzm, SUM(- TableTableChast2.Kolichestvo) AS Kolichestvo, " +
"                         SUM(- TableTableChast2.Summa) AS Summa " +
" FROM            TableTableChast2 LEFT OUTER JOIN " +
               " Nomenklatura AS Nomenklatura_1 ON TableTableChast2.Nomenklatura = Nomenklatura_1.Id LEFT OUTER JOIN " +
               " EdIzm AS EdIzm_1 ON TableTableChast2.EdIzm = EdIzm_1.Id LEFT OUTER JOIN " +
               " TableDoc2 AS TableDoc2 ON TableDoc2.Id = TableTableChast2.Id LEFT OUTER JOIN " +
               " Kontragent AS Kontragent_1 ON TableDoc2.Kontragent = Kontragent_1.Id " +
  " WHERE(TableDoc2.DataDoc BETWEEN '" + fromDate.SelectedDate + "' AND '" + toDate.SelectedDate + "') " +
  " GROUP BY Kontragent_1.Naimenovanie, Nomenklatura_1.Naimenovanie, EdIzm_1.Naimenovanie ";


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
            ReportParameter[] p = new ReportParameter[]
                       {
                            new ReportParameter("ReportParameter1",fromDate.SelectedDate.ToString()),
                            new ReportParameter("ReportParameter2",toDate.SelectedDate.ToString())
                       };
            MyReportViewer.ProcessingMode = ProcessingMode.Local;
            MyReportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            MyReportViewer.LocalReport.ReportPath = "Report4.rdlc";
            MyReportViewer.LocalReport.DataSources.Clear();
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", MyDataSet.Tables[0]));
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", MyDataSet.Tables[0]));
            MyReportViewer.LocalReport.SetParameters(p);
            MyReportViewer.RefreshReport();
        }

    }
}
