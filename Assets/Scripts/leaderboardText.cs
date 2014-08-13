using UnityEngine;
using System.Collections;

public class leaderboardText : MonoBehaviour {
	private string scoreText;
	private bool loadedScore = false;
	public static GameObject[] tenDangNhap;
	public static GameObject[] hoTen;
	public static GameObject[] maPhongBan;
	public static GameObject[] diem;

	public GameObject tenDangNhap1;
	public GameObject hoTen1;
	public GameObject maPhongBan1;
	public GameObject diem1;

	IEnumerator GetScores()
	{
		WWW hs_get = new WWW(GlobalInfo.highscoreURL);
		yield return hs_get;
		
		if (hs_get.error != null)
		{
			scoreText = "There was an error getting the high score: " + hs_get.error;
			loadedScore = true;
		}
		else
		{
			
			scoreText = hs_get.text;
			loadedScore = true;
		}
	}

	void Start()
	{
		StartCoroutine(GetScores());
	}

	void Update()
	{
		if(loadedScore){
			string[] player = scoreText.Split('!');

			tenDangNhap = new GameObject[player.Length-1];
			hoTen = new GameObject[player.Length-1];
			maPhongBan = new GameObject[player.Length-1];
			diem = new GameObject[player.Length-1];

			for (int i = 0;i<player.Length-1;i++)
			{
				string[] info = player[i].Split('|');

				tenDangNhap[i] = NGUITools.AddChild(gameObject,tenDangNhap1);
				tenDangNhap[i].transform.localPosition = new Vector3(tenDangNhap1.transform.localPosition.x, tenDangNhap1.transform.localPosition.y - i*45f,tenDangNhap1.transform.localPosition.z );
				tenDangNhap[i].GetComponentInChildren<UILabel>().text = info[0];

				hoTen[i] = NGUITools.AddChild(gameObject,hoTen1);
				hoTen[i].transform.localPosition = new Vector3(hoTen1.transform.localPosition.x, hoTen1.transform.localPosition.y - i*45f,hoTen1.transform.localPosition.z );;
				hoTen[i].GetComponentInChildren<UILabel>().text = info[1];
//
				maPhongBan[i] = NGUITools.AddChild(gameObject,maPhongBan1);
				maPhongBan[i].transform.localPosition = new Vector3(maPhongBan1.transform.localPosition.x, maPhongBan1.transform.localPosition.y - i*45f,maPhongBan1.transform.localPosition.z );;
				maPhongBan[i].GetComponentInChildren<UILabel>().text = info[2];

				diem[i] = NGUITools.AddChild(gameObject,diem1);
				diem[i].transform.localPosition = new Vector3(diem1.transform.localPosition.x, diem1.transform.localPosition.y - i*45f,diem1.transform.localPosition.z );;
				diem[i].GetComponentInChildren<UILabel>().text = info[3];

			}
			loadedScore = false;
		}
	}
}
