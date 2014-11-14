using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeckScript : MonoBehaviour {
    public List<int> cardsObtained;
    CameraScript manager;

	// Use this for initialization
	void Start () {
        gameObject.tag = "Deck";
        cardsObtained = new List<int>();
        manager = GameObject.Find("_Manager").GetComponent<CameraScript>();
        //cardsObtained.Add(Random.Range(0, 14));   //Extra
        //DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void saveCards(List<int> cards)
    {
        cardsObtained.AddRange(cards);
    }

}
