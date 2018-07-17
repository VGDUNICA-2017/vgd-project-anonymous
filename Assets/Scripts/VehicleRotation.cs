using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleRotation : MonoBehaviour {
    public float rotationSpeed=10;
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);
	}
}
