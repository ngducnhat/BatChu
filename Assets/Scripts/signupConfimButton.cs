using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class signupConfimButton : MonoBehaviour {

	public GameObject username;
	public GameObject password;
	public GameObject fullName;
	public GameObject department;
	public GameObject popupPrefab;

	private bool signUpSuccess = false;
	private bool waitDone = false;

	IEnumerator SignUp(){
		if((username.GetComponent<UIInput>().value == "") || (password.GetComponent<UIInput>().value == "") || (fullName.GetComponent<UIInput>().value == "") || (department.GetComponent<UIPopupList>().value == "")){
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Hãy điền đầy đủ thông tin!",1f));
			yield return null;
		} else {
			string string_URL = "username=" + Uri.EscapeDataString(username.GetComponent<UIInput>().value) + "&password=" + Uri.EscapeDataString(password.GetComponent<UIInput>().value) + "&fullName=" + Uri.EscapeDataString(fullName.GetComponent<UIInput>().value) + "&department=" + Uri.EscapeDataString(department.GetComponent<UIPopupList>().value);
			string_URL = GlobalInfo.signupURL + string_URL;
			WWW string_get = new WWW(string_URL);
			yield return string_get;

			if (string_get.error != null)
			{
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"There was an error : " + string_get.error,1f));
			}
			else
			{
				string[] errorString = Regex.Split(string_get.text.ToString(),"'"); 
				switch (errorString[0].Trim ())
				{
				case "1":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Đăng ký thành công!",3f));
					signUpSuccess = true;
					break;
				case "Query failed: Duplicate entry":
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Tài khoản đã tồn tại!",3f));
					break;
				default:
					StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,string_get.text,2f));
					break;
				}
			}
		}
	}

	void OnClick(){
		StartCoroutine(SignUp ());
	}

	void Update(){
		if (signUpSuccess){
			signUpSuccess = false;
			StartCoroutine(Wait (2f));
		}

		if (waitDone){
			waitDone = false;
			Application.LoadLevel("Login");
		}
	}

	IEnumerator Wait(float waitTime) {
		Debug.Log (Time.time);
		yield return new WaitForSeconds(waitTime);
		waitDone = true;
	}
}
