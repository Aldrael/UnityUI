using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecondLevel : MonoBehaviour {
    public List<int> cardsObtained;

	// Use this for initialization
	void Start () {
        DeckScript deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();
        cardsObtained.AddRange(deck.cardsObtained);
        foreach (int value in cardsObtained) print(value);
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
            Application.LoadLevel(0);
	}
}
