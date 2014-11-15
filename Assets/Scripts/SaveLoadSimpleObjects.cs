using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SaveLoadSimpleObjects : MonoBehaviour {
	public string saveFile = @"SaveFile.save";
    GameObject message;

    void Start()
    {
        message = GameObject.Find("DiskMessage");
        message.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
            GameObject deck = GameObject.Find("Deck");
            Destroy(deck);
			ReadSimpleObjects();
		}

		if(Input.GetKeyDown(KeyCode.S)) {
			WriteSimpleObjects();
		}
	}

    public void SaveCards()
    {
        WriteSimpleObjects();
        message.SetActive(true);
        message.GetComponent<Text>().text = "Cards successfully written to disk";
        StartCoroutine(TimeText());
    }

    public void LoadCards()
    {
        GameObject deck = GameObject.Find("Deck");
        Destroy(deck);
        ReadSimpleObjects();
        message.SetActive(true);
        message.GetComponent<Text>().text = "Cards successfully read from disk";
        StartCoroutine(TimeText());
    }

    IEnumerator TimeText()
    {
        yield return new WaitForSeconds(3);
        message.SetActive(false);
    }

	void ReadSimpleObjects() {
		if(File.Exists(saveFile)) {
			using(FileStream fs = File.OpenRead(saveFile)) {
				BinaryReader fileReader = new BinaryReader(fs);
				int simpleObjectCount = fileReader.ReadInt32();
				for(int simpleCount = 0; simpleCount < simpleObjectCount; simpleCount++) {
					GameObject deckScript = new GameObject();
					DeckScript simpleScript = deckScript.AddComponent<DeckScript>();
					simpleScript.ReadObjectState(fileReader);
				}
			}
		}
	}

	void WriteSimpleObjects() {
		//using statement will dispose of the object inside when we're done using it.
		//This is important for objects like files, that we don't want to leave open.
		using(FileStream fs = File.OpenWrite(saveFile)) {
			DeckScript[] deckScripts = UnityEngine.Object.FindObjectsOfType<DeckScript>();
			BinaryWriter fileWriter = new BinaryWriter(fs);
			fileWriter.Write(deckScripts.Length);
			foreach (DeckScript deckScript in deckScripts) {
				deckScript.WriteObjectState(fileWriter);
			}
		}
	}
}
