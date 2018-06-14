using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Creates the UI for the player's castbar*/
public class CastBar : MonoBehaviour {


    public static CastBar Instance { get; private set; }

    Slider castBarSlider;
    Coroutine castBarRoutine;



    void Start()
    {
        EnsureSingleton();
        castBarSlider = GameObject.Find("Canvas/CastBar").GetComponent<Slider>();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
    public void CastSpellUI(float castTime)
    {
        castBarSlider.value = 0;

        castBarSlider.gameObject.SetActive(true);
        castBarRoutine = StartCoroutine(FillBar(castTime));
    }

    /*Fills the cast bar UI, showing a spell is being casted*/
    private IEnumerator FillBar(float castTime)
    {

        castBarSlider.gameObject.SetActive(true);
        float rate = 1.0f / castTime;
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
