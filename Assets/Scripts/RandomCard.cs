using UnityEngine;
using System.Collections;

public class RandomCard : MonoBehaviour
{

    public Sprite[] cards;
    const int randomReroll = 1;
    public bool rare;
    public int rareIndex;
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

    CameraScript manager;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("_Manager").GetComponent<CameraScript>();
        randomizeCards(manager.currentPack, false);
        gameObject.tag = "Card";
        rare = false;
        rareIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void randomizeCards(int booster, bool cheat)
    {
        Sprite backcard = GameObject.Find("CardBack").GetComponent<SpriteRenderer>().sprite;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject

        int lowrange = 0;
        int highrange = 4;
        for (int i = booster; i > 0; i--)
        {
            lowrange += 5;
            highrange += 5;
        }

        index = Random.Range(lowrange, highrange);
        int reroll = randomReroll;
        while ((reroll > 0) && (isRare(index)))
        {
            index = Random.Range(lowrange, highrange);
            reroll--;
        }

        if (cheat) index = 0;
        rare = isRare(index);
        spriteRenderer.sprite = cards[index];
        boundsthis = spriteRenderer.sprite.bounds;

        factor_x = (float)backcard.bounds.size.x / boundsthis.size.x;
        factor_y = (float)backcard.bounds.size.y / boundsthis.size.y;
        factor_z = (float)backcard.bounds.size.z / boundsthis.size.z;

        spriteRenderer.transform.localScale = new Vector3(-factor_x, factor_y, factor_z);
    }

    bool isRare(int index)
    {
        switch (index)
        {
            case 0:
                rareIndex = 0;
                return true;
            case 5:
                rareIndex = 1;
                return true;
            case 10:
                rareIndex = 2;
                return true;
            default:
                rareIndex = 0;
                return false;
        }
    }

    public void disableObject()
    {
        gameObject.SetActive(false);
    }

    public void enableObject()
    {
        gameObject.SetActive(true);
    }
}
