using UnityEngine;
using System.Collections;

public class HelpButton : MonoBehaviour {
    public GameObject helpScreen;
    public bool Active = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseOver()
    {
        if (!Active)
        {
            helpScreen.SetActive(true);
            helpScreen.GetComponent<HelpScreen>().enableHelp();
            Active = true;
        }
    }
}
