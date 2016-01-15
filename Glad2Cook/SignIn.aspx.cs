using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text; /* For encoding */
using System.Data.OleDb; /* DB */

namespace Glad2Cook
{
    public partial class SignIn : System.Web.UI.Page
    {
        bool flgValidationError = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            // By coming to Login page, user should (will) be logged out
            if (System.Web.HttpContext.Current.Request.Cookies["timeout"] != null)
            {
                HttpCookie myCookie = new HttpCookie("timeout");
                myCookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            System.Web.HttpContext.Current.Session["UserId"] = "";
            System.Web.HttpContext.Current.Session["DisplayName"] = "";
            System.Web.HttpContext.Current.Session["Favorites"] = "";
        }

        protected void btnSignIn_Click(object sender, EventArgs e)
        {
            // Validation Code
            // Set flag to false
            flgValidationError = false;

            // If built in validation finds an error
            //if (IsValid != true) { flgValidationError = true; }

              // ** Email validation **
            lblErrorEmail.Visible = false;
            // Check for blank email
            if (txtEmail.Text.Trim() == "")
            {
                lblErrorEmail.Visible = true;
                flgValidationError = true;
                ValidationError.Display("Email address is blank");
            }
            else
            {
                // Check for valid email format
                if (GlobalClass.isValidEmail(txtEmail.Text) == false)
                {
                    lblErrorEmail.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Email address is formatted incorrectly");
                }
            }

            // ** Password validation **
            lblErrorPassword.Visible = false;
            // Check for blank password
            if (txtPassword.Text.Trim() == "")
            {
                lblErrorPassword.Visible = true;
                flgValidationError = true;
                ValidationError.Display("Password is blank");
            }
     
            if (flgValidationError == true)
            {
                return;
            }

            // Try to sign in user
            if (SignInUser(txtEmail.Text, GlobalClass.encodePassword(txtPassword.Text)) == false)
            {
                flgValidationError = true;
                ValidationError.Display("Email address or Password is incorrect");
            }
            else
            {
                GlobalClass.checkFavorites();
                GlobalClass.logLogin("SignIn");

                if (Request.QueryString["page"] == "List")
                {
                    Response.Redirect("List.aspx");
                }
                else if (Request.QueryString["page"] == "Preferences") 
                {
                    Response.Redirect("Preferences.aspx");
                }
                else if (Request.QueryString["page"] == "PreferencesReset")
                {
                    Response.Redirect("Preferences.aspx?page=PreferencesReset");
                }
                else
                {
                    Response.Redirect("Shopping.aspx");
                }
            }
               
         }

        public bool SignInUser(string inputUserEmail, string inputEncodedPassword)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            //OleDbConnection conn = new OleDbConnection(connectionString); 
            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();
            OleDbDataReader myOleDbDataReader = null;

            myOleDbCommand.CommandText = "SELECT UserId, UserName, UserEmail FROM G2CUser WHERE UserEmail = '" + inputUserEmail + "' AND UserPassword = '" + inputEncodedPassword + "'";

            string errorMsg = "";
            try
            {
                myOleDbConnection.Open();
                myOleDbDataReader = myOleDbCommand.ExecuteReader();
                myOleDbDataReader.Read();

                if (myOleDbDataReader.HasRows == true)
                {
                    Session["UserId"] = myOleDbDataReader.GetValue(0).ToString(); ;
                    if ((myOleDbDataReader.GetValue(1).ToString() == null) || (myOleDbDataReader.GetValue(1).ToString() == ""))
                    {
                        Session["DisplayName"] = myOleDbDataReader.GetValue(2).ToString();
                    }
                    else
                    {
                        Session["DisplayName"] = myOleDbDataReader.GetValue(1).ToString();
                    }

                    // Favorites
                    if ((myOleDbDataReader.GetValue(2).ToString() == null))
                    {
                        System.Web.HttpContext.Current.Session["Favorites"] = "0";
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["Favorites"] = myOleDbDataReader.GetValue(2).ToString();
                    }
                    
                    // write coded user id cookie
                    //http://stackoverflow.com/questions/1093181/how-can-i-encrypt-a-cookie-content-in-a-simple-way-in-c-3-0
                    var plainBytes = Encoding.ASCII.GetBytes(myOleDbDataReader.GetValue(0).ToString());
                    var codedBytes = plainBytes;
                    Response.Cookies["timeout"].Value = Convert.ToBase64String(codedBytes);
                    Response.Cookies["timeout"].Expires = DateTime.Now.AddDays(30);
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
     }


















}

