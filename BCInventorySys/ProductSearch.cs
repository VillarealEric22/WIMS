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
	public partial class ProductSearch : Form
	{
		private AdministratorView mainForm = null;
		
		public ProductSearch(Form callingForm)
		{
			mainForm = callingForm as AdministratorView;
			InitializeComponent();
		}
		public static string prodCode;
		public static string prodDesc;
		public static string categ;
		public static string price;
		private void AutoSug(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populate();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populate();
		}
		private DataTable populate()
		{
			string query = "SELECT * FROM product WHERE product.productID LIKE '%'+ @searchTerm +'%'";
				query += " OR product.category LIKE '%'+ @searchTerm +'%'";
				query += " OR product.productDesc LIKE '%' + @searchTerm + '%'";
				query += " OR product.unitPrice LIKE '%' + @searchTerm + '%'";

			string constr = @"Data Source=DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True";
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand(query, con))
				{
					cmd.Parameters.AddWithValue("@searchTerm", tbSearch.Text.Trim());
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt = new DataTable();
						sda.Fill(dt);
						return dt;
					}
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
					string selProd = "SELECT * FROM product WHERE productID = '" + a + "' ";
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
							prodCode = (read["productID"].ToString());
							prodDesc = (read["productDesc"].ToString());
							price = (read["unitPrice"].ToString());
							categ = (read["category"].ToString());

						}
						read.Close();
						con.Close();

						this.mainForm.textProdDesc = prodDesc;
						this.mainForm.textProdCode = prodCode;
						this.mainForm.textPrice = price;
						this.mainForm.textCat = categ;

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
