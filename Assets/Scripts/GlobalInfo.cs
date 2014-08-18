using UnityEngine;
using System.Collections;

public class GlobalInfo : MonoBehaviour {
	public static int currentLevel;
	public static string currentUser;
	public static int maxLevel;
	public static string[] picture;

	public static string addScoreURL = "http://54.186.229.83/Batchu/addscore.php?";
	public static string highscoreURL = "http://54.186.229.83/Batchu/leaderboard.php";
	public static string loginURL = "http://54.186.229.83/Batchu/login.php?";
	public static string signupURL = "http://54.186.229.83/Batchu/signup.php?";
	public static string checkConnectionURL = "http://54.186.229.83/Batchu/checkConnection.php";
	public static string keyURL = "http://54.186.229.83/Batchu/Keys.txt";

	public GameObject popupPrefab;

	private bool loadedKey = false;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		StartCoroutine(getKey());
	}

	// Use this for initialization
	void Start () {
		currentLevel = 1;
		currentUser = "";
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
			StartCoroutine(PrintAndWait(popupPrefab,keys_get.error,3600f));
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
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Không thể kết nối đến server",3600f));
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
				StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Không thể kết nối đến server!",3600f));
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
