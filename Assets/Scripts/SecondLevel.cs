using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class SecondLevel : MonoBehaviour {
    public List<int> cardsObtained;
    DisplayTextDeck deckDisplay;
    StringBuilder displayText;

	// Use this for initialization
	void Start () {
        DeckScript deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();
        cardsObtained.AddRange(deck.cardsObtained);
        //foreach (int value in cardsObtained) print(value);
        deckDisplay = GameObject.FindGameObjectWithTag("Deckdisplay").GetComponent<DisplayTextDeck>();
        int[] cards = new int[5];
        cards = sortCards(cardsObtained);
        displayText = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            displayText.AppendLine(cardNames(i) + ": " + cards[i].ToString());
        }
        deckDisplay.displayText(displayText.ToString());
        //DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
            Application.LoadLevel(0);
	}

    int[] sortCards(List<int> deck)
    {
        int[] cards = new int[5];
        foreach (int value in deck)
        {
            switch (value)
            {
                case (0):
                    cards[0]++;
                    break;
                case (1):
                    cards[1]++;
                    break;
                case (2):
                    cards[2]++;
                    break;
                case (3):
                    cards[3]++;
                    break;
                default:
                    cards[4]++;
                    break;
            }
        }
        return cards;
    }

    string cardNames(int index)
    {
            switch (index)
            {
                case (0):
                    return "Head";
                case (1):
                    return "Leftarm";
                case (2):
                    return "Leftleg";
                case (3):
                    return "Rightarm";
                default:
                    return "Rightleg";
            }
    }
}
