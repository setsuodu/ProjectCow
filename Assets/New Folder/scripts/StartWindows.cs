using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartWindows : MonoBehaviour 
{
	[SerializeField] private bool flag = false;
	[SerializeField] private Image image;
	[SerializeField] private Button button;

	#region Unity Methods

	void Start () 
	{
		
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			button.image.color = new Color (0.5f,0.5f,0.5f,1);
		}

		if (Input.GetKeyUp (KeyCode.Space))
		{
			button.image.color = new Color (1,1,1,1);
			EnterGame ();
		}

		if (flag)
		{
			Color init, end;
			init = image.color;
			end = Color.black;
			image.color = Color.Lerp (init, end, Time.deltaTime);
		}
	}

	#endregion

	void EnterGame ()
	{
		flag = true;
		Invoke ("LoadScene", 2f);
	}

	void LoadScene()
	{
		SceneManager.LoadScene ("jiaocheng");
	}
}