using UnityEngine;
using System.Collections;

public class connectButton : MonoBehaviour {

	public GameObject serverURL;
	private string resultText;
	private bool checkedConnection = false;
	public GameObject popupPrefab;

//	void OnClick(){
//		if (serverURL.GetComponent<UIInput>().value != ""){
//			GlobalInfo.addScoreURL = "http://" + serverURL.GetComponent<UIInput>().value + GlobalInfo.addScoreURL0.Substring(16,GlobalInfo.addScoreURL0.Length-16);
//			GlobalInfo.highscoreURL = "http://" + serverURL.GetComponent<UIInput>().value + GlobalInfo.highscoreURL0.Substring(16,GlobalInfo.highscoreURL0.Length-16);
//			GlobalInfo.loginURL = "http://" + serverURL.GetComponent<UIInput>().value + GlobalInfo.loginURL0.Substring(16,GlobalInfo.loginURL0.Length-16);
//			GlobalInfo.signupURL = "http://" + serverURL.GetComponent<UIInput>().value + GlobalInfo.signupURL0.Substring(16,GlobalInfo.signupURL0.Length-16);
//			GlobalInfo.checkConnectionURL = "http://" + serverURL.GetComponent<UIInput>().value + GlobalInfo.checkConnectionURL0.Substring(16,GlobalInfo.checkConnectionURL0.Length-16);
//			Debug.Log(GlobalInfo.checkConnectionURL);
//			StartCoroutine(checkConnection());
//		}
//	}

	IEnumerator checkConnection()
	{
		WWW hs_get = new WWW(GlobalInfo.checkConnectionURL);
		yield return hs_get;

		if (hs_get.error != null)
		{
			resultText = "There was an error: " + hs_get.error;
			checkedConnection = true;
		}
		else
		{			
			resultText = hs_get.text;
			checkedConnection = true;
		}
	}

	void Update(){
		if (checkedConnection){
			if (resultText.Trim() == "1")
			{
				Application.LoadLevel("Login");
				checkedConnection = false;
			} 
			else {
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Không thể kết nối đến server: " + resultText,3f));
				checkedConnection = false;

			}
		}
	}


}
