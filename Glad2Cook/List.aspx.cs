using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb; /* DB */

namespace Glad2Cook
{
    public partial class List : System.Web.UI.Page
    {
        public List() {
            Init += new EventHandler(List_Init);
            Unload += new EventHandler(List_Unload);
        }

        protected void List_Init(object sender, EventArgs e)
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
                        GlobalClass.logLogin("List");
                    }
                    else
                    {
                        Response.Redirect("SignIn.aspx?page=List");
                    }
                }
                else
                {
                    // If session vars do not have valid values then back to the home page
                    Response.Redirect("SignIn.aspx?page=List");
                }
            }

            lblDisplayName.Text = svDisplayName;
            // *** Auto Log In Code ***
            if (IsPostBack == false) 
            {
                UseFavorites();
                LoadShow(); 
            }
        }

        protected void List_Unload(object sender, EventArgs e)
        {
           Session["FirstVisit"] = "No";
        }

        protected void updateList(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "addOne") 
            {
                string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
                OleDbConnection conn = new OleDbConnection(connectionString);

                int index = Convert.ToInt32(e.CommandArgument);

                string item = MakeList.DataKeys[index].Values[0].ToString();
                string sql;
                string svUserId = (string)(Session["UserId"]);
                sql = "Update Item set Item.ItemQuantity=Item.ItemQuantity+1, Item.ItemTimesUpdated=Item.ItemTimesUpdated+1, Item.ItemModifyDate=now Where Item.ItemUserId='" + svUserId + "' and Item.ItemId=" + item;

                OleDbCommand cmd = new OleDbCommand(sql, conn);
                conn.Open();

                OleDbDataReader reader;
                reader = cmd.ExecuteReader();

                reader.Close();
                conn.Close();

                MakeList.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            // If built in validation finds an error
            if (IsValid != true) { return; }
            TextBox Qty = MakeList.FooterRow.FindControl("txtQty") as TextBox;
            TextBox ItemName = MakeList.FooterRow.FindControl("txtItem") as TextBox;
            DropDownList Category = MakeList.FooterRow.FindControl("lstCategory") as DropDownList;
            TextBox Aisle = MakeList.FooterRow.FindControl("txtAisle") as TextBox;
            TextBox Price = MakeList.FooterRow.FindControl("txtPrice") as TextBox;
            TextBox Notes = MakeList.FooterRow.FindControl("txtNotes") as TextBox;

            AccessDataSource1.InsertParameters["ItemQuantity"].DefaultValue = Qty.Text;
            AccessDataSource1.InsertParameters["ItemName"].DefaultValue = ItemName.Text;
            AccessDataSource1.InsertParameters["ItemCategoryId"].DefaultValue = Category.SelectedValue;
            AccessDataSource1.InsertParameters["ItemAisle"].DefaultValue = Aisle.Text;
            AccessDataSource1.InsertParameters["ItemPrice"].DefaultValue = Price.Text;
            AccessDataSource1.InsertParameters["ItemNotes"].DefaultValue = Notes.Text;
            AccessDataSource1.Insert();
            
            //btnAddItem.Enabled = true;
            //MakeList.ShowFooter = false;
        }

        //protected void btnAddItem_Click(object sender, EventArgs e)
        //{
        //    if (btnAddItem.Text == "Show Add Items")
        //    {
        //        MakeList.ShowFooter = true;
        //        btnAddItem.Text = "Hide Add Items";
        //        //btnAddItem.Enabled = false;
        //    }
        //    else 
        //    {
        //        MakeList.ShowFooter = false;
        //        btnAddItem.Text = "Show Add Items";
        //    }
        //}

        protected void lnkbtnSignOut_Click(object sender, EventArgs e)
        {
            GlobalClass.logOut();
        }

        //int rowCnt = 0;
        protected void mlRowUpdate(object sender, GridViewRowEventArgs e)
        {
            // hide empty row
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "ItemName").ToString() == "~")
                {
                    e.Row.Visible = false;
                }
                //rowCnt += 1;
            }
            //else if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if (rowCnt < 1) // force footer to show when no rows (well 1 insvible row)
            //    {
            //        //MakeList.ShowFooter = true;
            //        //btnAddItem.Enabled = false;
            //    }
            //}
            //else if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    rowCnt = 0;
            //}
        }

        protected void AccessDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (lstShow.SelectedValue == "Favorite") // need to add fav filter
            {
                Int32 iFav = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["NumberOfUpdatesForFavorites"]);
                e.Command.Parameters[2].Value = 0;
                e.Command.Parameters[3].Value = 9999;
                e.Command.Parameters[4].Value = iFav;
            }
            else if (lstShow.SelectedValue == "OnList")
            {
                e.Command.Parameters[2].Value = 1;
                e.Command.Parameters[3].Value = 9999;
                e.Command.Parameters[4].Value = 0;
            }
            else if (lstShow.SelectedValue == "NotOnList")
            {
                e.Command.Parameters[2].Value = 0;
                e.Command.Parameters[3].Value = 0;
                e.Command.Parameters[4].Value = 0;
            }
            else // All
            {
                e.Command.Parameters[2].Value = 0;
                e.Command.Parameters[3].Value = 9999;
                e.Command.Parameters[4].Value = 0;
            }
        }

        protected void UseFavorites()
        {
            // 0 - Off, 1 - New, 2 - First Use, 3 - On
            string svFavorites = (string)(Session["Favorites"]);
            if (svFavorites == "2")
            {
                litFavorites.Text = "<div align='right'>";
                litFavorites.Text += "<div class='displayHelpOnOff' id='box2nd' align='center' style='width: 460px;' >";
                litFavorites.Text += "<div>";
                litFavorites.Text += "<b class='boxHelpOnOff'>";
                litFavorites.Text += "<b class='boxHelpOnOff1'><b></b></b>";
                litFavorites.Text += "<b class='boxHelpOnOff2'><b></b></b>";
                litFavorites.Text += "<b class='boxHelpOnOff3'></b>";
                litFavorites.Text += "<b class='boxHelpOnOff4'></b>";
                litFavorites.Text += "<b class='boxHelpOnOff5'></b></b>";
                litFavorites.Text += "<div class='boxHelpOnOfffg'>";
                litFavorites.Text += "Use Show Favorite Items below to show frequently added items";
                litFavorites.Text += "</div>";
                litFavorites.Text += "<b class='boxHelpOnOff'>";
                litFavorites.Text += "<b class='boxHelpOnOff5'></b>";
                litFavorites.Text += "<b class='boxHelpOnOff4'></b>";
                litFavorites.Text += "<b class='boxHelpOnOff3'></b>";
                litFavorites.Text += "<b class='boxHelpOnOff2'><b></b></b>";
                litFavorites.Text += "<b class='boxHelpOnOff1'><b></b></b></b>";
                litFavorites.Text += "</div>";
                litFavorites.Text += "</div>";
                litFavorites.Text += "</div>";
     
                UpdateUsersFavoritesToOn();
                Session["Favorites"] = "3";
            }

        }
        
        protected void UpdateUsersFavoritesToOn() // 3
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            string svUserId = (string)(Session["UserId"]);
            sql = "Update G2CUser set G2CUser.UserFavorites=3, G2CUser.UserModifyDate=now Where G2CUser.UserId='" + svUserId + "'";

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
            }
        }










        protected void LoadShow()
        {
            lstShow.Items.Clear();
            lstShow.Items.Add(new ListItem("Show All Items", "All"));
            lstShow.Items.Add(new ListItem("Show Items On List", "OnList"));
            lstShow.Items.Add(new ListItem("Show Items Not On List", "NotOnList"));

            string svFavorites = (string)(Session["Favorites"]);
            if (svFavorites == "2" || svFavorites == "3")
            {
                lstShow.Items.Add(new ListItem("Show Favorite Items", "Favorite"));
            }
        }


    }
}
