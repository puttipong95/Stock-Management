using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Management.ReportForm
{
    public partial class StockForm : Form
    {
        ReportDocument cryrpt = new ReportDocument();

        public StockForm()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void StockForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cryrpt.Load(@"H:\Project\C#\Stock-Management\Stock Management\Stock Management\Reports\Stock.rpt");
            SqlConnection con = Connection.GetConnection();
            con.Open();
            DataSet dst = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [dbo].[Stock] where Cast( TranDate as Date ) between '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' and '" + dateTimePicker2.Value.ToString("MM/dd/yyyy") + "'", con);
            sda.Fill(dst, "Stock");
            cryrpt.SetDataSource(dst);
            cryrpt.SetParameterValue("@FormDate", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            cryrpt.SetParameterValue("@ToDate", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            crystalReportViewer1.ReportSource = cryrpt;
        }
    }
}
