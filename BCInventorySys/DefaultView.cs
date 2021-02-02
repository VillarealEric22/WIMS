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
using System.Diagnostics;

namespace BCInventorySys
{
	public partial class DefaultView : Form
	{
		
		public DefaultView()
		{
			InitializeComponent();
			btCrit.Visible = false;
			btCrit.Enabled = false;
			label1.Visible = false;

		}
		private void DataSelect(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populateTable();
			
		}
		private DataTable populateTable()
        {
			string query = "SELECT product.productID, product.productDesc, product.unitPrice, product.category, whseInventory.quantity, whse.whseName " +
					"FROM ((whseInventory INNER JOIN product ON whseInventory.productID = product.productID) " +
					"INNER JOIN whse ON whseInventory.whseID = whse.whseID) WHERE ";
			query += "product.productID LIKE '%'+ @searchTerm +'%' ";
			query += " OR product.category LIKE '%'+ @searchTerm +'%'";
			query += " OR product.productDesc LIKE '%' + @searchTerm + '%'";
			query += " OR whse.whseName LIKE '%' + @searchTerm + '%'";
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

		private void DefaultView_Load(object sender, EventArgs e)
		{
			dataGridView1.DataSource = this.populateTable();
			autodetectCriticalLevel();
		}

		private void enterAdministratorViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string utype = Login.utype;
			string checkstring = "Administrator";
			if (utype == checkstring)
			{
				AdministratorView adminView = new AdministratorView();
				adminView.Show();
				this.Hide();
			}
			else
			{
				string message = "Please login again with an administrator account";
				MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
				DialogResult result = MessageBox.Show(message, null, buttons, MessageBoxIcon.Warning);
				if (result == DialogResult.OK)
				{
					this.Hide();
					Login login = new Login();
					login.Show();
				}

			}

		}
        private void DefaultView_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (e.CloseReason == CloseReason.UserClosing)
			{
				Login ll = new Login();
				ll.Show();
			}
		}
		private void autodetectCriticalLevel()
        {
			string query = "SELECT * FROM whseInventory INNER JOIN product ON whseInventory.productID = product.productID INNER JOIN categ ON" +
				" product.category = categ.category WHERE whseInventory.quantity <= categ.criticalLimit";

			SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog = BCWhseInvtrySys; Integrated Security = True");
			SqlDataAdapter adapt = new SqlDataAdapter(query, con);
			DataTable dt = new DataTable();
			adapt.Fill(dt);

			if (dt.Rows.Count > 0)
			{
				btCrit.Visible = true;
				btCrit.Enabled = true;
				label1.Visible = true;
				SoftBlink(btCrit, Color.FromArgb(30, 30, 30), Color.Green, 2000, true);
			}
		}
        private void btCrit_Click(object sender, EventArgs e)
        {
			criticalAlert cA = new criticalAlert();
			cA.Show();

        }
		private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
		{
			var sw = new Stopwatch(); sw.Start();
			short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
			while (true)
			{
				await Task.Delay(1);
				var n = sw.ElapsedMilliseconds % CycleTime_ms;
				var per = (double)Math.Abs(n - halfCycle) / halfCycle;
				var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
				var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
				var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
				var clr = Color.FromArgb(red, grn, blw);
				if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr;
			}
		}
	}
}
