using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class view : System.Web.UI.Page
{
    public ExcelDocument Exceldocument { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        string filename = "";
        if (Request.QueryString["filename"] != null)
        {
            filename = Request.QueryString["filename"].ToString();

            if (filename.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                SheetList.Visible = true;
                dataGrid.Visible = true;

                Exceldocument = new ExcelDocument(filename);
                SheetList.DataSource = Exceldocument.WorkSheets;
                SheetList.DataBind();
            }
            else if (filename.EndsWith("pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                pdfview.Style["display"] = "block";
                pdfview.Attributes["src"] = "sender.ashx?filename=" + filename;
            }
            else if (filename.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase))
            {
                image.Style["display"] = "block";
                image.Style["background-image"] = "url(\"sender.ashx?filename=" + filename + "\")";
            }
        }
        else
        {
            Response.Redirect("mobile.aspx");
        }            

    }
    public void btn_OnClick(object sender, EventArgs e)
    {        
        Button bu = (sender as Button);
        string sheet = bu.Text;
        WorkSheet wsheet = Exceldocument.WorkSheets.Where(x => x.SheetName.Equals(sheet)).FirstOrDefault();
        if (wsheet != null)
        {
            dataGrid.DataSource = null;
            dataGrid.DataSource = wsheet.Rows;
            dataGrid.DataBind();
        }
    }
}