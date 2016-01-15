using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb; /* DB */
using System.Net.Mail; /* Send email */

namespace Glad2Cook
{
    public partial class View : System.Web.UI.Page
    {
        public View() {
            Init += new EventHandler(View_Init);
        }

        decimal priceTotal = 0;
        int quantityTotal = 0;

        protected void View_Init(object sender, EventArgs e)
        {
            // *** Auto Log In Code ***
            string svUserId = (string)(Session["UserId"]);
            string svDisplayName = (string)(Session["DisplayName"]);

            // If one of the session vars is empty -- Meaning NOT logged in
            if ((string.IsNullOrEmpty(svUserId) == true) || (string.IsNullOrEmpty(svDisplayName) == true))
            {
                // Try to get UserId from Cookie                
                string strUserId = GlobalClass.readUserIdFromCookie();
                if (strUserId != null)
                {
                    // If valid User Id try to look up User in DB & populate session vars
                    if (GlobalClass.getUserInfo(strUserId) == true)
                    {
                        svDisplayName = (string)(Session["DisplayName"]);
                        GlobalClass.checkFavorites();
                        GlobalClass.logLogin("Shopping");
                    }
                    else 
                    {
                        Response.Redirect("SignIn.aspx");
                    }
                }
                else
                {
                    // If session vars do not have valid values then back to the home page
                    Response.Redirect("SignIn.aspx");
                }
            }

            lblDisplayName1.Text = svDisplayName;
            lblDisplayName2.Text = svDisplayName;
            // *** Auto Log In Code ***

            if (IsPostBack == false) { LoadSendTo(); }
            LoadGrid();
        }

