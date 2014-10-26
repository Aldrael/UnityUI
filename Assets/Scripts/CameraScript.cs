using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {

    private bool mute;
    private float current_volume;
    public GameObject[] cards;
    public GameObject basecard_holder;
    public GameObject doneholder;
    public GameObject newcardholder;
    public List<int> cardsObtained;
    public int cards_up {get; set;}
  
	// Use this for initialization
	void Start () {
        mute = false;
        current_volume = 1f;
        cards_up = 0;
        gameObject.tag = "Manager";
        doneholder.GetComponent<DoneButton>().toggleDone();
        toggleAllCards();
        cardsObtained = new List<int>();
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(doneholder);
	}

    void OnLevelWasLoaded(int level)
    {
        /*
        if (level == 0)
        {
            doneholder = GameObject.FindWithTag("Donebutton");
            GameObject initDone = doneholder;
            initDone.GetComponent<DoneButton>().toggleDone();
        }
        */
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
        cards = GameObject.FindGameObjectsWithTag("Card");
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
            //foreach (int value in cardsObtained) print(value);
            doneholder.GetComponent<DoneButton>().toggleDone();
            cards_up = 0;
        }
    }

    public void toggleAllCards()
    {
        foreach(GameObject card in cards) {
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
        }
    }

    public void addCard(int index)
    {
        cardsObtained.Add(index);
    }

}
