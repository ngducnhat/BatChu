	using UnityEngine;
using System.Collections;

public class GlobalInfo : MonoBehaviour {
	public static int currentLevel;
	public static string currentUser;
	public static int maxLevel;
	public Texture2D[] picture;
	public static Texture2D currentPicture;

	public static string addScoreURL = "http://54.186.229.83/Batchu/addscore.php?";
	public static string highscoreURL = "http://54.186.229.83/Batchu/leaderboard.php";
	public static string loginURL = "http://54.186.229.83/Batchu/login.php?";
	public static string signupURL = "http://54.186.229.83/Batchu/signup.php?";
	public static string checkConnectionURL = "http://54.186.229.83/Batchu/checkConnection.php";

	public static GameObject popupPrefab;

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
		maxLevel = picture.Length;
	}

	// Use this for initialization
	void Start () {
		currentLevel = 1;
		currentUser = "";
		Application.LoadLevel("Login");
	}
	
	// Update is called once per frame
	void Update () {
		if((0 <currentLevel) && (currentLevel < maxLevel+1)){
			currentPicture = picture[currentLevel-1];
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
