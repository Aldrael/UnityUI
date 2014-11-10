using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class SecondLevel : MonoBehaviour
{
    //public List<int> cardsObtained;
    public int[] cardsObtained;
    DisplayTextDeck deckDisplay;
    StringBuilder displayText;
    public GameObject cardTemplate;
    //GameObject[] cards;
    Vector3[] cardPositions;
    Quaternion[] cardAngles;

    GameObject[] currentCards;
    bool released;
    bool inAnimation;
    float speed;
    Stack<int> cardStack_more;
    Stack<int> cardStack_less;
    bool zoomed;
    //server
    int playerCount = 8;
    int serverPort = 23467;
    bool useNAT = false;
    bool serverStarted = false;
    // Use this for initialization
    void Start()
    {
        DeckScript deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();
        //cardsObtained.AddRange(deck.cardsObtained);
        // cards = new GameObject[cardsObtained.Count];
        cardsObtained = deck.cardsObtained.ToArray();
        //cards = new GameObject[cardsObtained.Length];
        currentCards = new GameObject[7];

        cardStack_more = new Stack<int>(deck.cardsObtained);
        cardStack_less = new Stack<int>();
  
        initCardPositions();
        initCardAngles();
        // layoutCards(cardsObtained.Count);

        layoutCards(cardStack_more.Count);
        released = true;
        inAnimation = false;
        speed = 0.75f;
        zoomed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
            Application.LoadLevel(0);
        if (Input.GetKey("left") && released && !inAnimation && (currentCards[4] != null) && !zoomed)
        {
            inAnimation = true;
            released = false;
            shiftLeft();
        }
        if (Input.GetKeyUp("left"))
        {
            released = true;
        }
        if (Input.GetKey("right") && released && !inAnimation && (currentCards[2] != null) && !zoomed)
        {
            inAnimation = true;
            released = false;
            shiftRight();
        }
        if (Input.GetKeyUp("right"))
        {
            released = true;
        }
        if (Input.GetKeyDown("up") && !zoomed)
        {
            iTween.MoveTo(currentCards[3], iTween.Hash("path", iTweenPath.GetPath("ZoomIn"), "time", 1f));
            zoomed = true;
        }
        else if (Input.GetKeyDown("up") && zoomed)
        {
            iTween.MoveTo(currentCards[3], iTween.Hash("path", iTweenPath.GetPath("ZoomOut"), "time", 1f));
            zoomed = false;
        }

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
        cardAngles[0].eulerAngles = new Vector3(0, 0, 90f);
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
            case 0:
                {
                    return;
                }
            case 1:
                {
                    instantiateMoreCard(3);
                    break;
                }
            case 2:
                instantiateMoreCard(3);
                instantiateMoreCard(4);
                break;
            case 3:
                instantiateMoreCard(3);
                instantiateMoreCard(4);
                instantiateMoreCard(5);
                break;
            default:
                instantiateMoreCard(3);
                instantiateMoreCard(4);
                instantiateMoreCard(5);
                instantiateMoreCard(6);
                break;
        }
    }
    void shiftLeft()
    {
        if (currentCards[0] != null)
        {
            cardStack_less.Push(currentCards[0].GetComponentInChildren<CardSet>().index);
            DestroyObject(currentCards[0]);
            currentCards[0] = null;
        }
        for (int i = 1; i < 7; i++)
        {
            if (currentCards[i] != null)
            {
                iTween.MoveTo(currentCards[i], iTween.Hash("path", iTweenPath.GetPath("RL" + i.ToString()), "time", speed));
                iTween.RotateAdd(currentCards[i], new Vector3(0, 0, 30f), speed);
            }
        }
        
        GameObject[] newAr = new GameObject[currentCards.Length];

        for (int i = 1; i < currentCards.Length; i++)
        {
            newAr[i - 1] = currentCards[i];

        }

        currentCards = newAr;


        if (cardStack_more.Count > 0)
        {
            instantiateMoreCard(6);
        }
        else
        {
            currentCards[6] = null;

        }
        StartCoroutine(waitAnimation());
    }

    void shiftRight()
    {
        if (currentCards[6] != null)
        {
            cardStack_more.Push(currentCards[6].GetComponentInChildren<CardSet>().index);
            DestroyObject(currentCards[6]);
            currentCards[6] = null;
        }
        for (int i = 0; i < 6; i++)
        {
            if (currentCards[i] != null)
            {
                iTween.MoveTo(currentCards[i], iTween.Hash("path", iTweenPath.GetPath("LR" + i.ToString()), "time", speed));
                iTween.RotateAdd(currentCards[i], new Vector3(0, 0, -30f), speed);
            }
        }
        
        GameObject[] newAr = new GameObject[currentCards.Length];
        for (int i = currentCards.Length-2; i > -1; i--)
        {
            newAr[i + 1] = currentCards[i];
        }
        currentCards = newAr;

        if (cardStack_less.Count > 0)
        {
            instantiateLessCard(0);
        }
        else
        {
            currentCards[0] = null;
        }
        StartCoroutine(waitAnimation());
    }

    IEnumerator waitAnimation()
    {
        yield return new WaitForSeconds(speed);
        inAnimation = false;
    }

    void instantiateMoreCard(int index)
    {
        cardTemplate.GetComponentInChildren<CardSet>().setCard(cardStack_more.Pop());
        currentCards[index] = Instantiate(cardTemplate, cardPositions[index], cardAngles[index]) as GameObject;
        currentCards[index].GetComponentInChildren<BackCard>().disableObject();
        if (!currentCards[index].GetComponentInChildren<CardSet>().rare)
        {
            currentCards[index].GetComponentInChildren<RareCard>().disableRare();
        }
        else
        {
            currentCards[index].GetComponentInChildren<RareCard>().enableRare();
        }
    }

    void instantiateLessCard(int index)
    {
        cardTemplate.GetComponentInChildren<CardSet>().setCard(cardStack_less.Pop());
        currentCards[index] = Instantiate(cardTemplate, cardPositions[index], cardAngles[index]) as GameObject;
        currentCards[index].GetComponentInChildren<BackCard>().disableObject();
        if (!currentCards[index].GetComponentInChildren<CardSet>().rare)
        {
            currentCards[index].GetComponentInChildren<RareCard>().disableRare();
        }
        else
        {
            currentCards[index].GetComponentInChildren<RareCard>().enableRare();
        }
    }

    public void StartServer()
    {
        if (!serverStarted)
        {
            NetworkConnectionError state = Network.InitializeServer(playerCount, serverPort, useNAT);
            if (state == NetworkConnectionError.NoError)
            {
                MasterServerUtils.RegisterWithMasterServer("Unity UI CCG", "This is a comment about TestServer");
                serverStarted = true;
            }
            else
            {
                Debug.Log("Couldn't initialize server! " + state);
            }
        }
    }
}
