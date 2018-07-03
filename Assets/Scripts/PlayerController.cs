using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isInputActive = true;
    private Animator animator;
    
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (isInputActive && (x != 0 || z != 0)) {
            animator.SetBool("Moving", true);
            if (isInputActive && Input.GetButtonDown("Fire3")) {
                animator.SetBool("Running", true);
            }
            if (Input.GetButtonUp("Fire3")){
                animator.SetBool("Running", false);
            }
            animator.SetFloat("Ahead", z);
            animator.SetFloat("Direction", x);
        }else {
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }

        if (isInputActive && Input.GetButtonDown("Jump")) {
            animator.SetBool("Jump", true);
        }
        if (Input.GetButtonUp("Jump")) {
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