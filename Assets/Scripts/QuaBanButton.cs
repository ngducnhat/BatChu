using UnityEngine;
using System.Collections;
using System;

public class QuaBanButton : MonoBehaviour {

	public static string secretKey = "matkhau";	
	public static bool updatedScore = false;
	public static bool sendScore = false;

	IEnumerator AddScore(){
		string hash = GlobalInfo.Md5Sum(GlobalInfo.currentUser + GlobalInfo.currentLevel.ToString() + secretKey);
		string string_URL = GlobalInfo.addScoreURL + "currentUser=" + Uri.EscapeDataString(GlobalInfo.currentUser) + "&currentLevel=" + GlobalInfo.currentLevel + "&hash=" + hash;
		Debug.Log(string_URL);
		WWW string_get = new WWW(string_URL);
		yield return string_get;

		if (string_get.error != null)
		{
			Application.ExternalCall("alert","There was an error : " + string_get.error);
			
		}
		else
		{
			Application.ExternalCall("alert",string_get.text);
		}
		updatedScore = true;
	}

	void OnClick(){
		Application.ExternalCall("alert","Bạn trả lời đúng rồi");
		GlobalInfo.currentLevel ++;		

		//							
		//Send score to server
		sendScore = true;
	}

	void Update(){
		if (sendScore){
			sendScore = false;
			StartCoroutine(AddScore());
		}
		
		if(updatedScore){
			updatedScore = false;
			Application.LoadLevel("Game");
		}
	}
}
