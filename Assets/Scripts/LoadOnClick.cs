using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadOnClick : MonoBehaviour {
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public GameObject maleBikePrefab;
    public GameObject femaleBikePrefab;
    public int mainMenu=0;
    public int characterSelection=1;
    public int map=2;
    public int vehicleEnhancement=3;

    public Slider loadingBar;
    public GameObject loadingImage;
    
    private AsyncOperation async;
    private Transform bikeTransform;

    public void ClickAsync(int level) {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    public void AddAsync(int level) {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    IEnumerator LoadLevelWithBar(int level) {
        async = SceneManager.LoadSceneAsync(level);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    IEnumerator AddLevelWithBar(int level) {
        async = SceneManager.LoadSceneAsync(level,LoadSceneMode.Additive);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    IEnumerator UnloadLevelWithBar(int level) {
        async = SceneManager.UnloadSceneAsync(level);
        while (!async.isDone) {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    public void NewGame() {
        File.Delete(Application.persistentDataPath + "/gamesave.save");
        ClickAsync(characterSelection);
    }

    public void MainMenu() {
        ClickAsync(mainMenu);
    }

    public void LoadGame() {
        ClickAsync(characterSelection);
    }
    
    public void FreeMode() {
        Save save = new Save {
            level = 0,
            coins = 100,

            engineLevel = 1,
            brakeLevel = 1,
            steerLevel = 1
        };

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        ClickAsync(characterSelection);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void MaleCharacter() {
        DontDestroyOnLoad(GameObject.Instantiate(maleBikePrefab));
        DontDestroyOnLoad(GameObject.Instantiate(malePrefab));
        ClickAsync(map);
    }

    public void FemaleCharacter() {
        DontDestroyOnLoad(GameObject.Instantiate(femaleBikePrefab));
        DontDestroyOnLoad(GameObject.Instantiate(femalePrefab));
        ClickAsync(map);
    }

    public void EnhanceLevel() {
        AddAsync(vehicleEnhancement);
        bikeTransform = GameObject.FindGameObjectWithTag("RiderPlayer").transform;
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("RiderPlayer"), SceneManager.GetSceneByBuildIndex(vehicleEnhancement));
        GameObject.FindGameObjectWithTag("RiderPlayer").transform.position.Set(0, 0, 0);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(vehicleEnhancement));
    }

    public void EndEnhancement() {
        loadingImage.SetActive(true);
        SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("RiderPlayer"), SceneManager.GetSceneByBuildIndex(map));
        GameObject.FindGameObjectWithTag("RiderPlayer").transform.position.Set(bikeTransform.position.x, bikeTransform.position.y, bikeTransform.position.z);
        StartCoroutine(UnloadLevelWithBar(vehicleEnhancement));
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(map));
    }
    
}