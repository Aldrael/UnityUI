using UnityEngine;
using System.Collections;

public class Tracer : MonoBehaviour {
    TrailRenderer m_TrailRenderer;
    public bool inZone;
	// Use this for initialization
	void Start () {
        m_TrailRenderer = GetComponent<TrailRenderer>();
        inZone = false;
	}
	
	// Update is called once per frame
	void Update () {
        
        //print(currentMousePosition);
        if (Input.GetMouseButton(0))
        {
            if (m_TrailRenderer && inZone)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = 100f;
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(mousePos);
                m_TrailRenderer.transform.position = currentMousePosition;
            }
        }
	}
}
