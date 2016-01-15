<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="List.aspx.cs" Inherits="Glad2Cook.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Glad2Cook - Make List</title>
    <link rel="stylesheet" type="text/css" href="g2c.css" />
    <link rel="shortcut icon" href="favicon.ico" >

<script type="text/javascript" src="Resources/jquery-1.3.2.js"></script>
<script type="text/javascript">

    function numbersonly(e, decimal) {
        var key;
        var keychar;

        if (window.event) {
            key = window.event.keyCode;
        }
        else if (e) {
            key = e.which;
        }
        else {
            return true;
        }
        keychar = String.fromCharCode(key);

        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 13) || (key == 27)) {
            return true;
        }
        else if ((("0123456789").indexOf(keychar) > -1)) {
            return true;
        }
        else if (decimal && keychar == ".") {
            return true;
        }
        else
            return false;
    }
  
    $(document).ready(function() {
      $("#showHelp").click(function() {
          $("#boxHelp").slideDown("slow", function() { });
          $("#showHelp").hide();
          $("#hideHelp").show();
      });

      $('#hideHelp').click(function() {
          $("#boxHelp").slideUp("slow", function() { });
          $("#box1st").slideUp("slow", function() { });
          $("#box2nd").slideUp("slow", function() { });
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
    <a href = "Preferences.aspx?page=List">Preferences</a>
    | 
    <% if (Session["FirstVisit"] == "Yes")
       { %>
    <a href="#" onclick="return false" id="hideHelp">Help off</a>
    <a href="#" onclick="return false" id="showHelp" style="display:none">Help on</a>  
    <% }
       else
       { %>
    <a href="#" onclick="return false" id="hideHelp" style="display:none">Help off</a>
    <a href="#" onclick="return false" id="showHelp">Help on</a>  
    <%} %>
    | 
    <asp:LinkButton ID="lnkbtnSignOut" runat="server" onclick="lnkbtnSignOut_Click">Sign out</asp:LinkButton>
  </div>

<ul class = "tabs primary">
    <li><a href = "Shopping.aspx">Go Shopping</a></li>
    <li class = "active"><a href = "List.aspx" class = "active">Make List</a></li>
</ul>
</div>

   <div class="topSpace" />
   <div align="center">    

    <% if (Session["FirstVisit"] == "Yes")
       { %>
        
        <div align="right">
        <div class="displayHelpOnOff" id="box1st" align="center">
        <div>
  <b class="boxHelpOnOff">
  <b class="boxHelpOnOff1"><b></b></b>
  <b class="boxHelpOnOff2"><b></b></b>
  <b class="boxHelpOnOff3"></b>
  <b class="boxHelpOnOff4"></b>
  <b class="boxHelpOnOff5"></b></b>

  <div class="boxHelpOnOfffg">
  Use the Help off/on link above to show page instructions
  </div>

  <b class="boxHelpOnOff">
  <b class="boxHelpOnOff5"></b>
  <b class="boxHelpOnOff4"></b>
  <b class="boxHelpOnOff3"></b>
  <b class="boxHelpOnOff2"><b></b></b>
  <b class="boxHelpOnOff1"><b></b></b></b>
  </div>
</div>
</div>
        <div class="displayHelp" id="boxHelp" align="left">
            <p>Create your grocery list here, any item you give a <b>quantity of one or more</b> will appear on your final list at the Go Shopping page.</p>
            <p>To quickly increase the quantity of an item by one just click the <b>up arrow</b> next to the item.</p>
            <p>To <u>edit</u> an item click the <b>Edit button</b>, make your changes, and then click the <b>Save button.</b></p>
            <p>To <u>add a new item</u> go to the last row on the page, make your changes, and then click the <b>Add button.</b></p> 
            <p>To permanently <u>delete</u> an item, just click the <b>Delete button</b>. Don’t delete items you might want on the list another time, just make the quantity zero.</p>
        </div>
    <%}
       else
       { %>
        <div class="displayHelp" id="boxHelp" align="left" style="display:none">
            <p>Create your grocery list here, any item you give a <b>quantity of one or more</b> will appear on your final list at the Go Shopping page.</p>
            <p>To quickly increase the quantity of an item by one just click the <b>up arrow</b> next to the item.</p>
            <p>To <u>edit</u> an item click the <b>Edit button</b>, make your changes, and then click the <b>Save button.</b></p>
            <p>To <u>add a new item</u> go to the last row on the page, make your changes, and then click the <b>Add button.</b></p> 
            <p>To permanently <u>delete</u> an item, just click the <b>Delete button</b>. Don’t delete items you might want on the list another time, just make the quantity zero.</p>
        </div>
    <%} %>  

    <asp:Literal ID="litFavorites" runat="server"></asp:Literal>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/G2C.mdb"
            DeleteCommand="DELETE FROM [Item] WHERE [ItemUserId] = ? AND [ItemId] = ?" 
            UpdateCommand="UPDATE Item SET ItemQuantity = ?, ItemName = ?, ItemCategoryId = ?, ItemAisle = ?, ItemPrice = ?, ItemNotes = ?, ItemTimesUpdated=ItemTimesUpdated+1, ItemModifyDate=now WHERE (ItemUserId = ? AND ItemId = ?)" 
            SelectCommand="SELECT Item.ItemName, ItemCategoryId, Category.CategoryName, Item.ItemAisle, Item.ItemPrice, Item.ItemQuantity, [ItemPrice]*[ItemQuantity] AS Total, Item.ItemNotes, Item.ItemId FROM Category INNER JOIN Item ON Category.CategoryId = Item.ItemCategoryId WHERE ItemUserId=? AND ((ItemName LIKE '%' + ? + '%' AND ItemQuantity >= ? AND ItemQuantity <= ? AND ItemTimesUpdated >= ?) OR ItemName='~') ORDER BY Category.CategoryName, Item.ItemName" 
            InsertCommand="INSERT INTO Item(ItemUserId, ItemCategoryId, ItemName, ItemAisle, ItemPrice, ItemQuantity, ItemNotes, ItemTimesUpdated, ItemCreateDate, ItemModifyDate) VALUES (?,?,?,?,?,?,?,1,now,now)" 
            onselecting="AccessDataSource1_Selecting">

            <SelectParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="" />
                <asp:ControlParameter Name="ItemName" Type="String" ControlID="txtSearch" PropertyName="Text" DefaultValue="%%"/>
                <asp:Parameter Name="QtyMin" Type="Int16" DefaultValue="0" />
                <asp:Parameter Name="QtyMax" Type="Int16" DefaultValue="9999" />
                <asp:Parameter Name="FavNum" Type="Int16" DefaultValue="0" />
                <asp:ControlParameter Name="ItemNotes" Type="String" ControlID="lstShow" PropertyName="Text" DefaultValue="This feels like such a hack!"/>
            </SelectParameters>
            <DeleteParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="" />
                <asp:Parameter Name="ItemId" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="ItemQuantity" Type="Int16" />
                <asp:Parameter Name="ItemName" Type="String" />
                <asp:Parameter Name="ItemCategoryId" Type="Int16" />
                <asp:Parameter Name="ItemAisle" Type="Int16" />
                <asp:Parameter Name="ItemPrice" Type="Decimal" />
                <asp:Parameter Name="ItemNotes" Type="String" />
                <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="" />
                <asp:Parameter Name="ItemId" Type="Int16" />
            </UpdateParameters>
            <InsertParameters>
                <asp:SessionParameter Name="UserId" SessionField="UserId" DefaultValue="" />
                <asp:Parameter Name="ItemCategoryId" Type="Int16" />
                <asp:Parameter Name="ItemName" Type="String" />
                <asp:Parameter Name="ItemAisle" Type="Int16" />
                <asp:Parameter Name="ItemPrice" Type="Decimal" />
                <asp:Parameter Name="ItemQuantity" Type="Int16" />
                <asp:Parameter Name="ItemNotes" Type="String" />
            </InsertParameters>
    </asp:AccessDataSource>
    
    <asp:AccessDataSource ID="AccessDataSource2" runat="server" 
        DataFile="~/App_Data/G2C.mdb" 
        SelectCommand="SELECT [CategoryId], [CategoryName] FROM [Category]">
    </asp:AccessDataSource>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <asp:Button ID="btnDummy" runat="server" OnClientClick="return false;" class="offScreen"/>
            
            <div id="filterBox">
              <div class="filterBoxLeft">
                <asp:TextBox ID="txtSearch" runat="server" MaxLength="30" Width="200px"></asp:TextBox>
                &nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="Search" />
              </div>
              <div class="filterBoxRight">
                <asp:DropDownList ID="lstShow" runat="server" CssClass="textChar" AutoPostBack="True">
                </asp:DropDownList>
              </div>
            </div>
    
            <asp:GridView ID="MakeList" runat="server" AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False" 
            DataSourceID="AccessDataSource1" 
            DataKeyNames="ItemId" CellPadding="2" 
             OnRowCommand="updateList"
             ForeColor="#333333" GridLines="Vertical" OnRowDataBound="mlRowUpdate" ShowFooter="true">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <Columns>
                
                    <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CausesValidation="False" 
                                CommandName="Edit" Text="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                                          
                    <asp:TemplateField HeaderText="Qty" SortExpression="ItemQuantity">
                        <EditItemTemplate>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" 
                                MinimumValue="0" MaximumValue="99" Text="*" Display="Dynamic" 
                                ErrorMessage="Quantity must be 0 to 99" ControlToValidate="txtQty" EnableClientScript="False"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                Text="*" Display="Dynamic" ErrorMessage="Quantity must be 0 to 99" 
                                ControlToValidate="txtQty" EnableClientScript="False"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtQty" onkeypress="return numbersonly(event, false)" 
                                class="textNumber" Width="20" MaxLength="2" runat="server" 
                                Text='<%# Bind("ItemQuantity") %>'/>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("ItemQuantity") %>'/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:RangeValidator ID="RangeValidator1" ValidationGroup="AddItem" 
                                runat="server" Type="Integer" MinimumValue="0" MaximumValue="99" Text="*" 
                                Display="Dynamic" ErrorMessage="Quantity must be 0 to 99" 
                                ControlToValidate="txtQty" EnableClientScript="False"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                ValidationGroup="AddItem" runat="server" Text="*" Display="Dynamic" 
                                ErrorMessage="Quantity must be 0 to 99" ControlToValidate="txtQty" EnableClientScript="False"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtQty" onkeypress="return numbersonly(event, false)" 
                                class="textNumber" Width="20" MaxLength="2" Text="1" runat="server" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField > 
                               
                    <asp:ButtonField Text="<img src='Images/up-icon.png' alt='Add One' border='0' />" ButtonType="Link" CommandName="addOne" />                               
                                            
                    <asp:TemplateField HeaderText="Item" SortExpression="ItemName">
                        <EditItemTemplate>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                Text="*" Display="Dynamic" ErrorMessage="Item Name is Required" 
                                ControlToValidate="txtItem" EnableClientScript="False"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtItem" class="textChar" Width="120" MaxLength="30" 
                                runat="server" Text='<%# Bind("ItemName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblItem" runat="server" Text='<%# Bind("ItemName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ValidationGroup="AddItem" Text="*" Display="Dynamic" 
                                ErrorMessage="Item Name is Required" ControlToValidate="txtItem" EnableClientScript="False"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtItem" class="textChar" Width="120" MaxLength="30" 
                                runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                                     
                    <asp:TemplateField HeaderText="Category" SortExpression="CategoryName">
                        <EditItemTemplate>
                            <asp:DropDownList ID="lstCategory" runat="server" 
                                DataSourceID="AccessDataSource2" DataTextField="CategoryName" 
                                DataValueField="CategoryId"
                                SelectedValue='<%# Bind("ItemCategoryId") %>'
                                Class="textChar">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("CategoryName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                           <asp:DropDownList ID="lstCategory" class="textChar" runat="server" 
                                DataSourceID="AccessDataSource2" DataTextField="CategoryName" 
                                DataValueField="CategoryId" 
                                SelectedValue='<%# "7" %>'
                                >
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>                                       
                                                         
                    <asp:TemplateField HeaderText="Aisle" SortExpression="ItemAisle">
                        <EditItemTemplate>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" Type="Integer" 
                                MinimumValue="0" MaximumValue="99" Text="*" Display="Dynamic" 
                                ErrorMessage="Aisle Number must be 0 to 99, or blank" 
                                ControlToValidate="txtAisle" EnableClientScript="False"></asp:RangeValidator>
                            <asp:TextBox ID="txtAisle" onkeypress="return numbersonly(event, false)" 
                                class="textNumber" Width="20" MaxLength="2" runat="server" 
                                Text='<%# Bind("ItemAisle") %>'/>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblAisle" runat="server" Text='<%# Bind("ItemAisle") %>'/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                ValidationGroup="AddItem" Type="Integer" MinimumValue="0" MaximumValue="99" 
                                Text="*" Display="Dynamic" 
                                ErrorMessage="Aisle Number must be 0 to 99, or blank" 
                                ControlToValidate="txtAisle" EnableClientScript="False"></asp:RangeValidator>
                            <asp:TextBox ID="txtAisle" onkeypress="return numbersonly(event, false)" 
                                class="textNumber" Width="20" MaxLength="2" runat="server" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField > 
                    
                    <asp:TemplateField HeaderText="Price" SortExpression="ItemPrice">
                        <EditItemTemplate>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" Type="Currency" 
                                MinimumValue="0" MaximumValue="999.99" Text="*" Display="Dynamic" 
                                ErrorMessage="Price must be 0 to 999.99, or blank" 
                                ControlToValidate="txtPrice" EnableClientScript="False"></asp:RangeValidator>
                            <asp:TextBox ID="txtPrice" onkeypress="return numbersonly(event, true)" 
                                class="textNumber" Width="50" MaxLength="6" runat="server" 
                                Text='<%# Bind("ItemPrice","{0:0.00}") %>'/>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server" 
                                Text='<%# Bind("ItemPrice","{0:c}") %>'/>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterTemplate>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" 
                                ValidationGroup="AddItem" Type="Currency" MinimumValue="0" 
                                MaximumValue="999.99" Text="*" Display="Dynamic" 
                                ErrorMessage="Price must be 0 to 999.99, or blank" 
                                ControlToValidate="txtPrice" EnableClientScript="False"></asp:RangeValidator>
                            <asp:TextBox ID="txtPrice" onkeypress="return numbersonly(event, true)" 
                                class="textNumber" Width="50" MaxLength="6" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField > 
                    
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" 
                        DataFormatString="{0:c} " HtmlEncode="False" ReadOnly="True" >
                    <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Notes" SortExpression="ItemNotes">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtNotes" class="textChar" Width="120" MaxLength="30" 
                                runat="server" Text='<%# Bind("ItemNotes") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNotes" runat="server" Text='<%# Bind("ItemNotes") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterTemplate>
                            <asp:TextBox ID="txtNotes" class="textChar" Width="120" MaxLength="30" 
                                runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:Button ID="btnSaveExisting" runat="server" CausesValidation="True" 
                                CommandName="Update" Text="Save" Width="56"/> <%--was 48--%>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                                CommandName="Delete" Text="Delete" Width="56" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                        </ItemTemplate>
                        <FooterTemplate>
                          <asp:Button ID="btnAdd" runat="server" ValidationGroup="AddItem" CausesValidation="True" 
                                onclick="btnAdd_Click" Text="Add" Width="56"/>
                        </FooterTemplate>
                    </asp:TemplateField>
                                          
                </Columns>
                <FooterStyle BackColor="#DDDDDD" />
                <PagerStyle BackColor="#F7F6F3" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#182F8C" Font-Underline="False" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
           
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errorDisplay2" ForeColor="#E11101"/>
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errorDisplay2" ValidationGroup="AddItem" ForeColor="#E11101"/>
            
        </ContentTemplate>
    </asp:UpdatePanel>
     
    </div>
      
    <div id="footer">Make · Share · Savor</div>
    <div id="footer2">Have a question or suggestion, send an email to Glad2Cook creator <a href='mailto:glad2cook@gmail.com'>Jason Butler</a></div><br />
    <a name="bottom"></a>       
    </form>
    
</body>
</html>
