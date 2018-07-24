using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnhancementController : MonoBehaviour {
    public ShippingController shippingController;
    public VehicleController vehicle;
    public PlayerController player;
    public Text interaction;
    public Text error;
    public LoadOnClick load;

    public Slider loadingBar;
    public GameObject loadingImage;

    private int engineLevel=1;
    private int brakeLevel=1;
    private int steerLevel=1;
    private Transform bikeTransform;

    public void Start() {
        shippingController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();
        vehicle = GameObject.FindGameObjectWithTag("RiderPlayer").GetComponent<VehicleController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        foreach (Text t in GameObject.FindObjectsOfType<Text>()) {
            if (t.name.Equals("Interaction")){
                interaction = t;
            }
        }
    }

    public void EngineEnhance() {
        if (shippingController.GetCoins() >= (5 * engineLevel)) {
            shippingController.Spend(5 * engineLevel);
            vehicle.maxMotorTorque *= 0.05f;
            engineLevel++;
        }else {
            error.text = "Credito non sufficiente";
        }
    }

    public void BrakeEnhance() {
        if (shippingController.GetCoins() >= (5 * brakeLevel)) {
            shippingController.Spend(5 * brakeLevel);
            vehicle.maxForwardBrake *= 0.05f;
            vehicle.maxBackBrake *= 0.05f;
            brakeLevel++;
        }
        else {
            error.text = "Credito non sufficiente";
        }
    }

    public void SteerEnhance() {
        if (shippingController.GetCoins() >= (5 * steerLevel)) {
            shippingController.Spend(5 * steerLevel);
            vehicle.maxSteerAngle *= 0.05f;
            steerLevel++;
        }else {
            error.text = "Credito non sufficiente";
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (shippingController.InDelivery() == false) {
            interaction.text = "Press 'E' to enter workshop";
        }
    }

    private void OnTriggerStay(Collider other) {
        if (shippingController.InDelivery() == false && Input.GetButtonDown("Interact")) {
            loadingImage.SetActive(true);
            load.AddAsync(3);
            bikeTransform = GameObject.FindGameObjectWithTag("RiderPlayer").transform;
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("RiderPlayer"), SceneManager.GetSceneByBuildIndex(3));
            GameObject.FindGameObjectWithTag("RiderPlayer").transform.position.Set(0, 0, 0);
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(3));
        }
    }

    private void OnTriggerExit(Collider other) {
        interaction.text = "";
    }
}

