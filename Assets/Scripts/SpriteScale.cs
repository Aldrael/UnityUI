using UnityEngine;
using System.Collections;

public class SpriteScale : MonoBehaviour {
    SpriteRenderer sr;

    Vector3 designScaleW = Vector3.zero; //scale in world space
    float designSizeW = 0f; // size of the sprite in x in world space.
    float designScaleV = 0f; // viewport size of the sprite- percent of x that the sprite takes.
    public Vector2 designScreen = Vector2.zero; // the pixel size of the screen
    float designScreenWidth = 0f; // the size of the width of the screen in units.    

    float wScreenWidth = 0f; //how many units wide the screen is- "w" means world space.
    float wScreenHeight = 0f; //how many units tall the screen is
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("Gameobject doesn't have a SpriteRenderer!");
        }

        designScaleW = transform.localScale; //save the original scale.
        designSizeW = sr.sprite.bounds.size.x * designScaleW.x; //save the original size, at the original scale.
        designScreenWidth = (Camera.main.orthographicSize * 2f) / designScreen.y * designScreen.x; //get the width of the design screen
        designScaleV = designSizeW / designScreenWidth; //get the design viewport size. this is the target for the new resolution.
	}
	
	// Update is called once per frame
	void Update () {
        wScreenHeight = Camera.main.orthographicSize * 2f; // calculation for how many units tall the screen is.
        wScreenWidth = wScreenHeight / Screen.height * Screen.width; // calculation for how many units wide the screen is.

        //We use the amount of space it took up in design to calculate what the scale needs to be now.
        float desiredSize = designScaleV * wScreenWidth; //The desired size is the design viewport size times the width of the screen in world units
        float xScale = desiredSize / sr.sprite.bounds.size.x; //calculate the scale in the x axis.
        float yScale = (xScale - designScaleW.x) + designScaleW.y; //calculate the scale in the y axis.
        Vector3 spriteScale = new Vector3(xScale, yScale, 1);
        transform.localScale = spriteScale; //Set the scale!
	}
}
