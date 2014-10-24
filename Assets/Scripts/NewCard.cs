using UnityEngine;
using System.Collections;

public class NewCard : MonoBehaviour {

    public bool toggle = true;
	// Use this for initialization
	void Start () {
        gameObject.tag = "Basecard";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void toggleNew()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }

}
