using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text; /* For encoding */
using System.Data.OleDb; /* DB */
using System.Net.Mail; /* Send email */

namespace Glad2Cook
{
    public partial class ForgotPassword : System.Web.UI.Page
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

        protected void btnNewPwd_Click(object sender, EventArgs e)
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
            if (flgValidationError == true)
            {
                return;
            }

            if (GlobalClass.isUniqueEmail(txtEmail.Text) == false)
            {
                Random rnd = new Random();
                string strNewPassword = RandomString(rnd, 8);
                if (UpdatePassword(strNewPassword) == true)
                {
                    EmailPassword(strNewPassword);
                }
            }
            Response.Redirect("SignIn.aspx?page=PreferencesReset");
        }

        public string RandomString(Random r, int len)
        {
            string str
            = "abcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder sb = new StringBuilder();

            while ((len--) > 0)
                sb.Append(str[(int)(r.NextDouble() * str.Length)]);

            return sb.ToString();
        }

        public bool UpdatePassword(string psNewPassword)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            sql = "UPDATE G2CUser SET UserPassword=@Password,UserModifyDate=now";
            sql += " WHERE UserEmail=@AccEmail";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            cmd.Parameters.Add(new OleDbParameter("@Password", GlobalClass.encodePassword(psNewPassword)));
            cmd.Parameters.Add(new OleDbParameter("@AccEmail", txtEmail.Text));

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

        public void EmailPassword(string psNewPassword)
        {
            MailMessage m = new MailMessage();
            m.From = new MailAddress("message@glad2cook.com");
            m.To.Add(txtEmail.Text);
            m.Subject = "Password Reset for Glad2Cook.com";

            m.IsBodyHtml = true;
            string msgHTML = null;
            msgHTML = "<font size='2' face='arial'>Your password to access Glad2Cook.com has been reset.<br /><br />";
            msgHTML += "<a href='http://jasonbutler.com/Glad2Cook/SignIn.aspx?page=PreferencesReset'>Click here</a> to Sign in to Glad2Cook with your new password of:";
            msgHTML += " <b>" + psNewPassword + " </b><br /><br />";
            msgHTML += "It's suggested that you change your password (through Preferences) to one easier to remember after signing in.</font>";
            m.Body = msgHTML;

            SmtpClient sc = new SmtpClient();
            sc.Host = System.Configuration.ConfigurationSettings.AppSettings["SMTP-Server"];
            sc.Send(m);
        }

     }


















}

