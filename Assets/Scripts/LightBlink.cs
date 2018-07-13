using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlink : MonoBehaviour {
    public float delay = 2;
    public float onTime = 0.5f;

    private float timer;
    private new Light light;

    // Use this for initialization
    void Start () {
        timer = onTime;
        light = GetComponent<Light>();
	}    

    void Update() {

        timer -= Time.deltaTime;

        if (timer < 0) {
            if (light.enabled == true) {
                light.enabled = false;
                timer = delay;
            }
            else if (light.enabled == false) {
                light.enabled = true;
                timer = onTime;
            }
        }
    }
}
