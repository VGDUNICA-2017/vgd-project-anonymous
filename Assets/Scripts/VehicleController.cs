using System.Collections;
using UnityEngine;

public class VehicleController : MonoBehaviour {
    //Vehicle Characteristics
    public float maxMotorTorque = 200;
    public float maxForwardBrake = 400;
    public float maxBackBrake = 400;
    public float maxSteerAngle = 40;

    //Variables for player mount/dismount
    public string playertag;
    public GameObject rider;
    public Transform spawn;

    //Vehicle Physics
    public WheelCollider forwardCollider;
    public WheelCollider backCollider;
    public Transform forwardWheel;
    public Transform backWheel;
    public Transform centerOfMass;
    public GameObject lights;
    public GameObject mainLights;
    public GameObject stopLights;
    public GameObject retroLights;
    public GameObject leftArrowLights;
    public GameObject rightArrowLights;
    public float wheelRadius = 0.3f;
    public float wheelOffset = 0.1f;
    public float steerSensivity = 30;
    public float controlAngle = 25;
    public float controlOmega = 30;
    public float lowSpeed = 8;
    public float highSpeed = 25;
    public float topSpeed = 30f;
    public AudioClip engineOn;
    public AudioClip engine;
    public AudioClip engineOff;

    private GameObject player;
    private Animator animator;
    private new AudioSource audio;
    private bool isinvehicle;
    private bool playerClose;
    private WheelData[] wheels;
    private bool retro = false;
    private float pitch;

    //Information about previous FixedUpdate
    private Vector3 prevPos = new Vector3();
    private float prevAngle = 0;
    private float prevOmega = 0;
    private float speedVal = 0;
    private float prevSteer = 0f;
    private float brakeForward;
    private float brakeBack;
    
    //Class contains wheels informations
    public class WheelData {

        public WheelData(Transform transform, WheelCollider collider) {
            wheelTransform = transform;
            wheelCollider = collider;
            wheelStartPos = transform.transform.localPosition;
        }

        public Transform wheelTransform;
        public WheelCollider wheelCollider;
        public Vector3 wheelStartPos;
        public float rotation = 0f;
    }

    //Keeps track of the input
    public struct MotoInput {
        public float steer;
        public float acceleration;
        public float brakeForward;
        public float brakeBack;
    }


