using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text; /* For Encoding */
using System.Data.OleDb; /* DB */
using System.Net.Mail; /* Send email */

namespace Glad2Cook
{
    public partial class Home : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            // By coming to Home page, user should (will) be logged out
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

        protected void btnCreateList_Click(object sender, EventArgs e)
        {
            // Validation Code
            // Set flag to false
            bool flgValidationError = false; 

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
            else
            {
                // Check for password lenght
                if (txtPassword.Text.Length < 6)
                {
                    lblErrorPassword.Visible = true;
                    flgValidationError = true;
                    ValidationError.Display("Password needs to be longer");
                }
            }

            if (flgValidationError == true)
            {
                return;
            }

            // Check for unique email
            if (GlobalClass.isUniqueEmail(txtEmail.Text) == false)
            {
                lblErrorEmail.Visible = true;
                flgValidationError = true;
                ValidationError.Display("A list already exisit for this email address, use the Sign in link below to see it");
            }
            else
            {
                // Create new user account
                string strUserId = System.Guid.NewGuid().ToString();
                if (CreateAccount(strUserId, txtEmail.Text, GlobalClass.encodePassword(txtPassword.Text)) == false)
                {
                    flgValidationError = true;
                    ValidationError.Display("Error creating account");
                }
                else
                {
                    Session["UserId"] = strUserId;
                    Session["DisplayName"] = txtEmail.Text;
                    Session["FirstVisit"] = "Yes";
                    Session["Favorites"] = "1"; // new

                    // write coded user id cookie
                    //http://stackoverflow.com/questions/1093181/how-can-i-encrypt-a-cookie-content-in-a-simple-way-in-c-3-0
                    var plainBytes = Encoding.ASCII.GetBytes(strUserId);
                    var codedBytes = plainBytes;
                    Response.Cookies["timeout"].Value = Convert.ToBase64String(codedBytes);
                    Response.Cookies["timeout"].Expires = DateTime.Now.AddDays(30);

                    
                    CreateSampleItems(strUserId, 3, "Milk", 8, 3.49, 2, "Whole");
                    CreateSampleItems(strUserId, 1, "White Bread", 1, 1.25, 1, "Check for fresh");
                    CreateSampleItems(strUserId, 5, "Ice Cream", 4, 4, 0, "Gallon of Vanilla");
                    CreateSampleItems(strUserId, 3, "Ceddar Cheese", 7, 2, 1, "Small bag shredded");
                    CreateSampleItems(strUserId, 3, "Eggs", 8, 2.25, 1, "Large, Grade A");
                    CreateSampleItems(strUserId, 8, "Paper Towels", 0, 0, 1, "" );
                    CreateSampleItems(strUserId, 2, "Ground Chuck", 1, 6.50, 1, "2 pounds");
                    CreateSampleItems(strUserId, 6, "Tomatoes", 10, 0, 3, "Get extra if they look good");
                    CreateSampleItems(strUserId, 1, "~", 0, 0, 0, "");

                    EmailWelcome(txtEmail.Text);
                    GlobalClass.logLogin("Home - New Account");
                    Response.Redirect("List.aspx");
                }
            }
        }

        public bool CreateAccount(string inputUserId, string inputEmail, string inputPassword)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            sql = "INSERT INTO G2CUser (UserId,UserEmail,UserPassword,UserFavorites,UserCreateDate,UserModifyDate)";
            sql += sql = " VALUES ('" + inputUserId + "'";
            sql += sql = ", '" + inputEmail + "'";
            sql += sql = ", '" + inputPassword + "'";
            sql += sql = ", 1";
            sql += sql = ", now";
            sql += sql = ", now)";

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

            if (errorMsg == "")
            {
                return (true);
            }
            else
            {
                return (false);
            }

        }

        public void CreateSampleItems(string inputUserId, int inputCategoryId, string inputName, int inputAisle, double inputPrice, int inputQuantity, string inputNotes)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            sql = "INSERT INTO Item (ItemUserId,ItemCategoryId,ItemName,ItemAisle,ItemPrice,ItemQuantity,ItemNotes,ItemTimesUpdated,ItemCreateDate,ItemModifyDate)";
            sql += sql = " VALUES ('" + inputUserId + "'";
            sql += sql = ", " + inputCategoryId;
            sql += sql = ", '" + inputName + "'";
            if (inputAisle == 0) {sql += sql = ", null";}
                else {sql += sql = ", " + inputAisle;}
            if (inputPrice == 0) { sql += sql = ", null"; }
                else { sql += sql = ", " + inputPrice; }
            sql += sql = ", " + inputQuantity;
            sql += sql = ", '" + inputNotes + "'";
            sql += sql = ", 0";
            sql += sql = ", now";
            sql += sql = ", now)";

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

        public void EmailWelcome(string inputEmail)
        {
            MailMessage m = new MailMessage();
            m.From = new MailAddress("message@glad2cook.com");
            m.To.Add(txtEmail.Text);
            m.Subject = "Welcome to Glad2Cook.com";

            m.IsBodyHtml = true;
            string msgHTML = null;
            
            msgHTML = "<font size='2' face='arial'>";
            msgHTML += "Thanks for creating an account at <a href='http://glad2cook.com'>Glad2Cook.com</a>.";
            msgHTML += "<br /><br />";
            msgHTML += "I hope you find creating and using grocery list at Glad2Cook both easy and helpful.";
            msgHTML += "<br /><br />";
            msgHTML += "My family and I share an account on Glad2Cook, and we’ve found it very useful for creating shopping list no matter where we are. Be it from our home computers, at work, or even from our phones. It really helps us shop smarter and faster (while eliminating forgotten items).";
            msgHTML += "<br /><br />";
            msgHTML += "Using Glad2Cook myself gives me real insight into ways to improve the experience – and it also forces me to fix found problems. I would love to hear form you about any suggestions you have for improving Glad2Cook, just send me an email.";
            msgHTML += "<br /><br />";
            msgHTML += "Thanks again-";
            msgHTML += "<br /><br />";
            msgHTML += "<a href='mailto:glad2cook@gmail.com'>Jason Butler</a>";
            msgHTML += "<br />";
            msgHTML += "<i>Creator of Glad2Cook.com</i>";
            msgHTML += "<br /><br />";
            msgHTML += "PS: Remember you can use the Help off/on link on most pages to show instructions if needed";
            m.Body = msgHTML;

            SmtpClient sc = new SmtpClient();
            sc.Host = System.Configuration.ConfigurationSettings.AppSettings["SMTP-Server"];
          //  sc.Send(m);
        }
    }

    public class ValidationError : IValidator
    {
        private ValidationError(string message)
        {
            ErrorMessage = message;
            IsValid = false;
        }

        public string ErrorMessage { get; set; }

        public bool IsValid { get; set; }

        public void Validate()
        {
            // no action required
        }

        public static void Display(string message)
        {
            Page currentPage = HttpContext.Current.Handler as Page;
            currentPage.Validators.Add(new ValidationError(message));
        }
    }
















}

