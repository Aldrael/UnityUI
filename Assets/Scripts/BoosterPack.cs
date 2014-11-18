using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoosterPack : MonoBehaviour
{
    CanvasManager canvas;
    CameraScript manager;
    List<GameObject> pieces;
    GameObject[] packButtons;
    Packs packs;
    // Use this for initialization
    void Start()
    {
        gameObject.tag = "Boosterpack";
        pieces = new List<GameObject>();
        //canvas = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        manager = GameObject.Find("_Manager").GetComponent<CameraScript>();
        packButtons = GameObject.FindGameObjectsWithTag("Packselection");
        packs = GameObject.Find("Packs").GetComponent<Packs>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSpriteSliced(SpriteSlicer2DSliceInfo sliceInfo)
    {
        if (manager.currentPack == 0)
        {
            FloatingText.Show("-$10!", "PointStarText", new FromWorldPointTextPositioner(Camera.main, new Vector3(0, 0, 500f), 1.75f, 50));
            manager.deck.bankAmount -= 10;
            manager.SetBankText(manager.deck.bankAmount);
        }
        else if (manager.currentPack == 1)
        {
            FloatingText.Show("-$8!", "PointStarText", new FromWorldPointTextPositioner(Camera.main, new Vector3(0, 0, 500f), 1.75f, 50));
            manager.deck.bankAmount -= 8;
            manager.SetBankText(manager.deck.bankAmount);
        }
        else
        {
            FloatingText.Show("-$11!", "PointStarText", new FromWorldPointTextPositioner(Camera.main, new Vector3(0, 0, 500f), 1.75f, 50));
            manager.deck.bankAmount -= 11;
            manager.SetBankText(manager.deck.bankAmount);
        }
        //print("Sliced");
        //rigidbody2D.AddForce(new Vector2( (Random.Range(-200f,200f)) , (Random.Range(1000f,10000f))));
        packs.haloOn = false;
        packButtons = GameObject.FindGameObjectsWithTag("Packselection");
        manager.disableCut();
        manager.packholder.GetComponent<BoxCollider>().enabled = false;
        foreach (GameObject packButton in packButtons)
        {
            packButton.GetComponent<PackSelection>().disableButton();
        }
        pieces = sliceInfo.ChildObjects;
        //canvas.getPackPieces(pieces);
        manager.getPackPieces(pieces);
        float lowrange, highrange;
        int loop = 1;
        foreach (GameObject piece in pieces)
        {
            float mass = piece.GetComponent<Rigidbody2D>().mass;
            int randomx = Random.Range(0, 2);
            float forcex;
            lowrange = (1000 * mass);
            highrange = (10000 * mass);

            if (randomx == 0)
            {
                forcex = (Random.Range(-highrange, -lowrange));
            }
            else
            {
                forcex = (Random.Range(lowrange, highrange));
            }
            int randomy = Random.Range(0, 2);
            float forcey;
            if (randomy == 0)
            {
                forcey = (Random.Range(-highrange, -lowrange));
            }
            else
            {
                forcey = (Random.Range(lowrange, highrange));
            }
            piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcex, forcey));

            loop++;
           // StartCoroutine(vanish(tr));
        }
    }

    IEnumerator vanish(Transform tr)
    {
        //SpriteRenderer sr = piece.GetComponent<SpriteRenderer>();
        //MeshRenderer sr = piece.GetComponent<MeshRenderer>();
        
        if (tr != null)
        {
           // yield return new WaitForSeconds(1);
            print("Beginning loop");
            while (tr.localScale.x > 0)
            {
                tr.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                yield return new WaitForSeconds(1.0f / 60);
            }
            print("Out");
        }
        else
        {
            print("Missing tr");
        }
    }


}
