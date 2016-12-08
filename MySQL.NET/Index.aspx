<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MySQL.NET</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        [<a href="Logout.aspx">Kirjaudu ulos</a>]
        <hr />
        <table>
        <tr>
            <td class="td">Tunnus:</td>
            <td>
                <asp:TextBox ID="txtTunnus" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td">Etunimi:</td>
            <td>
                <asp:TextBox ID="txtEnimi" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td">Sukunimi:</td>
            <td>
                <asp:TextBox ID="txtSnimi" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td">Osoite:</td>
            <td>
                <asp:TextBox ID="txtOsoite" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td">Puhnro:</td>
            <td>
                <asp:TextBox ID="txtPuhnro" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="td">Email:</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            </td>
        </tr>
    </table>

    <div style="padding: 10px; margin: 0px; width: 100%;">
        <asp:GridView ID="gvHenkilot" runat="server" OnSelectedIndexChanged="gvHenkilot_SelectedIndexChanged" OnRowDeleting="gvHenkilot_RowDeleting">
            <Columns>
                <asp:CommandField HeaderText="Update" ShowSelectButton="True" />
                <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
    </div>
    <asp:Label ID="lblMessages" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
