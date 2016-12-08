<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="Label1" runat="server" Text="Tunnus:"></asp:Label>
        <br />
        <asp:TextBox ID="usernamelogin" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Salasana:"></asp:Label>
        <br />
        <asp:TextBox ID="passwordlogin" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="LogIn" runat="server" Text="Kirjaudu" OnClick="LogIn_Click" />
        <a href="Register.aspx">Rekisteröidy</a><br />
        <asp:Label ID="lblMessages" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
