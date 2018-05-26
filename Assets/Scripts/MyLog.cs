using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLog : MonoBehaviour {


    static string logOutput;
    public static void log(string logInput)
    {
        logOutput += logInput + "\n" + logOutput;
    }

    
    public static void printToScreen()
    {
        GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), logOutput);
    }

    public static void printString(string input)
    {
        //GUI.TextArea(new Rect(10, 10, Screen.width - 10, Screen.height - 10), input);
        print(input);
    }

}
