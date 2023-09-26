using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    public GameObject world;
    public GameObject playButtonObject;
    private bool play = true;
    
    // Start is called before the first frame update
    void Start()
    {
        playButtonObject.GetComponent<Image>().color = Color.green;
        
        //playButton = playButtonObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseUnpause()
    {
        Image img = playButtonObject.GetComponent<Image>();
        if(play)
            img.color = Color.red;
        else
            img.color = Color.green;
        play = !play;
    }
}
