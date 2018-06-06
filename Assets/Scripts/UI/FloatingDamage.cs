using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour {

    public Animator animator;
    TextMesh damageText;
    Vector3 positionOfText;

    Vector3 shooterPosition;
    Vector3 hitLocation;

    float lerpedVal; //[0.1] represents distance from shooter to hit
    float magnifyer;

    void Awake()
    {
        //AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //float timeTilDestroy = clipInfo[0].clip.length;
        Destroy(gameObject, 2);
        damageText = GetComponent<TextMesh>();
        positionOfText = new Vector3(Random.Range(-.7f, .7f), 3.5f + Random.Range(-.25f, .25f), Random.Range(-.7f, .7f));

    }

    public void initFloatingDamage(Vector3 shooterPosition, Vector3 hitLocation, float damage)
    {
        this.shooterPosition = shooterPosition;
        this.hitLocation = hitLocation;
        transform.position = hitLocation + PositionOfText;
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
        transform.localScale *= 1.5f;
        positionOfText += new Vector3(0, 2.0f * magnifyer, 0);
    }
    
    public void CalculateDistanceMagnifyer()
    {
        float differenceMagnitude = (hitLocation - shooterPosition).magnitude;
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
