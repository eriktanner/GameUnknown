using System.Collections.Generic;
using UnityEngine;

/*Manages game-related events*/
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    void Start()
    {
        EnsureSingleton();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

}