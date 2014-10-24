using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {

    public int cardIndex;

    public Texture[] faces;

    public Texture background;

    public void showBackground()
    {
        renderer.enabled = true;
        renderer.material.mainTexture = background;
    }

    public void hideCard()
    {
        renderer.enabled = false;
    }

    public void showFace()
    {
        renderer.enabled = true;
        renderer.material.mainTexture = faces[0];
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
