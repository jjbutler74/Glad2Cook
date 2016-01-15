<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preferences.aspx.cs" Inherits="Glad2Cook.Preferences" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Glad2Cook - Preferences</title>
    <link rel="stylesheet" type="text/css" href="g2c.css" />
    <link rel="shortcut icon" href="favicon.ico" >
    
    <script type="text/javascript" src="Resources/jquery-1.3.2.js"></script>
    <script type="text/javascript">
    $(document).ready(function() {

        $("#showHelp").click(function() {
            $("#boxHelp").slideDown("slow", function() { });
            $("#showHelp").hide();
            $("#hideHelp").show();
        });

        $('#hideHelp').click(function() {
            $("#boxHelp").slideUp("slow", function() { });
            $("#showHelp").show();
            $("#hideHelp").hide();
        });
    });
</script>

<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-17155188-1']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>
    
</head>
<body>
 
  <form id="form1" runat="server">
 
    <div class="forceCenter">
        <img src="Images/g2c.gif" alt="Glad2Cook" />
    </div>

    <div> <%-- added to balance spacing with shopping page --%>
        <div class="displayName">
            <asp:Label ID="lblDisplayName" runat="server" Text="Label"></asp:Label>
                | 
                <a href = "Preferences.aspx">Preferences</a>
                | 
                <a href="#" onclick="return false" id="hideHelp" style="display:none">Help off</a>
                <a href="#" onclick="return false" id="showHelp">Help on</a>  
                | 
            <asp:LinkButton ID="lnkbtnSignOut" runat="server" onclick="lnkbtnSignOut_Click">Sign out</asp:LinkButton>
        </div>

        <ul class = "tabs primary">
            <li><a href = "Shopping.aspx">Go Shopping</a></li>
            <li><a href = "List.aspx" class = "active">Make List</a></li>
        </ul>
    </div>

<div align="center"> 
    
            
        <% if (Request.QueryString["page"] == "PreferencesReset")
            { %>
                <div class="displayReset" align="center">
                    <p><b>It's suggested that you change your password to one easier to remember</b></p>
                </div>
        <%} %>
            
            <div class="displayHelp" id="boxHelp" align="left" style="display:none">
                <p><b>Account Name</b> is optional. The <b>Account Email Address</b> is used to sign in to your list.</p>
                <p>The <b>New Password</b> field is used to change your sign in password. Leave this field blank if you don’t want to change your password.</p>
                <p>To send grocery list to addresses other than your Account Email Address enter them in the <b>Optional “Send List To”</b> section. <b>Send List To Name</b> is optional.</p>
                <p><b>Send List To Email Address</b> is the email address you want the list sent to. You can also <u>text list to cell phone numbers</u> by entering the cell phone email address here. Cell phone email addresses are determined by the carrier, some examples:</p>
                <p>
                    <b>AT&amp;T:</b> 1235551234@txt.att.net<br />
                    <b>Sprint:</b> 1235551234@messaging.sprintpcs.com<br />
                    <b>T-Mobile:</b> 1235551234@tmomail.net<br />
                    <b>Verizon:</b> 1235551234@vtext.com<br />
                    <a href="http://en.wikipedia.org/wiki/List_of_carriers_providing_SMS_transit">Click here for a more extensive list</a> 
                </p>
                <p><b>SMS (Text)</b> should be checked when the email address is to a cell phone, so the sent list can be <b>formatted properly for texting.</b></p>
            </div>
            
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>  
            
        <table align="center">
            <tr>
                <td colspan="3" align="left">
                    <div class="subheading">Preferences</div>
                    <br />
                    Account Information
                    <div class="line"></div>
                </td>
            </tr>
            <tr>
                <td class="smallFieldName" align="left">
                    Name</td>
                <td class="smallFieldName" align="left">
                    Email Address (used to sign in)<asp:Label ID="lblErrorAccEmail" runat="server" ForeColor="#E11101" Text=" *" Visible="False"></asp:Label>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtAccName" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtAccEmail" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:HiddenField ID="hfOldEmail" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="left">
                    <br />  
                    Change Password
                    <div class="line"></div>  
                </td>
            </tr>
            <tr>
                <td class="smallFieldName" align="left">
                    New Password<asp:Label ID="lblErrorNewPassword" runat="server" ForeColor="#E11101" Text=" *" Visible="False"></asp:Label>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:TextBox ID="txtPassword" runat="server" Width="218px" MaxLength="20" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="left">
                    <br />  
                    Optional "Send List To" addresses
                    <div class="line"></div>  
                </td>
            </tr>
            <tr>
                <td class="smallFieldName" align="left">
                    Name</td>
                <td class="smallFieldName" align="left">
                    Email Address<asp:Label ID="lblErrorToEmail" runat="server" ForeColor="#E11101" Text=" *" Visible="False"></asp:Label>
                </td>
                <td class="smallFieldName" align="center">
                SMS (Text)
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtToName1" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtToEmail1" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:CheckBox ID="chkToSMS1" runat="server" />
                </td>
            </tr>
                        <tr>
                <td>
                    <asp:TextBox ID="txtToName2" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtToEmail2" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:CheckBox ID="chkToSMS2" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtToName3" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtToEmail3" runat="server" Width="218px" MaxLength="60"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:CheckBox ID="chkToSMS3" runat="server" />
                </td>
            </tr>
            
        </table>

        <br />
    
        <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" 
            onclick="btnSave_Click" /> 
        &nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="100px" 
            onclick="btnCancel_Click" />
    
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorDisplay2" ForeColor="#E11101"/>
       
           </ContentTemplate>
    </asp:UpdatePanel>
    
    <div id="footer">Make · Share · Savor</div>  
    <div id="footer2">Have a question or suggestion, send an email to Glad2Cook creator <a href='mailto:glad2cook@gmail.com'>Jason Butler</a></div><br />
    </div> <%--center--%> 
  </form>
</body>
</html>
