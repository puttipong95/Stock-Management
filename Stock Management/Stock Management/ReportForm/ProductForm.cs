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
    public partial class ProductForm : Form
    {
        ReportDocument cryrpt = new ReportDocument();

        public ProductForm()
        {
            InitializeComponent();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            cryrpt.Load(@"H:\Project\C#\Stock-Management\Stock Management\Stock Management\Reports\Product.rpt");
            SqlConnection con = Connection.GetConnection();
            con.Open();
            DataSet dst = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            cryrpt.SetDataSource(dt);
            crystalReportViewer1.ReportSource = cryrpt;
        }
    }
}
