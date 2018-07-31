using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnhancementController : MonoBehaviour {
    public ShippingController shippingController;
    public GameObject MotoVehicle;
    public GameObject player;
    public Text interaction;
    public Text motoInteraction;
    public Text error;
    public Text coins;
    public GameObject gui;
    public GameObject enhanceGui;
    public GameObject enhCamera;
    public Slider engineSlider;
    public Slider brakeSlider;
    public Slider steerSlider;

    private int engineLevel=0;
    private int brakeLevel=0;
    private int steerLevel=0;

    private bool isIn = false;
    private VehicleController vehicle;

    public void Start() {
        shippingController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();
        MotoVehicle = GameObject.FindGameObjectWithTag("RiderPlayer");

        player = GameObject.FindGameObjectWithTag("Player");
        vehicle = MotoVehicle.GetComponent<VehicleController>();
        vehicle.interaction = motoInteraction;
        vehicle.gameInteraction = interaction;

        foreach (Text t in GameObject.FindObjectsOfType<Text>()) {
            if (t.name.Equals("Interaction")){
                interaction = t;
            }
        }
        vehicle.maxMotorTorque += vehicle.maxMotorTorque * (0.1f * (engineLevel));
        vehicle.maxForwardBrake += vehicle.maxForwardBrake * (0.1f * (brakeLevel));
        vehicle.maxBackBrake += vehicle.maxBackBrake * (0.1f * (brakeLevel));
        vehicle.maxSteerAngle += vehicle.maxSteerAngle * (0.05f * (engineLevel));

        engineSlider.value = engineLevel;
        brakeSlider.value = brakeLevel;
        steerSlider.value = steerLevel;
    }

    public void EngineEnhance() {
        if (engineLevel<3 && shippingController.GetCoins() >= (5 * (engineLevel+1))) {
            shippingController.Spend(5 * (engineLevel+1));
            vehicle.maxMotorTorque += vehicle.maxMotorTorque*0.1f;
            engineLevel++;
            engineSlider.value = engineLevel;
            shippingController.UpdateCoins();
            coins.text = shippingController.GetCoins().ToString();
        }
        else {
            if (engineLevel >= 3) {
                StartCoroutine(ShowMessage(1));
            }
            else {
                StartCoroutine(ShowMessage(0));
            }
        }
    }

    public void BrakeEnhance() {
        if (brakeLevel < 3 && shippingController.GetCoins() >= (5 * (brakeLevel+1))) {
            shippingController.Spend(5 * (brakeLevel+1));
            vehicle.maxForwardBrake += vehicle.maxForwardBrake*0.1f;
            vehicle.maxBackBrake += vehicle.maxBackBrake*0.1f;
            brakeLevel++;
            brakeSlider.value = brakeLevel;
            shippingController.UpdateCoins();
            coins.text = shippingController.GetCoins().ToString();
        }
        else {
            if (brakeLevel >= 3) {
                StartCoroutine(ShowMessage(1));
            } else {
                StartCoroutine(ShowMessage(0));
            }
        }
    }

    public void SteerEnhance() {
        if (steerLevel < 3 && shippingController.GetCoins() >= (5 * (steerLevel+1))) {
            shippingController.Spend(5 * (steerLevel+1));
            vehicle.maxSteerAngle += vehicle.maxSteerAngle*0.05f;
            steerLevel++;
            steerSlider.value = steerLevel;
            shippingController.UpdateCoins();
            coins.text = shippingController.GetCoins().ToString();

        }
        else {
            if (steerLevel >= 3) {
                StartCoroutine(ShowMessage(1));
            }
            else {
                StartCoroutine(ShowMessage(0));
            }
        }
    }

    public int GetEngineLevel() {
        return engineLevel;
    }

    public int GetBrakeLevel() {
        return brakeLevel;
    }

    public int GetSteerLevel() {
        return steerLevel;
    }

    public bool InEnhancement() {
        return isIn;
    }

    public void SetEngineLevel(int l) {
        engineLevel=l;
    }

    public void SetBrakeLevel(int l) {
        brakeLevel=l;
    }

    public void SetSteerLevel(int l) {
        steerLevel =l;
    }

    private void OnTriggerEnter(Collider other) {
        if (shippingController.InDelivery() == false) {
            interaction.text = "Press 'E' to enter workshop";
        }
    }

    private void Entering() {
        gui.SetActive(false);
        enhanceGui.SetActive(true);
        enhCamera.SetActive(true);
        Cursor.visible = true;
        player.SetActive(false);
        isIn = true;
        coins.text = shippingController.GetCoins().ToString();
        foreach (AudioSource audio in MotoVehicle.GetComponents<AudioSource>()) {
            audio.Pause();
        }
    }

    IEnumerator ShowMessage(int i) {
        if (i == 0) {
            error.text = "Not enough money";
        } else {
            error.text = "Level Max";
        }
        yield return new WaitForSeconds(2);
        error.text = "";
    }

    public void Exiting() {
        gui.SetActive(true);
        enhanceGui.SetActive(false);
        enhCamera.SetActive(false);
        Cursor.visible = false;
        player.SetActive(true);
        isIn = false;
        foreach (AudioSource audio in MotoVehicle.GetComponents<AudioSource>()) {
            audio.UnPause();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (shippingController.InDelivery() == false && Input.GetButtonDown("Interact") && other.tag.Equals("Player")) {
            Entering();
        }
    }

    private void OnTriggerExit(Collider other) {
        interaction.text = "";
    }
}

