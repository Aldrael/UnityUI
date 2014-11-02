using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/*
public class Deck : IEquatable<Deck>
{
    public string CardName { get; set; }

    public int CardId { get; set; }

    public override string ToString()
    {
        return "ID: " + CardId + "   Name: " + CardName;
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Deck objAsCard = obj as Deck;
        if (objAsCard == null) return false;
        else return Equals(objAsCard);
    }
    public override int GetHashCode()
    {
        return CardId;
    }
    public bool Equals(Deck other)
    {
        if (other == null) return false;
        return (this.CardId.Equals(other.CardId));
    }
    // Should also override == and != operators.

}
  */
public class CameraScript : MonoBehaviour
{

    private bool mute;
    private float current_volume;
    public GameObject[] cards, boosterpacks;
    public GameObject doneholder, newholder;
    public int cards_up;
    public List<int> cardsObtained;
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

    const float boosterscale = 70f;
    const float newCardDelay = 0.8f;

    public bool inScaling;
    int currentPack;
    public GameObject packText;
    // Use this for initialization
    void Start()
    {
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
        cardsObtained = new List<int>();
        newCardFlag = false;
        AudioListener.pause = false;

        //booster = GameObject.Find("LOBBooster");
        booster = GameObject.FindGameObjectWithTag("Boosterpack");
        startPositionPack = booster.transform.position;

        packText = GameObject.Find("CurrentPack");

        inScaling = false;
        currentPack = 0;
        disableAllBoosters();
        //enableBooster(0);
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        //Debug.DrawLine(new Vector3(-100f, 0, 0), new Vector3(100f, 0, 0), Color.red);
        if (Input.GetKey("escape"))
            Application.Quit();
        if ((cards_up == 0) && newCardFlag)
        {
            resetAllCards();
            newholder.GetComponent<NewCard>().toggleNew();
            newCardFlag = false;
        }

        cutPacks();

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
        GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>().saveCards(cardsObtained);
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
            randomCard.randomizeCards();
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
        cardsObtained.Add(index);
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
        if (inScaling)
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
        foreach (GameObject piece in pieces)
        {
            StartCoroutine(changeScale(piece));
        }
    }

    IEnumerator changeScale(GameObject piece)
    {
        yield return new WaitForSeconds(1);
        enableAllCards();
        float thisscale = boosterscale;
        while (thisscale > 0)
        {
            thisscale -= 1f;
            piece.GetComponent<Transform>().localScale = new Vector3(thisscale, thisscale, thisscale);
            yield return new WaitForSeconds(1.0f / 60);
        }
        inScaling = false;
        Destroy(piece);
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
        disableBooster(currentPack++);
        if (currentPack > boosterpacks.Length - 1) currentPack = 0;
        //enableBooster();
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
    }

    public void decrePack()
    {
        disableBooster(currentPack--);
        if (currentPack < 0) currentPack = boosterpacks.Length - 1;
        //enableBooster();
        packText.GetComponent<Text>().text = "Current Pack: " + boosterpacks[currentPack].name;
    }
}
