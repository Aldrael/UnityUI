using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanvasManager : MonoBehaviour {
    const float scale = 70f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getPackPieces(List<GameObject> pieces)
    {
        foreach (GameObject piece in pieces)
        {
            StartCoroutine(changeAlpha(piece));
        }
    }

    IEnumerator changeAlpha(GameObject piece)
    {
        yield return new WaitForSeconds(1);
        float thisscale = scale;
        while (thisscale > 0)
        {
            thisscale -= 1f;
            piece.GetComponent<Transform>().localScale = new Vector3(thisscale, thisscale, thisscale);
            yield return new WaitForSeconds(1.0f / 60);
        }
        
    }
}
