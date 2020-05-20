using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock_Management
{
    public partial class Stock : Form
    {
        public Stock()
        {
            InitializeComponent();
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            this.ActiveControl = dateTimePicker1;
            cbStatus.SelectedIndex = 0;
            LoadData();
            Search();
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtProductCode.Focus();
            }
        }

        private void cbStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (cbStatus.SelectedIndex == -1)
                {
                    button2.Focus();
                }
                else
                {
                    button1.Focus();
                }
            }

        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgview.Rows.Count > 0)
                {
                    txtProductCode.Text = dgview.SelectedRows[0].Cells[0].Value.ToString();
                    txtProductName.Text = dgview.SelectedRows[0].Cells[1].Value.ToString();
                    this.dgview.Visible = false;
                    txtQuantity.Focus();
                }
                else
                {
                    this.dgview.Visible = false;
                }
            }
        }

        private void txtProductName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQuantity.Focus();
            }
        }

        bool change = true;
        private void proCode_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (change)
            {
                change = false;
                txtProductCode.Text = dgview.SelectedRows[0].Cells[0].Value.ToString();
                txtProductName.Text = dgview.SelectedRows[0].Cells[1].Value.ToString();
                this.dgview.Visible = false;
                txtQuantity.Focus();
                change = true;
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbStatus.Focus();
            }
        }

        private void txtProductCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void ResetRecords()
        {
            dateTimePicker1.Value = DateTime.Now;
            txtProductCode.Clear();
            txtProductName.Clear();
            txtQuantity.Clear();
            cbStatus.SelectedIndex = -1;
            button1.Text = "Add";
            dateTimePicker1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetRecords();
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(txtProductCode.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtProductCode, "Product Code Required");
            } else if (string.IsNullOrEmpty(txtProductName.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtProductName, "Product Name Required");
            }
            else if (string.IsNullOrEmpty(txtQuantity.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(txtQuantity, "Quantity Required");
            }
            else if (cbStatus.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(cbStatus, "Status Required");
            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }

            return result;
        }

        private bool IfProductExists(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT 1 FROM [Stock] WHERE [ProductCode] = '" + productCode + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                SqlConnection con = Connection.GetConnection();
                //Insert Logic
                con.Open();
                bool status = false;
                if (cbStatus.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                var sqlQuery = "";

                if (IfProductExists(con, txtProductCode.Text))
                {
                    sqlQuery = @"UPDATE [dbo].[Stock] SET [ProductName] = '" + txtProductName.Text + "' ,[Trandate] = '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' ,[Quantity] = '" + txtQuantity.Text + "' ,[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + txtProductCode.Text + "' ";
                }
                else
                {
                    sqlQuery = @"INSERT INTO [dbo].[Stock]([ProductCode],[ProductName],[Trandate],[Quantity] ,[ProductStatus])
            VALUES ('" + txtProductCode.Text + "','" + txtProductName.Text + "','" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "','" + txtQuantity.Text + "','" + status + "')";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record Seved Succressfully");
                LoadData();
                ResetRecords();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        public void LoadData()
        {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [dbo].[Stock]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView2.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {

                int n2 = dataGridView2.Rows.Add();
                dataGridView2.Rows[n2].Cells[0].Value = n2 + 1;
                dataGridView2.Rows[n2].Cells[1].Value = item["ProductCode"].ToString();
                dataGridView2.Rows[n2].Cells[2].Value = item["ProductName"].ToString();
                dataGridView2.Rows[n2].Cells[3].Value = float.Parse(item["Quantity"].ToString());
                dataGridView2.Rows[n2].Cells[4].Value = Convert.ToDateTime(item["Trandate"].ToString()).ToString("dd/MM/yyyy");
                if (item["ProductStatus"].ToString() == "True")
                {
                    dataGridView2.Rows[n2].Cells[5].Value = "Active";
                }
                else if (item["ProductStatus"].ToString() == "False")
                {
                    dataGridView2.Rows[n2].Cells[5].Value = "Deactive";
                }
            }

            if (dataGridView2.Rows.Count > 0)
            {
                lbTotalProduct.Text = dataGridView2.Rows.Count.ToString();
                float totqty = 0;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    totqty += float.Parse(dataGridView2.Rows[i].Cells[3].Value.ToString());
                    lbTotalQuantity.Text = totqty.ToString();
                }
            }
            else
            {
                lbTotalProduct.Text = "0";
                lbTotalQuantity.Text = "0";
            }
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            button1.Text = "Update";
            txtProductCode.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            txtProductName.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            txtQuantity.Text = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
            //dateTimePicker1.Text = DateTime.Parse(dataGridView2.SelectedRows[0].Cells[4].Value.ToString()).ToString("dd/MM/yyyy");
            //DateTime dateTimePicker1 = DateTime.ParseExact(dataGridView2.SelectedRows[0].Cells[4].Value.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
            dateTimePicker1.Value = DateTime.ParseExact(dataGridView2.SelectedRows[0].Cells[4].Value.ToString(), "dd/MM/yyyy", null);
            if (dataGridView2.SelectedRows[0].Cells[5].Value.ToString() == "Active")
            {
                cbStatus.SelectedIndex = 0;
            }
            else
            {
                cbStatus.SelectedIndex = 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure want to Delete", "Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (Validation())
                {
                    SqlConnection con = Connection.GetConnection();

                    var sqlQuery = "";

                    if (IfProductExists(con, txtProductCode.Text))
                    {
                        con.Open();
                        sqlQuery = @"DELETE FROM [dbo].[Stock] WHERE [ProductCode] = '" + txtProductCode.Text + "' ";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Record Deleted Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Record Not Exists...!");
                    };
                    //Reading Data
                    LoadData();
                    ResetRecords();
                }
            }
        }

        private void txtProductCode_TextChanged(object sender, EventArgs e)
        {
            if (txtProductCode.Text.Length > 0)
            {
                this.dgview.Visible = true;
                dgview.BringToFront();
                Search(150, 105, 430, 200, "Pro Code,Pro Name", "100,0");
                this.dgview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.proCode_MouseDoubleClick);
                // Search Product
                SqlConnection con = Connection.GetConnection();
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT ProductCode,ProductName FROM [Products] WHERE [ProductCode] Like '" + txtProductCode.Text + "%'", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgview.Rows.Clear();
                foreach (DataRow row in dt.Rows) {
                    int n = dgview.Rows.Add();
                    dgview.Rows[n].Cells[0].Value = row["ProductCode"].ToString();
                    dgview.Rows[n].Cells[1].Value = row["ProductName"].ToString();
                }
            }
            else
            {
                this.dgview.Visible = false;
            }
        }

        private DataGridView dgview;
        private DataGridViewTextBoxColumn dgviewcol1;
        private DataGridViewTextBoxColumn dgviewcol2;

        void Search()
        {
            dgview = new DataGridView();
            dgviewcol1 = new DataGridViewTextBoxColumn();
            dgviewcol2 = new DataGridViewTextBoxColumn();
            this.dgview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
               this.dgviewcol1, 
               this.dgviewcol2
            });
            this.dgview.Name = "dgview";
            dgview.Visible = false;
            this.dgviewcol1.Visible = false;
            this.dgviewcol2.Visible = false;
            this.dgview.AllowUserToAddRows = false;
            this.dgview.RowHeadersVisible = false;
            this.dgview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.Controls.Add(dgview);
            this.dgview.ReadOnly = true;
            dgview.BringToFront();
        }

        //Two column
        void Search(int LX, int LY, int DW, int DH, string ColName, string ColSize)
        {
            this.dgview.Location = new System.Drawing.Point(LX, LY);
            this.dgview.Size = new System.Drawing.Size(DW, DH);

            string[] ClSize = ColSize.Split(',');

            //Size
            for (int i=0; i < ClSize.Length; i++)
            {
                if (int.Parse(ClSize[i]) != 0)
                {
                    dgview.Columns[i].Width = int.Parse(ClSize[i]);
                }
                else
                {
                    dgview.Columns[i].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            //Name
            string[] ClName = ColName.Split(',');
            for (int i = 0; i < ClName.Length; i++) 
            {
                this.dgview.Columns[i].HeaderText = ClName[i];
                this.dgview.Columns[i].Visible = true;
            }
        }
    }
}
