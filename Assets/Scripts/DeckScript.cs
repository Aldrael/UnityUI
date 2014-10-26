using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckScript : MonoBehaviour {
    public List<int> cardsObtained;

	// Use this for initialization
	void Start () {
        gameObject.tag = "Deck";
        cardsObtained = new List<int>();
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void saveCards(List<int> cards)
    {
        cardsObtained.AddRange(cards);
    }
}
