using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Phenotype", menuName = "Scriptable Objects/Phenotype")]
public class Phenotype : ScriptableObject
{
    public Sprite sprite;
    public float scale=1f;
    public float colorValue;
    public string[] geneIds;
    public float r;
    public float theta;
    public bool reflected;

    private (int,int,int) min = (255,0,0);      //red
    private (int,int,int) mid = (255,255,255);  //white
    private (int,int,int) max = (0,255,0);      //green

    private (int,int,int) SetColor(float _value)
    {
        float x = Math.Min(_value,1f); 
        x = Math.Max(x,0f);
        colorValue = x;
        int r,g,b;
        if (x < 0.5f)
        {
            r = (int)((mid.Item1 - min.Item1) * x + min.Item1);
            g = (int)((mid.Item2 - min.Item2) * x + min.Item2);
            b = (int)((mid.Item3 - min.Item3) * x + min.Item3);
        }
        else
        {
            r = (int)((max.Item1 - mid.Item1) * x + mid.Item1);
            g = (int)((max.Item2 - mid.Item2) * x + mid.Item2);
            b = (int)((max.Item3 - mid.Item3) * x + mid.Item3);
        }
        return (r,g,b);
    }
}
