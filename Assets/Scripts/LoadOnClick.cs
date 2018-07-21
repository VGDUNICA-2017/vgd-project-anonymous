using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {
    public GameObject malePrefab;
    public GameObject femalePrefab;
    public GameObject maleBikePrefab;
    public GameObject femaleBikePrefab;

    public Slider loadingBar;
    public GameObject loadingImage;
    
    private AsyncOperation async;


    public void ClickAsync(int level) {
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

    public void NewGame(int level) {
        ClickAsync(level);
    }
    
    public void QuitGame() {
        Application.Quit();
    }

    public void MaleCharacter(int level) {
        DontDestroyOnLoad(GameObject.Instantiate(maleBikePrefab));
        DontDestroyOnLoad(GameObject.Instantiate(malePrefab));
        ClickAsync(level);
    }

    public void FemaleCharacter(int level) {
        DontDestroyOnLoad(GameObject.Instantiate(femaleBikePrefab));
        DontDestroyOnLoad(GameObject.Instantiate(femalePrefab));
        ClickAsync(level);
    }
}