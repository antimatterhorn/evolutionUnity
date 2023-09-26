using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class TextWriter
{
    //[MenuItem("Tools/Write file")]
    public void WriteString(string _string, string _filename)
    {
        string path = "Assets/Outputs/" + _filename;
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(_string);
        writer.Close();
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path); 
        //TextAsset asset = Resources.Load("test");
        //Print the text from the file
        //Debug.Log(asset.text);
    }
    //[MenuItem("Tools/Read file")]
    static void ReadString()
    {
        string path = "Assets/Resources/test.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path); 
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
