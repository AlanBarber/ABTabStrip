<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ABTabStripDemo.aspx.cs" Inherits="DemoWebsite.ABTabStripDemo" %>
<%@ Register TagPrefix="ABTabStrip" Namespace="ABTabStrip" Assembly="ABTabStrip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <ABTabStrip:TabStrip runat="server" id="TabStrip1" Text="My Tab Strip" OnClick="TabStrip1_OnClick">
            <div>
                <asp:Label runat="server" ID="lblSelectedTabInfo"/>
            </div>
        </ABTabStrip:TabStrip>

    </div>
    </form>
</body>
</html>
