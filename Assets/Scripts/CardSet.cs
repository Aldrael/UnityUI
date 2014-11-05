using UnityEngine;
using System.Collections;

public class CardSet : MonoBehaviour
{
    public Sprite[] cards;

    public bool rare;
    public int index;

    public Bounds boundsthis;

    public float factor_x;
    public float factor_y;
    public float factor_z;

    // Use this for initialization
    void Start()
    {
        rare = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setCard(int index)
    {
        Sprite backcard = GameObject.Find("CardBack").GetComponent<SpriteRenderer>().sprite;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject

        this.index = index;
        rare = isRare(index);
        spriteRenderer.sprite = cards[index];
        boundsthis = spriteRenderer.sprite.bounds;

        factor_x = (float)backcard.bounds.size.x / boundsthis.size.x;
        factor_y = (float)backcard.bounds.size.y / boundsthis.size.y;
        factor_z = (float)backcard.bounds.size.z / boundsthis.size.z;

        spriteRenderer.transform.localScale = new Vector3(factor_x, factor_y, factor_z);
   
    }

    bool isRare(int index)
    {
        switch (index)
        {
            case 0:
                return true;
            case 5:
                return true;
            case 10:
                return true;
            default:
                return false;
        }
    }
}
