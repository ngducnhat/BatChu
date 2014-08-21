
using UnityEngine;
using System.Collections;

public class popupWindow_noButton : MonoBehaviour {
	public GameObject popupWindow;
	
	void OnClick()
	{
		popupWindow.animation.Play("AppearOut");
	}
}
