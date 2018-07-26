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
    public EnhancementController enhancementController;
    public Text interaction;

    void Update() {
        if ((Input.GetButtonDown("Pause") || Input.GetKeyDown(KeyCode.Escape))) {
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
            GameObject.FindObjectOfType<VehicleController>().paused = false;
        }
        else {
            Time.timeScale = 0f;
            gui.SetActive(false);
            pauseGui.SetActive(true);
            Cursor.visible = true;
            GameObject.FindObjectOfType<MusicPlayer>().paused = true;
            GameObject.FindObjectOfType<VehicleController>().paused = true;
            foreach (AudioSource audios in GameObject.FindObjectsOfType<AudioSource>()) {
                audios.Pause();
            }
        }
    }

    public void SaveGame() {
        ShippingController shipping = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShippingController>();

        if (shipping.InDelivery() == false) {
            Save save = CreateSaveGameObject();
            
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, save);
            file.Close();

            StartCoroutine(ShowMessage(0));
        } else {
            StartCoroutine(ShowMessage(3));
        }
        
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

            StartCoroutine(ShowMessage(1));

            if (shipping.InDelivery() == true) {
                shipping.EndDelivery();
            }
        }
        else {
            shipping.SetLevel(0);
            shipping.SetCoins(0);
            vehicle.SetEngineLevel(0);
            vehicle.SetBrakeLevel(0);
            vehicle.SetSteerLevel(0);
            StartCoroutine(ShowMessage(2));
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    IEnumerator ShowMessage(int i) {
        switch (i) {
            case 0:
                interaction.text = "Game Saved";
                break;
            case 1:
                interaction.text = "Game loaded";
                break;
            case 2:
                interaction.text = "No Game Saved";
                break;
            case 3:
                interaction.text = "Cannot save in delivery";
                break;
        }
        yield return new WaitForSeconds(2);
        interaction.text = "";
    }
}
