using UnityEngine;
using System.Collections;

public class MainCard : MonoBehaviour {
    public bool toggle = true;
	// Use this for initialization
	void Start () {
        gameObject.tag = "Maincard";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleCard()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }
}
