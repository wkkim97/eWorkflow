using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// JsonConvert의 요약 설명입니다.
/// </summary>
public class JsonConvert
{
	public JsonConvert()
	{
	}

    static public List<T> JsonListDeserialize<T>(string objStr)
    {
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        return js.Deserialize<List<T>>(objStr);
    }

    static public T JsonDeserialize<T>(string objStr)
    {
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        return js.Deserialize<T>(objStr);
    }

    static public string toJson<T>(List<T> arrList)
    {
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        return js.Serialize(arrList);
    }

    static public string toJson<T>(T arrList)
    {
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
        return js.Serialize(arrList);
    }
     
}