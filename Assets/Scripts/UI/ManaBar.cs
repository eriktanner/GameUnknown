using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*Handles UI for user controlled manabar*/
public class ManaBar : Photon.MonoBehaviour, IHaveMana {

    public Slider manaBarSlider = null;  //reference for slider

    public float MaxMana { get { return 1000.0f; } set { MaxMana = value; } }
    public float CurrentMana { get { return currentMana; } set { currentMana = value; } }
    float currentMana { get; set; }

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

        manaBarSlider.maxValue = MaxMana;
        manaBarSlider.value = MaxMana;
        currentMana = MaxMana;
    }
    
    public void burnMana(float manaCost)
    {
        currentMana -= manaCost;
        if (currentMana < 0)
            currentMana = 0;
        manaBarSlider.value = currentMana;
        regenerateMana();
    }

    public void AddMana(float mana)
    {
        currentMana += mana;
        if (currentMana > MaxMana)
            currentMana = MaxMana;
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
        float progress = currentMana/MaxMana;
        while (progress <= 1.0f)
        {
            currentMana = Mathf.Lerp(0, MaxMana, progress);
            manaBarSlider.value = currentMana;
            
            progress += regenerationRate * Time.deltaTime;
            yield return null;
        }
    }


}
