using UnityEngine;
using System.Collections;
using System;

public class hintBox : MonoBehaviour {
	public int Pos;
	private string answerTemp;
	public GameObject popupPrefab;

	public static string secretKey = "matkhau";	
	public static bool updatedScore = false;
	public static bool sendScore = false;

	void OnClick(){
//		bool answerBoxFilled = false;
		for(int i=0;i<GameManager.numOfAnswer;i++){
			if(GameManager.answerBox[i].GetComponentInChildren<UILabel>().text == ""){
				GameManager.answerBox[i].GetComponentInChildren<UILabel>().text = gameObject.GetComponentInChildren<UILabel>().text;
				GameManager.answerBox[i].GetComponent<answerBox>().hintBoxPos = gameObject.GetComponent<hintBox>().Pos;

				//Check the answer is full or not
				bool answerIsFull = false;
				int checkAnswer = 0;
				for(int j=0;j<GameManager.numOfAnswer;j++){
					if(GameManager.answerBox[j].GetComponentInChildren<UILabel>().text == ""){
						checkAnswer++;
					}
				}

				if(checkAnswer == 0){
					answerIsFull = true;
				}

				//Check answer
				if (answerIsFull){
					answerTemp = "";
					for(int j=0;j<GameManager.numOfAnswer;j++){
						answerTemp += GameManager.answerBox[j].GetComponentInChildren<UILabel>().text;
					}

					if(answerTemp == GameManager.answer){
						if(GlobalInfo.currentLevel < GlobalInfo.maxLevel)
						{
							StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Đáp án chính xác!",2f));
							GlobalInfo.currentLevel ++;
//							
							//Send score to server
							sendScore = true;

						}else{
							StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Bạn đã hoàn thành trò chơi",2f));
							GlobalInfo.currentLevel ++;

							//Send score to server
							sendScore = true;
						}

					}
					else{
						StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Đáp án chưa chính xác!",2f));
					}
				}

				gameObject.transform.localScale = new Vector3(0, 0, 0);
				break;
			}
		}
	}

	IEnumerator AddScore(){
		string hash = GlobalInfo.Md5Sum(GlobalInfo.currentUser + GlobalInfo.currentLevel.ToString() + secretKey);
		string string_URL = GlobalInfo.addScoreURL + "currentUser=" + Uri.EscapeDataString(GlobalInfo.currentUser) + "&currentLevel=" + GlobalInfo.currentLevel + "&hash=" + hash;
		WWW string_get = new WWW(string_URL);
		yield return string_get;
		
		if (string_get.error != null)
		{
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"There was an error : " + string_get.error,3f));			
		}

		updatedScore = true;
	}

	void Update(){
		if (sendScore){
			sendScore = false;
			StartCoroutine(AddScore());
		}

		if(updatedScore){
			updatedScore = false;
			if (GlobalInfo.currentLevel > GlobalInfo.maxLevel){
				Application.LoadLevel("Complete");
			} else {
				Application.LoadLevel("Game");
			}
		}
	}
}
