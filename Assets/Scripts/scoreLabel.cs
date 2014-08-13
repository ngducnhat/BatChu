using UnityEngine;
using System.Collections;

public class scoreLabel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UILabel>().text = GlobalInfo.currentLevel.ToString();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
