using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
    public Transform player;
    public Transform bikePlayer;
    public Transform objective;
    public Transform arrow;

    private Vector3 direction;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bikePlayer = GameObject.FindGameObjectWithTag("RiderPlayer").GetComponent<Transform>();
    }

    void Update() {
        Transform active=player;

        if (player.gameObject.activeSelf == false) {
            active = bikePlayer;
        }
        Vector3 perp;

        direction.z = active.eulerAngles.y;
        transform.localEulerAngles = direction;
        
        direction =objective.position - active.position;
        perp = Vector3.Cross(active.forward, direction);

        Vector3 p1 = Project(direction);
        Vector3 p2 = Project(active.forward);

        direction.x = 0;
        direction.y = 0;
        if ((Vector3.Dot(perp, active.up)) >= 0f) {
            direction.z = -Vector3.Angle(p1, p2);
        } else{
            direction.z = Vector3.Angle(p1, p2);
        }
        
        arrow.eulerAngles = direction;


    }

    public Vector3 Project(Vector3 v) {
        return v - (Vector3.Dot(v, Vector3.up) / Vector3.Dot(Vector3.up, Vector3.up)) * Vector3.up;
    }
}
