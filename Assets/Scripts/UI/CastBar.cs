﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Creates the UI for the player's castbar*/
public class CastBar : MonoBehaviour {

    public Slider castBarSlider = null;  //reference for slider
    public Text castText;   //reference for text
    private Coroutine castBarRoutine;


    void Start()
    {
        castBarSlider = GameObject.Find("Canvas/CastBar").GetComponent<Slider>();
        //castBarSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("CancelCast"))
        {
            if (castBarRoutine != null)
            {
                StopCoroutine(castBarRoutine);
                castBarSlider.gameObject.SetActive(false);
            }
        }

    }

    /*Displays the UI for a spell being casted*/
    public void CastSpellUI(SpellStats spell)
    {
        castBarSlider.value = 0;

        castBarSlider.gameObject.SetActive(true);
        castBarRoutine = StartCoroutine(FillBar(spell));
        //castBarSlider.gameObject.SetActive(false);
    }

    /*Fills the cast bar UI, showing a spell is being casted*/
    private IEnumerator FillBar(SpellStats spell)
    {

        castBarSlider.gameObject.SetActive(true);
        float rate = 1.0f / spell.castTime;
        float progress = 0.0f;
        while (progress <= 1.0f)
        {
            castBarSlider.value = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }


        castBarSlider.gameObject.SetActive(false);
    }



    
}
