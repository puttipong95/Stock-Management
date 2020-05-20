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

namespace Stock_Management
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                SqlConnection con = Connection.GetConnection();
                //Insert Logic
                con.Open();
                bool status = false;
                if (comboBox1.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                var sqlQuery = "";

                if (IfProductsExists(con, textBox1.Text))
                {
                    sqlQuery = @"UPDATE [dbo].[Products] SET [ProductName] = '" + textBox2.Text + "' ,[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + textBox1.Text + "' ";
                }
                else
                {
                    sqlQuery = @"INSERT INTO [dbo].[Products]([ProductCode],[ProductName],[ProductStatus])
            VALUES ('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
                }

                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                con.Close();

                //Reading Data
                LoadData();
                ResetRecord(); 
            }
        }

        //Check ProductCode Exist
        private bool IfProductsExists(SqlConnection con, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT 1 FROM [Products] WHERE [ProductCode] = '"+ productCode +"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LoadData() {
            SqlConnection con = Connection.GetConnection();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM [dbo].[Products]", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }

            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            button2.Text = "Update";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["ProductCode"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "Active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure want to Delete", "Message", MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes)
            {
                if (Validation())
                {
                    SqlConnection con = Connection.GetConnection();

                    var sqlQuery = "";

                    if (IfProductsExists(con, textBox1.Text))
                    {
                        con.Open();
                        sqlQuery = @"DELETE FROM [dbo].[Products] WHERE [ProductCode] = '" + textBox1.Text + "' ";
                        SqlCommand cmd = new SqlCommand(sqlQuery, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("Record Not Exists...!");
                    };
                    //Reading Data
                    LoadData();
                    ResetRecord();
                }
            }
        }

        private void ResetRecord()
        {
            button2.Text = "Add";
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ResetRecord();
        }

        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox1, "Please Product Code Require");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Please Product Name Require");
            }
            else if(comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Please Select Status");
            }
            else
            {
                result = true;
            }

            return result;
        }
    }
}
