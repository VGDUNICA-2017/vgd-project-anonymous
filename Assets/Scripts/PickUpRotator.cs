using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRotator : MonoBehaviour {
    private ShippingController shippingController;
    public GameObject explosion;

	// Use this for initialization
	void Start () {
        shippingController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player") || other.tag.Equals("RiderPlayer")) {
            shippingController.PickUp();
            GetComponent<MeshRenderer>().enabled=false;
            explosion.SetActive(true);
            Destroy(gameObject, 2);
        }
    }
}
