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
using BookShopMangement.FORMS;
using MySql.Data.MySqlClient;

namespace BookShopMangement.FORMS
{
    public partial class storage : Form
    {
        public storage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cont = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            SqlConnection con = new SqlConnection(cont);
            try
            {
                
                String sql = "INSERT INTO dbo.wharehouse (ProductName,quantity,price,date,[total price]) VALUES(@user,@qyt,@price,@date,@total)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@user",name.Text);
                cmd.Parameters.AddWithValue("@qyt", Convert.ToInt32(qyt.Text));
                cmd.Parameters.AddWithValue("@price",price.Text.ToString());
                cmd.Parameters.AddWithValue("@date", dateTimePicker1.Text);
                MessageBox.Show(price.Text.ToString());
                int tot = Convert.ToInt32(qyt.Text) * Convert.ToInt32(price.Text);
                cmd.Parameters.AddWithValue("@total",tot.ToString());
                con.Open();
                cmd.ExecuteReader();


                MessageBox.Show("add");
                con.Close();
                name.Text = qyt.Text = price.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
