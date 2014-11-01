using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour {
    bool toggle;
    float currenty;
    const float offsety = 200f; 
	// Use this for initialization
	void Start () {
        currenty = transform.position.y;
        toggle = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void togglePanel()
    {
        toggle = !toggle;
        if (toggle)
        {
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, currenty, transform.position.z), 1f);
        }
        else
        {
            iTween.MoveTo(gameObject, new Vector3(transform.position.x, currenty + offsety, transform.position.z), 1f);
        }
    }
}
