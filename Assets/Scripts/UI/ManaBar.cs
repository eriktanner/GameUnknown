using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour {

    public Slider manaBarSlider = null;  //reference for slider
    public Text manaText;   //reference for text

    float currentMana, totalMana = 100;
    float manaRegenerationWaitTime = 3.0f;
    float regenerationRate = 0.15f;

    private Coroutine regenerateManaRoutine;

    void Start()
    {
        //manaBarSlider = GameObject.Find("Canvas/ManaBar").GetComponent<Slider>();
        manaBarSlider.maxValue = totalMana;
        manaBarSlider.value = totalMana;
        currentMana = totalMana;
    }
    
    public void burnMana(float manaCost)
    {
        currentMana -= manaCost;
        manaBarSlider.value = currentMana;
    }

    public void AddMana(float mana)
    {
        currentMana += mana;
        manaBarSlider.value = currentMana;
    }

    /*Waits a predetermined set amount of time, then regenerates mana*/
    public void regenerateMana()
    {
        if (regenerateManaRoutine != null)
            StopCoroutine(regenerateManaRoutine);
        regenerateManaRoutine = StartCoroutine(waitAndRegenerateMana());
    }

    private IEnumerator waitAndRegenerateMana()
    {
        yield return new WaitForSeconds(manaRegenerationWaitTime);
        float progress = currentMana/totalMana;
        while (progress <= 1.0f)
        {
            currentMana = Mathf.Lerp(0, totalMana, progress);
            manaBarSlider.value = currentMana;
            
            progress += regenerationRate * Time.deltaTime;
            yield return null;
        }
    }



    public float GetTotalMana
    {
        get { return totalMana; }
    }

    public void SetTotalMana(float newTotalMana)
    {
        totalMana = newTotalMana;
    }

}
