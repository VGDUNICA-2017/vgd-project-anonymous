using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isInputActive = true;

    public float rotation;
    public AudioClip[] footStepSouds;

    private Animator animator;
    private new AudioSource audio;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
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
            if (Input.GetButtonUp("Fire3")) {
                animator.SetBool("Running", false);
            }
            animator.SetFloat("Ahead", z);
            animator.SetFloat("Direction", x);
            transform.Rotate(0, x * rotation, 0);
        }
        else {
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }
        if (isInputActive && Input.GetButton("Jump")) {
            animator.SetBool("Jump", true);
        }
        else {
            animator.SetBool("Jump", false);
        }

        if (isInputActive && Input.GetButtonDown("Interact")) {
            animator.SetBool("Pizza", true);
        }
        if (Input.GetButtonUp("Interact")) {
            animator.SetBool("Pizza", false);
        }
    }

    void PlaySound(Object sound) {
        audio.clip = sound as AudioClip;
        audio.PlayOneShot(audio.clip);
    }

    void FootStepAudio() {
        // pick & play a random footstep sound from the array,
        // excluding last used sound
        int n = Random.Range(1, footStepSouds.Length);
        audio.clip = footStepSouds[n];
        audio.PlayOneShot(audio.clip);
        footStepSouds[n] = footStepSouds[0];
        footStepSouds[0] = audio.clip;
    }
}