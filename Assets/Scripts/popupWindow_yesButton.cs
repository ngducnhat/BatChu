using UnityEngine;
using System.Collections;
using System;

public class popupWindow_yesButton : MonoBehaviour {
	public GameObject popupPrefab;
	public GameObject popupWindow;
	public GameObject pointLabel;

	void OnClick()
	{
		if(GlobalInfo.point < 15)
		{
			StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Bạn không đủ điểm để bóc bánh",2f));
		}
		else
		{
			for(int i=0;i<GameManager.answerBox.Length;i++)
			{
				if(!GameManager.answerBox[i].GetComponent<answerBox>().isOpened)
				{
					//Display hint on answer box
					GameManager.answerBox[i].GetComponentInChildren<UILabel>().text = GameManager.answer[i].ToString();
					GameManager.answerBox[i].GetComponentInChildren<UILabel>().color = Color.green;
					GameManager.answerBox[i].GetComponent<answerBox>().isOpened = true;
					GameManager.answerBox[i].GetComponent<answerBox>().hintBoxPos = -1;
					
					//Decrease point
					GlobalInfo.point -= 15;
					pointLabel.GetComponentInChildren<UILabel>().text = GlobalInfo.point.ToString();
					StartCoroutine(GlobalInfo.addPoint(GlobalInfo.point,popupPrefab));
					
					
					
					//Deactive the hint box which display on "opened" answer box
					for(int j=0;j<(2*GameManager.numOfHint);j++)
					{
						if((GameManager.hintBox[j].GetComponentInChildren<UILabel>().text == GameManager.answerBox[i].GetComponentInChildren<UILabel>().text) && (!GameManager.hintBox[j].GetComponent<hintBox>().isOpened))
						{
							GameManager.hintBox[j].transform.localScale = new Vector3(0, 0, 0);
							GameManager.hintBox[j].GetComponent<hintBox>().isOpened = true;
							break;
						}
					}
					
					//Delete all answer box which is not opened
					for(int j=0;j<GameManager.answerBox.Length;j++)
					{
						if(!GameManager.answerBox[j].GetComponent<answerBox>().isOpened)
						{
							GameManager.answerBox[j].GetComponentInChildren<UILabel>().text = "";
						}
					}
					
					//Reactive all hint box which is deactive
					for(int j=0;j<(2*GameManager.numOfHint);j++)
					{
						if(!GameManager.hintBox[j].GetComponent<hintBox>().isOpened)
						{
							GameManager.hintBox[j].transform.localScale = new Vector3(1f, 1f, 1f);
						}					
					}
					
					//Save the progress of the hint
					if( i < (GameManager.answerBox.Length-1))
					{
						GlobalInfo.openedPos++;
						StartCoroutine(GlobalInfo.addHintPos(GlobalInfo.openedPos,popupPrefab));
					}
					
					//Complete level if this is the last answer box
					if(i == (GameManager.answerBox.Length-1))
					{
						GlobalInfo.currentLevel ++;
						GlobalInfo.point += 15;
						GlobalInfo.openedPos = 0;
						StartCoroutine(GlobalInfo.addHintPos(GlobalInfo.openedPos,popupPrefab));
						StartCoroutine(GlobalInfo.addPoint(GlobalInfo.point,popupPrefab));
						StartCoroutine(GlobalInfo.AddScore(GlobalInfo.currentLevel,popupPrefab));
						StartCoroutine(GlobalInfo.PrintAndWait(popupPrefab,"Đáp án chính xác!",2f));
					}
					
					
					break;
				}
			}
		}
		popupWindow.animation.Play("AppearOut");
	}

	void Update()
	{
		if (GlobalInfo.savedPoint && GlobalInfo.savedScore && GlobalInfo.savedHint)
		{
			GlobalInfo.savedPoint = false;
			GlobalInfo.savedScore = false;
			GlobalInfo.savedHint = false;
			Application.LoadLevel("Game");
			
		}
	}
}
