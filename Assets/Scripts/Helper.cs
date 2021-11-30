using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class Helper
{
    public static string Serialize(string toSerialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(string));
        StringWriter writer = new StringWriter();
        xml.Serialize(writer, toSerialize);
        return writer.ToString();
    }

    public static string Deserialize(this string toDesirialize)
    {
        XmlSerializer xml = new XmlSerializer(typeof(string));
        StringReader reader = new StringReader(toDesirialize);
        return (string)xml.Deserialize(reader);
    }
}
