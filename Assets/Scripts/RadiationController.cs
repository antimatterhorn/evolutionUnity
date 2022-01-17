using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RadiationController : MonoBehaviour
{
    public float pulseRate;
    private SerializedObject myHalo;
    Color haloColor;
    float r,g,b;
    private WorldController world;
    
    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();
        myHalo = new SerializedObject(GetComponent("Halo"));
        haloColor = myHalo.FindProperty("m_Color").colorValue;

    }

    // Update is called once per frame
    void Update()
    {
        r = haloColor.r * Mathf.Abs(Mathf.Sin(pulseRate*Time.time));
        g = haloColor.g * Mathf.Abs(Mathf.Sin(pulseRate*Time.time));
        b = haloColor.b * Mathf.Abs(Mathf.Sin(pulseRate*Time.time));
        myHalo.FindProperty("m_Color").colorValue = new Color(r,g,b,1f);
        myHalo.ApplyModifiedProperties();
    }

    void OnMouseDown()
    {
        if(world.play)
        {
            Destroy(this.gameObject);
        }
        
    }
}
