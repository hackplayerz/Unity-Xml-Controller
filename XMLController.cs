using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using JetBrains.Annotations;
using UnityEngine;

public class XMLController
{
    /// <summary>
    /// Load XML
    /// </summary>
    /// <param name="filePath">file exist path</param>
    /// <param name="header">Xml header string</param>
    /// <param name="strKeys">Search key</param>
    /// <returns>dictionary data</returns>
    public static Dictionary<string, string> LoadData(string filePath,string header,string[] strKeys)
    {
        var dataDictionary = new Dictionary<string, string>(); // Load Data
        var stringBuilder = new StringBuilder();
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        var fileName = Application.streamingAssetsPath + '/' + filePath; // Use StreamingAsset folder
#endif
        using (var reader = XmlReader.Create(fileName))
        {
            // Read all data
            while( reader.Read() )
            {
                if ( reader.Name.Equals(header) )
                {
                    reader.Read();
                    foreach( var key in strKeys )
                    {
                        stringBuilder.Clear();
                        stringBuilder.Append(reader.ReadElementString(key, ""));
                        dataDictionary.Add(key, stringBuilder.ToString());
                    }

                }
            }
        }

        return dataDictionary;
    }

    /// <summary>
    /// Create Xml file 
    /// </summary>
    /// <param name="fileName">Save file name</param>
    /// <param name="xmlDictionary">Save Data</param>
    /// <param name="strStartElement">Xml start element</param>
    /// <param name="strAttribute">Xml attribute</param>
    /// <param name="strAttributeValue">Xml attribute value</param>
    /// <param name="strKeys">Save data's key</param>
    public static void CreateXmlFile(string fileName, Dictionary<string, string> xmlDictionary, string[] strKeys,
                                     string strStartElement, [CanBeNull] string strAttribute = null, [CanBeNull] string
                                         strAttributeValue = null)
    {
        // None file Exception
        if ( fileName == null || xmlDictionary.Count <= 0 )
        {
            Debug.LogError("FileName : " + (string.IsNullOrEmpty(fileName) ? fileName : "NULL") +
                           "Data : " + (xmlDictionary.Count <= 0 ? xmlDictionary.ToString() : "Empty"));
            return;
        }
#if ( UNITY_EDITOR || UNITY_STANDALONE_WIN )
        var filePath = Application.streamingAssetsPath + "/" + fileName;
#endif
        // instancing xml writer
        using (var writer = XmlWriter.Create(filePath))
        {
            // Write start
            writer.WriteStartDocument();

            writer.WriteStartElement(strStartElement);
            
            // Attribute option
            if(strAttribute != null && strAttributeValue != null)
                writer.WriteAttributeString(strAttribute,strAttributeValue);
            
            // Maybe...GC 
            foreach( var key in strKeys )
            {
                // Write data
                writer.WriteElementString(key, xmlDictionary[key]);
            }
            writer.WriteEndElement();
            
            // Write end
            writer.WriteEndDocument();
        }
    }
}

