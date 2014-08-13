using UnityEngine;
using System.Collections;

public class playButton : MonoBehaviour {

	void OnClick(){
		if(GlobalInfo.currentLevel<GlobalInfo.maxLevel){
			Application.LoadLevel("Game");
		}
	}
}
