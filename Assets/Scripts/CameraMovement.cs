using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    public Transform cameraOrbit;
    public Transform target;
    public float cameraSpeed=5;

    void Start() {
        cameraOrbit.position = target.position;
    }

    void Update() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0) {
            cameraOrbit.transform.localScale = cameraOrbit.transform.localScale * (1f - scroll);
        }

        float horizontal = cameraSpeed * Input.GetAxis("Mouse X");
        float vertical = cameraSpeed * Input.GetAxis("Mouse Y");

        target.rotation = Quaternion.Euler(0, horizontal, 0);

        if (cameraOrbit.transform.eulerAngles.z + vertical <= 0.1f || cameraOrbit.transform.eulerAngles.z + vertical >= 89.9f) {
            vertical = 0;
        }

        cameraOrbit.transform.eulerAngles = new Vector3(cameraOrbit.transform.eulerAngles.x, cameraOrbit.transform.eulerAngles.y + horizontal, cameraOrbit.transform.eulerAngles.z + vertical);

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
        transform.LookAt(target.position);
    }
}