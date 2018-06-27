using System.Collections.Generic;
using UnityEngine;

public class OurGameManager : MonoBehaviour
{
    public static OurGameManager Instance { get; private set; }


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