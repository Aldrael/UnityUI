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
            lowrange = (1000 * mass) / loop;
            highrange = (10000 * mass) / loop;

            if (randomx == 0)
            {
                forcex = (Random.Range(-highrange, -lowrange));
            }
            else
            {
                forcex = (Random.Range(lowrange, highrange));
            }
            piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(forcex, (Random.Range(lowrange, highrange))));
            //piece.AddComponent<BoosterPack>();
            //Transform tr = piece.GetComponent<Transform>();
            //piece.GetComponent<MeshRenderer>().renderer.enabled = false;
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
