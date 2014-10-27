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
    public GameObject doneholder;
    public int cards_up { get; set; }
    public List<int> cardsObtained;
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    // Use this for initialization
    void Start()
    {
        mute = false;
        current_volume = 0.5f;
        AudioListener.volume = current_volume;
        cards_up = 0;
        gameObject.tag = "Manager";
        doneholder.GetComponent<DoneButton>().toggleDone();
        toggleAllCards();
        cardsObtained = new List<int>();

        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

    }

    public void MuteToggle()
    {
        if (mute)
        {
            AudioListener.volume = current_volume;
            mute = false;
        }
        else
        {
            AudioListener.volume = 0;
            mute = true;
        }
    }

    public void ChangeScene(int scene)
    {
        GameObject.FindGameObjectWithTag("Deck").GetComponent<DeckScript>().saveCards(cardsObtained);
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

    public void cardCounter(int index)
    {
        addCard(index);
        cards_up++;
        if (cards_up >= 5)
        {
            doneholder.GetComponent<DoneButton>().toggleDone();
            cards_up = 0;
        }
    }

    public void toggleAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<MainCard>().toggleCard();
        }
    }

    public void resetAllCards()
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().resetCard();
        }

        toggleAllCards();
    }

    public void speedChangeAllCards(float speed)
    {
        foreach (GameObject card in cards)
        {
            card.GetComponent<FlipCard>().SpeedSlide(speed);
           // card.GetComponent<MainCard>().SpeedSlide(speed);
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
