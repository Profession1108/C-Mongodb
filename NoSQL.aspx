<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CMongoDB.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        #Submit1 {
            width: 63px;
            height: 24px;
            margin-top: 0px;
        }
        #Reset1 {
            width: 58px;
            height: 23px;
        }
        #Reset2 {
            width: 58px;
            height: 23px;
        }
    </style>
</head>
<body style="height: 83px">
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label3" runat="server" style="font-size: x-large; color: #0000FF" Text="Model:"></asp:Label>
&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="model" runat="server" Height="25px" style="font-size: medium" AutoPostBack="True" >
            <asp:ListItem>Insert</asp:ListItem>
            <asp:ListItem>Delete</asp:ListItem>
            <asp:ListItem>Update</asp:ListItem>
            <asp:ListItem>Search</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownList1" runat="server" Height="20px" Width="71px" AutoPostBack="True">
        </asp:DropDownList>
        <br />
    
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Height="22px" Text="Name:" style="font-size: large" ></asp:Label>
&nbsp;<asp:TextBox ID="name" runat="server" style="margin-bottom: 0px" Width="130px" Height="20px"></asp:TextBox>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" Height="20px" Width="71px">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label2" runat="server" ForeColor="Red" Height="22px" Text="Age:   " style="font-size: large"></asp:Label>
&nbsp;&nbsp;&nbsp;
        <asp:TextBox ID="age" runat="server" Width="130px" Height="20px"></asp:TextBox>
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button1" runat="server" Height="23px" OnClick="Button1_Click" Text="Submit" Width="58px" />
&nbsp;
        <input id="Reset1" type="reset" value="Reset" /><br />
        &nbsp;</div>
    </form>
</body>
</html>
