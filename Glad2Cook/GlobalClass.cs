using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text; /* For Encoding */
using System.Text.RegularExpressions; /* Email Validation */
using System.Data.OleDb; /* DB */
using System.Security.Cryptography; /* Password hashing */

namespace Glad2Cook
{
    public class GlobalClass
    {
        public static string readUserIdFromCookie()
        {
            try
            {
                var codedBytes = Convert.FromBase64String(System.Web.HttpContext.Current.Request.Cookies["timeout"].Value);
                var uncodedBytes = codedBytes;
                String cookieUserId = Encoding.ASCII.GetString(uncodedBytes);
                if (string.IsNullOrEmpty(cookieUserId) == true) 
                    { return null; }
                else
                    { return cookieUserId; }
            }
            catch
            {
                return null;
            }
            
        }

        public static void logOut()
        {
            if (System.Web.HttpContext.Current.Request.Cookies["timeout"] != null)
            {
                HttpCookie myCookie = new HttpCookie("timeout");
                myCookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
            }
            System.Web.HttpContext.Current.Session["UserId"] = "";
            System.Web.HttpContext.Current.Session["DisplayName"] = "";
            System.Web.HttpContext.Current.Session["Favorites"] = "";
            System.Web.HttpContext.Current.Response.Redirect("Home.aspx");
        }

        public static void resetCookieExpDate(string inputUserid)
        {
            var plainBytes = Encoding.ASCII.GetBytes(inputUserid);
            var codedBytes = plainBytes;
            System.Web.HttpContext.Current.Response.Cookies["timeout"].Value = Convert.ToBase64String(codedBytes);
            System.Web.HttpContext.Current.Response.Cookies["timeout"].Expires = DateTime.Now.AddDays(30);
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        public static bool isUniqueEmail(string inputEmail)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\G2C.mdb");
            //OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT G2CUser.UserName FROM G2CUser";
            myOleDbCommand.CommandText += " WHERE G2CUser.UserEmail='" + inputEmail + "'";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            myOleDbDataReader.Read();

            if (myOleDbDataReader.HasRows == true) // Account with email address already exisit
            {
                myOleDbDataReader.Close();
                myOleDbConnection.Close();
                return (false); // false, this email is not unique
            }
            else
            {
                myOleDbDataReader.Close();
                myOleDbConnection.Close();
                return (true); // true, this email is unique
            }
        }

        public static string encodePassword(string originalPassword)
        //http://bloggingabout.net/blogs/rick/archive/2005/05/18/4118.aspx
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

        public static bool getUserInfo(string inputUserId)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\G2C.mdb");
            //OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();
            OleDbDataReader myOleDbDataReader = null;

            myOleDbCommand.CommandText = "SELECT UserName, UserEmail, UserFavorites FROM G2CUser WHERE UserId = '" + inputUserId + "'";

            string errorMsg = "";
            try
            {
                myOleDbConnection.Open();
                myOleDbDataReader = myOleDbCommand.ExecuteReader();
                myOleDbDataReader.Read();

                if (myOleDbDataReader.HasRows == true)
                {
                    System.Web.HttpContext.Current.Session["UserId"] = inputUserId;
                    if ((myOleDbDataReader.GetValue(0).ToString() == null) || (myOleDbDataReader.GetValue(0).ToString() == ""))
                    {
                        System.Web.HttpContext.Current.Session["DisplayName"] = myOleDbDataReader.GetValue(1).ToString();
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["DisplayName"] = myOleDbDataReader.GetValue(0).ToString();
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

                    // refresh cookie experiation date
                    GlobalClass.resetCookieExpDate(inputUserId);
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

        public static void logLogin(string inputPage)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\G2C.mdb");
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql;
            string strHostName = System.Net.Dns.GetHostName();

            sql = "INSERT INTO UserLog (UserLogUserId,UserLogDate,UserLogHost,UserLogAddress,UserLogBrowser,UserLogPage)";
            sql += sql = " VALUES ('" + System.Web.HttpContext.Current.Session["UserId"] + "'";
            sql += sql = ", now";
            sql += sql = ", '" + strHostName + "'";
            sql += sql = ", '" + System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString() + "'";
            sql += sql = ", '" + System.Web.HttpContext.Current.Request.Browser.Type + "'";
            sql += sql = ", '" + inputPage + "')";

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
                
        public static void checkFavorites()
        {
            // 0 - Off, 1 - New, 2 - First Use, 3 - On
            string svFavorites = (string)(System.Web.HttpContext.Current.Session["Favorites"]);

            if (svFavorites == "1")
            {
                Int32 iFav =  Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["NumberOfUpdatesForFavorites"]);
                if (isFavoriteNumberOfUpdatesReached(iFav))
                {
                    System.Web.HttpContext.Current.Session["Favorites"] = "2";
                }
            }
        }

        public static bool isFavoriteNumberOfUpdatesReached(Int32 inputNumberOfUpdates)
        {
            string connectionString = "Provider=Microsoft.JET.OLEDB.4.0;data source=" + System.Web.HttpContext.Current.Server.MapPath("~\\App_Data\\G2C.mdb");
            //OleDbConnection conn = new OleDbConnection(connectionString);
            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT Item.ItemId FROM Item";
            myOleDbCommand.CommandText += " WHERE Item.ItemUserId='" + System.Web.HttpContext.Current.Session["UserId"] + "'";
            myOleDbCommand.CommandText += " AND   Item.ItemTimesUpdated >= " + inputNumberOfUpdates;
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            myOleDbDataReader.Read();

            if (myOleDbDataReader.HasRows == true) // Account with email address already exisit
            {
                myOleDbDataReader.Close();
                myOleDbConnection.Close();
                return (true); // at least one item has been update enough times to activate favorites
            }
            else
            {
                myOleDbDataReader.Close();
                myOleDbConnection.Close();
                return (false); // favorites not yet activated
            }
        }


        // more global functions here
        

    }
}
