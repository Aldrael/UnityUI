using UnityEngine;
using System.Collections;

public class DoneButton : MonoBehaviour {
    public bool toggle = true;

	// Use this for initialization
	void Start () {
        gameObject.tag = "Donebutton";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleDone()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }
}
