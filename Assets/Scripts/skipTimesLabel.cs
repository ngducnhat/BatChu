using UnityEngine;
using System.Collections;

public class skipTimesLabel : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UILabel>().text = GlobalInfo.point.ToString();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
