using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShippingController : MonoBehaviour {
    public GameObject[] npc;
    public float time1;
    public float time2;
    
    public float time3;
    public Text interaction;
    public Text countdown;
    public Text livello;
    public Text monete;
    public Compass compass;

    private GameObject[] tier1=null;
    private GameObject[] tier2=null;
    private GameObject[] tier3=null;

    private int level=0;
    private float timer;
    private bool delivering = false;
    private float coins = 0;
    private GameObject objective=null;

    // Use this for initialization
    void Start() {
        //Loading the savegame
        Pause pause=FindObjectOfType<Pause>();
        pause.LoadGame();
        monete.text = ((int)coins).ToString() + " €";

        //Loading levels
        if (tier1 == null) {
            tier1 = GameObject.FindGameObjectsWithTag("Tier1");
            Shuffle(tier1);
            foreach (GameObject tier in tier1) {
                tier.SetActive(false);
            }
        }
        if (tier2 == null) {
            tier2 = GameObject.FindGameObjectsWithTag("Tier2");
            Shuffle(tier2);
            foreach (GameObject tier in tier2) {
                tier.SetActive(false);
            }
        }
        if (tier3 == null) {
            tier3 = GameObject.FindGameObjectsWithTag("Tier3");
            Shuffle(tier3);
            foreach (GameObject tier in tier3) {
                tier.SetActive(false);
            }
        }
        Shuffle(npc);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        StartCoroutine(ShowMessage("To start a level go to pizzeria", 5));
    }

    private void Update() {
        //If player is not delivering countdown is disabled
        if (delivering == true && timer>0) {
            timer -= Time.deltaTime;
            countdown.text = ((int)timer).ToString();
        } else {
            timer = 0;
            countdown.text = "";
        }
    }

    void Shuffle(GameObject[] array) {
        // Knuth shuffle algorithm
        for (int i = 0; i < array.Length; i++) {
            GameObject tmp = array[i];
            int r = Random.Range(i, array.Length);
            array[i] = array[r];
            array[r] = tmp;
        }
    }

    private void NextLevel() {
        int i=level/3;
        Shipped ship;

        if (level < 9) {
            switch (level % 3) {
                case 0:
                    if (level != 0) {
                        tier3[i-1].SetActive(false);
                    }
                    tier1[i].SetActive(true);
                    objective =GameObject.Instantiate(npc[level],tier1[i].GetComponent<Transform>());
                    timer = time1;
                    break;
                case 1:
                    tier1[i].SetActive(false);
                    tier2[i].SetActive(true);
                    objective = GameObject.Instantiate(npc[level], tier2[i].GetComponent<Transform>());
                    timer = time2;
                    break;
                case 2:
                    tier2[i].SetActive(false);
                    tier3[i].SetActive(true);
                    objective = GameObject.Instantiate(npc[level], tier3[i].GetComponent<Transform>());
                    timer = time3;
                    break;
            }
        } else {
            tier3[i-1].SetActive(false);
            tier3[i].SetActive(true);
            objective = GameObject.Instantiate(npc[level], tier3[i].GetComponent<Transform>());
            timer = time3;
        }

        ship = objective.GetComponent<Shipped>();
        ship.shipping = this;
        ship.interaction = interaction;

        compass.objective = objective.transform;
        switch (i) {
            case 1:
                timer *= 0.95f;
                break;
            case 2:
                timer *= 0.9f;
                break;
            case 3:
                timer *= 0.8f;
                break;
        }
        level++;
    }

    public void PickUp() {
        float i=Random.Range(0, 1f);
        float gain;

        //Se viene preso un potenziamento in caso rimane poco tempo la possibilità di ottenerne altro aumenta
        if (timer < 15 && timer >0) { 
            if (i > .75) {
                gain = Random.Range(1, 5);
                coins += gain;
                monete.text = ((int)coins).ToString() + " €";
                StartCoroutine(ShowMessage("Coins + " + ((int)gain).ToString(), 2));
            } else {
                gain = Random.Range(3, 8);
                timer += gain;
                StartCoroutine(ShowMessage("Time + " + ((int)gain).ToString(), 2));
            }
        } else if (timer>0){
            if (i > .5) {
                gain = Random.Range(1, 5);
                coins += gain;
                monete.text = ((int)coins).ToString() + " €";
                StartCoroutine(ShowMessage("Coins + " + ((int)gain).ToString(), 2));
            }
            else {
                gain = Random.Range(1, 5);
                timer += gain;
                StartCoroutine(ShowMessage("Time + " + ((int)gain).ToString(), 2));
            }
        } else {
            gain= Random.Range(0, .5f);
            coins += gain;
            monete.text = ((int)coins).ToString() + " €";
            StartCoroutine(ShowMessage("Coins + " + gain.ToString(), 2));
        }
    }

    public void ShippingCompleted() {
        int time = (int)timer;
        float gain;

        timer = 0;
        delivering = false;
        livello.text = "";
        countdown.text = "";

        if (time > 0) {
            gain = (time*level)*0.1f + Random.Range(2, 7);
            coins += gain;
            monete.text = ((int)coins).ToString() + " €";
            StartCoroutine(ShowMessage("Level " + level.ToString() + " Completed: Coins + " + ((int)gain).ToString(), 2));
        } else {
            level--;
            StartCoroutine(ShowMessage("Level failed", 2));
        }

        compass.objective = transform;
    }

    IEnumerator ShowMessage(string message, float delay) {
        interaction.text = message;
        yield return new WaitForSeconds(delay);
        interaction.text = "";
    }

    public int GetCoins() {
        return (int)coins;
    }

    public void SetCoins(float c) {
        coins = c;
    }

    public int GetLevel() {
        if (delivering == false) {
            return level;
        }
        else {
            return level-1;
        }
        
    }

    public void SetLevel(int l) {
        level=l;
    }

    public void Spend(float spent) {
        coins -= spent;
    }


    public bool InDelivery() {
        return delivering;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("Player") && delivering == false) {
            interaction.text = "Press 'E' to load next level";
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag.Equals("Player") && delivering == false && Input.GetButtonDown("Interact")) {
            if (objective != null) {
                GameObject.Destroy(objective);
            }
            NextLevel();
            interaction.text = "";
            livello.text = "Lv " + level.ToString();
            delivering = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        interaction.text = "";
    }
}