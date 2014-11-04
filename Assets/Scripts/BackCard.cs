using UnityEngine;
using System.Collections;

public class BackCard : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.tag = "Backcard";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void disableObject()
    {
        gameObject.SetActive(false);
    }

    public void enableObject()
    {
        gameObject.SetActive(true);
    }
}
