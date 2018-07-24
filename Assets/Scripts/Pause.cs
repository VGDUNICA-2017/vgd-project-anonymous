using System;
using UnityEngine;

public class Pause : MonoBehaviour {
    public GameObject gui;
    public GameObject pauseGui;

    void Update() {
        if (Input.GetButtonDown("Pause"))
            TogglePause();
    }

    public void TogglePause() {
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
            pauseGui.SetActive(false);
            gui.SetActive(true);
        }
        else {
            Time.timeScale = 0f;
            gui.SetActive(false);
            pauseGui.SetActive(true);
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
