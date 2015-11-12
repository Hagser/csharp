using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Trafikverket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #if debug
            this.timer1.Interval = 10000;
            #endif

            ThreadPool.QueueUserWorkItem(GetLatestMeasure);
            ThreadPool.QueueUserWorkItem(removeDuplicates);
            ThreadPool.QueueUserWorkItem(removeInvalid);
            ThreadPool.QueueUserWorkItem(fixAllNames);
        }

        List<OdbcConnection> connections = new List<OdbcConnection>();
        OdbcConnection _con = new OdbcConnection();
        OdbcConnection MySqlConnection { get {
            
            string MyConString = "dsn=mysql_trafikverket32";
            try
            {
                if (connections.Count < 10)
                {
                    OdbcConnection ocon = new OdbcConnection(MyConString);
                    ocon.Open();
                    /*
                    System.Threading.Timer tim = new System.Threading.Timer((b) => { });
                    tim = new System.Threading.Timer((b) => {
                        if ((b as OdbcConnection) != null)
                        {
                            OdbcConnection con = b as OdbcConnection;
                            con.Close(); con.Dispose(); connections.Remove(con);
                            tim.Dispose();
                        }
                    }, ocon,60000,55000);
                    */
                    connections.Add(ocon);
                    return ocon;
                }
                else
                {
                    var ocon = connections.FirstOrDefault(c => c != null && c.State == ConnectionState.Open);
                    if (ocon == null)
                    {
                        Thread.Sleep(100);
                        ocon = connections.FirstOrDefault(c => c != null && c.State == ConnectionState.Open);
                        if (ocon == null)
                        {
                            ocon = connections.FirstOrDefault(c => c != null && c.State == ConnectionState.Closed);
                            if (ocon != null)
                                ocon.Open();
                        }
                    }
                    return ocon;
                }
                if (_con.State == ConnectionState.Closed)
                {
                    _con = new OdbcConnection(MyConString);
                    try
                    {

                        _con.Open();
                        CanConnect = true;
                    }
                    catch
                    {
                        CanConnect = false;
                    }
                }
            }
            catch { }
            return _con;
        } }
        private DataTable dtn = new DataTable();
        bool _CanConnect=false;
        bool CanConnect
        {
            get
            {
                if (_CanConnect || MySqlConnection.State == ConnectionState.Open)
                {
                    return true;
                }
                return _CanConnect;
            }
            set
            {
                if (_CanConnect != value)
                {
                    _CanConnect = value;
                }
            }
        }
        private void GetLatestMeasure(object state)
        {
            if (!CanConnect)
                return;

            using (DataSet ds = ExecuteSql("select * from trafikverket.LatestMeasures"))
            {
                using (DataTable dt1 = ds.Tables[0])
                {
                    if (dtn.TableName.Equals(""))
                    {
                        dtn = new DataTable(dt1.TableName);
                        foreach (DataColumn dc in dt1.Columns)
                            if (!dc.ColumnName.EndsWith("idweather", StringComparison.InvariantCultureIgnoreCase) && !dc.ColumnName.EndsWith("_Id", StringComparison.InvariantCultureIgnoreCase) && !dc.ColumnName.EndsWith("_Id_0", StringComparison.InvariantCultureIgnoreCase))
                            {
                                dtn.Columns.Add(dc.ColumnName, dc.DataType);
                            }
                    }
                    foreach (DataRow dr in dt1.Select("Active=true"))
                    {
                        if (bAbort)
                            return;

                        Weather weather = GetWeather(dr);
                        if (!list.Any(w => w.Id.Equals(weather.Id)))
                        {
                            list.Add(weather);
                            dtn.ImportRow(dr);
                        }
                    }
                    if (showDataToolStripMenuItem.Checked)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (dataGridView1.DataSource == null)
                                dataGridView1.DataSource = dtn;
                            else
                                dataGridView1.Refresh();
                            if (tabPage3.Controls.Count == 0)
                            {
                                Map map = new Map(list.ToList());
                                map.Dock = DockStyle.Fill;
                                tabPage3.Controls.Add(map);
                                map.Show();
                            }
                        });
                    }
                }
            }
        }
        private void DownTrCams(object state)
        { 
            int i = int.Parse(state.ToString());
            DownTrCams(i);
        }
        List<string> listOfText = new List<string>();
        string _thisText;
        string thisText {
            get { return _thisText; }
            set { /*if(!listOfText.Contains(value))listOfText.Add(value);
            if (listOfText.Count % 1000 == 1) { dataGridView2.DataSource = listOfText.Select(x => new { A = x.ToString() }).ToList(); } 
                */_thisText = value; }
        }
        CookieCollection cookies = new CookieCollection();
        private void DownTrCams(int idata)
        {
            System.GC.Collect();

            this.UseWaitCursor = true;
            string strText = "Trafikverket - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string url = "http://trafikinfo.trafikverket.se/lit/orion/orionproxy.ashx";

            string[] data = new string[]
            {"<ORIONML version='1.0'><REQUEST plugin='CameraInfo' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"Cameras\" filter=\"TypeValue='RoadConditionCamera' or TypeValue = 'atk' or TypeValue = 'TrafficCamera'\"  /></REQUEST></ORIONML>"
            ,"<ORIONML version='1.0'><REQUEST plugin='TrissData2' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"Weather\" filter=\"(EW IS NOT NULL) AND (NS IS NOT NULL)\" columns=\"Id,WindIconId,AmountOfPrecipitation,PrecipitationIconId,PrecipitationAmountName,EW,NS,MeasurePoint,Active,ZoomLevel,StationIconId,RoadTempIconId,AirTempIconId,AirTemp,RoadTemp,WindForce,MaxWindIconId,MaxWindForce,AverageWindIconId,AverageWindForce,CountyNo,MeasureTime,Moisture\"  /></REQUEST></ORIONML>"
            ,"<ORIONML version='1.0'><REQUEST plugin='TrissData2' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"Weather\" filter=\"(EW IS NOT NULL) AND (NS IS NOT NULL)\" columns=\"Id,Active,MeasurePoint\"  /></REQUEST></ORIONML>"
            ,"<ORIONML version='1.0'><REQUEST plugin='ATK' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"Cameras\"  /></REQUEST></ORIONML>"};
            try
            {
                HttpWebResponse resp;
                if (cookies != null && cookies.Count == 0)
                {
                    string s = MyWebClient.DownloadString("http://trafikinfo.trafikverket.se/lit/", out resp);
                    cookies = resp.Cookies;
                }
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "text/xml";
                request.Referer = "http://trafikinfo.trafikverket.se/LIT/";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.KeepAlive = true;
                string json = MyWebClient.UploadString(request, data[idata], cookies);

                XmlDocument xd = new XmlDocument();
                json = json.ToString().Replace(".", ",").Replace("{ \"MeasurePoint\": [", "{ \"Mps\": [");
                

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json, "Weather");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                string strCols = "";

                List<Weather> jsonlist = new List<Weather>();
                using (DataTable dt1 = ds.Tables[1])
                {
                    if (dtn.TableName.Equals(""))
                    {
                        dtn = new DataTable(dt1.TableName);
                        foreach (DataColumn dc in dt1.Columns)
                            if (!dc.ColumnName.EndsWith("_Id", StringComparison.InvariantCultureIgnoreCase) && !dc.ColumnName.EndsWith("_Id_0", StringComparison.InvariantCultureIgnoreCase))
                            {
                                dtn.Columns.Add(dc.ColumnName, GetDataType(dt1.Select("Active=true and " + dc.ColumnName + "<>''")[0][dc]), dc.Expression);
                            }
                    }
                    foreach (DataRow dr in dt1.Select("Active=true"))
                    {
                        if (bAbort)
                            return;
                        /*
                        this.Invoke((MethodInvoker)delegate
                        {
                            thisText = strText + " " + countAll(); //list.Count + "/" + dt1.Rows.Count + " - " + this.dataRowBuffer.Count ;
                        });
                        */
                        Weather weather = GetWeather(dr);
                        jsonlist.Add(weather);

                        continue;
                        if (weather.EW < 1)
                        {
                            string ssdfdfffff = "";
                            if (!string.IsNullOrEmpty(ssdfdfffff))
                            {

                            }
                        }
                        if (!list.Any(w => w.Id.Equals(weather.Id)))
                        {
                            list.Add(weather);

                            dtn.ImportRow(dr);
                        }
                        else
                        {
                            DataRow[] rows = dtn.Select("Id='" + dr["Id"].ToString() + "'");
                            if (rows.Length < 1)
                                dtn.ImportRow(dr);
                            else
                            {
                                if (areDifferent(rows[0], dr))
                                {
                                    rows[0].BeginEdit();
                                    //rows[0].SetColumnError(0, DateTime.Now.ToString("HH:mm:ss"));
                                    mergeRows(rows[0], dr);
                                    rows[0].EndEdit();
                                }
                                else
                                    rows[0].ClearErrors();
                            }
                        }
#if !DEBUG
                        try
                        {
                            ThreadPool.QueueUserWorkItem(insertDataRowToMySql, new object[] { dr, weather });
                        }
                        catch (Exception ex)
                        {
                            string sasdasd = ex.Message;
                            System.GC.Collect();
                        }
#endif
                    }
                    if (showDataToolStripMenuItem.Checked)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            thisText += "-" + countAll();
                            if (dataGridView1.DataSource == null)
                                dataGridView1.DataSource = dtn;
                            else
                                dataGridView1.Refresh();
                            if (tabPage3.Controls.Count == 0)
                            {
                                Map map = new Map(list.ToList());
                                map.Dock = DockStyle.Fill;
                                tabPage3.Controls.Add(map);
                                map.Show();
                            }
                        });
                    }
                    if (!bFirstHistoryRun)
                    {
                        bFirstHistoryRun = true;
                        timer2_Tick(this, EventArgs.Empty);
                    }
                }

                string njson = JsonConvert.SerializeObject(jsonlist);
                uploadJson(njson);

                


            }
            catch (System.OutOfMemoryException ex)
            {
                GC.Collect();
            }
            catch (Exception ex)
            {
                string ssdf = ex.Message;
                if (string.IsNullOrEmpty(ssdf))
                { }
            }
            this.UseWaitCursor = false;
            System.GC.Collect();
            return;
            /*
            dynamic obj = JObject.Parse(json);
            foreach (var ev in obj.Weather.MeasurePoint)
            {
                string id = ev.Id;
                int icnt = list.Count(x => x.Id.Equals(id));
                Weather w = icnt > 0 ? list.FirstOrDefault(x => x.Id.Equals(id)) : new Weather();
                w=AddProperties(w, ev);
                w.PropertyChanged += w_PropertyChanged;
                if (w.Active && icnt == 0)
                    list.Add(w);
                if (icnt == 1)
                {
                    string sdfölksdöflksöösöld = "";
                }
            }
            if (dataGridView1.DataSource == null)
            {
                dataGridView1.DataSource = GetList(icol, bDesc);
            }
             */
        }

        private void uploadJson(string json)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json; charset=ISO 8859-1");
                string jsonin = json/*.Remove(0, 21);
                jsonin = jsonin.Remove(jsonin.Length - 2, 2);
                jsonin = jsonin.TrimStart().TrimEnd().Trim()*/;
                
                wc.UploadData("http://php.hagser.se/insert_trafikverket.php",(Encoding.UTF8.GetBytes(jsonin)));

                wc.DownloadString("http://php.hagser.se/insert_trafikverket_fix.php");
            }
        }
        private void uploadJsonHist(string json)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/json; charset=ISO 8859-1");
                string jsonin = json/*.Remove(0, 21);
                jsonin = jsonin.Remove(jsonin.Length - 2, 2);
                jsonin = jsonin.TrimStart().TrimEnd().Trim()*/;
                wc.UploadData("http://php.hagser.se/insert_trafikverket_hist.php", (Encoding.UTF8.GetBytes(jsonin)));

                wc.DownloadString("http://php.hagser.se/insert_trafikverket_fix.php");
            }
        }

        private Weather GetWeather(DataRow dr)
        {
            Weather w = new Weather();
            Type source = dr.GetType();
            Type dest = w.GetType();
            foreach (PropertyInfo pi in dest.GetProperties())
            {

                if (dr.Table.Columns.Contains(pi.Name))
                {
                    string str = dr[pi.Name].ToString();
                    if (pi.PropertyType == typeof(bool))
                    {
                        bool bout = false;
                        bool.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(double))
                    {
                        double bout = 0;
                        double.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(DateTime))
                    {
                        DateTime bout = DateTime.MinValue;
                        DateTime.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else if (pi.PropertyType == typeof(Int32))
                    {
                        Int32 bout = Int32.MinValue;
                        Int32.TryParse(str, out bout);
                        pi.SetValue(w, bout);
                    }
                    else
                        pi.SetValue(w, str);
                }

            }
            try
            {
                string ew = dr["EW"].ToString();
                string ns = dr["NS"].ToString();
                if (!string.IsNullOrEmpty(ew) && !string.IsNullOrEmpty(ns))
                {
                    double dlat = double.Parse(ew.Replace(".", ","));
                    double dlng = double.Parse(ns.Replace(".", ","));

                    Weather wcoo = ConvertXYZToLatLngAlt(w.Id, dlat, dlng);
                    w.Lat = wcoo.Lat;
                    w.Lng = wcoo.Lng;
                }
            }
            catch (Exception ex)
            { }
            return w;
        }
        private Weather ConvertXYZToLatLngAlt(string Id,double Lat,double Lng)
        {
            Weather wcout = new Weather() { Id = Id, Lat = Lat, Lng = Lng };

            GeoUTMConverter conv = new GeoUTMConverter();
            conv.ToLatLon(Lat, Lng, 33, GeoUTMConverter.Hemisphere.Northern);
            wcout.Lat = Math.Round(conv.Latitude, 6);
            wcout.Lng = Math.Round(conv.Longitude, 6);

            return wcout;
        }
        private void removeInvalid(object state)
        {
            try
            {
                string strSql = "delete FROM trafikverket.weather where airtemp<-50 limit 300;";
                if (!CanConnect)
                    return;
                using (OdbcCommand com = new OdbcCommand(strSql, MySqlConnection))
                    com.ExecuteNonQuery();
            }
            catch { }
        }
        private void removeDuplicates(object state)
        {
            string strText = "Trafikverket - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                if (!CanConnect)
                    return;
                using (OdbcCommand comIdMes = new OdbcCommand("select distinct count(Id), Id,MeasureTime from trafikverket.weather group by Id,MeasureTime having count(Id) > 1", MySqlConnection))
                {
                    using (OdbcDataReader drIdMes = comIdMes.ExecuteReader())
                    {
                        while (drIdMes.Read())
                        {
                            /*
                            this.Invoke((MethodInvoker)delegate
                            {
                                thisText = strText +" "+ countAll() ;
                            });
                            */
                            Thread.Sleep(100);
                            if (bAbort)
                                return;
                            int icnt = drIdMes.GetInt32(0);
                            if (icnt > 1)
                            {
                                string strId = drIdMes.GetString(1);
                                string strMt = drIdMes.GetString(2);
                                string strSql = string.Format("delete from trafikverket.weather where Id='{0}' and MeasureTime ='{1}' limit {2}", strId, strMt, icnt - 1);
                                OdbcCommand comDel = new OdbcCommand(strSql, MySqlConnection);
                                comDel.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch(Exception ex) {
                string sdlfksdf = ex.Message;
                if (!string.IsNullOrEmpty(sdlfksdf))
                { }
            }
            this.Invoke((MethodInvoker)delegate
            {
                thisText = strText + " " + countAll();
            });

        }
        private void fixAllNames(object state)
        {
            try
            {
                if (!CanConnect)
                    return;
                using (OdbcCommand comID = new OdbcCommand("select distinct Id from trafikverket.weather where isnull(MeasurePoint);", MySqlConnection))
                {
                    using (OdbcDataReader drID = comID.ExecuteReader())
                    {
                        string strUpdateText = "";
                        while (drID.Read())
                        {
                            Thread.Sleep(100);
                            if (bAbort)
                                return;
                            string strId = drID.GetString(0);

                            using (OdbcCommand comName = new OdbcCommand("select distinct MeasurePoint from trafikverket.weather where Id='" + strId + "' and not isnull(MeasurePoint);", MySqlConnection))
                            using (OdbcDataReader drName = comName.ExecuteReader())
                            {
                                if (drName.Read())
                                {
                                    string strName = drName.GetString(0);
                                    strUpdateText += "update trafikverket.weather set MeasurePoint='" + strName + "' where Id='" + strId + "' and isnull(MeasurePoint);\r\n";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(strUpdateText))
                        {
                            try
                            {
                                using (OdbcCommand updateName = new OdbcCommand(strUpdateText, MySqlConnection))
                                    updateName.ExecuteNonQueryAsync();

                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            catch { }
        }
        
        private int countAll()
        {
            if (!CanConnect)
                return 0;
            using (OdbcCommand com = new OdbcCommand("select count(Id) from trafikverket.weather;", MySqlConnection))
            {
                OdbcDataReader odr = com.ExecuteReader();
                if (odr.Read())
                {
                    return odr.GetInt32(0);
                }
            }
            return 0;
        }
        private void insertDataRowToMySql(object state)
        {
            if ((state as object[]) != null)
            {
                var oa = state as object[];
                insertDataRowToMySql(oa[0] as DataRow,oa[1] as Weather);
            }
        }
        private void insertDataRowToMySql(DataRow dr,Weather wr)
        {
            //if(drin!=null)
            //    addToBuffer(drin);

            if (!CanConnect || !saveDataToolStripMenuItem.Checked)
            {
                return;
            }
            //if (dataRowBuffer.Count < 100)
            //    return;
            //List<DataRow> deleteDataRow = new List<DataRow>();
            //string strSqls = "";
            //    var thislist = dataRowBuffer.ToList();
            //foreach (DataRow dr in thislist.Take(200))
            {
                string strFields = "";
                string strValues = "";
                string[] notCol = new string[] { "MeasurePoint_Id", "MeasurePoint_Id_0", "Weather_Id" };

                foreach (DataColumn dc in dr.Table.Columns)
                {
                    if (bAbort)
                        return;
                    if (!notCol.Contains(dc.ColumnName) && (dc.ColumnName.Equals("PrecipitationAmount")|| this.dtn.Columns.Contains(dc.ColumnName)))
                    {
                        strValues += string.Format("{0}", fixValue(dr[dc.ColumnName])) + ",";
                        strFields += string.Format("{0}", dc.ColumnName.Equals("PrecipitationAmount")?"AmountOfPrecipitation":dc.ColumnName) + ",";
                    }
                }
                if (wr != null)
                {
                    strValues += string.Format("{0}", fixValue(wr.Lat)) + ",";
                    strFields += string.Format("{0}", "Lat") + ",";
                    strValues += string.Format("{0}", fixValue(wr.Lng)) + ",";
                    strFields += string.Format("{0}", "Lng") + ",";
                }

                strFields = strFields.Length > 5 ? strFields.Substring(0, strFields.Length - 1) : strFields;
                strValues = strValues.Length > 5 ? strValues.Substring(0, strValues.Length - 1) : strValues;
                string strSql = "INSERT INTO trafikverket.weather (" + strFields + ") values(" + strValues + ");";
                //strSqls += strSql += "\r\n";
                DateTime dtMt = DateTime.MinValue;
                DateTime.TryParse(dr["MeasureTime"].ToString(), out dtMt);
                using (OdbcCommand comexist = new OdbcCommand("select count(Id) from trafikverket.weather where Id='" + dr["Id"] + "' and MeasureTime='" + dtMt.ToString("yyyy-MM-dd HH:mm") + "'", MySqlConnection))
                {
                    
                    try
                    {
                        using (OdbcDataReader drexist = comexist.ExecuteReader())
                            if (drexist.Read() && drexist.GetInt32(0) == 0)
                            {
                                Thread.Sleep(100);
                                try
                                {
                                    using (OdbcCommand com = new OdbcCommand(strSql, MySqlConnection))
                                        com.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    string ss = ex.Message;
                                }
                            }
                    }
                    catch (Exception ex)
                    {
                        string ss = ex.Message;
                    }
                }
                //deleteDataRow.Add(dr);
            }

            //foreach (DataRow dr in deleteDataRow)
            //    dataRowBuffer.Remove(dr);
        }
        List<DataRow> dataRowBuffer = new List<DataRow>();
        private void addToBuffer(DataRow dr)
        {
            lock (dataRowBuffer)
            {
                if (!dataRowBuffer.Any(r => r["Id"].ToString().Equals(dr["Id"].ToString()) && r["MeasureTime"].ToString().Equals(dr["MeasureTime"].ToString())))
                    dataRowBuffer.Add(dr);
            }
        }

        private string fixValue(object c)
        {
            bool b = false;
            double d = 0;
            DateTime dt;
            if (bool.TryParse(c.ToString(), out b) )
            {
                return b ? "1" : "0";
            }
            else if (double.TryParse(c.ToString().Replace(".", ","), out d))
            {
                return "" + d.ToString().Replace(",", ".") + "";
            }
            else if (DateTime.TryParse(c.ToString().Replace(".", ","), out dt))
            {
                return "'" + dt + "'";
            }
            else
            {
                return "'" + c.ToString() + "'";
            }
        }

        private void mergeRows(DataRow org, DataRow chg)
        {
            foreach (DataColumn dc in dtn.Columns)
            {
                if (bAbort)
                    return;
                try
                {
                    var o = org[dc.ColumnName];
                    var c = chg[dc.ColumnName];
                    bool bchanged = false;
                    bool b = false;
                    double d = 0;
                    DateTime dt;
                    bool ob = false;
                    double od = 0;
                    DateTime odt;
                    if (bool.TryParse(c.ToString(), out b) && bool.TryParse(o.ToString(), out ob))
                    {
                        if (!ob.Equals(b))
                        {
                            org[dc.ColumnName] = b;
                            bchanged = true;
                        }
                    }
                    else if (double.TryParse(c.ToString().Replace(".", ","), out d) && double.TryParse(o.ToString().Replace(".", ","), out od))
                    {
                        if (!od.Equals(d))
                        {
                            org[dc.ColumnName] = d;
                            bchanged = true;
                        }
                    }
                    else if (DateTime.TryParse(c.ToString().Replace(".", ","), out dt) && DateTime.TryParse(o.ToString().Replace(".", ","), out odt))
                    {
                        if (!odt.Equals(dt))
                        {
                            org[dc.ColumnName] = dt;
                            bchanged = true;
                        }
                    }
                    else if (!o.Equals(c))
                    {
                        org[dc.ColumnName] = c;
                        bchanged = true;
                    }
                    continue;
                    if (bchanged)
                        org.SetColumnError(dc.ColumnName, string.Format("{0}-{1}", org[dc.ColumnName], chg[dc.ColumnName]));
                }
                catch (ArgumentException ex)
                { }
            }
        }

        private bool areDifferent(DataRow org, DataRow chg)
        {
            string strmt = "MeasureTime";
            var o1 = org[strmt];
            var c1 = chg[strmt];
            DateTime odt1;
            DateTime dt1;
            if (DateTime.TryParse(c1.ToString().Replace(".", ","), out dt1)&&DateTime.TryParse(o1.ToString().Replace(".", ","), out odt1))
            {
                if (dt1 > odt1)
                    return true;
            }
            foreach (DataColumn dc in dtn.Columns)
            {
                if (bAbort)
                    return false;
                try
                {
                    var o = org[dc.ColumnName];
                    var c = chg[dc.ColumnName];
                    bool b = false;
                    double d = 0;
                    DateTime dt;
                    bool ob = false;
                    double od = 0;
                    DateTime odt;
                    if (bool.TryParse(c.ToString(), out b) && bool.TryParse(o.ToString(), out ob))
                    {
                        if (!ob.Equals(b))
                            return true;
                    }
                    else if (double.TryParse(c.ToString().Replace(".", ","), out d) && double.TryParse(o.ToString().Replace(".", ","), out od))
                    {
                        if (!od.Equals(d))
                            return true;
                    }
                    else if (DateTime.TryParse(c.ToString().Replace(".", ","), out dt) && DateTime.TryParse(o.ToString().Replace(".", ","), out odt))
                    {
                        if (!odt.Equals(dt))
                            return true;
                    }
                    else if (!o.Equals(c))
                    {
                        return true;
                    }
                }
                catch (ArgumentException ex)
                {
                    
                }
            }
            return false;
        }

        private Type GetDataType(object p)
        {
            bool b = false;
            if (bool.TryParse(p.ToString(), out b))
                return typeof(bool);
            /*
            int i = 0;
            if (int.TryParse(p.ToString(), out i))
                return typeof(int);*/
            double d = 0;
            if (double.TryParse(p.ToString().Replace(".",","),out d))
                return typeof(double);

            DateTime dt;
            if (DateTime.TryParse(p.ToString().Replace(".", ","), out dt))
                return typeof(DateTime);

            return typeof(string);
        }

        void w_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Weather w = sender as Weather;
            string p = e.PropertyName;
            DataGridViewCellStyle style = new DataGridViewCellStyle();

            if (w.oldvalues != null && w.oldvalues.ContainsKey(p))
            { 
                PropertyInfo pi =w.GetType().GetProperty(p);
                switch (pi.PropertyType.FullName)
                {                        
                    case "System.Double":
                        double dval = double.Parse(pi.GetValue(w).ToString());
                        double oval = double.Parse(w.oldvalues[p].ToString());
                        if (dval > oval)
                        {
                            style.BackColor = Color.DarkGreen;
                            style.ForeColor = Color.White;
                            updateDataGridColors(style, p, w.Id, dval);
                        }
                        else if (oval > dval)
                        {
                            style.BackColor = Color.DarkRed;
                            style.ForeColor = Color.White;
                            updateDataGridColors(style, p, w.Id, dval);
                        }
                        else
                        {
                            style.BackColor = Color.White;
                            style.ForeColor = Color.Black;
                            updateDataGridColors(style, p, w.Id, dval);
                        }
                        break;
                    case "System.Int32":
                        int ival = int.Parse(pi.GetValue(w).ToString());
                        int ioval = int.Parse(w.oldvalues[p].ToString());
                        if (ival > ioval)
                        {
                            style.BackColor = Color.DarkGreen;
                            style.ForeColor = Color.White;
                            updateDataGridColors(style, p, w.Id, ival);
                        }
                        else if (ioval > ival)
                        {
                            style.BackColor = Color.DarkRed;
                            style.ForeColor = Color.White;
                            updateDataGridColors(style, p, w.Id, ival);
                        }
                        else
                        {
                            style.BackColor = Color.White;
                            style.ForeColor = Color.Black;
                            updateDataGridColors(style, p, w.Id, ival);
                        }
                        
                        break;
                    default: break;
                }
            }
        }

        private void updateDataGridColors(DataGridViewCellStyle style, string param, string Id,object oval)
        {
            if (dataGridView1.Columns.Contains(param))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (bAbort)
                        return;
                    if (row.Displayed && row.Cells["Id"] != null && row.Cells["Id"].Value.ToString().Equals(Id))
                    {
                        row.Cells[param].Style = style;
                        if (row.Cells[param].Displayed)
                        {
                            row.Cells[param].Value = oval;
                        }
                        break;
                    }
                }
            }
        }

        List<Weather> list = new List<Weather>();
        private List<SlimWeather> GetList(int colindx, bool bDesc)
        {
            return GetList(colindx, bDesc,"").Select(x => new SlimWeather {AirTemp= x.AirTemp, 
                CountyNo= x.CountyNo,
                Id= x.Id, 
                Lat = x.Lat, 
                Lng = x.Lng, 
                AverageWindForce = x.AverageWindForce,
                MaxWindForce= x.MaxWindForce, 
                MeasurePoint= x.MeasurePoint, 
                RoadTemp= x.RoadTemp, 
                WindForce= x.WindForce,
                PrecipitationIconId = x.PrecipitationIconId}).ToList();
        }
        private List<Weather> GetList(int colindx,bool bDesc,string gs)
        {
            switch(colindx)
            {
                case 0: return bDesc ? list.OrderByDescending(x => x.AirTemp).ToList() : list.OrderBy(x => x.AirTemp).ToList();
                case 1: return bDesc ? list.OrderByDescending(x => x.RoadTemp).ToList() : list.OrderBy(x => x.RoadTemp).ToList();
                case 2: return bDesc ? list.OrderByDescending(x => x.MeasurePoint).ToList() : list.OrderBy(x => x.MeasurePoint).ToList();
                case 3: return bDesc ? list.OrderByDescending(x => x.Id).ToList() : list.OrderBy(x => x.Id).ToList();
                case 4: return bDesc ? list.OrderByDescending(x => x.Lat).ToList() : list.OrderBy(x => x.Lat).ToList();
                case 5: return bDesc ? list.OrderByDescending(x => x.Lng).ToList() : list.OrderBy(x => x.Lng).ToList();
                case 6: return bDesc ? list.OrderByDescending(x => x.WindForce).ToList() : list.OrderBy(x => x.WindForce).ToList();
                case 7: return bDesc ? list.OrderByDescending(x => x.AverageWindForce).ToList() : list.OrderBy(x => x.AverageWindForce).ToList();
                case 8: return bDesc ? list.OrderByDescending(x => x.MaxWindForce).ToList() : list.OrderBy(x => x.MaxWindForce).ToList();
                case 9: return bDesc ? list.OrderByDescending(x => x.CountyNo).ToList() : list.OrderBy(x => x.CountyNo).ToList();
                case 10: return bDesc ? list.OrderByDescending(x => x.PrecipitationIconId).ToList() : list.OrderBy(x => x.PrecipitationIconId).ToList();
            }
            return list.ToList();
        }

        private Weather AddProperties(Weather w, dynamic ev)
        {
            try
            {
                w.Active = ev.Active==null?false:ev.Active;
                w.AirTemp = ev.AirTemp == null ? 0 : ev.AirTemp;
                w.AirTempIconId = ev.AirTempIconId == null ? 0 : ev.AirTempIconId;
                w.AverageWindIconId = ev.AverageWindIconId == null ? 0 : ev.AverageWindIconId;
                w.CountyNo = ev.CountyNo == null ? 0 : ev.CountyNo;
                w.EW = ev.EW == null ? 0 : ev.EW;
                w.Id = ev.Id == null ? "" : ev.Id;
                w.MaxWindIconId = ev.MaxWindIconId == null ? 0 : ev.MaxWindIconId;
                w.MeasurePoint = ev.MeasurePoint == null ? "" : ev.MeasurePoint;
                w.NS = ev.NS == null ? 0 : ev.NS;
                w.PrecipitationIconId = ev.PrecipitationIconId == null ? 0 : ev.PrecipitationIconId;
                w.RoadTemp = ev.RoadTemp == null ? 0 : ev.RoadTemp;
                w.RoadTempIconId = ev.RoadTempIconId == null ? 0 : ev.RoadTempIconId;
                w.StationIconId = ev.StationIconId == null ? 0 : ev.StationIconId;
                w.WindIconId = ev.WindIconId == null ? 0 : ev.WindIconId;
                w.ZoomLevel = ev.ZoomLevel == null ? 0 : ev.ZoomLevel;

                w.AverageWindForce = ev.AverageWindForce == null ? 0 : ev.AverageWindForce;
                w.MaxWindForce = ev.MaxWindForce == null ? 0 : ev.MaxWindForce;
                w.WindForce = ev.WindForce == null ? 0 : ev.WindForce;

                GeoUTMConverter conv = new GeoUTMConverter();
                conv.ToLatLon(w.EW, w.NS, 33, GeoUTMConverter.Hemisphere.Northern);
                w.Lat = Math.Round(conv.Latitude, 6);
                w.Lng = Math.Round(conv.Longitude, 6);
                
            }
            catch(Exception ex) {
                string s = ex.Message;
            }
            return w;
        }
        int icol = -1;
        bool bDesc = false;
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bDesc = (icol == e.ColumnIndex) ? !bDesc : false;
            dataGridView1.Sort(dataGridView1.Columns[e.ColumnIndex], bDesc ? ListSortDirection.Descending : ListSortDirection.Ascending);
            icol = e.ColumnIndex;
            //dataGridView1.DataSource = GetList(e.ColumnIndex,bDesc);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(DownTrCams, 1);
            this.UseWaitCursor = true;
            if (showDataToolStripMenuItem.Checked)
            {
                ThreadPool.QueueUserWorkItem(GetStats);
                if (tabPage3.Controls.Count > 0)
                {
                    Map map = tabPage3.Controls[0] as Map;
                    if (map != null)
                    {
                        if (map.List != null && map.List.Count > 0)
                        {
                            foreach (Weather w in list)
                            {
                                var mw = map.List.FirstOrDefault(x => x.Id.Equals(w.Id));
                                if (mw != null)
                                    mw = w;
                            }
                        }
                        else
                            map.List.AddRange(list.ToList());

                        map.addWeather();
                    }
                }
            }
            this.UseWaitCursor = false;
            System.GC.Collect();
        }

        private void GetStats(object state)
        {
            if (!CanConnect)
                return;
            try
            {
                DataSet dsViews = ExecuteSql(@"SELECT table_name FROM INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='trafikverket' and table_type='VIEW' and table_name like 'Get%' order by table_name;");
                var views = dsViews.Tables[0].Rows.Cast<DataRow>().Select(x => x.ItemArray[0]).ToArray();
                //var views = "GetMaxMinAvgPerDay;GetMaxMinDiffPerMeasurePoint;GetMaxminAvgPerMonth".Split(';');
                foreach (string view in views)
                {
                    if (!flpViews.Controls.Cast<Control>().Any(c => c.Tag != null && c.Tag.ToString().Equals(view)))
                    {
                        DataGridView dgv = new DataGridView();
                        dgv.AutoGenerateColumns = true;
                        dgv.Name = "dgv" + view;
                        dgv.Tag = view;
                        dgv.Height = flpViews.Height - 10;
                        double dc = double.Parse(views.Count().ToString());
                        double w = (flpViews.Width / dc) - (dc * 3);
                        dgv.Width = int.Parse(Math.Floor(w).ToString());
                        using (DataSet ds = ExecuteSql("select * from trafikverket." + view + ";"))
                            this.Invoke((MethodInvoker)delegate
                            {
                                dgv.DataSource = ds.Tables[0];
                                flpViews.Controls.Add(dgv);
                                dgv.Refresh();
                                flpViews.Refresh();
                                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                            });
                    }
                    else
                    {

                        DataGridView dgv = flpViews.Controls.Cast<Control>().FirstOrDefault(c => c.Tag != null && c.Tag.ToString().Equals(view)) as DataGridView;
                        if (dgv == null)
                            continue;
                        using (DataSet ds = ExecuteSql("select * from trafikverket." + view + ";"))
                            this.Invoke((MethodInvoker)delegate
                            {
                                dgv.DataSource = ds.Tables[0];
                                dgv.Refresh();
                                flpViews.Refresh();
                                dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                            });
                    }
                }
            }
            catch { }
            System.GC.Collect();
        }
        private DataSet ExecuteSql(string query)
        {
            DataSet ds = new DataSet();
            if (!CanConnect)
                return ds;
            try
            {
                using (OdbcCommand com = new OdbcCommand(query, MySqlConnection))
                using (OdbcDataAdapter oda = new OdbcDataAdapter(com))
                    oda.Fill(ds);
            }
            catch { }
            return ds;
        }
        bool bAbort = false;
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            bAbort = true;
        }

        private void fetchWeatherToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = fetchWeatherToolStripMenuItem.Checked;
        }

        private void flpViews_Resize(object sender, EventArgs e)
        {
            foreach (Control c in flpViews.Controls)
            {
                c.Height = flpViews.Height - 10;
                double dc = double.Parse(flpViews.Controls.Count.ToString());
                double w = (flpViews.Width / dc) - (dc * 2);
                c.Width = int.Parse(Math.Floor(w).ToString());
            }
        }

        private void sequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CanConnect)
                return;
            this.UseWaitCursor = true;
            DateTime dt = DateTime.MinValue;
            switch ((sender as ToolStripItem).Text)
            { 
                case "sequenceToolStripMenuItem":
                    dt = DateTime.Today;
                    break;
                default:
                    DateTime.TryParse((sender as ToolStripItem).Text, out dt);
                    break;
            }
            string strSql = @"select MeasurePoint, round(avg(AirTemp),1)AirTemp, PrecipitationIconId, WindIconId, AverageWindIconId, MaxWindIconId,EW,Id,NS,
round(avg( AmountOfPrecipitation),1)AmountOfPrecipitation,round(avg( WindForce),1) WindForce,round(avg( AverageWindForce),1) AverageWindForce,round(avg( MaxWindForce),1) MaxWindForce, 
year(MeasureTime), month(MeasureTime), day(MeasureTime), hour(MeasureTime),MeasureTime
from trafikverket.weather
where year(MeasureTime)='{0}' and month(MeasureTime)='{1}' and day(MeasureTime)='{2}'
group by MeasurePoint, PrecipitationIconId, WindIconId, AverageWindIconId, MaxWindIconId,EW,Id,NS, 
year(MeasureTime), month(MeasureTime), day(MeasureTime), hour(MeasureTime)
order by year(MeasureTime), month(MeasureTime), day(MeasureTime), hour(MeasureTime),Measurepoint";
            using (DataSet ds = ExecuteSql(
                string.Format(strSql,
                dt.ToString("yyyy"), dt.ToString("MM"), dt.ToString("dd"))
                ))
            {
                List<Weather> times = new List<Weather>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Weather weather = GetWeather(dr);
                    times.Add(weather);
                }
                if (tabPage3.Controls.Count == 0)
                {
                    Map mp = new Map(list.ToList());
                    mp.Dock = DockStyle.Fill;
                    tabPage3.Controls.Add(mp);
                }
                Map map = (tabPage3.Controls[0] as Map);
                map.addTimes(times.OrderBy(x => x.MeasureTime).ToList());
                tabControl1.SelectedTab = tabPage3;
            }
            this.UseWaitCursor = false;
        }
        private void GetDates(object state)
        {
            if (!CanConnect)
                return;
            try
            {
                List<string> li = new List<string>();
                foreach(ToolStripMenuItem t in sequenceToolStripMenuItem.DropDownItems)
                    li.Add(t.Text);
                string strSql = "select distinct Year,Month,Day from trafikverket.GetMaxMinAvgPerDay order by Year,Month,Day;";
                using (DataSet ds = ExecuteSql(string.Format(strSql)))
                {
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    for(int i=ds.Tables[0].Rows.Count-10;i<ds.Tables[0].Rows.Count;i++)
                    {
                        DataRow dr = ds.Tables[0].Rows[i];
                        string strDate = dr[0] + "-" + dr[1].ToString().PadLeft(2, '0') + "-" + dr[2].ToString().PadLeft(2, '0');
                        if (!li.Contains(strDate))
                        {
                            this.Invoke((MethodInvoker)delegate
                            {

                                ToolStripItem tsi = sequenceToolStripMenuItem.DropDownItems.Add(strDate);
                                tsi.Click += sequenceToolStripMenuItem_Click;
                            });
                        }
                    }
                }

            }
            catch { }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                if (tabControl1.SelectedTab.Text.Equals("Stats"))
                {
                    foreach (DataGridView dgv in flpViews.Controls.Cast<DataGridView>())
                        dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                }
                if (tabControl1.SelectedTab.Text.Equals("Weather"))
                {
                    tabControl1.SelectedTab.Controls.Cast<DataGridView>().FirstOrDefault().AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                }
            }
        }

        private void mapToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            //if(sequenceToolStripMenuItem.DropDownItems.Count==0)
                ThreadPool.QueueUserWorkItem(GetDates);
        }
        bool bFirstHistoryRun = false;
        private void fetchWeatherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer2.Enabled = fetchWeatherToolStripMenuItem.Checked;
            if (fetchWeatherToolStripMenuItem.Checked)
            {
                ThreadPool.QueueUserWorkItem(DownTrCams, 1);
                if (showDataToolStripMenuItem.Checked)
                {
                    ThreadPool.QueueUserWorkItem(GetStats);
                    ThreadPool.QueueUserWorkItem(GetDates);
                }
                
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (Weather w in list)
                ThreadPool.QueueUserWorkItem(GetHistory, w.Id);

            if (saveDataToolStripMenuItem.Checked)
            {
                ThreadPool.QueueUserWorkItem(removeDuplicates);
                ThreadPool.QueueUserWorkItem(removeInvalid);
                ThreadPool.QueueUserWorkItem(fixAllNames);
            }
        }

        private void GetHistory(object state)
        {
            System.GC.Collect();

            string url = "http://trafikinfo.trafikverket.se/lit/orion/orionproxy.ashx";
            string strText = "Trafikverket - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string data3 = "<ORIONML version='1.0'><REQUEST plugin='TrissData2' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"WeatherHistory\" filter=\"Id=='" + state.ToString() + "'\" "+
                "columns=\"PrecipitationAmount,PrecipitationIconId,PrecipitationAmountName,RoadTempIconId,AirTempIconId,AirTemp,RoadTemp,MaxWindIconId,MaxWindForce,AverageWindIconId,AverageWindForce,MeasureTime\"  /></REQUEST></ORIONML>";
            string data = "<ORIONML version='1.0'><REQUEST plugin='TrissData2' version='' locale='SE_sv' authenticationkey='7fd72d2a-4746-482c-b856-15a64f85a205'><PLUGINML  table=\"WeatherHistory\" filter=\"Id == '" + state.ToString() + "'\" " +
                "columns=\"Id,MeasureTime,PrecipitationIconId,PrecipitationType,PrecipitationAmount,PrecipitationAmountName,RoadTemp,RoadTempIconId,AirTemp,AirTempIconId,AverageWindIconId,MaxWindIconId,Moisture,AverageWindForce,AverageWindDirectionValue,AverageWindDirection,MaxWindForce,MaxWindDirectionValue,MaxWindDirection,PrecipitationTypeValue\" orderby=\"MeasureTime desc\"  /></REQUEST></ORIONML>";
            try
            {
                HttpWebResponse resp;
                if (cookies != null && cookies.Count == 0)
                {
                    string s = MyWebClient.DownloadString("http://trafikinfo.trafikverket.se/lit/", out resp);
                    cookies = resp.Cookies;
                }

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "text/xml";
                request.Referer = "http://trafikinfo.trafikverket.se/LIT/";
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.KeepAlive = true;
                string json = MyWebClient.UploadString(request, data, cookies);

                XmlDocument xd = new XmlDocument();
                json = json.ToString().Replace(".", ",").Replace("{ \"MeasurePoint\": [", "{ \"Mps\": [");

                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(json, "WeatherHistory");
                DataSet ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
                List<Weather> jsonlist = new List<Weather>();
                using (DataTable dt1 = ds.Tables[1])
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (bAbort)
                            return;

                        try
                        {
                            Weather wr = GetWeather(dr);

                            jsonlist.Add(wr);
                            continue;

                            ThreadPool.QueueUserWorkItem(insertDataRowToMySql,new object[]{dr,wr});
                        }
                        catch (Exception ex)
                        {
                            string sasdasd = ex.Message;
                            System.GC.Collect();

                        }
                    }
                }

                string njson = JsonConvert.SerializeObject(jsonlist);
                uploadJsonHist(njson.Replace("WeatherHistory", "Weather"));

                this.Invoke((MethodInvoker)delegate
                {
                    thisText = strText + " " + countAll(); //list.Count + "/" + dt1.Rows.Count + " - " + this.dataRowBuffer.Count ;
                });

            }
            catch (Exception ex)
            {
                string ssdf = ex.Message;
                if (string.IsNullOrEmpty(ssdf))
                { }
            }
            System.GC.Collect();
        }

    }
}
