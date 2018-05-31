using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*This class, I believe, will need to implement networking in the future
 -- I believe we will do all the calculating on the other machines, and just 
 request total and current health, so that most of this code will disapear
 -- Only send packet if we need it for display purposes, otherwise dont send*/
public class EnemyHealthBar : MonoBehaviour {

    public Slider healthBarSlider = null;  //reference for slider
    public Text healthText;   //reference for text

    float currentHealth, totalHealth = 100;
    float HealthRegenerationWaitTime = 5.0f;
    float regenerationRate = 0.05f;

    private Coroutine regenerateHealthRoutine;

    void Start()
    {
        healthBarSlider = GameObject.Find("Canvas/EnemyHealthBar").GetComponent<Slider>();
        healthBarSlider.maxValue = totalHealth;
        healthBarSlider.value = totalHealth;
        currentHealth = totalHealth;
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        healthBarSlider.value = currentHealth;
    }

    /*Waits a predetermined set amount of time, then regenerates mana*/
    public void regenerateHealth()
    {
        if (regenerateHealthRoutine != null)
            StopCoroutine(regenerateHealthRoutine);
        regenerateHealthRoutine = StartCoroutine(waitAndRegenerateHealth());
    }

    private IEnumerator waitAndRegenerateHealth()
    {
        yield return new WaitForSeconds(HealthRegenerationWaitTime);
        float progress = currentHealth / totalHealth;
        while (progress <= 1.0f)
        {
            currentHealth = Mathf.Lerp(0, totalHealth, progress);
            healthBarSlider.value = currentHealth;

            progress += regenerationRate * Time.deltaTime;
            yield return null;
        }
    }



    public float GetTotalHealth
    {
        get { return totalHealth; }
    }

    public void SetTotalHealth(float newTotalHealth)
    {
        totalHealth = newTotalHealth;
    }



}
