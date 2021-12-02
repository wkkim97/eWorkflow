<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EncryptTest.aspx.cs" Inherits="Templete_EncryptTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtInput" runat="server"></asp:TextBox>
        <asp:Button runat="server" ID="btnEncrypt" OnClick="btnEncrypt_Click" Text="Encrypt" /> 
        <asp:Button runat="server" ID="btnDecrypt" OnClick="btnDecrypt_Click" Text="Decrypt" />
        <asp:Label ID="lblResponse" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
