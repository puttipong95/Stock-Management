using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Management
{
    public partial class StockMain : Form
    {

        public StockMain()
        {
            InitializeComponent();
        }

        private void StockMain_Load(object sender, EventArgs e)
        {

        }

        bool close = true;

        private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {
                DialogResult result = MessageBox.Show("Are You Sure You Want To Exit", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    close = false;
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products pro = new Products();
            pro.MdiParent = this;
            pro.StartPosition = FormStartPosition.CenterScreen;
            pro.Show();
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.MdiParent = this;
            stock.StartPosition = FormStartPosition.CenterScreen;
            stock.Show();
        }

        private void productsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm.ProductForm repPro = new ReportForm.ProductForm();
            repPro.MdiParent = this;
            repPro.StartPosition = FormStartPosition.CenterScreen;
            repPro.Show();
        }

        private void stockListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm.StockForm repStk = new ReportForm.StockForm();
            repStk.MdiParent = this;
            repStk.StartPosition = FormStartPosition.CenterScreen;
            repStk.Show();
        }
    }
}
