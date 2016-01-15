using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb; /* DB */

namespace Glad2Cook
{
    public partial class Preferences : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
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
                        GlobalClass.logLogin("Preferences");
                    }
                    else
                    {
                        Response.Redirect("SignIn.aspx?page=Preferences");
                    }
                }
                else
                {
                    // If session vars do not have valid values then back to the home page
                    Response.Redirect("SignIn.aspx?page=Preferences");
                }
            }
            lblDisplayName.Text = svDisplayName;
            // *** Auto Log In Code ***

            LoadPrefs();
        }

        protected void lnkbtnSignOut_Click(object sender, EventArgs e)
        {
            GlobalClass.logOut();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["page"] == "List")
            {
                Response.Redirect("List.aspx");
            }
            else
            {
                Response.Redirect("Shopping.aspx");
            }
        }
        
        public bool LoadPrefs()
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            //OleDbConnection conn = new OleDbConnection(connectionString); 
            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();
            OleDbDataReader myOleDbDataReader = null;

            string svUserId = (string)(Session["UserId"]);
            myOleDbCommand.CommandText = "SELECT UserName, UserEmail, UserToName1, UserToEmail1, UserToSMS1, UserToName2, UserToEmail2, UserToSMS2, UserToName3, UserToEmail3, UserToSMS3 FROM G2CUser WHERE UserId = '" + svUserId + "'" ;

            string errorMsg = "";
            try
            {
                myOleDbConnection.Open();
                myOleDbDataReader = myOleDbCommand.ExecuteReader();
                myOleDbDataReader.Read();

                if (myOleDbDataReader.HasRows == true)
                {
                    txtAccName.Text = myOleDbDataReader["UserName"].ToString();
                    txtAccEmail.Text = myOleDbDataReader["UserEmail"].ToString();
                    hfOldEmail.Value = myOleDbDataReader["UserEmail"].ToString();

                    txtToName1.Text = myOleDbDataReader["UserToName1"].ToString();
                    txtToEmail1.Text = myOleDbDataReader["UserToEmail1"].ToString();
                    chkToSMS1.Checked = Convert.ToBoolean(myOleDbDataReader["UserToSMS1"]);

                    txtToName2.Text = myOleDbDataReader["UserToName2"].ToString();
                    txtToEmail2.Text = myOleDbDataReader["UserToEmail2"].ToString();
                    chkToSMS2.Checked = Convert.ToBoolean(myOleDbDataReader["UserToSMS2"]);

                    txtToName3.Text = myOleDbDataReader["UserToName3"].ToString();
                    txtToEmail3.Text = myOleDbDataReader["UserToEmail3"].ToString();
                    chkToSMS3.Checked = Convert.ToBoolean(myOleDbDataReader["UserToSMS3"]);
                    return (true);
                }
                else
                {
                    return (false);
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return (false);
            }
            finally
            {
                if (myOleDbDataReader != null) { myOleDbDataReader.Close(); }
                if (myOleDbConnection != null) { myOleDbConnection.Close(); }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Validation Code
            // Set flag to false
            bool flgValidationError = false; 

            // If built in validation finds an error
            //if (IsValid != true) { flgValidationError = true; }

            // ** Email validation **
            lblErrorAccEmail.Visible = false;
            // Check for blank email
            if (txtAccEmail.Text.Trim() == "")
            {
                lblErrorAccEmail.Visible = true;
                flgValidationError = true;
                ValidationError.Display("Account Email address is blank");
            }
            else
            {
                // Check for valid email format
                if (GlobalClass.isValidEmail(txtAccEmail.Text) == false)
                {
                    lblErrorAccEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Account Email address " + txtAccEmail.Text.Trim() + " is formatted incorrectly");
                }
            }

            // ** New Password validation **
            lblErrorNewPassword.Visible = false;
            // Check for blank password
            if (txtPassword.Text.Trim() != "")
            {
                // Check for password lenght
                if (txtPassword.Text.Length < 6)
                {
                    lblErrorNewPassword.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Password needs to be longer");
                }
            }
            
            // ** Send To Email validation **
            lblErrorToEmail.Visible = false;
            
            // Check for blank email 1
            if (txtToEmail1.Text.Trim() != "")
            {
                // Check for valid email format
                if (GlobalClass.isValidEmail(txtToEmail1.Text) == false)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address " + txtToEmail1.Text.Trim() + " is formatted incorrectly");
                }
            }
            else 
            { 
                // Check for name with no email
                if (txtToName1.Text.Trim() != "")
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address is required for " + txtToName1.Text);
                }
                else if (chkToSMS1.Checked==true)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("First Send List To email address is required for checked SMS (Text)");
                }
            }
            // Check for blank email 2
            if (txtToEmail2.Text.Trim() != "")
            {
                // Check for valid email format
                if (GlobalClass.isValidEmail(txtToEmail2.Text) == false)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address " + txtToEmail2.Text.Trim() + " is formatted incorrectly");
                }
            }
            else
            {
                // Check for name with no email
                if (txtToName2.Text.Trim() != "")
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address is required for " + txtToName2.Text);
                }
                else if (chkToSMS2.Checked == true)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Second Send List To email address is required for checked SMS (Text)");
                }
            }
            // Check for blank email 3
            if (txtToEmail3.Text.Trim() != "")
            {
                // Check for valid email format
                if (GlobalClass.isValidEmail(txtToEmail3.Text) == false)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address " + txtToEmail1.Text.Trim() + " is formatted incorrectly");
                }
            }
            else
            {
                // Check for name with no email
                if (txtToName3.Text.Trim() != "")
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address is required for " + txtToName3.Text);
                }
                else if (chkToSMS3.Checked == true)
                {
                    lblErrorToEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Third Send List To email address is required for checked SMS (Text)");
                }
            }

            // if error found by this point return control to the user
            if (flgValidationError == true)
            {
                return;
            }

            // Check for unique email
            if (hfOldEmail.Value != txtAccEmail.Text.Trim())
            {
                if (GlobalClass.isUniqueEmail(txtAccEmail.Text) == false)
                {
                    lblErrorAccEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Another list already exisit for the email address " + txtAccEmail.Text.Trim());
                    return;
                }
            }

            // Update Preferences
            if (UpdatePreferences() == false)
            {
                flgValidationError = true;
                ValidationError.Display("Error updating preferences");
            }
            else
            {
                // load new display name
                if (string.IsNullOrEmpty(txtAccName.Text) == true)
                {
                    Session["DisplayName"] = txtAccEmail.Text;  
                }
                else
                {
                    Session["DisplayName"] = txtAccName.Text;  
                }

                // go back to previous page
                if (Request.QueryString["page"] == "List")
                {
                    Response.Redirect("List.aspx");
                }
                else
                {
                    Response.Redirect("Shopping.aspx");
                }
            }
        }

        public bool UpdatePreferences()
        {

            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            sql = "UPDATE G2CUser SET UserName=@UserName, UserEmail=@AccEmail,";
            if (txtPassword.Text.Trim() != "") {
                sql += "UserPassword=@Password,";
            }
            sql += "UserToName1=@ToName1,UserToEmail1=@ToEmail1,UserToSMS1=@ToSMS1,UserToName2=@ToName2,UserToEmail2=@ToEmail2,UserToSMS2=@ToSMS2,UserToName3=@ToName3,UserToEmail3=@ToEmail3,UserToSMS3=@ToSMS3,UserModifyDate=now WHERE UserId=@UserId";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.Parameters.Add(new OleDbParameter("@UserName", txtAccName.Text));
            cmd.Parameters.Add(new OleDbParameter("@AccEmail", txtAccEmail.Text));
            if (txtPassword.Text.Trim() != "") {
                cmd.Parameters.Add(new OleDbParameter("@Password", GlobalClass.encodePassword(txtPassword.Text)));
            }
            cmd.Parameters.Add(new OleDbParameter("@ToName1", txtToName1.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToEmail1", txtToEmail1.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToSMS1", chkToSMS1.Checked));
            cmd.Parameters.Add(new OleDbParameter("@ToName2", txtToName2.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToEmail2", txtToEmail2.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToSMS2", chkToSMS2.Checked));
            cmd.Parameters.Add(new OleDbParameter("@ToName3", txtToName3.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToEmail3", txtToEmail3.Text));
            cmd.Parameters.Add(new OleDbParameter("@ToSMS3", chkToSMS3.Checked));
            cmd.Parameters.Add(new OleDbParameter("@UserId", Session["UserId"]));

            OleDbDataReader reader = null;
            string errorMsg = "";
            try
            {
                conn.Open();
                reader = cmd.ExecuteReader();
                return (true);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return (false);
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (conn != null) { conn.Close(); }
            }

        }
    }
}
