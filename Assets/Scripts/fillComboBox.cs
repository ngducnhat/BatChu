using UnityEngine;
using System.Collections;

public class fillComboBox : MonoBehaviour {
	public string[] phongBan = {"Giam Doc","Ke Toan","Bao ve"};

	// Use this for initialization
	void Start () {
		foreach (string w in phongBan)
		{
			gameObject.GetComponent<UIPopupList>().items.Add(w);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
