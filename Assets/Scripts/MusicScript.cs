using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class MusicScript : MonoBehaviour {
    AudioSource audioSource;
    public AudioClip[] clips;
    Text current;
    StringBuilder theText;
    private int songindex;
    private const string header = "Currently Playing:";

	// Use this for initialization
	void Start () {

        audioSource = gameObject.GetComponent<AudioSource>();
        current = GameObject.Find("CurrentText").GetComponent<Text>();
        songindex = 0;
        playClip();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void playClip()
    {
        if (songindex < 0)
        {
            songindex = clips.Length - 1;
        }
        else if (songindex >= clips.Length)
        {
            songindex = 0;
        }
        audioSource.clip = clips[songindex];
        audioSource.Play();   
        theText = new StringBuilder();
        theText.AppendLine(header).AppendLine(clips[songindex].name);
        current.text = theText.ToString();
    }

    public void changeClip(int index)
    {
        songindex = index;
        audioSource.Stop();
        playClip();
    }

    public void increClip()
    {
        audioSource.Stop();
        songindex++;
        playClip();
    }

    public void decreClip()
    {
        audioSource.Stop();
        songindex--;
        playClip();
    }
}
