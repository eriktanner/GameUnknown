using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/*UI for player healthbars. Handles both upper elft health bar and floating health bars*/
public class HealthBar : Photon.MonoBehaviour, IHaveHealth {

    public Slider healthBarSlider;  //reference for slider

    public float MaxHealth { get { return 1000.0f; } set { MaxHealth = value; } }
    public float CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    float currentHealth { get; set; }

    float HealthRegenerationWaitTime = 5.0f;
    float regenerationRate = 0.015f;

    private Coroutine regenerateHealthRoutine;

    GameObject localPlayer;


    

    void Start()
    {
        if (photonView.isMine)
        {
            healthBarSlider.gameObject.SetActive(false);
            healthBarSlider = GameObject.Find("Canvas/HealthBar").GetComponent<Slider>();
        }

        healthBarSlider.maxValue = MaxHealth;
        healthBarSlider.value = MaxHealth;
        currentHealth = MaxHealth;
        localPlayer = PlayerManager.LocalPlayer;
    }

    void Update()
    {
        OrientAndSizeEnemyHealthBars();
    }

    void OrientAndSizeEnemyHealthBars()
    {
        if (!photonView.isMine)
        {   //Looks away for some reason (makes enemy healthbars appear straight)
            healthBarSlider.transform.LookAt(2 * transform.position - localPlayer.transform.position + new Vector3(0, 3, 0));

            Vector3 enemyPosition = gameObject.transform.position;
            Vector3 playerPosition = localPlayer.transform.position;

            float scaleBar = .8f + (enemyPosition - playerPosition).magnitude/40;
            scaleBar = Mathf.Clamp(scaleBar, 1, 2);

            healthBarSlider.transform.localScale = new Vector3(scaleBar - .5f, scaleBar + .5f,scaleBar);

        }
    }

    /*For server to calculate would-be health to then send over the network*/
    public float CalculateTakeDamage(float damage)
    {
        float temp = currentHealth - damage;
        return temp;
    }

    /*Tell clients to set health to this amount, then start regenerating health*/
    public void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
        healthBarSlider.value = currentHealth;

        if (currentHealth > MaxHealth)
            currentHealth = MaxHealth;

        regenerateHealth();
        
    }

    

    /*Syncrhonize health bar*/
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(healthBarSlider.value);
        } else
        {
            healthBarSlider.value = (float) stream.ReceiveNext();
        }
    }

    /*Waits a predetermined set amount of time, then regenerates health*/
    public void regenerateHealth()
    {
        if (regenerateHealthRoutine != null)
            StopCoroutine(regenerateHealthRoutine);
        regenerateHealthRoutine = StartCoroutine(waitAndRegenerateHealth());
    }

    private IEnumerator waitAndRegenerateHealth()
    {
        yield return new WaitForSeconds(HealthRegenerationWaitTime);
        float progress = currentHealth / MaxHealth;
        while (progress <= 1.0f)
        {
            currentHealth = Mathf.Lerp(0, MaxHealth, progress);
            healthBarSlider.value = currentHealth;

            progress += regenerationRate * Time.deltaTime;
            yield return null;
        }
    }


}
