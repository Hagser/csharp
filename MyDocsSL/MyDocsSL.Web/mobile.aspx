<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mobile.aspx.cs" Inherits="mobile" EnableEventValidation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body,td,input
        {
            
            font-family:Calibri;
            font-size:12px;
        }
        .headerstyle
        {
            margin:0px;
            padding:0px;
            background-color:Navy;
            color:White;
            font-weight:bold;
            font-size:14px;
        }
        .itemstyle
        {
            margin:0px;
            padding:0px;
            background-color:white;
        }
        .altitemstyle
        {
            margin:0px;
            padding:0px;
            background-color:Silver;
        }
        a,a.hover,a.visited
        {
            text-decoration:none;
            color:Navy;
        }
    </style>
     <script type="text/javascript">
         function MM(el) {
             var els = document.getElementsByTagName("span");
             for (var i = 0; i < els.length; i++) {
                 if (els[i].id != null && els[i].id.indexOf("showme") != -1) {
                     els[i].style.display = "none";
                 }
             }
             toggleDisplay(el);
         }
         function toggleDisplay(el) {
             var elsp = document.getElementById(el);
             if (elsp != null) {
                 if (elsp.style.display == "none") {
                     elsp.style.display = "block";
                 }
                 else if (elsp.style.display == "block") {
                     elsp.style.display = "none";
                 }
             }         
         }
         function Download(el) {
             toggleDisplay("showme_" + el.split("=")[1]);
             document.location = el;
         }
         function Delete(el) {
             if (confirm("Delete file '" + el + "'?")) { document.location = 'mobile.aspx?delete=' + el; }
         }
         function Versions(el) {
             alert(el);
         }
         function CopyUrl(el) {
             window.clipboardData.setData('Text', el);
         }
         function ViewUrl(el) {
             window.open('view.aspx?filename=' + el, "_blank", "", "");
             var vf = document.getElementById("body");
             if (vf != null) {
                 vf.location = 'view.aspx?filename=' + el;
                 vf.style.visibility = "visible";
             }
             toggleDisplay("showme_"+el);
         }
         function setBytes(r, l) {
             var p = Math.round(eval(r) / eval(l)*100);
             var pr = document.getElementById("progress");
             pr.style.width = p + "%";
         }
     </script>
</head>
<body id="body" runat="server">
    <form id="form1" action="receiver.ashx" enctype='multipart/form-data' target="_uploadframe" runat="server">
    <div>
    
    <asp:DataGrid runat="server" DataSourceID="FileDataSource" AlternatingItemStyle-CssClass="altitemstyle" ItemStyle-CssClass="itemstyle" HeaderStyle-CssClass="headerstyle" GridLines="Both" AutoGenerateColumns="false" Width="100%" ID="DataGrid1">
    <AlternatingItemStyle Height="10" />
    <ItemStyle Height="10" />
    <HeaderStyle Height="10" />
    <Columns>
        <asp:TemplateColumn HeaderText="Options" ItemStyle-Width="70">
            <ItemTemplate>
            <span><a href="javascript:MM('showme_<%#DataBinder.Eval(Container.DataItem, "Name")%>');">Options</a><br />
            <span id="showme_<%#DataBinder.Eval(Container.DataItem, "Name")%>" style="position:absolute;display:none;background-color:White;border:solid black 1px;z-index:100;"><a href="javascript:Download('<%#DataBinder.Eval(Container.DataItem, "Uri")%>');">Download</a><br /><a href="javascript:Delete('<%#DataBinder.Eval(Container.DataItem, "Name")%>');">Delete</a><br /><a href="javascript:Versions('<%#DataBinder.Eval(Container.DataItem, "Name")%>');">Versions</a><br /><a href="javascript:CopyUrl('<%#DataBinder.Eval(Container.DataItem, "Uri")%>');">Copy Url</a><br /><a href="javascript:ViewUrl('<%#DataBinder.Eval(Container.DataItem, "Name")%>');">View</a><br /></span></span>
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:HyperLinkColumn DataTextField="Name" DataNavigateUrlField="Uri" HeaderText="Name" />
        <asp:BoundColumn DataField="Size" HeaderText="Size" ItemStyle-Width="30"/>
        <asp:BoundColumn DataField="Changed" HeaderText="Changed" ItemStyle-Width="150"/>
        <asp:BoundColumn DataField="Uri" Visible="false"/>
        <asp:BoundColumn DataField="Name" Visible="false"/>
    </Columns>
    </asp:DataGrid>
        <asp:ObjectDataSource ID="FileDataSource" runat="server" SelectMethod="ListFiles" TypeName="FileService"></asp:ObjectDataSource>
        <br />
        <input type="file" name="filename"/><br /><br />
        <span style="position:absolute; width:99%;height:15px;background-Color:Orange;border:solid black 1px;" id="progressspan">
        <span style="position:absolute; width:-1%;height:13px;background-Color:Green;padding:1px;" id="progress"></span>
        </span><br />
        <br />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" />&nbsp;<input type="button" value="Refresh" id="btnRefresh" onclick="document.location='mobile.aspx';" />
    </div>
    </form>
    <iframe id="_uploadframe" name="_uploadframe" style="height:100;width:200;position:absolute;visibility:hidden;" />
    <iframe id="viewframe" name="viewframe" style="height:80%;width:80%;position:absolute;visibility:hidden;top:10;left:10;" />
</body>
</html>
