using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*Handles UI for user controlled manabar*/
public class ManaBar : Photon.MonoBehaviour {

    public Slider manaBarSlider = null;  //reference for slider
    public Text manaText;   //reference for text

    float currentMana, totalMana = 1000;
    float manaRegenerationWaitTime = 3.0f;
    float regenerationRate = 0.15f;

    private Coroutine regenerateManaRoutine;

    void Start()
    {
        /*setting enabled to false is not enough, bar still apears must setactive to false*/
        if (!photonView.isMine)
        {
            manaBarSlider.gameObject.SetActive(false);
            return;
        }

        manaBarSlider.maxValue = totalMana;
        manaBarSlider.value = totalMana;
        currentMana = totalMana;
    }
    
    public void burnMana(float manaCost)
    {
        currentMana -= manaCost;
        if (currentMana < 0)
            currentMana = 0;
        manaBarSlider.value = currentMana;
        Debug.Log("burning mana: " + manaBarSlider.value);
        regenerateMana();
    }

    public void AddMana(float mana)
    {
        currentMana += mana;
        if (currentMana > 1000)
            currentMana = 1000;
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

    public float CurrentMana
    {
        get { return currentMana; }
    }

    public float TotalMana
    {
        get { return totalMana; }
    }

    public void SetTotalMana(float newTotalMana)
    {
        totalMana = newTotalMana;
    }

}
