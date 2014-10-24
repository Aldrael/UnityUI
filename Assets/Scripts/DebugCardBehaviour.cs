using UnityEngine;
using System.Collections;

public class DebugCardBehaviour : MonoBehaviour {
    CardController cardController;

    int state = 0;
	// Use this for initialization
	void Start () {
        cardController = GetComponent<CardController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        state++;
        switch (state)
        {
            case 1:
                cardController.showBackground();
                break;
            case 2:
                cardController.showFace();
                break;
            case 3:
                cardController.hideCard();
                state = 0;
                break;
        }
    }
}
