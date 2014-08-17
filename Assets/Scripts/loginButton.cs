using UnityEngine;
using System.Collections;

public class loginButton : MonoBehaviour {
	public GameObject username;
	public GameObject password;
	private bool nextScreen = false;
	public GameObject popupPrefab;
	
	void Start(){
		GlobalInfo.currentLevel = 1;
		GlobalInfo.currentUser = "";

//		#if UNITY_ANDROID
//		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
//		#endif
		
//		#if UNITY_WEBPLAYER
//		StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"WEbPlayer",5f));
//		#endif
	}

	IEnumerator Login()
	{
		string string_URL = GlobalInfo.loginURL + "username=" + username.GetComponent<UIInput>().value + "&password=" + GlobalInfo.Md5Sum(password.GetComponent<UIInput>().value);
		WWW string_get = new WWW(string_URL);
		yield return string_get;
		
		if (string_get.error != null)
		{
			Debug.Log(string_get.error);
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"There was an error : " + string_get.error,1f));
		}
		else
		{
			int result;
			if (int.TryParse(string_get.text.Trim(),out result)){
				switch (string_get.text.Trim())
				{
				case "-1":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Sai mật khẩu!",1f));
					break;
				case "0":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Tài khoản không tồn tại!",1f));
					break;
				default:
					GlobalInfo.currentLevel = int.Parse(string_get.text.Trim());
					GlobalInfo.currentUser = username.GetComponent<UIInput>().value;
					nextScreen = true;
					break;
				}
			}
			else {
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,string_get.text.Trim(),1f));
			}
		}
	}

	void OnClick(){
		StartCoroutine(Login());
	}

	void Update(){
		if (nextScreen)
		{
			if(GlobalInfo.currentLevel <= GlobalInfo.maxLevel){
				Application.LoadLevel("Game");
			}
			else{
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Bạn đã hoàn thành game rồi!",1f));
			}
		}
		nextScreen = false;
	}
}