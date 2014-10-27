using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayTextDeck : MonoBehaviour {
    Text deck;

	// Use this for initialization
	void Start () {
        gameObject.tag = "Deckdisplay";
        deck = GetComponent<Text>();
       // deck.text = "Test";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void displayText(string text)
    {
        deck.text = text;
    }
}
