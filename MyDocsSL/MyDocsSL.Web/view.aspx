<%@ Page Language="C#" AutoEventWireup="true" CodeFile="view.aspx.cs" Inherits="view" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .imgdiv
    {   
        float:left;height:90%;width:90%;
        border:1px solid black;margin:1px;
        text-align:center;vertical-align:middle;
        background-repeat:no-repeat;
        background-position:center center;
    }
    </style>
    <script type="text/javascript">
        function refreshSizes() {
            var el = document.getElementById('<%=image.ClientID%>');refreshSize(el);
            var el = document.getElementById('<%=pdfview.ClientID%>');refreshSize(el);
        }

        function refreshSize(el) {
            var h = Math.max(document.body.offsetHeight,800);
            var w = document.body.offsetWidth;
            el.style.width = w + "px";
            el.style.height = h + "px";
        }
    </script>
</head>
<body onload="javascript:refreshSizes();">
    <form id="form1" runat="server">
    <div class="imgdiv" id="image" style="display:none;" runat="server" /> 
    <iframe id="pdfview" runat="server" style="display:none;width:90%;height:90%" />
    <div style="float:left;height:90%;width:10%;">
        <asp:DataGrid ID="SheetList" Visible="false" runat="server" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateColumn>
                <ItemTemplate>
                <asp:Button Text='<%# Eval("SheetName") %>' Height="20px" ID="btn" OnClick="btn_OnClick" runat="server"/>
                </ItemTemplate>                
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>
    <div style="float:left;height:90%;width:89%;overflow:auto;">
    <asp:DataGrid ID="dataGrid" Visible="false" runat="server" ItemStyle-Height="26px" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundColumn DataField="Row" HeaderText="Row" ReadOnly="True"></asp:BoundColumn>
            <asp:BoundColumn DataField="A" HeaderText="A"></asp:BoundColumn>
            <asp:BoundColumn DataField="B" HeaderText="B"></asp:BoundColumn>
            <asp:BoundColumn DataField="C" HeaderText="C"></asp:BoundColumn>
            <asp:BoundColumn DataField="D" HeaderText="D"></asp:BoundColumn>
            <asp:BoundColumn DataField="E" HeaderText="E"></asp:BoundColumn>
            <asp:BoundColumn DataField="F" HeaderText="F"></asp:BoundColumn>
            <asp:BoundColumn DataField="G" HeaderText="G"></asp:BoundColumn>
            <asp:BoundColumn DataField="H" HeaderText="H"></asp:BoundColumn>
            <asp:BoundColumn DataField="I" HeaderText="I"></asp:BoundColumn>
            <asp:BoundColumn DataField="J" HeaderText="J"></asp:BoundColumn>
            <asp:BoundColumn DataField="K" HeaderText="K"></asp:BoundColumn>
            <asp:BoundColumn DataField="L" HeaderText="L"></asp:BoundColumn>
            <asp:BoundColumn DataField="M" HeaderText="M"></asp:BoundColumn>
            <asp:BoundColumn DataField="N" HeaderText="N"></asp:BoundColumn>
            <asp:BoundColumn DataField="O" HeaderText="O"></asp:BoundColumn>
            <asp:BoundColumn DataField="P" HeaderText="P"></asp:BoundColumn>
            <asp:BoundColumn DataField="Q" HeaderText="Q"></asp:BoundColumn>
            <asp:BoundColumn DataField="R" HeaderText="R"></asp:BoundColumn>
            <asp:BoundColumn DataField="S" HeaderText="S"></asp:BoundColumn>
            <asp:BoundColumn DataField="T" HeaderText="T"></asp:BoundColumn>
            <asp:BoundColumn DataField="U" HeaderText="U"></asp:BoundColumn>
            <asp:BoundColumn DataField="V" HeaderText="V"></asp:BoundColumn>
            <asp:BoundColumn DataField="W" HeaderText="W"></asp:BoundColumn>
            <asp:BoundColumn DataField="X" HeaderText="X"></asp:BoundColumn>
            <asp:BoundColumn DataField="Y" HeaderText="Y"></asp:BoundColumn>
            <asp:BoundColumn DataField="Z" HeaderText="Z"></asp:BoundColumn>
        </Columns>
        <ItemStyle Height="26px" />
        </asp:DataGrid>
    </div>
    
    </form>
</body>
</html>
