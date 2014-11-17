using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{

    private bool mute;
    private float current_volume;
    public GameObject[] cards, boosterpacks;
    public GameObject doneholder, newholder, packholder, cutholder, deckHolder;
    public int cards_up, cardsSelected;
    //public List<int> cardsObtained;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;

    public bool newCardFlag;

    int drawDepth = -1000;
    float alpha = 1.0f;
    int fadeDir = -1;

    GameObject booster;
    Vector3 mouseStartPosition, mousePos;
    bool mousePressed;
    Vector3 startPositionPack;
    List<SpriteSlicer2DSliceInfo> cuts = new List<SpriteSlicer2DSliceInfo>();

    const float boosterscale = 1f;
    const float newCardDelay = 0.1f;

    public bool inScaling;
    public int currentPack;
    public GameObject packText;
    GameObject[] packButtons;
    public bool inBoosterMove;
    public bool inSelectionMove;
    public bool notInZone;
    public Vector3 boosterStartPosition;

    //server
    int playerCount = 8;
    int serverPort = 23467;
    bool useNAT = false;
    bool serverStarted = false;
    DeckScript deck;
    Packs packs;

    //

    // Use this for initialization
    void Start()
    {
        deckHolder.SetActive(false);
        if (GameObject.Find("Deck") == null)
        {
            deckHolder.SetActive(true);
        }
        deck = GameObject.Find("Deck").GetComponent<DeckScript>();

        boosterStartPosition = packholder.transform.position;
        mute = false;
        current_volume = 0.25f;
        AudioListener.volume = current_volume;
        cards_up = 0;
        gameObject.tag = "Manager";
        doneholder.GetComponent<DoneButton>().disableDone();

        Vector3[] positions = new Vector3[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            positions[i] = cards[i].GetComponent<MainCard>().transform.position;
        }
        foreach (GameObject card in cards)
        {
            card.GetComponent<MainCard>().initializePositions(positions);
        }
        disableAllCards();
        //cardsObtained = new List<int>();
        newCardFlag = false;
        AudioListener.pause = false;

        //booster = GameObject.Find("LOBBooster");
        booster = GameObject.FindGameObjectWithTag("Boosterpack");
        startPositionPack = booster.transform.position;

        packText = GameObject.Find("CurrentPack");

        inScaling = false;
        currentPack = 0;
        initBoosters();
        disableAllBoosters();
        enableBooster();
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
        packButtons = GameObject.FindGameObjectsWithTag("Packselection");
        inBoosterMove = false;
        cardsSelected = 0;
        inSelectionMove = false;
        notInZone = true;

        packs = GameObject.Find("Packs").GetComponent<Packs>();
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
        if ((cards_up == 0) && newCardFlag)
        {
            
            newCardFlag = false;
            resetAllCards();
            enableCut();
            packholder.GetComponent<BoxCollider>().enabled = true;
            //newholder.GetComponent<NewCard>().toggleNew();
            enableBooster();
            foreach (GameObject packButton in packButtons)
            {
                packButton.GetComponent<PackSelection>().enableButton();
            }
        }

        if (cardsSelected == 5)
        {
            inSelectionMove = true;
            float waitTime = 1.5f;
            foreach (GameObject card in cards)
            {
                if (!card.GetComponent<MainCard>().hasGem)
                {
                    card.GetComponent<MainCard>().notSelected = true;
                    card.GetComponent<MainCard>().translateUp();
                    card.GetComponent<FlipCard>().CameraFlip();
                    StartCoroutine(waitFlip(card, waitTime));
                }
                
            }
            StartCoroutine(animateSelected(waitTime));
            cardsSelected = 0;
        }

        cutPacks();
    }

    IEnumerator waitFlip(GameObject card, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        iTween.MoveAdd(card, new Vector3(-1000f, 0, 0), 3f);
    }

    IEnumerator animateSelected(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        int fillSlot = 5;
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<MainCard>().hasGem)
            {
                card.GetComponent<MainCard>().selected = true;
                card.GetComponent<MainCard>().destroyGem();
                iTween.MoveTo(card, iTween.Hash("time", 1f, "x", -190.2523f, "y", 66.78287f, "z", 460f, "easetype", "linear"));
                card.GetComponent<MainCard>().inAnimation = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(moveLoop(card, fillSlot));
                fillSlot--;
            }
        }

    }

    IEnumerator moveLoop(GameObject card, int slot)
    {
        int nextPath = 1;
        card.GetComponent<MainCard>().slotOccupied = slot; 
        while (slot > nextPath)
        {
            iTween.MoveTo(card, iTween.Hash("time", 1f, "path", iTweenPath.GetPath("Arc" + nextPath.ToString()), "easetype", "linear"));
            nextPath++;
            yield return new WaitForSeconds(0.9f);
        }
        card.GetComponent<FlipCard>().isReady = true;
        card.GetComponent<MainCard>().inAnimation = false;
        if (slot == 1)
        {
            inSelectionMove = false;
        }
    }

    void moveTo(int spot, GameObject card)
    {
        switch (spot)
        {
            case 1:
                iTween.MoveTo(card, iTween.Hash("path", iTweenPath.GetPath("Arc1"), "time", 1f));
                break;
            case 2:
                iTween.MoveTo(card, iTween.Hash("path", iTweenPath.GetPath("Arc2"), "time", 1f));
                break;
            case 3:
                iTween.MoveTo(card, iTween.Hash("path", iTweenPath.GetPath("Arc3"), "time", 1f));
                break;
            case 4:
                iTween.MoveTo(card, iTween.Hash("path", iTweenPath.GetPath("Arc4"), "time", 1f));
                break;
            default:
                iTween.MoveTo(card, iTween.Hash("path", iTweenPath.GetPath("Arc5"), "time", 1f));
                break;
        }
    }

    void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and Time.deltatime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        //force (clamp) the number between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;  //make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }

    public void MuteToggle()
    {
        if (mute)
        {
            //AudioListener.volume = current_volume;
            AudioListener.pause = false;
            mute = false;
        }
        else
        {
            //AudioListener.volume = 0;
            AudioListener.pause = true;
            mute = true;
        }
    }

    public void ChangeScene(int scene)
    {
        GameObject deck = GameObject.FindGameObjectWithTag("Deck");
        //deck.GetComponent<DeckScript>().SaveCards(cardsObtained);
        DontDestroyOnLoad(deck);
        StartCoroutine(ChangeLevel(scene));
    }

    IEnumerator ChangeLevel(int scene)
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        Application.LoadLevel(scene);
    }

    public void VolumeSlide(float volume)
    {
        current_volume = volume;
        AudioListener.volume = current_volume;
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void randomizeAllCards()
    {
        // cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cards)
        {
            RandomCard randomCard = card.GetComponent<RandomCard>();
            randomCard.randomizeCards(currentPack, false);
        }
    }

    public void cardCounter()
    {
        cards_up++;
        if (cards_up >= 5)
        {
            doneholder.GetComponent<DoneButton>().toggleDone();
            //cards_up = 0;
        }
    }

    public void toggleAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<MainCard>().toggleCard();
        }
    }

    public void enableAllCards()
    {
        StartCoroutine(enableWait());
    }

    IEnumerator enableWait()
    {
        /*
        foreach (GameObject card in cards)
        {
            card.GetComponent<MainCard>().enableCard();
            float time = 0;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                yield return new WaitForSeconds(1.0f / 60);
            }
        }
         */
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<MainCard>().enableCard(i);
            float time = 0;
            while (time < newCardDelay)
            {
                time += Time.deltaTime;
                yield return new WaitForSeconds(1.0f / 60);
            }
        }
    }

    public void disableAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<MainCard>().disableCard();
        }
    }

    public void resetAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().resetCard();
        }

        disableAllCards();
    }

    public void speedChangeAllCards(float speed)
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().SpeedSlide(speed);
           // card.GetComponent<MainCard>().SpeedSlide(speed);
        }
    }

    public void alphaChangeAllCards(float speed)
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().AlphaSlide(speed);
        }
    }

    public void addCard(int index)
    {
        //cardsObtained.Add(index);
        deck.SaveCard(index);
    }

    public void changeRotateAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().changeRotation();
        }
    }

    public void cutPacks()
    {
        if (inScaling || inBoosterMove || notInZone)
        {
            return;
        }
        mousePos = Input.mousePosition;
        mousePos.z = startPositionPack.z;
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePos);
        //print(currentMousePosition);
        if (Input.GetMouseButton(0))
        {
            if (!mousePressed)
            {
                mouseStartPosition = currentMousePosition;
            }

            Debug.DrawLine(mouseStartPosition, currentMousePosition, Color.red);
            mousePressed = true;
        }
        else if (mousePressed)
        {
            
            //SpriteSlicer2D.SliceSprite(mouseStartPosition, currentMousePosition, booster, true,ref cuts);
            SpriteSlicer2D.SliceAllSprites(mouseStartPosition, currentMousePosition, false, ref cuts);
            /*
            GameObject[] packs = GameObject.FindGameObjectsWithTag("Boosterpack");
            foreach (GameObject pack in packs)
            {
                pack.GetComponent<Rigidbody2D>().AddForce(new Vector2(100f, 10000f));
            }
            */
            mousePressed = false;
        }
    }

    public void getPackPieces(List<GameObject> pieces)
    {
        inScaling = true;
        StartCoroutine(enableCardsDelay());
        foreach (GameObject piece in pieces)
        {
            StartCoroutine(changeScale(piece));
        }
    }

    IEnumerator enableCardsDelay()
    {
        yield return new WaitForSeconds(1);
        enableAllCards();
    }

    IEnumerator changeScale(GameObject piece)
    {
        yield return new WaitForSeconds(1);
        float thisscale = boosterscale;
        while (thisscale > 0)
        {
            thisscale -= 0.01f;
            piece.GetComponent<Transform>().localScale = new Vector3(thisscale, thisscale, thisscale);
            yield return new WaitForSeconds(1.0f / 60);
        }
        inScaling = false;
        Destroy(piece);
        iTween.MoveTo(packholder, boosterStartPosition, 0f);
        yield return new WaitForSeconds(0.1f);
        packs.haloOn = true;
    }

    public void disableBooster(int index)
    {
        boosterpacks[index].SetActive(false);
    }

    public void enableBooster() {
        boosterpacks[currentPack].SetActive(true);
    }

    public void disableAllBoosters()
    {
        foreach (GameObject pack in boosterpacks)
        {
            pack.SetActive(false);
        }
    }

    public void increPack()
    {
        /*
        if (inBoosterMove)
        {
            return;
        }
        */
        int previous = currentPack++;
        if (currentPack > boosterpacks.Length - 1) currentPack = 0;
        enableBooster();
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
        disableBooster(previous);
        /*
        
        iTween.MoveTo(boosterpacks[currentPack], iTween.Hash("path", iTweenPath.GetPath("ForwardPath"), "time", 1, "easetype", iTween.EaseType.easeInOutSine));
        iTween.MoveTo(boosterpacks[previous], iTween.Hash("path", iTweenPath.GetPath("NextPath"), "time", 1, "easetype", iTween.EaseType.easeInOutSine));
        StartCoroutine(waitMove(previous));
        */
    }

    IEnumerator waitMove(int previous)
    {
        inBoosterMove = true;
        yield return new WaitForSeconds(1);
        inBoosterMove = false;
        disableBooster(previous);
    }

    public void decrePack()
    {
        /*
        if (inBoosterMove)
        {
            return;
        }
        */
        int previous = currentPack--;
        if (currentPack < 0) currentPack = boosterpacks.Length - 1;
        enableBooster();
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
        disableBooster(previous);
        /*
        
        iTween.MoveTo(boosterpacks[currentPack], iTween.Hash("path", iTweenPath.GetPath("PreviousPath"), "time", 1, "easetype", iTween.EaseType.easeInOutSine));
        iTween.MoveTo(boosterpacks[previous], iTween.Hash("path", iTweenPath.GetPath("BackPath"), "time", 1, "easetype", iTween.EaseType.easeInOutSine));
        StartCoroutine(waitMove(previous));
        */
    }

    public void initBoosters()
    {
        foreach (GameObject pack in boosterpacks)
        {
            pack.GetComponent<Transform>().transform.Translate(0, 0, 8.6681f);
        }
        boosterpacks[currentPack].GetComponent<Transform>().transform.Translate(0, 0, -8.6681f);
    }

    public void donePushed()
    {
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<MainCard>().selected)
            {
                float x;
                if (card.GetComponent<FlipCard>().mode == 0)
                    x = -1000f;
                else
                {
                    x = 1000f;
                }
                iTween.MoveAdd(card, new Vector3(x, 0, 0), 3f);
                StartCoroutine(doneDelay());
            }
        }
    }

    IEnumerator doneDelay()
    {
        yield return new WaitForSeconds(2f);
        cards_up--;
        newCardFlag = true;
    }

    public void disableCut()
    {
        cutholder.SetActive(false);
    }

    public void enableCut()
    {
        cutholder.SetActive(true);
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
