using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class SecondLevel : MonoBehaviour {
    //public List<int> cardsObtained;
    public int[] cardsObtained;
    DisplayTextDeck deckDisplay;
    StringBuilder displayText;
    public GameObject cardTemplate;
    GameObject[] cards;
    Vector3[] cardPositions;
    Quaternion[] cardAngles;

	// Use this for initialization
	void Start () {
        DeckScript deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();
        //cardsObtained.AddRange(deck.cardsObtained);
       // cards = new GameObject[cardsObtained.Count];
        cardsObtained = deck.cardsObtained.ToArray();
        cards = new GameObject[cardsObtained.Length];

        initCardPositions();
        initCardAngles();
       // layoutCards(cardsObtained.Count);
        layoutCards(cardsObtained.Length);

        /*
        foreach (int value in cardsObtained)
        {
            cardTemplate.GetComponentInChildren<CardSet>().setCard(value);
           // cards[cardindex] = Instantiate(cardTemplate, new Vector3(currentx, 0, 250f), Quaternion.identity) as GameObject;
           // cards[cardindex].GetComponentInChildren<BackCard>().disableObject();
           // cardindex++;
           // currentx += 5f;
        }
        */
        
        /*
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
        */
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

    void initCardPositions()
    {
        cardPositions = new Vector3[7];
        cardPositions[0] = new Vector3(-245.0984f, -225.8663f, 250f);
        cardPositions[1] = new Vector3(-209.5863f, -116.39f, 250f);
        cardPositions[2] = new Vector3(-114.596f, -27.97304f, 250f);
        cardPositions[3] = new Vector3(0, 0, 250f);
        cardPositions[4] = new Vector3(114.596f, -27.97304f, 250f);
        cardPositions[5] = new Vector3(209.5863f, -116.39f, 250f);
        cardPositions[6] = new Vector3(245.0984f, -225.8663f, 250f);
    }

    void initCardAngles()
    {
        cardAngles = new Quaternion[7];
        for (int i = 0; i < 7; i++)
        {
            cardAngles[i] = Quaternion.identity;
        }
        cardAngles[0].eulerAngles = new Vector3(0,0,90f);
        cardAngles[1].eulerAngles = new Vector3(0, 0, 60f);
        cardAngles[2].eulerAngles = new Vector3(0, 0, 30f);
        cardAngles[3].eulerAngles = new Vector3(0, 0, 0f);
        cardAngles[4].eulerAngles = new Vector3(0, 0, -30f);
        cardAngles[5].eulerAngles = new Vector3(0, 0, -60f);
        cardAngles[6].eulerAngles = new Vector3(0, 0, -90f);
    }

    void layoutCards(int amount)
    {
        switch (amount)
        {
            case 1:
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[0]);
                cards[0] = Instantiate(cardTemplate, cardPositions[3], cardAngles[3]) as GameObject;
                cards[0].GetComponentInChildren<BackCard>().disableObject();
                break;
            case 2:
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[0]);
                cards[0] = Instantiate(cardTemplate, cardPositions[3], cardAngles[3]) as GameObject;
                cards[0].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[1]);
                cards[1] = Instantiate(cardTemplate, cardPositions[4], cardAngles[4]) as GameObject;
                cards[1].GetComponentInChildren<BackCard>().disableObject();
                break;
            case 3:
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[0]);
                cards[0] = Instantiate(cardTemplate, cardPositions[2], cardAngles[2]) as GameObject;
                cards[0].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[1]);
                cards[1] = Instantiate(cardTemplate, cardPositions[3], cardAngles[3]) as GameObject;
                cards[1].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[2]);
                cards[2] = Instantiate(cardTemplate, cardPositions[4], cardAngles[4]) as GameObject;
                cards[2].GetComponentInChildren<BackCard>().disableObject();
                break;
            case 4:
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[0]);
                cards[0] = Instantiate(cardTemplate, cardPositions[1], cardAngles[1]) as GameObject;
                cards[0].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[1]);
                cards[1] = Instantiate(cardTemplate, cardPositions[2], cardAngles[2]) as GameObject;
                cards[1].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[2]);
                cards[2] = Instantiate(cardTemplate, cardPositions[3], cardAngles[3]) as GameObject;
                cards[2].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[3]);
                cards[3] = Instantiate(cardTemplate, cardPositions[4], cardAngles[4]) as GameObject;
                cards[3].GetComponentInChildren<BackCard>().disableObject();
                break;
            default:
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[0]);
                cards[0] = Instantiate(cardTemplate, cardPositions[1], cardAngles[1]) as GameObject;
                cards[0].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[1]);
                cards[1] = Instantiate(cardTemplate, cardPositions[2], cardAngles[2]) as GameObject;
                cards[1].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[2]);
                cards[2] = Instantiate(cardTemplate, cardPositions[3], cardAngles[3]) as GameObject;
                cards[2].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[3]);
                cards[3] = Instantiate(cardTemplate, cardPositions[4], cardAngles[4]) as GameObject;
                cards[3].GetComponentInChildren<BackCard>().disableObject();
                cardTemplate.GetComponentInChildren<CardSet>().setCard(cardsObtained[4]);
                cards[4] = Instantiate(cardTemplate, cardPositions[5], cardAngles[5]) as GameObject;
                cards[4].GetComponentInChildren<BackCard>().disableObject();
                break;
        }
    }
}
