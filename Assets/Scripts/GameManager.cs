using UnityEngine;
using System.Collections;
using System.IO;

public class GameManager : MonoBehaviour {
	public GameObject box1;
	public GameObject box2;
	public GameObject picture;

	public static GameObject[] answerBox;
	public static GameObject[] hintBox;
	public static int numOfAnswer;
	public static string answer;

	private UISprite uiSprite;
	private UILabel uiLabel;
	private float boxSize;
	public float boxDistance = 5f;
	public static int numOfHint = 10;
	public static int[] hintPos;
	public WWW t_load = null;
	public static Texture2D t_dynamic_tx;

	void Start(){
		string pictureName = GlobalInfo.currentLevel.ToString() + GlobalInfo.picture[GlobalInfo.currentLevel-1].Trim();
		string targetFile = "Textures/Levels/"+pictureName;
		t_dynamic_tx = Resources.Load(targetFile, typeof(Texture2D)) as Texture2D;
		gameObject.GetComponentInChildren<UITexture>().mainTexture = t_dynamic_tx;

		//Set variable
		numOfAnswer = GlobalInfo.picture[GlobalInfo.currentLevel-1].Trim().Length;
		answer = GlobalInfo.picture[GlobalInfo.currentLevel-1].Trim().ToUpper();
		uiSprite = box1.GetComponent<UISprite>();
		boxSize = uiSprite.localSize.x;
		float pictureHeight = picture.GetComponent<UITexture>().height;

		//Display answerBox
		answerBox = new GameObject[numOfAnswer];
		if(numOfAnswer < 11)
		{
			for (int i = 0;i<numOfAnswer;i++){
				answerBox[i] = NGUITools.AddChild(gameObject,box1);
				if(numOfAnswer % 2 == 1){
					answerBox[i].transform.localPosition = new Vector3((numOfAnswer / 2 - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - boxSize,0);							
				}
				else {
					answerBox[i].transform.localPosition = new Vector3((numOfAnswer / 2 - .5f - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - boxSize,0);
				}
			}
		}
		else
		{
			for (int i = 0;i<10;i++){
				answerBox[i] = NGUITools.AddChild(gameObject,box1);
				answerBox[i].transform.localPosition = new Vector3((10 / 2 - .5f - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - boxSize,0);
			}

			for (int i = 10;i<numOfAnswer;i++){
				answerBox[i] = NGUITools.AddChild(gameObject,box1);
				if((numOfAnswer-10) % 2 == 1){
					answerBox[i].transform.localPosition = new Vector3(((numOfAnswer-10) / 2 - (i-10)) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 2.5f * boxSize,0);							
				}
				else {
					answerBox[i].transform.localPosition = new Vector3(((numOfAnswer-10) / 2 - .5f - (i-10)) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 2.5f * boxSize,0);
				}
			}
		}

		//Display hintBox
		hintBox = new GameObject[2 * numOfHint];
		for (int i = 0;i<numOfHint;i++){
			hintBox[i] = NGUITools.AddChild(gameObject,box2);
			hintBox[i + numOfHint] = NGUITools.AddChild(gameObject,box2);
			if(numOfHint % 2 == 1){
				hintBox[i].transform.localPosition = new Vector3((numOfHint / 2 - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 4.5f * boxSize,0);
				hintBox[i + numOfHint].transform.localPosition = new Vector3((numOfHint / 2 - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 6f * boxSize,0);
				hintBox[i].GetComponent<hintBox>().Pos = i;
				hintBox[i + numOfHint].GetComponent<hintBox>().Pos = i + numOfHint;
			}
			else {
				hintBox[i].transform.localPosition = new Vector3((numOfHint / 2 - .5f - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 4.5f * boxSize,0);
				hintBox[i + numOfHint].transform.localPosition = new Vector3((numOfHint / 2 - .5f - i) * (boxSize + boxDistance)*(-1) ,picture.transform.localPosition.y - pictureHeight/2 - 6f * boxSize,0);
				hintBox[i].GetComponent<hintBox>().Pos = i;
				hintBox[i + numOfHint].GetComponent<hintBox>().Pos = i + numOfHint;
			}
		}

		//Display answer in hintBox
		for (int i = 0;i<numOfAnswer;i++){
			int randomNum;
			do
			{
				randomNum = Random.Range (0,numOfHint*2);
			}
			while (hintBox[randomNum].GetComponentInChildren<UILabel>().text != "") ;
			hintBox[randomNum].GetComponentInChildren<UILabel>().text = answer[i].ToString();
		}

		//Display random letters in other hintBox
		int[] x = new int[2*numOfHint + 1];
		x[0]=Mathf.Abs(answer.GetHashCode()%26);
		int count = 0;
		for (int i = 0;i<2*numOfHint;i++){
			x[i+1] = (12345*x[i] + i) % 26;
			if(hintBox[i].GetComponentInChildren<UILabel>().text == ""){
				hintBox[i].GetComponentInChildren<UILabel>().text = ((char)(65 + x[count])).ToString();
				count ++;
			}
		}

		//Display hint from previous session
		if(GlobalInfo.openedPos > 0)
		{
			for(int i=0;i<GlobalInfo.openedPos;i++)
			{
				answerBox[i].GetComponentInChildren<UILabel>().text = GameManager.answer[i].ToString();
				answerBox[i].GetComponentInChildren<UILabel>().color = Color.green;
				answerBox[i].GetComponent<answerBox>().isOpened = true;
				answerBox[i].GetComponent<answerBox>().hintBoxPos = -1;

				//Deactive the hint box which display on "opened" answer box
				for(int j=0;j<(2*numOfHint);j++)
				{
					if((hintBox[j].GetComponentInChildren<UILabel>().text == answerBox[i].GetComponentInChildren<UILabel>().text) && (!hintBox[j].GetComponent<hintBox>().isOpened))
					{
						hintBox[j].transform.localScale = new Vector3(0, 0, 0);
						hintBox[j].GetComponent<hintBox>().isOpened = true;
						break;
					}
				}
			}
		}

	}
}
