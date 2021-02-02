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
    public partial class criticalAlert : Form
    {
        public criticalAlert()
        {
            InitializeComponent();
        }

        private void criticalAlert_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = this.populateTable();

        }
        private DataTable populateTable()
        {
            string query = "SELECT product.productID, product.productDesc, product.unitPrice, product.category, whseInventory.quantity, categ.criticalLimit, whse.whseName" +
                " FROM whseInventory INNER JOIN product ON whseInventory.productID = product.productID INNER JOIN categ ON product.category = categ.category INNER JOIN" +
                " whse ON whseInventory.whseID = whse.whseID WHERE whseInventory.quantity <= categ.criticalLimit";

            string constr = @"Data Source=DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

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
