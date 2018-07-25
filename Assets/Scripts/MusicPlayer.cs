using UnityEngine;
 using System.Collections;
 using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {
    private new AudioSource audio;
    private Object[] myMusic; // declare this as Object array
    private Object[] myMusic1;
    private Object[] myMusic2;
    private Object[] myMusic3;
    private int inPlay = 0;
    private AsyncOperation async;

    void Start() {
        audio = GetComponent<AudioSource>();
        audio.Stop();
        myMusic1 = Resources.LoadAll("Music/1", typeof(AudioClip));
        myMusic2 = Resources.LoadAll("Music/2", typeof(AudioClip));
        myMusic3 = Resources.LoadAll("Music/3", typeof(AudioClip));
    }

    // Update is called once per frame
    void Update() {
        if (inPlay !=0 && !audio.isPlaying) {
            PlayRandomMusic();
        }
        if (Input.GetKeyDown("1") && inPlay!=1) {
            audio.Stop();
            myMusic = myMusic1;
            audio.clip = myMusic[0] as AudioClip;
            inPlay = 1;
            audio.Play();
        }
        if (Input.GetKeyDown("2") && inPlay != 2) {
            audio.Stop();
            myMusic = myMusic2;
            audio.clip = myMusic[0] as AudioClip;
            inPlay = 2;
            audio.Play();
        }
        if (Input.GetKeyDown("3") && inPlay != 3) {
            audio.Stop();
            myMusic = myMusic3;
            audio.clip = myMusic[0] as AudioClip;
            inPlay = 3;
            audio.Play();
        }
        if (Input.GetKeyDown("0") && inPlay != 0) {
            inPlay = 0;
            audio.Stop();
        }
        if (Input.GetKeyDown("n") && inPlay != 0) {
            PlayRandomMusic();
        }
    }

    void PlayRandomMusic() {
        audio.clip = myMusic[Random.Range(0, myMusic.Length)] as AudioClip;
        audio.Play();
    }
}