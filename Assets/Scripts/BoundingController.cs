using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingController : MonoBehaviour {
    public GameObject wall;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            wall.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            wall.SetActive(false);
        }
    }
}
