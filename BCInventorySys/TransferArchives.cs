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

namespace BCInventorySys
{
    public partial class TransferArchives : Form
    {
        public TransferArchives()
        {
            InitializeComponent();
        }

        private void TransferArchives_Load(object sender, EventArgs e)
        {
			dataGridView1.DataSource = this.populate();
        }
		private void AutoS(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populate();
		}
		private DataTable populate()
		{
			string query = "SELECT * FROM arch_trans WHERE referenceNo LIKE '%'+ @searchTerm +'%'";
			query += " OR productID LIKE '%'+ @searchTerm +'%'";
			query += " OR source LIKE '%' + @searchTerm + '%'";
			query += " OR destination LIKE '%' + @searchTerm + '%'";
			query += " OR transRoute LIKE '%' + @searchTerm + '%'";
			query += " OR quantity LIKE '%'+ @searchTerm +'%'";
			query += " OR source LIKE '%' + @searchTerm + '%'";
			query += " OR deliveryStatus LIKE '%' + @searchTerm + '%'";
			query += " OR deliverySend LIKE '%' + @searchTerm + '%'";
			query += " OR deliveryRecieve LIKE '%' + @searchTerm + '%'";
			
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
	}
}
