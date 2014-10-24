using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    private bool mute;
    private float current_volume;
    public GameObject[] cards;
    public GameObject basecard_holder;
    public GameObject doneholder;
    public GameObject newcardholder;
    public int cards_up {get; set;}
  
	// Use this for initialization
	void Start () {
        mute = false;
        current_volume = 1f;
        cards_up = 0;
        gameObject.tag = "Manager";
        GameObject initDone = doneholder;
        initDone.GetComponent<DoneButton>().toggleDone();
        toggleAllCards();
    
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

    public void newCard()
    {
       // GameObject newcard = GameObject.FindWithTag("Basecard");
        //GameObject newcard = Instantiate(Resources.Load("Assets/Sprites/Cards/Basecard"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
      //  GameObject clone;
      //  clone = Instantiate(basecard_holder, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    }

    public void cardCounter()
    {
        cards_up++;
        if (cards_up >= 5)
        {
            GameObject done = doneholder;
            done.GetComponent<DoneButton>().toggleDone();
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

}
