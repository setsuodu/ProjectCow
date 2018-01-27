using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartWindows : MonoBehaviour 
{
	[SerializeField] private bool flag;
	[SerializeField] private Image image;

	#region Unity Methods

	void Awake () 
	{
		
	}

	void Start () 
	{
		flag = false;
	}

	void Update()
	{
		if (Input.GetKeyDown ("space"))
		{
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

	private void EnterGame ()
	{
		flag = true;
		Invoke ("loadScene", 2f);
	}

	void loadScene()
	{
		SceneManager.LoadScene ("jiaocheng");
	}
}