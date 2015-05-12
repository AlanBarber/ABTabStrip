<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DemoWebsite.Default" %>
<%@ Register TagPrefix="ABTabStrip" Namespace="ABTabStrip" Assembly="ABTabStrip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
    <ABTabStrip:TabStrip runat="server" id="TabStrip1" Text="TabStrip1" OnClick="TabStrip1_OnClick" >
        <div>
            <asp:Label runat="server" ID="lblSelectedTab"></asp:Label>
        </div>
    </ABTabStrip:TabStrip>
    </div>
    
    </form>
</body>
</html>
