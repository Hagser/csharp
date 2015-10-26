using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

/// <summary>
/// Summary description for ExcelCell
/// </summary>

[DataContract]
public class ExcelCell
{
    [DataMember]
    public string Formula { get; set; }
    [DataMember]
    public object Value { get; set; }
    [DataMember]
    public string Coord { get; set; }
	public ExcelCell()
	{
	}
}