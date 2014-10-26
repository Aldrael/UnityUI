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

    private SpriteRenderer spriteRenderer;
    public Sprite backcard;
    // Use this for initialization
    void Start()
    {
        backcard = GameObject.Find("CardBack").GetComponent<SpriteRenderer>().sprite;

        spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        randomizeCards();

        gameObject.tag = "Card";
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space)) // If the space bar is pushed down
        {
            index++;
            if (index >= cards.Length) index = 0;
            ChangeSprite(); // call method to change sprite
        }
         */
    }

    void ChangeSprite()
    {
        spriteRenderer.sprite = cards[index];
        setNewBounds();

    }

    void setNewBounds()
    {
        boundsthis = spriteRenderer.sprite.bounds;

        factor_x = backcard.bounds.size.x / boundsthis.size.x;
        factor_y = backcard.bounds.size.y / boundsthis.size.y;
        factor_z = backcard.bounds.size.z / boundsthis.size.z;

        spriteRenderer.transform.localScale = new Vector3(-factor_x, factor_y, factor_z);
    }

    public void randomizeCards()
    {
        index = Random.Range(0, cards.Length);
        ChangeSprite();
    }

}
