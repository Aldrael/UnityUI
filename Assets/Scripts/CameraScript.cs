using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject[] cards;
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
    // Use this for initialization
    void Start()
    {
        mute = false;
        current_volume = 0.25f;
        AudioListener.volume = current_volume;
        cards_up = 0;
        gameObject.tag = "Manager";
        doneholder.GetComponent<DoneButton>().toggleDone();
        toggleAllCards();
        cardsObtained = new List<int>();
        newCardFlag = false;
        AudioListener.pause = false;

        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
        if ((cards_up == 0) && newCardFlag)
        {
            resetAllCards();
            newholder.GetComponent<NewCard>().toggleNew();
            newCardFlag = false;
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
}