        protected void updateGList(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "buyAll") || (e.CommandName == "buyOne"))
            {
                string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
                OleDbConnection conn = new OleDbConnection(connectionString);

                int index = Convert.ToInt32(e.CommandArgument);
                string item = GoShopping.DataKeys[index].Values[0].ToString();
                string sql;
                string svUserId = (string)(Session["UserId"]);
                if (e.CommandName == "buyAll")
                    sql = "Update Item set Item.ItemQuantity=0, ItemModifyDate=now Where Item.ItemUserId='" + svUserId + "' and Item.ItemId=" + item;
                else
                    sql = "Update Item set Item.ItemQuantity=Item.ItemQuantity-1, ItemModifyDate=now Where Item.ItemUserId='" + svUserId + "' and Item.ItemId=" + item;

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                conn.Open();

                OleDbDataReader reader;
                reader = cmd.ExecuteReader();

                reader.Close();
                conn.Close();

                GoShopping.DataBind();
            }
        }

        protected void LoadSendTo()
        {
                lstSendTo.Items.Clear();

                lstSendTo.Items.Add( new ListItem("Send List To...","SendListTo"));
                
                string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
                OleDbConnection conn = new OleDbConnection(connectionString);

                OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

                OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

                string svUserId = (string)(Session["UserId"]);
                myOleDbCommand.CommandText = "SELECT G2CUser.UserName, G2CUser.UserEmail, G2CUser.UserToName1, G2CUser.UserToEmail1, G2CUser.UserToSMS1, G2CUser.UserToName2, G2CUser.UserToEmail2, G2CUser.UserToSMS2, G2CUser.UserToName3, G2CUser.UserToEmail3, G2CUser.UserToSMS3 FROM G2CUser WHERE G2CUser.UserId='" + svUserId + "'";
                myOleDbConnection.Open();

                OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

                myOleDbDataReader.Read();

                // User 1
                string strListDisplay = "";
                string strListValue = "";
                if (myOleDbDataReader.GetValue(0).ToString() == "")
                    {strListDisplay = myOleDbDataReader.GetValue(1).ToString() + " (Email)";}
                else
                    {strListDisplay = myOleDbDataReader.GetValue(0).ToString() + " (Email)";}
                strListValue = myOleDbDataReader.GetValue(1).ToString();
                lstSendTo.Items.Add(new ListItem(strListDisplay, strListValue));
                
                // User 2
                strListDisplay = "";
                strListValue = "";
                if (myOleDbDataReader.GetValue(3).ToString() != "") // if Email is provided
                {
                    if (myOleDbDataReader.GetValue(2).ToString() == "") // if User name is not provided
                    { strListDisplay = myOleDbDataReader.GetValue(3).ToString(); }
                    else
                    { strListDisplay = myOleDbDataReader.GetValue(2).ToString(); }
                    if (myOleDbDataReader.GetValue(4).ToString() == "True") // if is Text Message Account
                        {strListDisplay += " (Text)";}
                    else 
                        {strListDisplay += " (Email)";}
                    strListValue = myOleDbDataReader.GetValue(3).ToString();
                    lstSendTo.Items.Add(new ListItem(strListDisplay, strListValue));
                }

                // User 3
                strListDisplay = "";
                strListValue = "";
                if (myOleDbDataReader.GetValue(6).ToString() != "") // if Email is provided
                {
                    if (myOleDbDataReader.GetValue(5).ToString() == "") // if User name is not provided
                    { strListDisplay = myOleDbDataReader.GetValue(6).ToString(); }
                    else
                    { strListDisplay = myOleDbDataReader.GetValue(5).ToString(); }
                    if (myOleDbDataReader.GetValue(7).ToString() == "True") // if is Text Message Account
                    { strListDisplay += " (Text)"; }
                    else
                    { strListDisplay += " (Email)"; }
                    strListValue = myOleDbDataReader.GetValue(6).ToString();
                    lstSendTo.Items.Add(new ListItem(strListDisplay, strListValue));
                }

                // User 4
                strListDisplay = "";
                strListValue = "";
                if (myOleDbDataReader.GetValue(9).ToString() != "") // if Email is provided
                {
                    if (myOleDbDataReader.GetValue(8).ToString() == "") // if User name is not provided
                    { strListDisplay = myOleDbDataReader.GetValue(9).ToString(); }
                    else
                    { strListDisplay = myOleDbDataReader.GetValue(8).ToString(); }
                    if (myOleDbDataReader.GetValue(10).ToString() == "True") // if is Text Message Account
                    { strListDisplay += " (Text)"; }
                    else
                    { strListDisplay += " (Email)"; }
                    strListValue = myOleDbDataReader.GetValue(9).ToString();
                    lstSendTo.Items.Add(new ListItem(strListDisplay, strListValue));
                }
                myOleDbDataReader.Close();
                myOleDbConnection.Close();            
       }
        
        protected void LoadGrid()
        {
            string sql = null;
            string svUserId = (string)(Session["UserId"]);
            sql = "SELECT Item.ItemName, Category.CategoryName, Item.ItemAisle, Item.ItemPrice, Item.ItemQuantity, [ItemPrice]*[ItemQuantity] AS Total, Item.ItemNotes, Item.ItemId";
            sql = sql + " FROM Category INNER JOIN Item ON Category.CategoryId = Item.ItemCategoryId WHERE Item.ItemUserId='" + svUserId + "' AND Item.ItemQuantity > 0 ORDER BY Category.CategoryName, Item.ItemName";
            AccessDataSource1.SelectCommand = sql;
        }

        protected void SendTextMessage(string address)
        {
            string strMsg = "Email (Text) Error";
            strMsg = GetListForTexting().Trim();

            MailMessage m = new MailMessage();
            m.From = new MailAddress("message@glad2cook.com");
            m.To.Add(address);
            m.IsBodyHtml = false;
            m.Body = strMsg;

            SmtpClient sc = new SmtpClient();
            sc.Host = System.Configuration.ConfigurationSettings.AppSettings["SMTP-Server"];
            sc.Send(m);

            // reset list box
            try
            {
                lstSendTo.SelectedIndex = 0;
            }
            catch { }
            finally { }
        }

        protected void SendEmailMessage(string address)
        {
                string strMsg = "Email Error";
                strMsg = GetEmailList();
               
                MailMessage m = new MailMessage();
                m.From = new MailAddress("message@glad2cook.com");
                m.To.Add(address);
                m.Subject = "Grocery List from Glad2Cook.com";
                m.IsBodyHtml = true;
                m.Body = strMsg;
                             
                SmtpClient sc = new SmtpClient();
                sc.Host = System.Configuration.ConfigurationSettings.AppSettings["SMTP-Server"];
                sc.Send(m);

                // reset list box
                try
                {
                    lstSendTo.SelectedIndex = 0;
                }
                catch { }
                finally { }
        }

        protected void lstSendTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSendTo.SelectedValue != "SendListTo")
            {
                if (lstSendTo.SelectedItem.Text.EndsWith("(Text)") == true)
                {
                    SendTextMessage(lstSendTo.SelectedValue);
                }
                else 
                {
                    SendEmailMessage(lstSendTo.SelectedValue);
                }
            }
        }

        protected string GetEmailList()
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();
            
            string svUserId = (string)(Session["UserId"]);
            myOleDbCommand.CommandText = "SELECT Item.ItemQuantity, Item.ItemName, Category.CategoryName, Item.ItemAisle, Item.ItemPrice, [ItemPrice]*[ItemQuantity] AS Total, Item.ItemNotes FROM Category INNER JOIN Item ON Category.CategoryId = Item.ItemCategoryId WHERE Item.ItemUserId='" + svUserId + "' AND Item.ItemQuantity > 0 ORDER BY Category.CategoryName, Item.ItemName";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            string msgHTML = null;

            msgHTML = "<font size='2' face='arial'><a href='http://glad2cook.com'>Access your grocery list online at Glad2Cook.com</a></font><br /><br />";
                        
            msgHTML += "<table cellpadding='4' cellspacing='0'>";
            msgHTML += "<tr><th align='center'><font size='2' face='arial'>";
            msgHTML += "Qty";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Item";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Category";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Aisle";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Price";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Total";
            msgHTML += "</font></th><th align='center'><font size='2' face='arial'>";
            msgHTML += "Notes";
            msgHTML += "</font></th></tr>";

            string flgGrey = "Yes";
            Int32 intItems = 0;
            Double intTotal = 0;

            while (myOleDbDataReader.Read() == true)
            {
                if (flgGrey == "Yes")
                {
                    msgHTML += "<tr bgcolor='#DCDCDC'>";
                    flgGrey = "No";
                }
                else
                {
                    msgHTML += "<tr bgcolor='#FFFFFF'>";
                    flgGrey = "Yes";
                }

                msgHTML += "<td align='right'><font size='2' face='arial'>";
                msgHTML += myOleDbDataReader["ItemQuantity"];
                msgHTML += "</font></td><td align='left'><font size='2' face='arial'>";
                msgHTML += myOleDbDataReader["ItemName"];
                msgHTML += "</font></td><td align='left'><font size='2' face='arial'>";
                msgHTML += myOleDbDataReader["CategoryName"];
                msgHTML += "</font></td><td align='right'><font size='2' face='arial'>";
                msgHTML += myOleDbDataReader["ItemAisle"];
                msgHTML += "</font></td><td align='right'><font size='2' face='arial'>";
                msgHTML += String.Format("{0:c}", myOleDbDataReader["ItemPrice"]);
                msgHTML += "</font></td><td align='right'><font size='2' face='arial'>";
                msgHTML += String.Format("{0:c}", myOleDbDataReader["Total"]);
                msgHTML += "</font></td><td align='left'><font size='2' face='arial'>";
                msgHTML += myOleDbDataReader["ItemNotes"];
                msgHTML += "</font></td></tr>";

                intItems++;
                if (!myOleDbDataReader.IsDBNull(5))
                {
                    intTotal = intTotal + Convert.ToDouble(myOleDbDataReader["Total"]);
                }
                   
            }
            msgHTML += "<tr><td align='center'><font size='2' face='arial'>";
            msgHTML += "";
            msgHTML += "</font></td><td align='right'><font size='2' face='arial'><b>";
            msgHTML += "Unique Items";
            msgHTML += "</b></font></td><td align='left'><font size='2' face='arial'><b>";
            msgHTML += intItems;
            msgHTML += "</b></font></td><td align='center'><font size='2' face='arial'>";
            msgHTML += "";
            msgHTML += "</font></td><td align='right'><font size='2' face='arial'><b>";
            msgHTML += "Total";
            msgHTML += "</b></font></td><td align='right'><font size='2' face='arial'><b>";
            msgHTML += String.Format("{0:c}",intTotal);
            msgHTML += "</b></font></td><td align='center'><font size='2' face='arial'>";
            msgHTML += "";
            msgHTML += "</font></td></tr>";
            msgHTML += "</table>";
                                   
            myOleDbDataReader.Close();
            myOleDbConnection.Close();

            return msgHTML;
        }

        protected string GetListForTexting()
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            string svUserId = (string)(Session["UserId"]);
            myOleDbCommand.CommandText = "SELECT Item.ItemQuantity, Item.ItemName FROM Category INNER JOIN Item ON Category.CategoryId = Item.ItemCategoryId WHERE Item.ItemUserId='" + svUserId + "' AND Item.ItemQuantity > 0 ORDER BY Category.CategoryName, Item.ItemName";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            string msg = null;
            Int32 qty;
            msg = "Grocery List: ";

            while (myOleDbDataReader.Read() == true)
            {
                if (msg != "Grocery List: ")
                {
                    msg += ", ";
                }

                qty = myOleDbDataReader.GetInt32(0);
                if (qty > 1)
                {
                    msg += myOleDbDataReader["ItemQuantity"];
                    msg += " ";
                }
                msg += myOleDbDataReader["ItemName"];
            }
            myOleDbDataReader.Close();
            myOleDbConnection.Close();
                        
            return msg;
        }

        protected void GListLoading(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // add the Items and Totals to the running total variables
                if (DataBinder.Eval(e.Row.DataItem, "Total") != System.DBNull.Value)
                {
                    priceTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Total"));
                }
                quantityTotal += 1;
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                // display the totals in the footer
                e.Row.Cells[3].Text = "Unique Items";
                e.Row.Cells[4].Text = quantityTotal.ToString("d");
                e.Row.Cells[6].Text = "Total";
                e.Row.Cells[7].Text = priceTotal.ToString("c");
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Font.Bold = true;
            }
            // code below shouldn't be needed... but just in case
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                priceTotal = 0;
                quantityTotal = 0;
            }
        }

        protected void lnkbtnSignOut_Click(object sender, EventArgs e)
        {
            GlobalClass.logOut();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            string svUserId = (string)(Session["UserId"]);
            sql = "Update Item set Item.ItemQuantity=0, ItemModifyDate=now Where Item.ItemQuantity > 0 and Item.ItemUserId='" + svUserId + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);
            OleDbDataReader reader = null;

            string errorMsg = "";
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }

                GoShopping.DataBind();
            }
        }
    }
}
