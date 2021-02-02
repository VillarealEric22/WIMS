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
	public partial class AdministratorView : Form
	{
		SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True");
		public AdministratorView()
		{
			InitializeComponent();
			
		}
		public string textProdCode
		{
			get { return txtProdID.Text; }
			set { txtProdID.Text = value; }
		}
		public string textProdDesc
		{
			get { return txtProdDesc.Text; }
			set { txtProdDesc.Text = value; }
		}
		public string textPrice
		{
			get { return txtPrice.Text; }
			set { txtPrice.Text = value; }
		}
		public string textCat
		{
			get { return categBox.Text; }
			set { categBox.SelectedIndex = categBox.FindStringExact(value); }
		}
		private void AdministratorView_Load(object sender, EventArgs e)
		{

			// Drop-down DataSets
			
			getDataCombo();

			loadDataTrans();
			loadDataWhse();
			loadDataWhseIn();

			//Auto-Suggestion/Search Function

			//ProductID Auto-Suggest
			txtProdID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			txtProdID.AutoCompleteSource = AutoCompleteSource.CustomSource;
			AutoCompleteStringCollection ProdCol = new AutoCompleteStringCollection();
			getProd(ProdCol);
			txtProdID.AutoCompleteCustomSource = ProdCol;

			//Item-Product Code Auto-Suggest
			txtProdCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			txtProdCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
			getProd(ProdCol);
			txtProdCode.AutoCompleteCustomSource = ProdCol;

			//ReferenceNo Auto-Suggest
			txtRefNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			txtRefNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
			AutoCompleteStringCollection RefCol = new AutoCompleteStringCollection();
			getRef(RefCol);
			txtRefNo.AutoCompleteCustomSource = RefCol;

			//WhseID Auto-Suggest
			txtWhseCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
			txtWhseCode.AutoCompleteSource = AutoCompleteSource.CustomSource;
			AutoCompleteStringCollection WhseCol = new AutoCompleteStringCollection();
			getWhse(WhseCol);
			txtWhseCode.AutoCompleteCustomSource = WhseCol;

			
		}
		private void getProd(AutoCompleteStringCollection prodCol)
		{
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataSet ds = new DataSet();
			string sql = "SELECT DISTINCT productID FROM product";

			try
			{
				command = new SqlCommand(sql, con);
				adapter.SelectCommand = command;
				adapter.Fill(ds);
				adapter.Dispose();
				command.Dispose();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					prodCol.Add(row[0].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not open connection ! ");
			}
		}

		private void getRef(AutoCompleteStringCollection refCol)
		{
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataSet ds = new DataSet();
			string sql = "SELECT DISTINCT referenceNo FROM itemTransfer";

			try
			{
				command = new SqlCommand(sql, con);
				adapter.SelectCommand = command;
				adapter.Fill(ds);
				adapter.Dispose();
				command.Dispose();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					refCol.Add(row[0].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not open connection ! ");
			}
		}
		private void getWhse(AutoCompleteStringCollection whseCol)
		{
			SqlCommand command;
			SqlDataAdapter adapter = new SqlDataAdapter();
			DataSet ds = new DataSet();
			string sql = "SELECT DISTINCT whseID FROM whse";

			try
			{
				command = new SqlCommand(sql, con);
				adapter.SelectCommand = command;
				adapter.Fill(ds);
				adapter.Dispose();
				command.Dispose();
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					whseCol.Add(row[0].ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Can not open connection ! ");
			}
		}
		private void getDataCombo()
		{
			string loc = "SELECT whseID FROM whse";
			SqlDataAdapter load = new SqlDataAdapter();
			SqlCommand com = new SqlCommand(loc, con);
			DataSet dt = new DataSet();
			load.SelectCommand = com;
			load.Fill(dt);

			string cat = "SELECT category FROM categ";
			SqlDataAdapter catt = new SqlDataAdapter();
			SqlCommand ccc = new SqlCommand(cat, con);
			DataSet catT = new DataSet();
			catt.SelectCommand = ccc;
			catt.Fill(catT);

			categBox.DataSource = catT.Tables[0];
			categBox.DisplayMember = "category";
			categBox.ValueMember = "category";

			txtTRoute.DataSource = dt.Tables[0];
			txtTRoute.DisplayMember = "whseID";
			txtTRoute.ValueMember = "whseID";

			txtTRoute2.BindingContext = new BindingContext();   //create a new context
			txtTRoute2.DataSource = dt.Tables[0];
			txtTRoute2.DisplayMember = "whseID";
			txtTRoute2.ValueMember = "whseID";

			txtWhseID.DataSource = dt.Tables[0];
			txtWhseID.ValueMember = "whseID";
			txtWhseID.DisplayMember = "whseID";

		}
		//Load Data to DataGrid
		private void loadDataWhseIn()
		{
			string query = "SELECT * FROM whseInventory";
			SqlDataAdapter load = new SqlDataAdapter(query, con);
			DataTable dt = new DataTable();
			load.Fill(dt);
			dataGridView1.DataSource = dt;

		}
		private void loadDataTrans()
		{
			string query = "SELECT referenceNo, productID, quantity, transRoute," +
								" deliveryStatus, deliverySend, deliveryRecieve FROM itemTransfer";
			SqlDataAdapter load = new SqlDataAdapter(query, con);
			DataTable dt = new DataTable();
			load.Fill(dt);
			dataGridView2.DataSource = dt;
			
		}
		private void loadDataWhse()
		{
			string query = "SELECT * FROM whse";
			SqlDataAdapter load = new SqlDataAdapter(query, con);
			DataTable dt = new DataTable();
			load.Fill(dt);
			dataGridView3.DataSource = dt;
		}
		
		//Auto-Fill forms if data exists
		private void autoselect(object sender, EventArgs e)
		{
			string prodCode = txtProdID.Text;

			string query = "SELECT * FROM product where productID = '" + prodCode + "'";
			SqlDataAdapter checking = new SqlDataAdapter(query, con);
			SqlCommand com = new SqlCommand(query, con);
			DataTable dt = new DataTable();
			dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
			checking.Fill(dt);

			if (dt.Rows.Count == 1)
			{
				con.Open();
				SqlDataReader read = com.ExecuteReader();

				while (read.Read())
				{
					txtProdDesc.Text = (read["productDesc"].ToString());
					txtPrice.Text = (read["unitPrice"].ToString());
					categBox.SelectedIndex = categBox.FindStringExact(read["category"].ToString());

				}
				read.Close();
				con.Close();
			}

		}
		private void autoselect2(object sender, EventArgs e)
		{
			string reff = txtRefNo.Text;

				string query = "SELECT productID, quantity, source, destination, deliveryStatus, deliverySend, deliveryRecieve FROM itemTransfer where referenceNo = '" + reff + "'";
				SqlDataAdapter checking = new SqlDataAdapter(query, con);
				SqlCommand com = new SqlCommand(query, con);
				DataTable dt = new DataTable();
				dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
				checking.Fill(dt);
			
			if (dt.Rows.Count == 1)
				{
					
					con.Open();
					SqlDataReader read = com.ExecuteReader();

					while (read.Read())
					{
						quanty.Text = (read["quantity"].ToString());
						txtProdCode.Text = (read["productID"].ToString());
						txtTRoute.SelectedIndex = txtTRoute.FindStringExact(read["source"].ToString());
						txtTRoute2.SelectedIndex = txtTRoute2.FindStringExact(read["destination"].ToString());
						txtDevStat.SelectedIndex = txtDevStat.FindStringExact(read["deliveryStatus"].ToString());
						dateSend.Value = (DateTime)read["deliverySend"];

					}
					read.Close();
					con.Close();
				}

		}
		private void autoselect3(object sender, EventArgs e)
		{
			string whCode = txtWhseCode.Text;

			string query = "SELECT * FROM whse where whseID = '" + whCode + "'";
			SqlDataAdapter checking = new SqlDataAdapter(query, con);
			SqlCommand com = new SqlCommand(query, con);
			DataTable dt = new DataTable();
			checking.Fill(dt);

			if (dt.Rows.Count == 1)
			{
				con.Close();
				con.Open();
				SqlDataReader read = com.ExecuteReader();

				while (read.Read())
				{
					txtWhseName.Text = (read["whseName"].ToString());
					txtWhseCont.Text = (read["whseContactNo"].ToString());
					txtWhseLoc.Text = (read["whseLocation"].ToString());

				}
				read.Close();
				con.Close();
			}
		}

		//Button Functions (CRU)
		
		//Products
		private void btnProdInventoryOK_Click(object sender, EventArgs e)
		{

			if (txtProdCode.Text != "" || txtProdDesc.Text != "" || categBox.Text != "" || txtPrice.Text != "")
			{
				MessageBox.Show("Required Fields Empty");
			}
			else
			{
				string prodCode = txtProdID.Text;
				string prodDesc = txtProdDesc.Text;
				string price = txtPrice.Text;
				string pcateg = categBox.Text;
				double uPrice = Convert.ToDouble(price);

				string query = "SELECT * FROM product where productID = '" + prodCode + "'";
				SqlDataAdapter checking = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
				checking.Fill(dt);

				if (dt.Rows.Count == 1)
				{
					string message = "Proceed to UPDATE " + prodCode + "?";
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					DialogResult result = MessageBox.Show(message, null, buttons, MessageBoxIcon.Information);
					if (result == DialogResult.OK)
					{
						SqlCommand upd = new SqlCommand("UPDATE product SET productDesc = '" + prodDesc + "'," +
							" unitPrice ='" + uPrice + "', category = '" + pcateg + "' WHERE productID = '" + prodCode + "' ", con);

						con.Open();
						upd.ExecuteNonQuery();
						con.Close();

						clearFieldProd();

					}
				}
				else
				{
					string message = "Proceed to add " + prodCode + " to records?";
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					DialogResult result = MessageBox.Show(message, null, buttons, MessageBoxIcon.Information);
					if (result == DialogResult.OK)
					{
						SqlCommand adder = new SqlCommand("INSERT INTO product (productID, productDesc," +
							" unitPrice, category) VALUES('" + prodCode + "', '" + prodDesc + "' , '" + uPrice + "', '" + pcateg + "')", con);

						con.Open();
						adder.ExecuteNonQuery();
						con.Close();

						clearFieldProd();

						AutoCompleteStringCollection ProdCol = new AutoCompleteStringCollection();
						getProd(ProdCol);
						txtProdID.AutoCompleteCustomSource = ProdCol;
						getProd(ProdCol);
						txtProdCode.AutoCompleteCustomSource = ProdCol;

					}
				}
			}
		}

		//Warehouses
		private void btnWhseOK_Click(object sender, EventArgs e)
		{
			if (txtWhseCode.Text == "" || txtWhseLoc.Text == "" || txtWhseName.Text == "" || txtWhseCont.Text == "")
			{
				MessageBox.Show("Required Fields Empty!");
			}
			else
			{
				string whseCode = txtWhseCode.Text;
				string whseLoc = txtWhseLoc.Text;
				string whseName = txtWhseName.Text;
				string whseContact = txtWhseCont.Text;

				string query = "SELECT * FROM whse where whseID = '" + whseCode + "'";
				SqlDataAdapter checking = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				checking.Fill(dt);
				con.Close();

				if (dt.Rows.Count == 1)
				{
					string message = "Proceed to UPDATE " + whseCode + "?";
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);
					if (result == DialogResult.OK)
					{
						SqlCommand upd = new SqlCommand("UPDATE whse SET whseContactNo = '" + whseContact + "', whseName ='" + whseName + "'" +
							", whseLocation = '" + whseLoc + "' WHERE whseID = '" + whseCode + "' ", con);

						con.Open();
						upd.ExecuteNonQuery();

						loadDataWhse();
						con.Close();
						clearFieldWhse();
					}
				}
				else
				{
					string message = "Proceed to add " + whseCode + " to records?";
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);
					if (result == DialogResult.OK)
					{
						SqlCommand adder = new SqlCommand("INSERT INTO whse (whseID, whseName, whseLocation, whseContactNo) " +
							"VALUES('" + whseCode + "', '" + whseName + "' , '" + whseLoc + "', '" + whseContact + "')", con);

						con.Open();
						adder.ExecuteNonQuery();

						loadDataWhse();
						getDataCombo();
						con.Close();

						AutoCompleteStringCollection WhseCol = new AutoCompleteStringCollection();
						getWhse(WhseCol);
						txtWhseCode.AutoCompleteCustomSource = WhseCol;

						clearFieldWhse();
					}
				}
			}

}

		//Item Tranfer
		private void btnItemTransOK_Click_1(object sender, EventArgs e)
		{
            if (txtProdCode.Text == "" || txtTRoute.Text == "" || txtTRoute2.Text == "" || txtDevStat.Text == "" || quanty.Text == "")
            {
				MessageBox.Show("Required Fields Empty!");
            }
            else
            {
				
				string referN = "";

				string prodCode = txtProdCode.Text;
				string tR1 = txtTRoute.Text;
				string tR2 = txtTRoute2.Text;
				string refN = txtRefNo.Text.ToUpper();
				string transRoute = tR1 + " to " + tR2;
				string devStat = txtDevStat.Text;
				string qty = quanty.Text;
				int quantity;
				Int32.TryParse(qty, out quantity);

				DateTime dds = dateSend.Value.Date;
				DateTime ddr = dateRecieve.Value.Date;

				SqlCommand command;
				SqlDataAdapter adapter = new SqlDataAdapter();
				DataSet ds = new DataSet();
				string autoRef = "SELECT referenceNo FROM itemTransfer";
				int count = 1;

				try
				{
					command = new SqlCommand(autoRef, con);
					adapter.SelectCommand = command;
					adapter.Fill(ds);
					adapter.Dispose();
					command.Dispose();
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						count++;
					}
					if (count < 10)
					{
						referN = "TR-00000" + count.ToString();
					}
					else if (count > 9 && count < 100)
					{

						referN = "TR-0000" + count.ToString();

					}
					else if (count > 99 && count < 1000)
					{

						referN = "TR-000" + count.ToString();

					}
					else if (count > 999 && count < 10000)
					{

						referN = "TR-00" + count.ToString();

					}
					else if (count > 9999 && count < 100000)
					{

						referN = "TR-0" + count.ToString();

					}
					else if (count > 99999 && count < 1000000)
					{

						referN = "TR-" + count.ToString();

					}
					else
					{

					}

				}
				catch (Exception ex)
				{
					MessageBox.Show("Can not open connection ! ");
				}
				string delPr = "SELECT productID FROM itemTransfer WHERE referenceNo = '" + refN + "'";
				SqlDataAdapter delPro = new SqlDataAdapter(delPr, con);
				SqlCommand commen = new SqlCommand(delPr, con);
				DataTable tbPro = new DataTable();
				tbPro.Locale = System.Globalization.CultureInfo.InvariantCulture;
				delPro.Fill(tbPro);

				con.Open();
				string reference = (string)commen.ExecuteScalar();
				con.Close();

				string query = "SELECT * FROM itemTransfer WHERE referenceNo = '" + refN + "'";
				SqlDataAdapter checking = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				checking.Fill(dt);

				if (dt.Rows.Count == 1)
				{
					prodCode = reference;
				}
				//source
				string whQty = "SELECT quantity FROM whseInventory where productID = '" + prodCode + "' AND whseID = '" + tR1 + "'";
				SqlDataAdapter qt = new SqlDataAdapter(whQty, con);
				SqlCommand com2 = new SqlCommand(whQty, con);
				DataTable wQt = new DataTable();
				qt.Fill(wQt);
				//destination
				string whQty2 = "SELECT quantity FROM whseInventory where productID = '" + prodCode + "' AND whseID = '" + tR2 + "'";
				SqlDataAdapter qt2 = new SqlDataAdapter(whQty2, con);
				SqlCommand com22 = new SqlCommand(whQty2, con);
				DataTable wQt2 = new DataTable();
				qt2.Fill(wQt2);

				string delqty = "SELECT quantity FROM itemTransfer WHERE referenceNo = '" + refN + "'";
				SqlDataAdapter delqt = new SqlDataAdapter(delqty, con);
				SqlCommand comm = new SqlCommand(delqty, con);
				DataTable tbDel = new DataTable();
				delqt.Fill(tbDel);

				string delstr = "SELECT destination FROM itemTransfer WHERE referenceNo = '" + refN + "'";
				SqlDataAdapter delst = new SqlDataAdapter(delstr, con);
				SqlCommand comme = new SqlCommand(delstr, con);
				DataTable tbST = new DataTable();
				tbST.Locale = System.Globalization.CultureInfo.InvariantCulture;
				delst.Fill(tbST);

				con.Open();

				string wh = (string)comme.ExecuteScalar();

				con.Close();

				string des = "SELECT quantity FROM whseInventory where productID = '" + prodCode + "' AND whseID = '" + tR2 + "'";
				SqlDataAdapter de = new SqlDataAdapter(des, con);
				DataTable deq = new DataTable();
				de.Fill(deq);

				if (wQt.Rows.Count == 1)
				{
					con.Open();
					int whseQty = (int)com2.ExecuteScalar();
					con.Close();

					if (refN != "" && transRoute != "" && devStat != "")
					{

						if (dt.Rows.Count == 1)
						{
							con.Open();
							int delQuan = (int)comm.ExecuteScalar();
							con.Close();

							string message = "Proceed to UPDATE " + refN + "?";
							MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
							DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);
							if (result == DialogResult.OK)
							{
								SqlCommand upd = new SqlCommand("UPDATE itemTransfer SET deliveryStatus = '" + devStat + "'" +
									 "WHERE referenceNo = '" + refN + "'", con);
								con.Open();
								upd.ExecuteNonQuery();
								con.Close();

								// Auto-Add quantity to warehouse 

								if (devStat == "Recieved")
								{

									con.Open();
									SqlCommand dateRecieveada = new SqlCommand("UPDATE itemTransfer SET deliveryRecieve = @ddr " +
									 "WHERE referenceNo = '" + refN + "'", con);
									dateRecieveada.Parameters.Add("@ddr", SqlDbType.DateTime).Value = ddr;


									dateRecieveada.ExecuteNonQuery();
									con.Close();


									if (deq.Rows.Count != 0)
									{
										con.Open();
										int whseQty2 = (int)com22.ExecuteScalar();
										con.Close();
										int FwhseQty = delQuan + whseQty2;

										SqlCommand addQuantity = new SqlCommand("UPDATE whseInventory SET quantity = '" + FwhseQty + "' " +
										"WHERE productID = '" + reference + "' AND whseID = '" + wh + "'", con);

										con.Open();
										addQuantity.ExecuteNonQuery();
										con.Close();
										autoArchive();

										clearFieldTrans();

										loadDataTrans();
										loadDataWhseIn();
									}
									else
									{
										SqlCommand addera = new SqlCommand("INSERT INTO whseInventory (productID, " +
										" whseID, quantity) VALUES('" + reference + "', '" + tR2 + "', '" + delQuan + "')", con);

										con.Open();
										addera.ExecuteNonQuery();
										con.Close();
										autoArchive();

										clearFieldTrans();
										loadDataTrans();
										loadDataWhseIn();
									}
								}
								else if (devStat == "In-Transit")
								{
									SqlCommand dateRecieveada = new SqlCommand("UPDATE itemTransfer SET deliveryStatus = '" + devStat + "'" +
										"WHERE referenceNo = '" + refN + "'", con);

									con.Open();
									dateRecieveada.ExecuteNonQuery();
									con.Close();
								}
							}
						}
						else
						{

							string message = "Proceed to add " + referN + " to records?";
							MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
							DialogResult result = MessageBox.Show(message, null, buttons, MessageBoxIcon.Information);
							if (result == DialogResult.OK)
							{
								SqlCommand adder = new SqlCommand("INSERT INTO itemTransfer (referenceNo, productID, source, destination, transRoute" +
									",quantity, deliveryStatus, deliverySend) VALUES('" + referN + "', '" + prodCode + "', " +
									"'" + tR1 + "', '" + tR2 + "','" + transRoute + "','" + qty + "','" + devStat + "', @dds)", con);
								adder.Parameters.Add("@dds", SqlDbType.DateTime).Value = dds;

								con.Open();
								adder.ExecuteNonQuery();
								con.Close();

								//Auto-Subtract quantity to warehouse
								int FwhseQty2 = whseQty - quantity;

								SqlCommand subQuantity = new SqlCommand("UPDATE whseInventory SET quantity = '" + FwhseQty2 + "' " +
									"WHERE productID = '" + prodCode + "' AND whseID = '" + tR1 + "'", con);

								con.Open();
								subQuantity.ExecuteNonQuery();
								con.Close();

								clearFieldTrans();

								loadDataTrans();
								loadDataWhseIn();
							}

						}
					}
					else if (prodCode != "" && transRoute != "" && devStat != "")
					{

						string message = "Proceed to add " + referN + " to records?";
						MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
						DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);
						if (result == DialogResult.OK)
						{
							if (whseQty > quantity)
							{
								SqlCommand adder = new SqlCommand("INSERT INTO itemTransfer (referenceNo, productID, source, destination, transRoute" +
									",quantity, deliveryStatus, deliverySend) VALUES('" + referN + "', '" + prodCode + "' , " +
									"'" + tR1 + "', '" + tR2 + "','" + transRoute + "','" + quantity + "','" + devStat + "', @dds)", con);
								adder.Parameters.Add("@dds", SqlDbType.DateTime).Value = dds;

								con.Open();
								adder.ExecuteNonQuery();
								con.Close();

								//Auto-Subtract quantity to warehouse
								int FwhseQty2 = whseQty - quantity;

								SqlCommand subQuantity = new SqlCommand("UPDATE whseInventory SET quantity = '" + FwhseQty2 + "' " +
									"where productID = '" + prodCode + "' AND whseID = '" + tR1 + "'", con);

								con.Open();
								subQuantity.ExecuteNonQuery();
								con.Close();

								clearFieldTrans();

								AutoCompleteStringCollection RefCol = new AutoCompleteStringCollection();
								getRef(RefCol);
								txtRefNo.AutoCompleteCustomSource = RefCol;

								loadDataTrans();
								loadDataWhseIn();
							}
							else
							{
								MessageBox.Show("Not enough resources");
							}
						}

					}
				}

			}
			
		}
		private void btnInventorySave_Click(object sender, EventArgs e)
		{

			if (txtProdCode.Text != "" || txtProdDesc.Text != "" || categBox.Text != "" || txtPrice.Text != "" || txtWhseID.Text != "" || txtQty.Text != "")
			{
				string prodCode = txtProdID.Text;
				string prodDesc = txtProdDesc.Text;
				string whseID = txtWhseID.Text;
				string qq = txtQty.Text;
				double uPrice = Convert.ToDouble(txtPrice.Text);
				string pcateg = categBox.Text;
				int txtQuant;
				Int32.TryParse(qq, out txtQuant);

				string whQty = "SELECT quantity FROM whseInventory where productID = '" + prodCode + "' AND whseID = '" + whseID + "'";
				SqlDataAdapter qt = new SqlDataAdapter(whQty, con);
				SqlCommand com2 = new SqlCommand(whQty, con);
				DataTable wQt = new DataTable();
				wQt.Locale = System.Globalization.CultureInfo.InvariantCulture;
				qt.Fill(wQt);
				con.Close();

				string checkProd = "SELECT productID FROM product where productID = '" + prodCode + "'";
				SqlDataAdapter prodCk = new SqlDataAdapter(checkProd, con);
				DataTable ckProd = new DataTable();
				ckProd.Locale = System.Globalization.CultureInfo.InvariantCulture;
				prodCk.Fill(ckProd);
				con.Close();

				string query = "SELECT * FROM whseInventory where productID = '" + prodCode + "' AND whseID = '" + whseID + "'";
				SqlDataAdapter checking = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
				checking.Fill(dt);
				con.Close();

				if (prodCode != "" && whseID != "")
				{
					if (dt.Rows.Count == 0)
					{
						if (ckProd.Rows.Count == 0)
						{

							string message1 = "" + prodCode + " does not exists in records, proceed to create new record?";
							MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
							DialogResult result1 = MessageBox.Show(message1, "alert", buttons, MessageBoxIcon.Information);
							if (result1 == DialogResult.OK)
							{
								SqlCommand adder1 = new SqlCommand("INSERT INTO product (productID, productDesc," +
									" unitPrice, category) VALUES('" + prodCode + "', '" + prodDesc + "' , '" + uPrice + "', '" + pcateg + "')", con);

								con.Open();
								adder1.ExecuteNonQuery();
								con.Close();

								clearFieldProd();

								AutoCompleteStringCollection ProdCol = new AutoCompleteStringCollection();
								getProd(ProdCol);
								txtProdID.AutoCompleteCustomSource = ProdCol;
								getProd(ProdCol);
								txtProdCode.AutoCompleteCustomSource = ProdCol;

								string message = "Proceed to add " + txtQuant + "pcs. of " + prodCode + " in " + whseID + "?";
								DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);


								if (result == DialogResult.OK)
								{
									SqlCommand adder = new SqlCommand("INSERT INTO whseInventory (productID," +
										" whseID, quantity) VALUES('" + prodCode + "', '" + whseID + "', '" + txtQuant + "')", con);

									con.Open();
									adder.ExecuteNonQuery();

								}

								loadDataWhseIn();
								con.Close();

							}
						}
						else
						{
							string message = "Proceed to add " + txtQuant + "pcs. of " + prodCode + " in " + whseID + "?";
							MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
							DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);


							if (result == DialogResult.OK)
							{
								SqlCommand adder = new SqlCommand("INSERT INTO whseInventory (productID," +
									" whseID, quantity) VALUES('" + prodCode + "', '" + whseID + "', '" + txtQuant + "')", con);

								con.Open();
								adder.ExecuteNonQuery();

							}


							loadDataWhseIn();
							con.Close();
						}
					}
					else
					{
						con.Open();
						int upQuant = (int)com2.ExecuteScalar();
						con.Close();
						int finQuant = txtQuant + upQuant;

						string message = "Proceed to add/subtract " + txtQuant + "pcs. of " + prodCode + " in " + whseID + "?";
						MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
						DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);
						if (result == DialogResult.OK)
						{
							SqlCommand upd = new SqlCommand("UPDATE whseInventory SET quantity = '" + finQuant + "'" +
								" WHERE productID = '" + prodCode + "' AND whseID = '" + whseID + "'", con);

							con.Open();
							upd.ExecuteNonQuery();

							loadDataWhseIn();
							con.Close();

						}
					}

				}
				else
				{
					MessageBox.Show("Required Field/Fields Empty");
				}
			}
		}
		private void btnCateg_Click(object sender, EventArgs e)
		{
			string message = "Proceed to manage cateogory?";
			MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
			DialogResult result = MessageBox.Show(message, "alert" , buttons, MessageBoxIcon.Information);
			if (result == DialogResult.OK)
			{
				newCategory newCat = new newCategory();
				newCat.Show();
			}
		}
		private void btnProdInventoryCancel_Click(object sender, EventArgs e)
		{
			clearFieldProd();
		}
		private void btnItemTransCancel_Click(object sender, EventArgs e)
		{
			clearFieldTrans();
		}
		private void btnWhseCancel_Click(object sender, EventArgs e)
		{
			clearFieldWhse();
		}
		void clearFieldProd()
        {
			txtProdID.Text = "";
			txtProdDesc.Text = " ";
			txtPrice.Text = "0";
			categBox.SelectedIndex = 0;
		}
		void clearFieldTrans()
        {
			txtRefNo.Text = "";
			txtProdCode.Text = "";
			quanty.Text = "0";
			txtTRoute.SelectedIndex = 0;
			txtTRoute2.SelectedIndex = 0;
			txtDevStat.SelectedIndex = 0;
			dateSend.Value = DateTime.Now;
			dateRecieve.Value = DateTime.Now;
		}
		void clearFieldWhse()
        {
			txtWhseCode.Text = "";
			txtWhseName.Text = "";
			txtWhseCont.Text = "";
			txtWhseLoc.Text = "";
		}
		public void OpenProductList(object sender, EventArgs e)
		{
			ProductSearch frm = new ProductSearch(this);
			frm.Show();
		}
		//Return to defaultView after exit
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DefaultView df = new DefaultView();
			df.Show();
			this.Dispose();
		}

        private void AdministratorView_FormClosing(object sender, FormClosingEventArgs e)
        {
			DefaultView df = new DefaultView();
			df.Show();
        }
		// Delete
        private void btnDelete_Click(object sender, EventArgs e)
        {
			string prodCode = txtProdID.Text;
			
			if(prodCode == "")
            {
				MessageBox.Show("No Product Selected");
            }
            else
            {
				string query = "SELECT DISTINCT product.productID FROM product INNER JOIN whseInventory ON product.productID = whseInventory.productID WHERE whseInventory.productID  = '" + prodCode + "' AND whseInventory.quantity > 0";

				SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog = BCWhseInvtrySys; Integrated Security = True");
				SqlDataAdapter adapt = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				adapt.Fill(dt);

				if (dt.Rows.Count > 0)
				{
					string message = "Cannot delete " + prodCode + " in product records, item currently in stock.";
					MessageBox.Show(message, null);
				}
				else
				{
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					string message = "Proceed to delete " + prodCode + " in product records?";
					DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);

					if (result == DialogResult.OK)
					{
						string deleteQuery = "DELETE FROM product WHERE productID = '" + prodCode + "'";
						SqlCommand del = new SqlCommand(deleteQuery, con);
						con.Open();
						del.ExecuteNonQuery();
						con.Close();
					}

				}
			}
			
		}

        private void btnWHdelete_Click(object sender, EventArgs e)
        {
			string whCode = txtWhseCode.Text;
			if(whCode == "")
            {
				MessageBox.Show("No Warehouse Selected");
			}
            else
            {
				
				string query = "SELECT whse.whseID FROM whse INNER JOIN whseInventory ON whse.whseID = whseInventory.whseID WHERE whseInventory.whseID  = '" + whCode + "'";

				SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog = BCWhseInvtrySys; Integrated Security = True");
				SqlDataAdapter adapt = new SqlDataAdapter(query, con);
				DataTable dt = new DataTable();
				adapt.Fill(dt);

				if (dt.Rows.Count > 0)
				{
					string message = "Cannot delete " + whCode + " in product records, warehouse currently in use.";
					MessageBox.Show(message, null);
				}
				else
				{
					MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
					string message = "Proceed to delete " + whCode + " in warehouse records?";
					DialogResult result = MessageBox.Show(message, "alert", buttons, MessageBoxIcon.Information);

					if (result == DialogResult.OK)
					{
						try
						{
							string deleteQuery = "DELETE FROM whse WHERE whseID = '" + whCode + "'";
							SqlCommand del = new SqlCommand(deleteQuery, con);
							con.Open();
							del.ExecuteNonQuery();
							con.Close();
							loadDataWhse();
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message, null);
						}

					}

				}
			}
			
		}

		//Double Click Auto Fill
		private void DataGridProdSelect(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex == -1) 
			{
				return; 
			}
            else
            {
				SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True");
				try
				{
					DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
					string a = row.Cells[0].Value.ToString();
					string selProd = "SELECT * FROM product WHERE productID = '" + a + "'";
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
							txtProdID.Text = (read["productID"].ToString());

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
		private void DataGridTransferSelect(object sender, DataGridViewCellEventArgs e)
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
					DataGridViewRow row = this.dataGridView2.Rows[e.RowIndex];
					string a = row.Cells[0].Value.ToString();
					string selTR = "SELECT * FROM itemTransfer WHERE referenceNo = '" + a + "'";
					SqlDataAdapter ar = new SqlDataAdapter(selTR, con);
					SqlCommand com = new SqlCommand(selTR, con);
					DataTable da = new DataTable();
					ar.Fill(da);
					if (da.Rows.Count == 1)
					{
						con.Open();
						SqlDataReader read = com.ExecuteReader();
						while (read.Read())
						{
							txtRefNo.Text = (read["referenceNo"].ToString());
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
		private void DataGridWhseSelect(object sender, DataGridViewCellEventArgs e)
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
					DataGridViewRow row = this.dataGridView3.Rows[e.RowIndex];
					string a = row.Cells[0].Value.ToString();
					string selWhse = "SELECT * FROM whse WHERE whseID = '" + a + "'";
					SqlDataAdapter ar = new SqlDataAdapter(selWhse, con);
					SqlCommand com = new SqlCommand(selWhse, con);
					DataTable da = new DataTable();
					ar.Fill(da);
					if (da.Rows.Count == 1)
					{
						con.Open();
						SqlDataReader read = com.ExecuteReader();
						while (read.Read())
						{
							txtWhseCode.Text = (read["whseID"].ToString());
						}
						read.Close();
						con.Close();


					}
				}
                catch(Exception ee)
                {
					MessageBox.Show(ee.Message);
                }
            }
			
		}
		//Allow only numbers
		private void NumbersOnly_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Verify that the pressed key isn't CTRL or any non-numeric digit
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
			{
				e.Handled = true;
			}

			if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
			{
				e.Handled = true;
			}
		}
        private void viewArchives_Click(object sender, EventArgs e)
        {
			TransferArchives ta = new TransferArchives();
			ta.Show();
        }
		void autoArchive()
        {
			string checkString = "Recieved";
            try
            {
				SqlConnection con = new SqlConnection(@"Data Source = DESKTOP-VU2IJ9S; Initial Catalog=BCWhseInvtrySys; Integrated Security=True");
				string ins = "INSERT INTO arch_trans(referenceNo, productID, source, destination, transRoute, quantity, deliveryStatus, deliverySend, deliveryRecieve) " +
					"SELECT referenceNo, productID, source, destination, transRoute, quantity, deliveryStatus, deliverySend, deliveryRecieve FROM itemTransfer WHERE deliveryStatus = '" + checkString + "'";
				string deleteQuery = "DELETE FROM itemTransfer WHERE deliveryStatus =  '" + checkString + "'";
				SqlCommand com = new SqlCommand(ins, con);
				SqlCommand del = new SqlCommand(deleteQuery, con);
				con.Open();
				MessageBox.Show("Transaction '" + txtRefNo.Text + "' complete. Record stored in archives.");
				com.ExecuteNonQuery();
				del.ExecuteNonQuery();
				con.Close();

			}
            catch(Exception ee)
            {
				MessageBox.Show(ee.Message);
            }
			
		}
    }
}
	

