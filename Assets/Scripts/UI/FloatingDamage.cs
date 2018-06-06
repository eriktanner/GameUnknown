using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour {

    public Animator animator;
    TextMesh damageText;
    Vector3 positionOfText;

    void Awake()
    {
        //AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        //float timeTilDestroy = clipInfo[0].clip.length;
        Destroy(gameObject, 2);
        damageText = GetComponent<TextMesh>();
        positionOfText = new Vector3(Random.Range(-.5f, .5f), 3.0f + Random.Range(-.25f, .25f), Random.Range(-.5f, .5f));

    }

    public void SetText(float damage)
    {
        int intDamage = (int) damage;
        damageText.text = intDamage.ToString();
    }

    public void FontSizeIncrease()
    {
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        positionOfText += new Vector3(0, 1, 0);
    }


    public Vector3 PositionOfText
    {
        get { return positionOfText; }
    }




}
