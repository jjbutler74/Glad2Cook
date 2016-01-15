<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Shopping.aspx.cs" Inherits="Glad2Cook.View" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Glad2Cook - Go Shopping</title>
    <link rel="stylesheet" type="text/css" href="g2c.css" />
    <link rel="shortcut icon" href="favicon.ico" >
    <style type="text/css" media="print">
     .printHideOnPrint {	display: none	}
     .printShowOnPrint {	display: block	}
    </style>
        
    <script type="text/javascript" src="Resources/jquery-1.3.2.js"></script>
    <script type="text/javascript">
    $(document).ready(function() {

        $("#showHelp").click(function() {
            $("#boxHelp").slideDown("slow", function() {});
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
    <div class="printHideOnPrint"> 
        <img src="Images/g2c.gif" alt="Glad2Cook" />
    </div>
    <div class="printShowOnPrint"> 
        <img src="Images/mg2c.gif" alt="Glad2Cook" />
        <br />
        Grocery List for <asp:Label ID="lblDisplayName2" runat="server" Text="you"></asp:Label>
        <br /><br />
    </div>
</div>

<div class="printHideOnPrint"> 
  <div class="displayName">
    <asp:Label ID="lblDisplayName1" runat="server" Text="Label"></asp:Label>
    | 
    <a href = "Preferences.aspx">Preferences</a>
    | 
    <a href="#" onclick="return false" id="hideHelp" style="display:none">Help off</a>
    <a href="#" onclick="return false" id="showHelp">Help on</a>  
    | 
    <asp:LinkButton ID="lnkbtnSignOut" runat="server" onclick="lnkbtnSignOut_Click">Sign out</asp:LinkButton>
  </div>

    <ul class = "tabs primary">
        <li class = "active"><a href = "Shopping.aspx" class = "active">Go Shopping</a></li>
        <li><a href = "List.aspx">Make List</a></li>
    </ul> 

</div>
    
   <div class="topSpace" />
   <div align="center">  

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/G2C.mdb">
    </asp:AccessDataSource>
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
            <div class="printHideOnPrint"> 
            
            <div class="displayHelp" id="boxHelp" align="left" style="display:none">
                <p>This is your shopping list, any item with a <b>quantity of one or more</b> will show up here.</p>
                <p>If you want to decrease the quantity of an item by one just click the <b>down arrow</b> (one bought). To reduce the quantity directly to zero click the <b>checkmark</b> (all bought).</p>
                <p>Click a <b>column heading</b> once to <u>sort</u> the items down, and click the heading again to sort the items up.</p>
                <p>To <u>email or text</u> the grocery list just select the name or address from the <b>‘Send List To…” drop-down box</b>. You can add names here from the Preferences link above.</p>
                <p>To <u>print</u> the list just click the <b>Print List button</b>.</p>
            </div>
            
           <div id="filterBox">
              <div class="filterBoxLeft">
                <asp:DropDownList ID="lstSendTo" runat="server" AutoPostBack="True" CssClass="textChar" onselectedindexchanged="lstSendTo_SelectedIndexChanged">
                </asp:DropDownList>
              </div>
              <div class="filterBoxRight">
                <asp:Button ID="btnClear" runat="server" Text="Clear List" onclick="btnClear_Click" OnClientClick="return confirm('Are you sure you want to change all quantities to 0?');"/>
                 &nbsp;
                <input type='button' value='Print List' onclick='window.print();return false;' />
              </div>
            </div>              
           
        </div> <!-- end print hide -->
        
            <asp:GridView ID="GoShopping" runat="server" 
            AllowSorting="True" AutoGenerateColumns="False" 
            DataSourceID="AccessDataSource1" 
            DataKeyNames="ItemId" CellPadding="2" 
            OnRowCommand="updateGList"
                ForeColor="#333333" GridLines="Vertical" ShowFooter="True" 
                OnRowDataBound="GListLoading">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                    <asp:ButtonField Text="<img src='Images/accept-icon.png' alt='Bought All' border='0' />" ButtonType="Link" CommandName="buyAll" >
                    <ControlStyle CssClass="printHideOnPrint" />
                    <FooterStyle CssClass="printHideOnPrint" />
                    <HeaderStyle CssClass="printHideOnPrint" />
                    <ItemStyle CssClass="printHideOnPrint" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="ItemQuantity" HeaderText="Qty" 
                    SortExpression="ItemQuantity" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:ButtonField Text="<img src='Images/down-icon.png' alt='Bought One' border='0' />" ButtonType="Link" CommandName="buyOne" >
                    <ControlStyle CssClass="printHideOnPrint" />
                    <FooterStyle CssClass="printHideOnPrint" />
                    <HeaderStyle CssClass="printHideOnPrint" />
                    <ItemStyle CssClass="printHideOnPrint" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="ItemName" HeaderText="Item" 
                    SortExpression="ItemName" >
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" 
                    SortExpression="CategoryName" >
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemAisle" HeaderText="Aisle" 
                    SortExpression="ItemAisle" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemPrice" HeaderText="Price" 
                    SortExpression="ItemPrice" DataFormatString="{0:c} " HtmlEncode="False" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" 
                        DataFormatString="{0:c} " HtmlEncode="False" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ItemNotes" HeaderText="Notes" 
                    SortExpression="ItemNotes" >
                    <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#182F8C" Font-Underline="False" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                
                <emptydatatemplate>
                    <table cellpadding="20">
                        <tr> 
                            <td>
                                Grocery List is empty, click on <a href = "List.aspx">Make List</a> to add more items
                            </td>   
                        </tr>
                    </table>
                </emptydatatemplate> 
                
            </asp:GridView>
     
        </ContentTemplate>
    </asp:UpdatePanel>
    
<%-- <div class="printHideOnPrint"> 
        <br />
        <a href = "MobileShopping.aspx">Switch to Mobile Version</a> 
        <br /><br />
    </div> --%>
    
</div>

    <div id="footer">Make · Share · Savor</div>
    <div class="printHideOnPrint"> 
    <div id="footer2">Have a question or suggestion, send an email to Glad2Cook creator <a href='mailto:glad2cook@gmail.com'>Jason Butler</a></div><br />
    </div>
    </form>
 </body>
</html>
