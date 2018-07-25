using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShippingController : MonoBehaviour {
    public GameObject[] npc;
    public GameObject[] tier1;
    public float time1;
    public GameObject[] tier2;
    public float time2;
    public GameObject[] tier3;
    public float time3;
    public Text interaction;
    public Text countdown;
    public Text livello;
    public Text monete;
    public Compass compass;

    private int level;
    private float timer;
    private bool delivering = false;
    private float coins = 0;
    private GameObject objective=null;

    // Use this for initialization
    void Start() {
        Shuffle(npc);
        Shuffle(tier1);
        Shuffle(tier2);
        Shuffle(tier3);
    }

    private void Update() {
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
                Debug.Log("minore" + timer + " " + i);
                gain = Random.Range(1, 5);
                coins += gain;
                monete.text = ((int)coins).ToString() + " €";
                StartCoroutine(ShowMessage("Coins + " + ((int)gain).ToString(), 2));
            } else {
                Debug.Log("minore" + timer + " " + i);
                gain = Random.Range(3, 8);
                timer += gain;
                StartCoroutine(ShowMessage("Time + " + ((int)gain).ToString(), 2));
            }
        } else if (timer>0){
            if (i > .5) {
                Debug.Log("maggiore" + timer + " " + i);
                gain = Random.Range(1, 5);
                coins += gain;
                monete.text = ((int)coins).ToString() + " €";
                StartCoroutine(ShowMessage("Coins + " + ((int)gain).ToString(), 2));
            }
            else {
                Debug.Log("maggiore" + timer + " " + i);
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