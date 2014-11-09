using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]

public class Packs : MonoBehaviour
{
    Vector3 screenPoint;
    Vector3 offset;
    CameraScript cs;
    // Use this for initialization
    void Start()
    {
        cs = GameObject.Find("_Manager").GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localPosition.x > -30f && gameObject.transform.localPosition.x < 30f &&
            gameObject.transform.localPosition.y > -30f && gameObject.transform.localPosition.y < 30f)
            cs.notInZone = false;
        else
        {
            cs.notInZone = true;
        }
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
