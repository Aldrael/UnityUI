using UnityEngine;
using System.Collections;

public class MainCard : MonoBehaviour {
    public bool toggle = true;
    //bool isAnimationProcessing;
    //float waitTime;
    //bool done;
    public float translateSpeed;
    const float maxZoom = -60f;
    public bool moved;
    public Transform gem1, thunder;
    bool hasGem;
    Object gemPiece, thunder_obj;
    bool doneEnable;
	// Use this for initialization
	void Start () {
        gameObject.tag = "Maincard";
        //waitTime = 1.0f / 60;
        //done = false;
        translateSpeed = 180f;
        moved = false;
        //isAnimationProcessing = false;
        hasGem = false;
        doneEnable = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleCard()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }

    public void enableCard()
    {
        doneEnable = false;
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(270f, 0, 0);
        thunder_obj = Instantiate(thunder, new Vector3(transform.position.x, transform.position.y, transform.position.z - 10), rotation);
        gameObject.SetActive(true);
        SpriteRenderer[] sr = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sr)
        {
            s.color = new Vector4(1f, 1f, 1f, 0f);
        }
        StartCoroutine(waitThunder(sr));
    }

    IEnumerator waitThunder(SpriteRenderer[] sr)
    {
        float time = 0;
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            sr[0].color = new Vector4(1f, 1f, 1f, time);
            yield return new WaitForSeconds(1.0f / 60);
        }
        sr[1].color = new Vector4(1f, 1f, 1f, 1f);
        DestroyObject((thunder_obj as Transform).gameObject);
        doneEnable = true;
    }

    public void disableCard()
    {
        gameObject.SetActive(false);
    }

    void OnMouseOver()
    {
        if (!doneEnable) return;
        if(!moved)transform.Translate(0, 0, -10f, Space.Self);
        moved = true;
        if (GetComponent<FlipCard>().isFlipped())
        {
            hasGem = false;
            if(gemPiece != null)Destroy((gemPiece as Transform).gameObject);
            return;
        }
        if (!hasGem)
        {
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(270f, 0, 0);
            gem1.localScale = new Vector3(1f, 1f, 1f);
            gemPiece = Instantiate(gem1, new Vector3(transform.position.x, transform.position.y + 15, transform.position.z), rotation);
            hasGem = true;
            StartCoroutine(rotate());
        }
        /*
        if (isAnimationProcessing || done)
        {
            return;
        }
        StartCoroutine(translate());
        */
    }

    IEnumerator rotate()
    {
        while (hasGem)
        {
            float degree = 270f * Time.deltaTime;
            Transform transform = gemPiece as Transform;
            transform.Rotate(new Vector3(0, 0, degree));
            yield return new WaitForSeconds(1.0f / 60);
        }
    }

    void OnMouseExit()
    {
        hasGem = false;
        if (gemPiece != null) Destroy((gemPiece as Transform).gameObject);
        if (GetComponent<FlipCard>().isFlipped())
        {
            return;
        }
        if (moved)
        {
            resetZoom();
        }
        moved = false;
    }
    /*
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
     */
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

    public void resetZoom()
    {
        transform.Translate(0, 0, 10f, Space.Self);
    }
}
