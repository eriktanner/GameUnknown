using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour {

    public Slider healthBarSlider;  //reference for slider
    public Text healthText;   //reference for text

    float totalHealth = 1000;
    [SyncVar(hook = "OnChangeHealth")]
    float currentHealth;
    float HealthRegenerationWaitTime = 5.0f;
    float regenerationRate = 0.05f;

    private Coroutine regenerateHealthRoutine;

    GameObject localPlayer;

    void Start()
    {
        if (isLocalPlayer)
        {
            healthBarSlider.gameObject.SetActive(false);
            healthBarSlider = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
        }

        healthBarSlider.maxValue = totalHealth;
        healthBarSlider.value = totalHealth;
        currentHealth = totalHealth;
        localPlayer = GameObject.Find("Managers/NetworkManager").GetComponent<OurNetworkManager>().client.connection.playerControllers[0].gameObject;


    }

    void Update()
    {
        OrientAndSizeEnemyHealthBars();
    }

    void OrientAndSizeEnemyHealthBars()
    {
        if (!isLocalPlayer)
        {   //Looks away for some reason (makes enemy healthbars appear straight)
            healthBarSlider.transform.LookAt(2 * transform.position - localPlayer.transform.position + new Vector3(0, 3, 0));

            Vector3 enemyPosition = gameObject.transform.position;
            Vector3 playerPosition = localPlayer.transform.position;

            float scaleBar = .8f + (enemyPosition - playerPosition).magnitude/40;
            scaleBar = Mathf.Clamp(scaleBar, 1, 2);

            healthBarSlider.transform.localScale = new Vector3(scaleBar - .5f, scaleBar + .5f,scaleBar);

        }
    }

    public void takeDamage(float damage)
    {
        currentHealth -= damage;
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

    void OnChangeHealth(float newHealth)
    {
        healthBarSlider.value = newHealth;
    }

    
    [Command]
    public void CmdCollisionDamagePlayer(float damage, string playerName)
    {
        GameObject hitPlayer = OurGameManager.GetPlayerGameObject(playerName);
        HealthBar hitPlayerHealthBar = hitPlayer.GetComponent<HealthBar>();
        hitPlayerHealthBar.takeDamage(damage);
        hitPlayerHealthBar.regenerateHealth();
    }

}
