using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BCInventorySys
{
	public partial class newCategory : Form
	{
		public newCategory()
		{
			InitializeComponent();
		}
		private void newCategory_Load(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populate();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populate();
		}
		private DataTable populate()
		{
			string query = "SELECT * FROM categ WHERE categ.category LIKE '%'+ @searchTerm +'%'";
			string constr = @"Data Source=DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True";
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@searchTerm", tbCateg.Text.Trim());
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						return dt;
					}
				}
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			string categ = tbCateg.Text;
			int cLimit = Convert.ToInt32(tbCLimit.Text);
			string con = @"Data Source=DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True";
			string query = "SELECT * FROM categ WHERE category = '" + categ + "' ";
			SqlDataAdapter load = new SqlDataAdapter(query, con);
			DataTable searchTB = new DataTable();
			load.Fill(searchTB);
			dataGridView1.DataSource = searchTB;

			string message = "Proceed to add " + categ + " to records?";
			MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
			DialogResult result = MessageBox.Show(message, null, buttons, MessageBoxIcon.Warning);
			if (result == DialogResult.OK)
			{
				if (searchTB.Rows.Count == 1)
				{
					MessageBox.Show("Proceed to Update " + categ + " ?");
					SqlDataAdapter upper = new SqlDataAdapter("Update categ SET category = '" + categ + "', criticalLimit '" + cLimit + "')", con);
					DataTable da = new DataTable();
					upper.Fill(da);
				}
				else 
				{
					SqlDataAdapter adder = new SqlDataAdapter("INSERT INTO categ (category, criticalLimit) VALUES('" + categ + "', '" + cLimit + "')", con);
					DataTable da = new DataTable();
					adder.Fill(da);

				}
			}
		}
		private void SelectedItem(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1)
			{
				return;
			}
			else
			{
				try
				{
					SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True");
					DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
					string a = row.Cells[0].Value.ToString();
					string selProd = "SELECT * FROM categ WHERE category = '" + a + "' ";
					SqlDataAdapter ar = new SqlDataAdapter(selProd, con);
					SqlCommand com = new SqlCommand(selProd, con);
					DataTable da = new DataTable();
					ar.Fill(da);
					if (da.Rows.Count == 1)
					{
						con.Open();
						SqlDataReader read = com.ExecuteReader();

						while (read.Read())
						{
							tbCateg.Text = read["category"].ToString();
							tbCLimit.Text = read["criticalLimit"].ToString();

						}
						read.Close();
						con.Close();
					}
				}
				catch (Exception ee)
				{
					MessageBox.Show(ee.Message);
				}
			}
			
		}
	}
}
