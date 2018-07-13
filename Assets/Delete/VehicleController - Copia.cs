using System.Collections;
using UnityEngine;
    
public class VehicleController2 : MonoBehaviour{
    public float maxMotorTorque;
    public float maxBrakeTorque;
    public float maxSteeringAngle;
    public float handbrakeTorque;
    public string playertag;
    public Animator animator;
    public GameObject rider;
    public Transform spawn;
    public WheelCollider frontWheelCollider;
    public WheelCollider backWheelCollider;

    private GameObject player;
    private bool isinvehicle;
    private bool playerClose;

    void Start(){
        isinvehicle = false;
        animator = GetComponent<Animator>();
    }


    void Update() {
        if (playerClose == true) {
            if (isinvehicle == true) {
                Exiting();
            }
            else {
                Entering();
            }
        }
    }

    void FixedUpdate() {
        if (isinvehicle == true) {
            animator.SetFloat("Speed", Input.GetAxis("Vertical"));
            animator.SetFloat("Steer", Input.GetAxis("Horizontal"));
            float throttle = Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            float handbrake = handbrakeTorque * Input.GetAxis("Jump");

            if (throttle < 0) {
                throttle = maxBrakeTorque * Input.GetAxis("Vertical");
            }
            else {
                throttle = maxMotorTorque * Input.GetAxis("Vertical");
            }

            backWheelCollider.motorTorque = throttle;
            backWheelCollider.brakeTorque = handbrake;
        }
        else {
            animator.SetFloat("Speed", 0f);
            animator.SetFloat("Steer", 0f);
            backWheelCollider.brakeTorque = handbrakeTorque;
        }
    }

    void Exiting(){
        if (isinvehicle == true){
            if (Input.GetButtonDown("Interact")){
                player.transform.position = spawn.position;
                player.SetActive(true);
                rider.SetActive(false);
                Debug.Log(isinvehicle);
                isinvehicle = false;
                animator.SetBool("Driving", false);
            }
        }
    }

    void Entering(){
        if (playerClose == true && isinvehicle == false) {
            if (Input.GetButtonDown("Interact")){
                player.SetActive(false);
                rider.SetActive(true);
                isinvehicle = true;
                animator.SetBool("Driving", true);
            }
        }
    }

    public void OnTriggerEnter(Collider other){
        if (other.tag == playertag){
            playerClose = true;
            player = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other){
        if (other.tag == playertag){
            playerClose = false; //player out of range
            player = null;
        }
    }
}