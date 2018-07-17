using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidGenerator : MonoBehaviour {
    public new WheelCollider collider;
    public GameObject skid;
    public float time=.1f;

    private RaycastHit hit;
    private Vector3 colliderCenter;
    private WheelHit groundHit;
    private float timer;
    // Use this for initialization
    void Start () {
        timer = time;
	}
	
	// Update is called once per frame
	void Update () {
        colliderCenter = collider.transform.TransformPoint(collider.center);
                
        if (Physics.Raycast(colliderCenter, -collider.transform.up, out hit, collider.suspensionDistance + collider.radius)) {
            transform.position = hit.point + (collider.transform.up * collider.radius);
        } else {
            transform.position = colliderCenter - (collider.transform.up * collider.suspensionDistance);
        }

        collider.GetGroundHit(out groundHit);

        if (Mathf.Abs(groundHit.sidewaysSlip) > .55 || Mathf.Abs(groundHit.forwardSlip) > .75) {
            timer = time;
            skid.gameObject.SetActive(true);
        } else if (Mathf.Abs(groundHit.sidewaysSlip) <= .5 && Mathf.Abs(groundHit.forwardSlip) <= .7) {
            skid.GetComponent<AudioSource>().Pause();
            if (timer <= 0) {
                skid.gameObject.SetActive(false);
                skid.GetComponent<AudioSource>().Stop();
            } else {
                timer -= Time.deltaTime;
            }
        }
    }
}
