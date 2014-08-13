using UnityEngine;
using System.Collections;

public class answerBox : MonoBehaviour {
	public int hintBoxPos;

	void OnClick(){
		if(gameObject.GetComponent<answerBox>().hintBoxPos !=-1){
			GameManager.hintBox[gameObject.GetComponent<answerBox>().hintBoxPos].transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<answerBox>().hintBoxPos =-1;
			gameObject.GetComponentInChildren<UILabel>().text="";
		}
	}
}
