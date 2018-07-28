using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shipped : MonoBehaviour {
    public ShippingController shipping;
    public Text interaction;

    private void OnTriggerEnter(Collider other) {
        if ((other.tag.Equals("Player") || (other.tag.Equals("RiderPlayer"))) && shipping.InDelivery()==true) {
            interaction.text = "Press 'E' to deliver pizza";
        }
    }

    private void OnTriggerStay(Collider other) {
        if (shipping.InDelivery() == true && (other.tag.Equals("Player") || (other.tag.Equals("RiderPlayer"))) && Input.GetButtonDown("Interact")) {
            interaction.text = "";
            shipping.ShippingCompleted();
        }
    }

    private void OnTriggerExit(Collider other) {
        interaction.text = "";
    }
}
