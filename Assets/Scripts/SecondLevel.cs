using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

[RequireComponent (typeof (NetworkView))]
public class SecondLevel : MonoBehaviour
{
    //public List<int> cardsObtained;
    public int[] cardsObtained;
    public int[] cardsReceived;
    DisplayTextDeck deckDisplay;
    StringBuilder displayText;
    public GameObject cardTemplate;
    //GameObject[] cards;
    Vector3[] cardPositions, cardPositionsReceived;
    Quaternion[] cardAngles, cardAnglesReceived;

    GameObject[] currentCards, currentReceived;
    bool released;
    bool inAnimation;
    float speed;
    Stack<int> cardStack_more, receiveStack_more;
    Stack<int> cardStack_less, receiveStack_less;
    bool zoomed;
    //server
    int playerCount = 8;
    int serverPort = 23467;
    bool useNAT = false;
    bool serverStarted = false;
    string sendCards;
    bool connected = false;

    public ShowCards showCards;
    DeckScript deck;

    // Use this for initialization
    void Start()
    {
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>();

        //cardsObtained.AddRange(deck.cardsObtained);
        // cards = new GameObject[cardsObtained.Count];
        cardsObtained = deck.cardsObtained.ToArray();
        //cards = new GameObject[cardsObtained.Length];
        currentCards = new GameObject[7];
        cardStack_more = new Stack<int>(deck.cardsObtained);
        cardStack_less = new Stack<int>();
        sendCards = string.Join(",", new List<int>(cardsObtained).ConvertAll(i => i.ToString()).ToArray());
  
        initCardPositions();
        initCardAngles();
        // layoutCards(cardsObtained.Count);

        layoutCards(cardStack_more.Count);
        released = true;
        inAnimation = false;
        speed = 0.75f;
        zoomed = false;
        showCards = GameObject.Find("ShowCards").GetComponent<ShowCards>();
        showCards.DisableObject();
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

    [RPC]
    public void PrintCards(string cards)
    {
        print(cards);
    }

    void OnConnectedToServer()
    {
        connected = true;
    }

    void OnServerInitialized()
    {
        connected = true;
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
        cardPositions[0] = new Vector3(-245.0984f, -260.8663f, 250f);
        cardPositions[1] = new Vector3(-209.5863f, -151.39f, 250f);
        cardPositions[2] = new Vector3(-114.596f, -62.973f, 250f);
        cardPositions[3] = new Vector3(0, -35f, 250f);
        cardPositions[4] = new Vector3(114.596f, -62.973f, 250f);
        cardPositions[5] = new Vector3(209.5863f, -151.39f, 250f);
        cardPositions[6] = new Vector3(245.0984f, -260.8663f, 250f);
    }

    void initCardPositionsReceive()
    {
        cardPositionsReceived = new Vector3[7];
        cardPositionsReceived[0] = new Vector3(245.0984f, 260.8663f, 250f);
        cardPositionsReceived[1] = new Vector3(209.5863f, 151.39f, 250f);
        cardPositionsReceived[2] = new Vector3(114.596f, 62.973f, 250f);
        cardPositionsReceived[3] = new Vector3(0, 35f, 250f);
        cardPositionsReceived[4] = new Vector3(-114.596f, 62.973f, 250f);
        cardPositionsReceived[5] = new Vector3(-209.5863f, 151.39f, 250f);
        cardPositionsReceived[6] = new Vector3(-245.0984f, 260.8663f, 250f);
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

    void initCardAnglesReceived()
    {
        cardAnglesReceived = new Quaternion[7];
        for (int i = 0; i < 7; i++)
        {
            cardAnglesReceived[i] = Quaternion.identity;
        }
        cardAnglesReceived[0].eulerAngles = new Vector3(0, 0, 90f);
        cardAnglesReceived[1].eulerAngles = new Vector3(0, 0, 60f);
        cardAnglesReceived[2].eulerAngles = new Vector3(0, 0, 30f);
        cardAnglesReceived[3].eulerAngles = new Vector3(0, 0, 0f);
        cardAnglesReceived[4].eulerAngles = new Vector3(0, 0, -30f);
        cardAnglesReceived[5].eulerAngles = new Vector3(0, 0, -60f);
        cardAnglesReceived[6].eulerAngles = new Vector3(0, 0, -90f);
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

    void layoutReceivedCards(int amount)
    {
        switch (amount)
        {
            case 0:
                {
                    return;
                }
            case 1:
                {
                    instantiateMoreReceiveCard(3);
                    break;
                }
            case 2:
                instantiateMoreReceiveCard(3);
                instantiateMoreReceiveCard(4);
                break;
            case 3:
                instantiateMoreReceiveCard(3);
                instantiateMoreReceiveCard(4);
                instantiateMoreReceiveCard(5);
                break;
            default:
                instantiateMoreReceiveCard(3);
                instantiateMoreReceiveCard(4);
                instantiateMoreReceiveCard(5);
                instantiateMoreReceiveCard(6);
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

    void instantiateMoreReceiveCard(int index)
    {
        cardTemplate.GetComponentInChildren<CardSet>().setCard(receiveStack_more.Pop());
        currentReceived[index] = Instantiate(cardTemplate, cardPositionsReceived[index], cardAnglesReceived[index]) as GameObject;
        currentReceived[index].GetComponentInChildren<BackCard>().disableObject();
        if (!currentReceived[index].GetComponentInChildren<CardSet>().rare)
        {
            currentReceived[index].GetComponentInChildren<RareCard>().disableRare();
        }
        else
        {
            currentReceived[index].GetComponentInChildren<RareCard>().enableRare();
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

    public void SendCards()
    {
        if (connected)
        {
            networkView.RPC("ReceiveCards", RPCMode.Others, sendCards);
        }
    }

    [RPC]
    public void ReceiveCards(string cards)
    {
        List<int> received = new List<int>();
        if (cards != null)
        {
            foreach (string card in cards.Split(','))
            {
                int num;
                if (int.TryParse(card, out num))
                    received.Add(num);
            }
        }
        cardsReceived = received.ToArray();

        currentReceived = new GameObject[7];
        receiveStack_more = new Stack<int>(cardsReceived);
        receiveStack_less = new Stack<int>();

        initCardPositionsReceive();
        initCardAnglesReceived();

        layoutReceivedCards(receiveStack_more.Count);
    }
}
