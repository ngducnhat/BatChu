using UnityEngine;
using System.Collections;
using System;

public class GlobalInfo : MonoBehaviour {
	public static int currentLevel;
	public static int skipTimes;
	public static string currentUser;
	public static int maxLevel;
	public static int point;
	public static int openedPos;
	public static string matKhau = "matkhau";
	public static string[] picture;

	public static string addScoreURL = "http://54.186.229.83/Batchu/addscore.php?";
	public static string highscoreURL = "http://54.186.229.83/Batchu/leaderboard.php";
	public static string loginURL = "http://54.186.229.83/Batchu/login.php?";
	public static string signupURL = "http://54.186.229.83/Batchu/signup.php?";
	public static string checkConnectionURL = "http://54.186.229.83/Batchu/checkConnection.php";
	public static string keyURL = "http://54.186.229.83/Batchu/Keys.txt";
	public static string addskipTimesURL = "http://54.186.229.83/Batchu/skipTimes.php?";
	public static string addHintPosURL = "http://54.186.229.83/Batchu/addHintPos.php?";
	public GameObject popupPrefab;

	private bool loadedKey = false;
	public static bool savedScore = false;
	public static bool savedPoint = false;
	public static bool savedHint = false;

	public static string version = "2282";

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		StartCoroutine(getKey());
	}

	// Use this for initialization
	void Start () {
		currentLevel = 1;
		currentUser = "";
		skipTimes = 0;
		point = 0;
		openedPos = 0;

	}
	
	// Update is called once per frame
	void Update () {
		if (loadedKey)
		{
			maxLevel = picture.Length;
			StartCoroutine(ConnectToServer());
			loadedKey = false;
		}	

		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			if((Application.loadedLevelName == "Login")||(Application.loadedLevelName == "Global"))
			{
				Application.Quit();
			}
			else{
				Application.LoadLevel("Login");
			}
		}

	}

	IEnumerator getKey()
	{
		WWW keys_get = new WWW(GlobalInfo.keyURL);
		yield return keys_get;
		if (keys_get.error != null)
		{
			StartCoroutine(PrintAndWait(popupPrefab,"Không thể kết nối đến server!\n" + keys_get.error,3600f));
		}
		else
		{			
			picture = keys_get.text.Split('\n');
			loadedKey = true;
		}
		

	}

	IEnumerator ConnectToServer()
	{
		string string_URL = GlobalInfo.checkConnectionURL;
		WWW string_get = new WWW(string_URL);
		yield return string_get;
		
		if (string_get.error != null)
		{
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Không thể kết nối đến server!\n" + string_get.error ,3600f));
			Debug.Log (string_get.error);
		}
		else
		{
			if(string_get.text.Trim () == "1")
			{
				Application.LoadLevel("Login");
			}
			else
			{
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Không thể kết nối đến server!\n" + string_get.error,3600f));
			}
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,string_get.text,3600f));
		}
	}

	public static IEnumerator PrintAndWait(GameObject popupPrefab, string popupText, float waitTime) {
//		GameObject popupLabel = new GameObject();
		GameObject popupLabel = NGUITools.AddChild(GameObject.Find ("UI Root"), popupPrefab);
		popupLabel.GetComponent<UILabel>().text = popupText;
		yield return new WaitForSeconds(waitTime);
		Destroy(popupLabel);
	}
	
	public static IEnumerator addPoint(int point,GameObject popupPrefab)
	{
		string hash = GlobalInfo.Md5Sum(GlobalInfo.currentUser + point + GlobalInfo.matKhau);
		string string_URL = GlobalInfo.addskipTimesURL + "currentUser=" + Uri.EscapeDataString(GlobalInfo.currentUser) + "&skipTimes=" + point + "&hash=" + hash;
		WWW string_get = new WWW(string_URL);
		GameObject popupLabel = NGUITools.AddChild(GameObject.Find ("UI Root"), popupPrefab);
		popupLabel.transform.localPosition = new Vector3(0,-100f,0);
		yield return string_get;
		if (string_get.error != null)
		{
			popupLabel.GetComponent<UILabel>().text = string_get.error;
		}
		else
		{
			GlobalInfo.savedPoint = true;
		}
		yield return new WaitForSeconds(1f);
	}

	public static IEnumerator addHintPos(int hintPos,GameObject popupPrefab)
	{
		string hash = GlobalInfo.Md5Sum(GlobalInfo.currentUser + hintPos + GlobalInfo.matKhau);
		string string_URL = GlobalInfo.addHintPosURL + "currentUser=" + Uri.EscapeDataString(GlobalInfo.currentUser) + "&hintPos=" + hintPos + "&hash=" + hash;
		WWW string_get = new WWW(string_URL);
		GameObject popupLabel = NGUITools.AddChild(GameObject.Find ("UI Root"), popupPrefab);
		popupLabel.transform.localPosition = new Vector3(0,-100f,0);
		yield return string_get;
		if (string_get.error != null)
		{
			popupLabel.GetComponent<UILabel>().text = string_get.error;
		}
		else
		{
			GlobalInfo.savedHint = true;
		}
		yield return new WaitForSeconds(1f);
	}

	public static IEnumerator AddScore(int score,GameObject popupPrefab){
		string hash = GlobalInfo.Md5Sum(GlobalInfo.currentUser + score + GlobalInfo.matKhau);
		string string_URL = GlobalInfo.addScoreURL + "currentUser=" + Uri.EscapeDataString(GlobalInfo.currentUser) + "&currentLevel=" + score + "&hash=" + hash;
		WWW string_get = new WWW(string_URL);
		GameObject popupLabel = NGUITools.AddChild(GameObject.Find ("UI Root"), popupPrefab);
		popupLabel.transform.localPosition = new Vector3(0,-100f,0);
		yield return string_get;
		
		if (string_get.error != null)
		{
			popupLabel.GetComponent<UILabel>().text = string_get.error;
		}
		else
		{
			GlobalInfo.savedScore = true;
		}
		yield return new WaitForSeconds(1f);
	}

	public static string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

}
