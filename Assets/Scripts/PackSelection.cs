using UnityEngine;
using System.Collections;

public class PackSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.tag = "Packselection";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void disableButton()
    {
        gameObject.SetActive(false);
    }

    public void enableButton()
    {
        gameObject.SetActive(true);
    }
}
