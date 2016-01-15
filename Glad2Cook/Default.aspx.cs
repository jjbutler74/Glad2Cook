using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Glad2Cook
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.Browser["IsMobileDevice"] == "true")
            //{
            //    Response.Redirect("MobileShopping.aspx");
            //}
            //else
            //{
            //    Response.Redirect("Home.aspx");
            //}

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
                    if (GlobalClass.getUserInfo(strUserId) == false)
                    {
                        Response.Redirect("Home.aspx");
                    }
                }
                else
                {
                    // If session vars do not have valid values then back to the home page
                    Response.Redirect("Home.aspx");
                }
            }

            GlobalClass.checkFavorites();
            GlobalClass.logLogin("Default");
            Response.Redirect("Shopping.aspx");
            // *** Auto Log In Code ***
        }

    }
}
