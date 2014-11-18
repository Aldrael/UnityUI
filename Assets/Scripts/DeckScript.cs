using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class DeckScript : MonoBehaviour {
    public List<int> cardsObtained = new List<int>();
    public string aStringObject = "test";
    public float aFloatValue = 43.2f;
    DeckScript deck;
    public int bankAmount = 100;
	// Use this for initialization
	void Start () {
        gameObject.tag = "Deck";
        //cardsObtained.Add(Random.Range(0, 14));   //Extra
        //DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update () {
	
	}

    public void SaveCards(List<int> cards)
    {
        cardsObtained.AddRange(cards);
    }

    public void SaveCard(int card)
    {
        cardsObtained.Add(card);
    }

    public void WriteObjectState(BinaryWriter binaryWriter)
    {
        //Get all the subObjects that are children of this object.
        SimpleSubObject[] subObjects = this.transform.GetComponentsInChildren<SimpleSubObject>();
        //Write out how many objects there are, so we know how many to read in later
        binaryWriter.Write(subObjects.Length);

        //Each object is responsible for writing its own state.
        foreach (SimpleSubObject subObject in subObjects)
        {
            subObject.WriteObjectState(binaryWriter);
        }

        //SaveCards(manager.cardsObtained);
        string saveCards = string.Join(",", cardsObtained.ConvertAll(i => i.ToString()).ToArray());

        //Now write our own state
        binaryWriter.Write(aStringObject);
        binaryWriter.Write(aFloatValue);
        binaryWriter.Write(bankAmount);
        binaryWriter.Write(saveCards);

        binaryWriter.Write(this.gameObject.name);
    }

    public void ReadObjectState(BinaryReader binaryReader)
    {
        CameraScript manager = GameObject.Find("_Manager").GetComponent<CameraScript>();
        //Get the subObjects count
        int simpleSubCount = binaryReader.ReadInt32();
        for (int subCount = 0; subCount < simpleSubCount; subCount++)
        {
            GameObject simpleSub = new GameObject();
            SimpleSubObject simpleSubScript = simpleSub.AddComponent<SimpleSubObject>();
            simpleSubScript.ReadObjectState(binaryReader);
            simpleSub.transform.parent = this.transform;
        }

        this.aStringObject = binaryReader.ReadString();
        this.aFloatValue = binaryReader.ReadSingle();
        int bankAmount = binaryReader.ReadInt32();

        List<int> received = new List<int>();
        string cards = binaryReader.ReadString();
            foreach (string card in cards.Split(','))
            {
                int num;
                if (int.TryParse(card, out num))
                    received.Add(num);
            }

            SaveCards(received);

            this.bankAmount = bankAmount;
            manager.SetBankText(bankAmount);
        this.gameObject.name = binaryReader.ReadString();

    }

    public void WriteObjectState_Web(string prependKey)
    {
        //Get all the subObjects that are children of this object.
        SimpleSubObject[] subObjects = this.transform.GetComponentsInChildren<SimpleSubObject>();
        //Write out how many objects there are, so we know how many to read in later
        PlayerPrefs.SetInt(prependKey + "subObjectCount", subObjects.Length);

        //Each object is responsible for writing its own state.
        //Maintain a subCount variable to add distinction between objects
        int subCount = 0;
        foreach (SimpleSubObject subObject in subObjects)
        {
            subObject.WriteObjectState_Web(prependKey + subCount++);
        }

        PlayerPrefs.SetString(prependKey + "aStringObject", aStringObject);
        PlayerPrefs.SetFloat(prependKey + "aFloatValue", aFloatValue);

        PlayerPrefs.SetString(prependKey + "objectName", this.gameObject.name);
    }

    public void ReadObjectState_Web(string prependKey)
    {
        //Get the subObjects count
        int simpleSubCount = PlayerPrefs.GetInt(prependKey + "subObjectCount");
        for (int subCount = 0; subCount < simpleSubCount; subCount++)
        {
            GameObject simpleSub = new GameObject();
            SimpleSubObject simpleSubScript = simpleSub.AddComponent<SimpleSubObject>();
            simpleSubScript.ReadObjectState_Web(prependKey + subCount);
            simpleSub.transform.parent = this.transform;
        }

        this.aStringObject = PlayerPrefs.GetString(prependKey + "aStringObject");
        this.aFloatValue = PlayerPrefs.GetFloat(prependKey + "aFloatValue");

        this.gameObject.name = PlayerPrefs.GetString(prependKey + "objectName");
    }
}
