using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ComboBox : ScriptableObject  {

	public List<ComboItem> items;

	bool allowMultiple;
	
	//We'll keep track of the last marked item for clean transition between selecting multiple and single
	private ComboItem lastMarked;
	
	//The combo item is the object that'll hold all the information for each item in the list
	//It will draw itself, tell us if it's marked, and hold its name and value
	//The name and value will be defined by the user.
	//Additionally, it has events we can attach to for when this item is selected or unselected.
	[Serializable]
	public class ComboItem {
		public string name;
		public System.Object value;
		public bool marked;

		public delegate void OnMarked(ComboItem item);
		public delegate void OnUnMarked(ComboItem item);

		public event OnMarked OnMarkedAction;
		public event OnUnMarked OnUnMarkedAction;

		public ComboItem(string name, System.Object value) {
			this.name = name;
			this.value = value;
			this.marked = false;
		}

		//Draw ourselves. In this case, when selected, just append a check mark to the name
		//Extending this to instead draw a textured checkbox or empty check box may be needed 
		// for stylistic requirements

		public void Draw(GUIStyle style) {
			GUILayout.BeginHorizontal();
			string renderName = name;
			if(this.marked)
				renderName += " ✔";

			if(GUILayout.Button(renderName, style)) {
				if(this.marked)
					UnMark();
				else
					Mark();
			}

			GUILayout.EndHorizontal();
		}

		public void Mark() {
			this.marked = true;
			OnMarkedAction(this);
		}

		public void UnMark() {
			this.marked = false;
			OnUnMarkedAction(this);
		}
	}

	public ComboBox() {
		items = new List<ComboItem>();
		allowMultiple = true;
	}

	private void OnComboItemMarked(ComboItem item) {
		//Debug.Log("Item: " + item.name + " marked");

		//keep track of last marked
		lastMarked = item;

		//Since ComboItems don't know about each other, we need to handle 
		// unmarking any other Combo items a level up, here in the ComboBox
		if(!allowMultiple) {
			foreach(ComboItem comboItem in items) {
				if(comboItem != item && comboItem.marked) {
					comboItem.UnMark();
				}
			}
		}
	}

	private void OnComboItemUnMarked(ComboItem item) {
		//Debug.Log("Item: " + item.name + " unmarked");

		//clear the last marked if we've unmarked it
		if(item == lastMarked) 
			lastMarked = null;
	}

	public ComboItem AddItem(string name, System.Object value) {
		ComboItem newItem = new ComboItem(name, value);
		this.items.Add(newItem);
		newItem.OnMarkedAction += OnComboItemMarked;
		newItem.OnUnMarkedAction += OnComboItemUnMarked;
		return newItem;
	}

	public ComboItem AddItem(string name, System.Object value,
	                         ComboItem.OnMarked markedDelegate,
	                         ComboItem.OnUnMarked unmarkedDelegate) {
		ComboItem newItem = new ComboItem(name, value);
		this.items.Add(newItem);
		newItem.OnMarkedAction += OnComboItemMarked;
		newItem.OnMarkedAction += markedDelegate;
		newItem.OnUnMarkedAction += OnComboItemUnMarked;
		newItem.OnUnMarkedAction += unmarkedDelegate;
		return newItem;
	}

	public void AddItem(ComboItem newItem) {
		this.items.Add(newItem);
		newItem.OnMarkedAction += OnComboItemMarked;
		newItem.OnUnMarkedAction += OnComboItemUnMarked;
	}

	public void Clear() {
		items.Clear();
	}

	public void Draw(GUIStyle style, Rect rect) {
		GUILayout.BeginArea(rect);
		GUILayout.BeginVertical();
		foreach(ComboItem item in items) {
			item.Draw(style);
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	public void SetAllowMultiple(bool allow) {
		allowMultiple = allow;
		if(!allowMultiple) {
			foreach(ComboItem comboItem in items) {
				if(comboItem != lastMarked && comboItem.marked) {
					comboItem.UnMark();
				}
			}
		}
	}

	public bool AllowsMultiple() {
		return allowMultiple;
	}
}
