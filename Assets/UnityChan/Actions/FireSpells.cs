using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpells : MonoBehaviour
{

    bool spellLock = false;


    void Start()
    {

    }

    void Update()
    {
        GetInput();

        print("Here1");
    }

    void GetInput()
    {
        if (Input.GetButtonDown("Spell1"))
        {
            FireSpell(1);
        }
        if (Input.GetButtonDown("Spell2"))
        {
            FireSpell(2);
        }
        if (Input.GetButtonDown("Spell3"))
        {
            FireSpell(3);
        }
    }

    void FireSpell(int pos)
    {
        if (spellLock)
            return;

        print("Here");

        //Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f); // Center of viewport
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            //Do damage
        }

    }



}
