using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

public partial class mobile : System.Web.UI.Page
{
    FileService fileservice = new FileService();
    Timer tim = new Timer();
    protected void Page_Load(object sender, EventArgs e)
    {
        tim.Interval = 5 * 60 * 1000;
        tim.Tick += (a, b) =>
        {
            Response.Redirect("mobile.aspx");        
        };
        tim.Enabled = true;
        if (Request.QueryString.Get("delete") != null)
        {
            fileservice.DeleteFile(Request.QueryString.Get("delete"));
            Response.Redirect("mobile.aspx"); 
        }
    }


}