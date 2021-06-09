using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookShopMangement.FORMS
{
    public partial class view : Form
    {
       
        Timer tt = new Timer();
        int iwich=-1;
        int i = 0;
        int sumTotal = 0;
        public string ColumnNameTOSearch { get; private set; }

        public view(string x)
        {
            InitializeComponent();
            dataGridView1.CellClick += DataGridView1_CellClick;
           
            textBox1.Text = x;


        }

       
        void search()
        {
            string cont = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            SqlConnection con = new SqlConnection(cont);
            try
            {

                String sql = "select * from wharehouse where ProductName like '" + textBox1.Text + "%'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "wharehouse");
                dataGridView1.DataSource = ds.Tables["wharehouse"].DefaultView;
                cmd.ExecuteReader();
                con.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            iwich = e.RowIndex;
            if (iwich > -1)
            {
                textBox2.Text = dataGridView1.Rows[iwich].Cells["ProductName"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[iwich].Cells["quantity"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[iwich].Cells["price"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[iwich].Cells["date"].Value.ToString();
            }

        }

        private void view_Load(object sender, EventArgs e)
        {
            i = 0;
            sumTotal = 0;
            //----------- data base connect and read data -------------------------------
            string cont = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            SqlConnection con = new SqlConnection(cont);
            try
            {

                String sql = "SELECT * FROM wharehouse";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                con.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "wharehouse");
                dataGridView1.DataSource = ds.Tables["wharehouse"].DefaultView;
                cmd.ExecuteReader();
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

           
            //---------------- table design-------------------------
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.OldLace;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.White;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            // calc total

            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                sumTotal += Convert.ToInt32(row.Cells["price"].Value)* Convert.ToInt32(row.Cells["quantity"].Value);
            }

            label3.Text = sumTotal.ToString("C2");

            search();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(iwich<0)
            {
                MessageBox.Show("please select row");
            }
            // check if the user click YES
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes&&iwich>=0)
            {

                string cont = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                SqlConnection con = new SqlConnection(cont);
                try
                {
                    // delete query

                    String sql = "DELETE FROM dbo.wharehouse WHERE id=@id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[iwich].Cells["id"].Value);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // select again

                    sql = "SELECT * FROM wharehouse";
                    cmd = new SqlCommand(sql, con);
                    da = new SqlDataAdapter(cmd);
                    con.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds, "wharehouse");
                    dataGridView1.DataSource = ds.Tables["wharehouse"].DefaultView;
                    cmd.ExecuteReader();
                    con.Close();
                    sumTotal = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        sumTotal += Convert.ToInt32(row.Cells["price"].Value) * Convert.ToInt32(row.Cells["quantity"].Value);
                    }

                    label3.Text = sumTotal.ToString("C2");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            storage st = new storage();
            st.Show();
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            if (iwich>=0)
            {
                string cont = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                SqlConnection con = new SqlConnection(cont);
                try
                {
                    // Update query
                    String sql = "UPDATE wharehouse set ProductName=@name,quantity=@qty,price=@price,[total price]=@total where id=@id";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@name", textBox2.Text.ToString());
                    cmd.Parameters.AddWithValue("@qty", Convert.ToInt32(textBox3.Text));
                    cmd.Parameters.AddWithValue("@price",textBox4.Text.ToString());
                    int tot = Convert.ToInt32(textBox3.Text) * Convert.ToInt32(textBox4.Text);
                    cmd.Parameters.AddWithValue("@total", tot.ToString());

                    cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[iwich].Cells["id"].Value);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    // select again
                    sql = "SELECT * FROM wharehouse";
                    cmd = new SqlCommand(sql, con);
                    da = new SqlDataAdapter(cmd);
                    con.Open();
                    DataSet ds = new DataSet();
                    da.Fill(ds, "wharehouse");
                    dataGridView1.DataSource = ds.Tables["wharehouse"].DefaultView;
                   
                    cmd.ExecuteReader();
                    con.Close();
                    sumTotal = 0;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        sumTotal += Convert.ToInt32(row.Cells["price"].Value) * Convert.ToInt32(row.Cells["quantity"].Value);
                    }

                    label3.Text = sumTotal.ToString("C2");


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("");
            }
        }
        Bitmap bm;
        private void button4_Click(object sender, EventArgs e)
        {
            int height = dataGridView1.Height;
            dataGridView1.Height = dataGridView1.RowCount * dataGridView1.RowTemplate.Height*2;
             bm = new Bitmap(dataGridView1.Width,dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            dataGridView1.Height = height;

            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
           
            printPreviewDialog1.ShowDialog();
            //printDocument1.Print();
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
            e.Graphics.DrawImage(bm, 0, 0);
        }

       
    }
}
