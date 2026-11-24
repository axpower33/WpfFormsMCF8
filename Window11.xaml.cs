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
using Dapper;
using System.Data.SqlTypes;
using System.Globalization;
using System.ComponentModel;

namespace WpfAppMCF8
{
    /// <summary>
    /// Логика взаимодействия для Window11.xaml
    /// </summary>
    public partial class Window11 : Window
    {
        public Window11()
        {
            InitializeComponent();
            fromDate.SelectedDate = new DateTime(2021, 01, 01);
            toDate.SelectedDate = new DateTime(2022, 12, 03);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSet MyDataSet = new DataSet();
            string sSql1 = @"SELECT    Nomenklatura, SUM(NachOst) AS NachOst, SUM(Prihod) AS Prihod, SUM(Rashod) AS Rashod, SUM(KonOst) AS KonOst" +
   " FROM            (SELECT   Nomenklatura.Naimenovanie AS Nomenklatura, SUM(0.00) AS NachOst, SUM(TableTableChast.Summa) AS Prihod, SUM(0.00) AS Rashod, SUM(TableTableChast.Summa - 0.00) AS KonOst" +
                             " FROM            TableTableChast LEFT OUTER JOIN" +
                             " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id" +
                             " left outer join TableDoc on TableDoc.Id=TableTableChast.Id" +
                             " WHERE (TableDoc.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate) + "' AND '" + ((DateTime)toDate.SelectedDate) + "') " +
                             " GROUP BY Nomenklatura.Naimenovanie" +
                             " UNION" +
                             " SELECT  Nomenklatura.Naimenovanie AS Nomenklatura, SUM(0.00) AS NachOst, SUM(0.00) AS Prihod, SUM(D.Summa) AS Rashod, SUM(0.00 - D.Summa) AS KonOst" +
                             " FROM            TableTableChast2 AS D LEFT OUTER JOIN" +
                             " Nomenklatura AS Nomenklatura ON D.Nomenklatura = Nomenklatura.Id" +
                             " left outer join TableDoc2 on TableDoc2.Id=D.Id" +
                             " WHERE (TableDoc2.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate) + "' AND '" + ((DateTime)toDate.SelectedDate) + "') " +
                             " GROUP BY Nomenklatura.Naimenovanie) AS derivedtbl_1" +
   " GROUP BY Nomenklatura";
            string sSql2 = @"SELECT     Nomenklatura, SUM(NachOst) AS NachOst, Sum(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
" FROM            (SELECT   Nomenklatura.Naimenovanie AS Nomenklatura, SUM(TableTableChast.Summa) AS NachOst, SUM(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
                          " FROM            TableTableChast LEFT OUTER JOIN" +
                          " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id" +
                          " left outer join TableDoc on TableDoc.Id=TableTableChast.Id" +
                          " WHERE (TableDoc.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate).AddDays(-1000) + "' AND '" + ((DateTime)fromDate.SelectedDate).AddDays(-1) + "') " +
                          " GROUP BY Nomenklatura.Naimenovanie" +
                          " UNION" +
                          " SELECT  Nomenklatura.Naimenovanie AS Nomenklatura, SUM(-D.Summa) AS NachOst, SUM(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
                          " FROM           " +
                          " TableTableChast2 AS D LEFT OUTER JOIN" +
                          " Nomenklatura AS Nomenklatura ON D.Nomenklatura = Nomenklatura.Id" +
                          " left outer join TableDoc2 on TableDoc2.Id=D.Id" +
                          " WHERE (TableDoc2.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate).AddDays(-1000) + "' AND '" + ((DateTime)fromDate.SelectedDate).AddDays(-1) + "') " +
                          " GROUP BY Nomenklatura.Naimenovanie) AS derivedtbl_2" +
" GROUP BY Nomenklatura";

            IDbConnection db;
            db = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True; Connect Timeout = 20");
            db.Open();

            List<DataTable4> dt = db.Query<DataTable4>(sSql1).AsList<DataTable4>();
            List<DataTable4> dt2 = db.Query<DataTable4>(sSql2).AsList<DataTable4>();
            int i = 0;
            int j;

            while (i < dt.Count())
            {
                j = 0;
                while (j < dt2.Count())
                {
                    if (dt[i].Nomenklatura == dt2[j].Nomenklatura)
                    {
                        dt[i].NachOst = dt2[j].NachOst;
                        dt[i].KonOst = dt[i].NachOst + dt[i].Prihod - dt[i].Rashod;
                    }
                    j++;
                }
                i++;
            }
            db.Close();

            ReportParameter[] p = new ReportParameter[]
            {
                new ReportParameter("ReportParameter1",fromDate.SelectedDate.ToString()),
                new ReportParameter("ReportParameter2",toDate.SelectedDate.ToString())
            };
            MyReportViewer.ProcessingMode = ProcessingMode.Local;
            MyReportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            MyReportViewer.LocalReport.ReportPath = @"Report5.rdlc";
            MyReportViewer.LocalReport.DataSources.Clear();
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt));
            MyReportViewer.LocalReport.SetParameters(p);
            MyReportViewer.RefreshReport();
        }

        private void MyReportViewer_ReportRefresh(object sender, CancelEventArgs e)
        {
            DataSet MyDataSet = new DataSet();
            string sSql1 = @"SELECT    Nomenklatura, SUM(NachOst) AS NachOst, SUM(Prihod) AS Prihod, SUM(Rashod) AS Rashod, SUM(KonOst) AS KonOst" +
   " FROM            (SELECT   Nomenklatura.Naimenovanie AS Nomenklatura, SUM(0.00) AS NachOst, SUM(TableTableChast.Summa) AS Prihod, SUM(0.00) AS Rashod, SUM(TableTableChast.Summa - 0.00) AS KonOst" +
                             " FROM            TableTableChast LEFT OUTER JOIN" +
                             " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id" +
                             " left outer join TableDoc on TableDoc.Id=TableTableChast.Id" +
                             " WHERE (TableDoc.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate) + "' AND '" + ((DateTime)toDate.SelectedDate) + "') " +
                             " GROUP BY Nomenklatura.Naimenovanie" +
                             " UNION" +
                             " SELECT  Nomenklatura.Naimenovanie AS Nomenklatura, SUM(0.00) AS NachOst, SUM(0.00) AS Prihod, SUM(D.Summa) AS Rashod, SUM(0.00 - D.Summa) AS KonOst" +
                             " FROM            TableTableChast2 AS D LEFT OUTER JOIN" +
                             " Nomenklatura AS Nomenklatura ON D.Nomenklatura = Nomenklatura.Id" +
                             " left outer join TableDoc2 on TableDoc2.Id=D.Id" +
                             " WHERE (TableDoc2.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate) + "' AND '" + ((DateTime)toDate.SelectedDate) + "') " +
                             " GROUP BY Nomenklatura.Naimenovanie) AS derivedtbl_1" +
   " GROUP BY Nomenklatura";
            string sSql2 = @"SELECT     Nomenklatura, SUM(NachOst) AS NachOst, Sum(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
" FROM            (SELECT   Nomenklatura.Naimenovanie AS Nomenklatura, SUM(TableTableChast.Summa) AS NachOst, SUM(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
                          " FROM            TableTableChast LEFT OUTER JOIN" +
                          " Nomenklatura AS Nomenklatura ON TableTableChast.Nomenklatura = Nomenklatura.Id" +
                          " left outer join TableDoc on TableDoc.Id=TableTableChast.Id" +
                          " WHERE (TableDoc.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate).AddDays(-1000) + "' AND '" + ((DateTime)fromDate.SelectedDate).AddDays(-1) + "') " +
                          " GROUP BY Nomenklatura.Naimenovanie" +
                          " UNION" +
                          " SELECT  Nomenklatura.Naimenovanie AS Nomenklatura, SUM(-D.Summa) AS NachOst, SUM(0.00) AS Prihod, SUM(0.00) AS Rashod, SUM(0.00) AS KonOst" +
                          " FROM           " +
                          " TableTableChast2 AS D LEFT OUTER JOIN" +
                          " Nomenklatura AS Nomenklatura ON D.Nomenklatura = Nomenklatura.Id" +
                          " left outer join TableDoc2 on TableDoc2.Id=D.Id" +
                          " WHERE (TableDoc2.DataDoc BETWEEN '" + ((DateTime)fromDate.SelectedDate).AddDays(-1000) + "' AND '" + ((DateTime)fromDate.SelectedDate).AddDays(-1) + "') " +
                          " GROUP BY Nomenklatura.Naimenovanie) AS derivedtbl_2" +
" GROUP BY Nomenklatura";

            IDbConnection db;
            db = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = ""C:\Users\Basko\SqlBases\ProvisorBase.mdf""; Integrated Security = True; Connect Timeout = 20");
            db.Open();

            List<DataTable4> dt = db.Query<DataTable4>(sSql1).AsList<DataTable4>();
            List<DataTable4> dt2 = db.Query<DataTable4>(sSql2).AsList<DataTable4>();
            int i = 0;
            int j;

            while (i < dt.Count())
            {
                j = 0;
                while (j < dt2.Count())
                {
                    if (dt[i].Nomenklatura == dt2[j].Nomenklatura)
                    {
                        dt[i].NachOst = dt2[j].NachOst;
                        dt[i].KonOst = dt[i].NachOst + dt[i].Prihod - dt[i].Rashod;
                    }
                    j++;
                }
                i++;
            }
            db.Close();

            ReportParameter[] p = new ReportParameter[]
            {
                new ReportParameter("ReportParameter1",fromDate.SelectedDate.ToString()),
                new ReportParameter("ReportParameter2",toDate.SelectedDate.ToString())
            };
            MyReportViewer.ProcessingMode = ProcessingMode.Local;
            MyReportViewer.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            MyReportViewer.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.PageWidth;
            MyReportViewer.LocalReport.ReportPath = @"Report5.rdlc";
            MyReportViewer.LocalReport.DataSources.Clear();
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            MyReportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt));
            MyReportViewer.LocalReport.SetParameters(p);
            MyReportViewer.RefreshReport();
        }
    }
}
