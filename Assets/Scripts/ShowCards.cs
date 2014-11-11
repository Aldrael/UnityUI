using UnityEngine;
using System.Collections;

public class ShowCards : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisableObject()
    {
        gameObject.SetActive(false);
    }

    public void EnableObject()
    {
        gameObject.SetActive(true);
    }
}
