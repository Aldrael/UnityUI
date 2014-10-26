using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecondLevel : MonoBehaviour {
    List<int> cardsObtained;
    GameObject manager;

	// Use this for initialization
	void Start () {
        cardsObtained = new List<int>();
        manager = GameObject.FindWithTag("Manager");
        cardsObtained = manager.GetComponent<CameraScript>().cardsObtained;
        foreach (int value in cardsObtained) print(value);
        DontDestroyOnLoad(manager);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
            Application.LoadLevel(0);
	}
}
