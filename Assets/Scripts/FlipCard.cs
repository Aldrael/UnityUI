using UnityEngine;
using System.Collections;

public class FlipCard : MonoBehaviour {

    public int fps = 60;
    public float rotateDegreePerSecond;
    public bool isFaceUp = false;

    const float FLIP_LIMIT_DEGREE = 180f;

    float waitTime;
    bool isAnimationProcessing = false;

    public Quaternion originalRotationValue;

	// Use this for initialization
	void Start () {
        waitTime = 1.0f / fps;
        rotateDegreePerSecond = 180f;
        originalRotationValue = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        if (isAnimationProcessing || isFaceUp)
        {
            return;
        }
        StartCoroutine( flip() );
    }

    IEnumerator flip()
    {
        isAnimationProcessing = true;

        bool done = false;
        while (!done)
        {
  
            float degree = rotateDegreePerSecond * Time.deltaTime;

            transform.Rotate(new Vector3(0, degree, 0));

            if (FLIP_LIMIT_DEGREE < transform.eulerAngles.y)
            {
                transform.Rotate(new Vector3(0, -(transform.eulerAngles.y - FLIP_LIMIT_DEGREE), 0));
                done = true;
            }

            yield return new WaitForSeconds(waitTime);
        }

        CameraScript manager = GameObject.FindWithTag("Manager").GetComponent<CameraScript>();
        int cardObtained = GetComponentInChildren<RandomCard>().Index;
        manager.cardCounter(cardObtained);

        isFaceUp = !isFaceUp;
        isAnimationProcessing = false;
    }
    public void resetCard()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * 1.0f);
        isFaceUp = false;
        GetComponentInChildren<RandomCard>().randomizeCards();
    }

    public void SpeedSlide(float speed)
    {
        rotateDegreePerSecond = speed;
    }

}
