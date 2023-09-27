using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBuilder : MonoBehaviour
{
    
    public Sprite sprite1;
    public Sprite sprite2;

    private SpriteBuilder spriteBuilder;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteBuilder = new SpriteBuilder(sprite1);
        spriteBuilder.Add(sprite2,20,45,0.5f,true);

        GetComponent<SpriteRenderer>().sprite = spriteBuilder.Sprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
