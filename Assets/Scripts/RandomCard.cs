using UnityEngine;
using System.Collections;

public class RandomCard : MonoBehaviour
{

    public Sprite[] cards;


    private int index;
    public int Index
    {
        get
        {
            return index;
        }
        set
        {
            index = value;
        }
    }
    public Bounds boundsthis;

    public float factor_x;
    public float factor_y;
    public float factor_z;

    // Use this for initialization
    void Start()
    {
        randomizeCards();
        gameObject.tag = "Card";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void randomizeCards()
    {
        Sprite backcard = GameObject.Find("CardBack").GetComponent<SpriteRenderer>().sprite;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject

        index = Random.Range(0, cards.Length);
        spriteRenderer.sprite = cards[index];
        boundsthis = spriteRenderer.sprite.bounds;

        factor_x = (float)backcard.bounds.size.x / boundsthis.size.x;
        factor_y = (float)backcard.bounds.size.y / boundsthis.size.y;
        factor_z = (float)backcard.bounds.size.z / boundsthis.size.z;

        spriteRenderer.transform.localScale = new Vector3(-factor_x, factor_y, factor_z);
    }

}
