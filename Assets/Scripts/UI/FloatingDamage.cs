
using UnityEngine;
using UnityEngine.UI;


/*Instances of the floating damage text display above enemies hit by spells. Handles things such as
 text size and positioning*/
public class FloatingDamage : MonoBehaviour {

    public Animator animator;
    TextMesh damageText;
    Vector3 positionOfText;

    Vector3 shooterPosition;
    Vector3 hitLocation;

    GameObject Shooter;

    float lerpedVal; //[0.1] represents distance from shooter to hit
    float magnifyer;

    void Awake()
    {
        Destroy(gameObject, 1.7f);
        damageText = GetComponent<TextMesh>();
        positionOfText = new Vector3(Random.Range(-.7f, .7f), 3.5f + Random.Range(-.25f, .25f), Random.Range(-.7f, .7f));

    }

    void Update()
    {
        transform.LookAt(2 * hitLocation - Shooter.transform.position + new Vector3(0, 3, 0)); //Orients it the right way
    }

    public void initFloatingDamage(Vector3 hitLocation, float damage, bool criticalDamage, int shotBy)
    {
        Shooter = (GameObject)PhotonPlayer.Find(shotBy).TagObject;
        this.hitLocation = hitLocation;
        transform.position = hitLocation + PositionOfText;

        if (criticalDamage)
            FontSizeIncrease();

        SetText(damage);
        CalculateDistanceMagnifyer();
    }

    public void SetText(float damage)
    {
        int intDamage = (int) damage;
        damageText.text = intDamage.ToString();
    }

    public void FontSizeIncrease()
    {
        transform.localScale *= 1.6f;
        positionOfText += new Vector3(0, 2.0f * magnifyer, 0);
    }
    
    public void CalculateDistanceMagnifyer()
    {
        float differenceMagnitude = (hitLocation - Shooter.transform.position).magnitude;
        differenceMagnitude = Mathf.Clamp(differenceMagnitude, 0, 40);

        lerpedVal = Mathf.Lerp(0, 1, differenceMagnitude/40);

        magnifyer = 1 + lerpedVal * lerpedVal * lerpedVal * lerpedVal * lerpedVal * lerpedVal;

        transform.localScale *= magnifyer;
        positionOfText += new Vector3(0, magnifyer, 0);
    }

    public Vector3 PositionOfText
    {
        get { return positionOfText; }
    }




}
