﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace MyPhotoSlideshow.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string getReceiverURL()
        {
            return ConfigurationManager.AppSettings["receiverURL"].ToString();
        }
    }
}
