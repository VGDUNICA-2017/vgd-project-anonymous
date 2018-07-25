using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Pause : MonoBehaviour {
    public GameObject gui;
    public GameObject pauseGui;

    void Update() {
        if (Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
            pauseGui.SetActive(false);
            gui.SetActive(true);
            Cursor.visible = false;
            foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>()) {
                audios.UnPause();
            }
            GameObject.FindObjectOfType<MusicPlayer>().paused = false;
        }
        else {
            Time.timeScale = 0f;
            gui.SetActive(false);
            pauseGui.SetActive(true);
            Cursor.visible = true;
            GameObject.FindObjectOfType<MusicPlayer>().paused = true;
            foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>()) {
                audios.Pause();
            }
        }
    }

    public void SaveGame() {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
               
        Debug.Log("Game Saved");
    }

    private Save CreateSaveGameObject() {
        Save save = new Save();
        ShippingController shipping = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();
        EnhancementController vehicle= GameObject.FindGameObjectWithTag("Workshop").GetComponent<EnhancementController>();

        save.level = shipping.GetLevel();
        save.coins=shipping.GetCoins();

        save.engineLevel = vehicle.GetEngineLevel();
        save.brakeLevel = vehicle.GetBrakeLevel();
        save.steerLevel = vehicle.GetSteerLevel();

        return save;
    }

    public void LoadGame() {
        ShippingController shipping = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();
        EnhancementController vehicle = GameObject.FindGameObjectWithTag("Workshop").GetComponent<EnhancementController>();
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            
            shipping.SetLevel(save.level);
            shipping.SetCoins(save.coins);

             vehicle.SetEngineLevel(save.engineLevel);
             vehicle.SetBrakeLevel(save.brakeLevel);
             vehicle.SetSteerLevel(save.steerLevel);

            Debug.Log("Game Loaded");
        }
        else {
            shipping.SetLevel(0);
            shipping.SetCoins(0);
            vehicle.SetEngineLevel(1);
            vehicle.SetBrakeLevel(1);
            vehicle.SetSteerLevel(1);
            Debug.Log("No game saved!");
        }
    }

    public void QuitGame() {
        Application.Quit();
    }
}
