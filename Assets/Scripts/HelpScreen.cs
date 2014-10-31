using UnityEngine;
using System.Collections;

public class HelpScreen : MonoBehaviour {
    bool inAnimation;
    float currentx;
    const float offsetx = 75f; 
	// Use this for initialization
	void Start () {
        currentx = transform.position.x;
        iTween.MoveTo(gameObject, new Vector3(currentx + offsetx, transform.position.y, transform.position.z), 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void enableHelp()
    {
        iTween.MoveTo(gameObject, new Vector3(currentx, transform.position.y, transform.position.z), 0.5f);
    }

    void OnMouseExit()
    {
        iTween.MoveTo(gameObject, new Vector3(currentx + offsetx, transform.position.y, transform.position.z), 1f);
        GetComponentInParent<HelpButton>().Active = false; 
    }

}
