using UnityEngine;
using System.Collections;

public class loginButton : MonoBehaviour {
	public GameObject username;
	public GameObject password;
	private bool nextScreen = false;
	public GameObject popupPrefab;
	
	void Start(){
		GlobalInfo.currentLevel = 1;
		GlobalInfo.skipTimes = 0;
		GlobalInfo.point = 0;
		GlobalInfo.currentUser = "";
		GlobalInfo.openedPos = 0;
	}

	IEnumerator Login()
	{
		string string_URL = GlobalInfo.loginURL + "username=" + username.GetComponent<UIInput>().value + "&password=" + GlobalInfo.Md5Sum(password.GetComponent<UIInput>().value) + "&version=" + GlobalInfo.version;
		WWW string_get = new WWW(string_URL);
		yield return string_get;
		
		if (string_get.error != null)
		{
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"There was an error : " + string_get.error,1f));
		}
		else
		{
			int result;
			string[] textTemp = string_get.text.Trim().Split('|');
			if (int.TryParse(textTemp[0],out result)){
				switch (string_get.text.Trim())
				{
				case "-2":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Sai phiên bản, hay cập nhật phiên bản mới!",1f));
					break;
				case "-1":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Sai mật khẩu!",1f));
					break;
				case "0":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Tài khoản không tồn tại!",1f));
					break;
				default:
					GlobalInfo.currentLevel = int.Parse(textTemp[0]);
					GlobalInfo.skipTimes = int.Parse(textTemp[1]);
					GlobalInfo.point = int.Parse(textTemp[1]);
					GlobalInfo.openedPos = int.Parse(textTemp[2]);
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