using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
    private AudioSource audio;
    private Object[] myMusic; // declare this as Object array
    private bool inPlay = false;

    void Start() {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (inPlay == true && !audio.isPlaying) {
            playRandomMusic();
        }
        if (Input.GetKeyDown("1")) {
            myMusic= Resources.LoadAll("Music/1", typeof(AudioClip));
            audio.clip = myMusic[0] as AudioClip;
            inPlay = true;
            audio.Play();
        }
        if (Input.GetKeyDown("2")) {
            myMusic = Resources.LoadAll("Music/2", typeof(AudioClip));
            audio.clip = myMusic[0] as AudioClip;
            inPlay = true;
            audio.Play();
        }
        if (Input.GetKeyDown("3")) {
            myMusic = Resources.LoadAll("Music/3", typeof(AudioClip));
            audio.clip = myMusic[0] as AudioClip;
            inPlay = true;
            audio.Play();
        }
        if (inPlay == true && Input.GetKeyDown("0")) {
            inPlay = false;
            audio.Stop();
        }
        if (inPlay == true && Input.GetKeyDown("n")) {
            playRandomMusic();
        }
    }

    void playRandomMusic() {
        audio.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        audio.Play();
    }
}