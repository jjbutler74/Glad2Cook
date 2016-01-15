<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Glad2Cook.ForgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Glad2Cook - Forgot Password</title>
    <link rel="stylesheet" type="text/css" href="g2c.css" />
    <link rel="shortcut icon" href="favicon.ico" >

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

	<div id="header">
			<img src="Images/g2c.gif" alt="Glad2Cook" />
	</div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
<div class="sidebox">
	<div class="boxhead"><h2>Enter your email address to receive a new password</h2></div>
	<div class="boxbody">
		<p>Email<asp:Label ID="lblErrorEmail" runat="server" ForeColor="#E11101" Text=" *" Visible="False"></asp:Label>
		<br />
		<asp:TextBox ID="txtEmail" runat="server" Width="218px" MaxLength="60"></asp:TextBox></p>
		<div class="forceCenter">
            <asp:Button ID="btnNewPwd" runat="server" 
                Text="Email New Password" onclick="btnNewPwd_Click" /></div>
	</div>
</div>

	<div class="forceCenter">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorDisplay" ForeColor="#E11101"/>
    <a href="Home.aspx">Home</a> | <a href="SignIn.aspx">Sign in</a> 
    </div>

        </ContentTemplate>   
    </asp:UpdatePanel>

    <div id="footer">Make · Share · Savor</div>  
    <div id="footer2">Have a question or suggestion, send an email to Glad2Cook creator <a href='mailto:glad2cook@gmail.com'>Jason Butler</a></div><br />
    </form>
</body>
</html>
