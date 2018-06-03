using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMessageDisplay : MonoBehaviour {


    static Text DisplayText;

    float displayTime = 0;
    static float timeAtStartOfMessage = 0;
    static float maxDisplayTime = 3;

    // Use this for initialization
    void Start () {
		DisplayText = GameObject.Find("Canvas/DisplayMessage").GetComponent<Text>();
        Debug.Log("Here");
    }
	
	// Update is called once per frame
	void Update () {

        displayTime = Time.time - timeAtStartOfMessage;
        if (displayTime <= maxDisplayTime)
        {
            if (DisplayText.enabled == false) fadeInText();
            DisplayText.enabled = true;
        } else
        {
            if (DisplayText.enabled == true) fadeOutText(); //wont work cause it will be destroyed immediately after
            DisplayText.enabled = false;
        }
    }

    static void fadeInText()
    {
        DisplayText.CrossFadeAlpha(2.0f, 1, false);
    }

    static void fadeOutText()
    {
        DisplayText.CrossFadeAlpha(0.0f, maxDisplayTime / 200, false);
    }

   

    public static void DisplayMessage(string message)
    {
        DisplayText.text = message;
        timeAtStartOfMessage = Time.time;
    }
    
}
