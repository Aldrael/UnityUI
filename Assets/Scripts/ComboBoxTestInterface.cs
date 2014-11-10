using UnityEngine;
using System.Collections;

public class ComboBoxTestInterface : MonoBehaviour {

	ComboBox cb;
	GUIStyle style;


	// Use this for initialization
	void Start() {
		cb = new ComboBox();
		style = new GUIStyle();
		foreach(Transform child in this.transform) {
			ComboBox.ComboItem childItem = new ComboBox.ComboItem(child.name, child);
			childItem.OnMarkedAction += SelectItem;
			childItem.OnUnMarkedAction += DeselectItem;
			cb.AddItem(childItem);
		}
	}
	
	// Update is called once per frame
	void Update() {
		if(Input.GetKeyDown(KeyCode.Tab)) {
			cb.SetAllowMultiple(!cb.AllowsMultiple());
		}
	}

	void SelectItem(ComboBox.ComboItem item) {
		//Move the item up so we can see it's selected
		((Transform)item.value).Translate(0,1,0);
	}

	void DeselectItem(ComboBox.ComboItem item) {
		//Move the item down so we can see it's not selected
		((Transform)item.value).Translate(0,-1,0);
	}

	void OnGUI() {
		if(cb.AllowsMultiple())
			GUI.Label(new Rect(30, 10, 180, 20), "Please select items");
		else
			GUI.Label(new Rect(30, 10, 180, 20), "Please select an item");
		cb.Draw(style, new Rect(30, 30, 180, 400));
	}
}
