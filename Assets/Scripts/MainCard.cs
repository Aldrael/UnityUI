using UnityEngine;
using System.Collections;

public class MainCard : MonoBehaviour {
    public bool toggle = true;
    bool isAnimationProcessing = false;
    float waitTime;
    bool done;
    public float translateSpeed;
    const float maxZoom = -60f;
	// Use this for initialization
	void Start () {
        gameObject.tag = "Maincard";
        waitTime = 1.0f / 60;
        done = false;
        translateSpeed = 180f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleCard()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }

    void OnMouseOver()
    {
        if (isAnimationProcessing || done)
        {
            return;
        }
        StartCoroutine(translate());

    }
    IEnumerator translate()
    {
        isAnimationProcessing = true;

        while (!done)
        {
            float translate = Time.deltaTime * translateSpeed;
            transform.Translate(0,0, -translate, Space.Self);
            if (transform.localPosition.z < maxZoom)
            {
                done = true;
            }
          
            yield return new WaitForSeconds(waitTime);
        }

        isAnimationProcessing = false;
    }
    /*
    public void SpeedSlide(float speed)
    {
        translateSpeed = speed;
    }
    */
    bool GreaterEqual(Vector3 a, Vector3 b)
    {
        return (a.x >= b.x) && (a.y >= b.y) && (a.z >= b.z);
    }
}
