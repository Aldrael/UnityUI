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
    public bool hasGem;
    Object gemPiece, thunder_obj;
    bool doneEnable;
    float hoverOffset = 40f;
    SpriteRenderer[] sr;
    Vector3 originalPosition;
    CameraScript manager;
    Vector3[] positions;
    public bool notSelected;
    public int slotOccupied;

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
        notSelected = false;
        originalPosition = gameObject.transform.position;
        manager = GameObject.Find("_Manager").GetComponent<CameraScript>();
        slotOccupied = 0;
        //initializePositions();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void toggleCard()
    {
        toggle = !toggle;
        gameObject.SetActive(toggle);
    }

    public void enableCard(int index)
    {
        //initializePositions();
        doneEnable = false;
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(0,0,470f);
        originalPosition = gameObject.transform.position;
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(270f, 0, 0);
        thunder_obj = Instantiate(thunder, new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - 10), rotation);
        sr = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in sr)
        {
            s.color = new Vector4(1f, 1f, 1f, 0f);
        }
        StartCoroutine(waitThunder(sr, index));
    }

    IEnumerator waitThunder(SpriteRenderer[] sr, int index)
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            sr[0].color = new Vector4(1f, 1f, 1f, time * 2);
            yield return new WaitForSeconds(1.0f / 60);
        }
        sr[1].color = new Vector4(1f, 1f, 1f, 1f);
        iTween.MoveTo(gameObject, positions[index], 0.5f);
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            yield return new WaitForSeconds(1.0f / 60);
        }
        DestroyObject((thunder_obj as Transform).gameObject);

        //yield return new WaitForSeconds(1);
        doneEnable = true;
    }

    public void disableCard()
    {
        gameObject.SetActive(false);
    }

    void OnMouseOver()
    {
        if (!doneEnable || notSelected) return;
        if (!moved)
        {
            translateUp();
        }
        if (hasGem && Input.GetMouseButtonDown(0))
        {
            destroyGem();
            return;
        }
        if (!hasGem && Input.GetMouseButtonDown(0))
        {
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(270f, 0, 0);
            gem1.localScale = new Vector3(5f, 5f, 5f);
            gemPiece = Instantiate(gem1, new Vector3(transform.position.x, transform.position.y + 80, transform.position.z), rotation);
            hasGem = true;
            StartCoroutine(rotate());
            GetComponent<FlipCard>().isReady = true;
            manager.cardsSelected++;
            return;
        }
        
        /*
        if (GetComponent<FlipCard>().isFlipped())
        {
            hasGem = false;
            if(gemPiece != null)Destroy((gemPiece as Transform).gameObject);
            return;
        }
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
        if (hasGem || notSelected)
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
        transform.Translate(0, 0, hoverOffset, Space.Self);
    }

    public void moveCard1()
    {
        StartCoroutine(moveCard("CardPath1"));
    }

    public void moveCard2()
    {
        StartCoroutine(moveCard("CardPath2"));
    }
    public void moveCard3()
    {
        StartCoroutine(moveCard("CardPath3"));
    }
    public void moveCard4()
    {
        StartCoroutine(moveCard("CardPath4"));
    }
    public void moveCard5()
    {
        StartCoroutine(moveCard("CardPath5"));
    }

    IEnumerator moveCard(string cardpath)
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(cardpath), "time", 4));
        yield return new WaitForSeconds(3f);
        float alpha = 1f;
        sr[0].color = new Vector4(1f, 1f, 1f, 0f);
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            sr[1].color = new Vector4(1f, 1f, 1f, alpha);
            yield return new WaitForSeconds(1.0f / 60);
        }
        //print("done");
        iTween.MoveTo(gameObject, originalPosition, 0f);
        manager.cards_up--;
        manager.newCardFlag = true;
    }
    public void initializePositions(Vector3[] original)
    {
        /*
        positions = new Vector3[5];
        positions[0] = new Vector3(-78.1f, 46.0f, 491.4f);
        positions[1] = new Vector3(-0.7f, 71f, 491.4f);
        positions[2] = new Vector3(81.4f, 44.0f, 491.4f);
        positions[3] = new Vector3(-45.9f, -64.2f, 491.4f);
        positions[4] = new Vector3(46.3f, -66.5f, 491.4f);
        */
        positions = new Vector3[5];
        positions = original;
    }

    public void translateUp() {
        transform.Translate(0, 0, -hoverOffset, Space.Self);
        Quaternion oldrotation = Quaternion.identity;
        oldrotation.eulerAngles = new Vector3(315f, 0, 0);
        moved = true;
    }

    public void destroyGem()
    {
        hasGem = false;
        if (gemPiece != null) Destroy((gemPiece as Transform).gameObject);
    }
}
