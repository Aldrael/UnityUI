using UnityEngine;
using System.Collections;

public class RareCard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void disableRare()
    {
        gameObject.SetActive(false);
    }

    public void enableRare()
    {
        gameObject.SetActive(true);
    }
}
