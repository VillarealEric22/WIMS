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
	public partial class Login : Form
	{
		SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog = BCWhseInvtrySys; Integrated Security = True");
		public Login()
		{
			InitializeComponent();
		}
		public static string utype;
		private void Login_Load(object sender, EventArgs e)
		{
			
		}
		private void btnLogin_Click(object sender, EventArgs e)
		{
			string uname = userNametxt.Text;
			string pword = passTxt.Text;
			string query = "SELECT * FROM users where username = '" + uname + "' and password = '" + pword + "'";
			SqlDataAdapter adapt = new SqlDataAdapter(query, con);
			DataTable dt = new DataTable();
			adapt.Fill(dt);

			if (dt.Rows.Count == 1)
			{
				utype = dt.Rows[0]["userType"].ToString();
				DefaultView formHome = new DefaultView();
				this.Hide();
				formHome.Show();
			}
			else
			{
				MessageBox.Show("Invalid username/password");
			}
		}

	}
}
