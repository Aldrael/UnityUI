using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    private bool mute;
    private float current_volume;
    public GameObject[] cards;
  
	// Use this for initialization
	void Start () {
        mute = false;
        current_volume = 1;
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

}
