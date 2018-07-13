using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isInputActive = true;
    private Animator animator;
    private new Transform transform;
    public float rotation;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (isInputActive && (x != 0 || z != 0)) {
            animator.SetBool("Moving", true);
            if (isInputActive && Input.GetButton("Fire3")) {
                animator.SetBool("Running", true);
            }
            if (Input.GetButtonUp("Fire3")){
                animator.SetBool("Running", false);
            }
            animator.SetFloat("Ahead", z);
            animator.SetFloat("Direction", x);
            transform.Rotate(new Vector3(0, x*rotation, 0));
        }else {
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }
        if (isInputActive && Input.GetButton("Jump")) {
            animator.SetBool("Jump", true);
        } else {
            animator.SetBool("Jump", false);
        }

        if (isInputActive && Input.GetButtonDown("Interact")) {
            animator.SetBool("Pizza", true);
        }
        if (Input.GetButtonUp("Interact")) {
            animator.SetBool("Pizza", false);
        }

    }
}