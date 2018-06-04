﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_LightAutoOn : MonoBehaviour {

	public float fadeLimit = 1.0f;
	public float fadePassed = 0.0f;
	public float fadeSpeed = 1.0f;
	public float maxIntensity = 0.8f;
	public bool done = false;
	public float maxTime = 0.0f;	// Leave 0 if not in use
	public float timePassed = 0.0f;

    public float startDelay = 0.0f;
    float myTimePassed = 0.0f;
    float initialTime;

    void Start()
    {
        initialTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {

        myTimePassed += Time.time - initialTime;
        if (myTimePassed < startDelay)
            return;

		if (maxTime != 0.0 && !done) {
            timePassed += Time.time * Time.deltaTime;
			if (maxTime <= timePassed) {
				done = true;
			}
		}
		if (!done)
        {
            fadePassed += Time.deltaTime;
			if (fadePassed >= fadeLimit) {
				FadeIn ();
			}
		}
	}

	void FadeIn(){
		GetComponent<Light> ().intensity = Mathf.Clamp (GetComponent<Light> ().intensity + (Time.deltaTime * fadeSpeed), 0, maxIntensity);
		if (GetComponent<Light> ().intensity >= maxIntensity) {
			done = true;
		}
	}
}