    void Start() {
        isinvehicle = false;

        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;

        wheels = new WheelData[2];
        wheels[0] = new WheelData(forwardWheel, forwardCollider);
        wheels[1] = new WheelData(backWheel, backCollider);

        animator = rider.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    //Update checks if player wants to exit or enter vehicle
    void Update() {
        if (playerClose == true) {
            if (isinvehicle == true) {
                Exiting();
            }
            else {
                Entering();
            }
        }
        if (isinvehicle == true) {
            UpdateLights(brakeForward, brakeBack);

            if (!audio.isPlaying) {
                audio.Play();
            }
            if (speedVal > .25) {
                pitch = speedVal / topSpeed;
                if (pitch < .5) {
                    pitch = .5f;
                }
                GetComponent<AudioSource>().pitch = pitch;
            } else {
                GetComponent<AudioSource>().pitch = 1; 
            }
        }
        
    }

    void FixedUpdate() {
        var input = new MotoInput();

        if (isinvehicle == true) {
            if (speedVal <= 1) {
                if (Input.GetAxis("Vertical") >= 0) {
                    input.acceleration = Input.GetAxis("Vertical");
                    retro = false;
                }
                if (Input.GetAxis("Vertical") < 0) {
                    input.acceleration = Input.GetAxis("Vertical");
                    retro = true;
                }
            }
            else {
                if (retro==false) {
                    if (Input.GetAxis("Vertical") > 0) {
                        input.acceleration = Input.GetAxis("Vertical");
                    }
                    if (Input.GetAxis("Vertical") <= 0) {
                        input.brakeBack = 0.3f * -Input.GetAxis("Vertical");
                        input.brakeForward = 0.7f * -Input.GetAxis("Vertical");
                    }
                }
                if (retro==true) {
                    if (Input.GetAxis("Vertical") < 0) {
                        input.acceleration = Input.GetAxis("Vertical");
                    }
                    if (Input.GetAxis("Vertical") >= 0) {
                        input.brakeBack = 0.3f * Input.GetAxis("Vertical");
                        input.brakeForward = 0.7f * Input.GetAxis("Vertical");
                    }
                }
            }
            
            input.steer = Input.GetAxis("Horizontal");

            if (Input.GetAxis("Jump")>0) {
                input.brakeBack = Input.GetAxis("Jump");
            }
            MotoMove(MotoControl(input));
            UpdateWheels();
            if (Input.GetAxis("Fire3")==1) {
                PickUp();
            }
            animator.SetFloat("Speed", speedVal);
        }
        else {
            forwardCollider.brakeTorque = maxForwardBrake;
            backCollider.brakeTorque = maxBackBrake;
        }
        
        brakeForward=input.brakeForward;
        brakeBack=input.brakeBack;
    }

    void LateUpdate() {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }

    //Probably picks up the grounded vehicle
    private void PickUp() {
        Transform transform = GetComponent<Transform>();
        transform.position = transform.position + new Vector3(-0.1f, 0.2f, 0);
        transform.rotation = new Quaternion(0, 0, 0, 1);
    }

    //Controls vehicle movement
    private MotoInput MotoControl(MotoInput input) {       
        var actualPos = transform.position;
        var speed = (actualPos - prevPos) / Time.fixedDeltaTime;
        prevPos = actualPos;

        speedVal = speed.magnitude;
        var moveForward = speed.normalized;

        var angle = Vector3.Dot(moveForward, Vector3.Cross(transform.up, new Vector3(0, 1, 0)));
        var omega = (angle - prevAngle) / Time.fixedDeltaTime;
        prevAngle = angle;
        prevOmega = omega;
        
        if (speedVal < lowSpeed) {
            float t = speedVal / lowSpeed;
            input.steer *= t * t;
            omega *= t * t;
            angle = angle * (2 - t);
        }
        if (speedVal > highSpeed) {
            float t = speedVal / highSpeed;
            if (omega * angle < 0f) {
                omega *= t;
            }
        }
        
        input.steer *= (1 - 2.5f * angle * angle);

        input.steer = 1f / (speed.sqrMagnitude + 1f) * (input.steer * steerSensivity + angle * controlAngle + omega * controlOmega);
        float steerDelta = 10 * Time.fixedDeltaTime;
        input.steer = Mathf.Clamp(input.steer, prevSteer - steerDelta, prevSteer + steerDelta);
        prevSteer = input.steer;
        
        return input;
    }

    //Moves the vehicle
    private void MotoMove(MotoInput input) {
        forwardCollider.steerAngle = Mathf.Clamp(input.steer, -1, 1) * maxSteerAngle;

        forwardCollider.brakeTorque = maxForwardBrake * input.brakeForward;
        backCollider.brakeTorque = maxBackBrake * input.brakeBack;
        backCollider.motorTorque = maxMotorTorque * input.acceleration;
    }

    private void UpdateWheels() {
        float delta = Time.fixedDeltaTime;

        foreach (WheelData w in wheels) {
            WheelHit hit;

            Vector3 localPos = w.wheelTransform.localPosition;
            if (w.wheelCollider.GetGroundHit(out hit)) {
                localPos.y -= Vector3.Dot(w.wheelTransform.position - hit.point, transform.up) - wheelRadius;
                w.wheelTransform.localPosition = localPos;
            }
            else {
                localPos.y = w.wheelStartPos.y - wheelOffset;
            }
            w.wheelTransform.localPosition = localPos;

            w.rotation = Mathf.Repeat(w.rotation + delta * w.wheelCollider.rpm * 360.0f / 60.0f, 360f);
            w.wheelTransform.localRotation = Quaternion.Euler(w.rotation, w.wheelCollider.steerAngle, 90.0f);
        }
    }

    //Updates lights
    void UpdateLights(float brakefw, float brakeb) {
        if (Input.GetButtonDown("Lights")) {
            mainLights.SetActive(!(mainLights.activeSelf));
        }

        if (brakeb > 0 || brakefw > 0) {
            stopLights.SetActive(true);
        } else {
            stopLights.SetActive(false);
        }

        if (Input.GetAxis("Horizontal")<0) {
            leftArrowLights.SetActive(true);
        } else {
            leftArrowLights.SetActive(false);
        }

        if (Input.GetAxis("Horizontal")>0) {
            rightArrowLights.SetActive(true);
        } else {
            rightArrowLights.SetActive(false);
        }
        
         retroLights.SetActive(retro);
    }
    
    //If the player is on the vehicle he can abandon it
    void Exiting(){
        if (isinvehicle == true){
            if (Input.GetButtonDown("Interact")){
                player.transform.position = spawn.position;
                player.SetActive(true);
                rider.SetActive(false);
                lights.SetActive(false);
                isinvehicle = false;

                audio.Stop();
                audio.pitch = 1;
                audio.clip = engineOff;
                audio.PlayOneShot(audio.clip);
            }
        }
    }

    //If the player is near the vehicle he can mount it
    void Entering(){
        if (playerClose == true && isinvehicle == false) {
            if (Input.GetButtonDown("Interact")){
                player.SetActive(false);
                rider.SetActive(true);
                lights.SetActive(true);

                audio.pitch = 1;
                audio.clip = engineOn;
                audio.PlayOneShot(audio.clip);
                audio.clip = engine;

                isinvehicle = true;                
            }
        }
    }

    //Knows if player is near
    public void OnTriggerEnter(Collider other){
        if (other.tag == playertag){
            playerClose = true;
            player = other.gameObject;
        }
    }

    //Knows when player got too far
    public void OnTriggerExit(Collider other){
        if (other.tag == playertag){
            playerClose = false; //player out of range
            player = null;
        }
    }

    //Eliminare
    void OnGUI() {
        GUI.color = Color.black;
        var area = new Rect(0, 0, 100, 250);
        GUI.Label(area, speedVal.ToString("f1") + " m/s" + "\nangle = " + prevAngle.ToString("f3") + "\nangle' = " + prevOmega.ToString("f3") + "\nfwbrake' = " + brakeForward.ToString("f3") + "\nbkbrake' = " + brakeBack.ToString("f3"));
    }
}