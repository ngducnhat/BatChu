using UnityEngine;
using System.Collections;

public class skipButton : MonoBehaviour {
	public GameObject popupWindow;

	void OnClick()
	{
		popupWindow.animation.Play("AppearIn");
	}
}
