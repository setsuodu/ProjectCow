using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartWindows : MonoBehaviour {


	//public GameObject canvas;
	[SerializeField] private Image wave;
	[SerializeField] private bool flag;
	[SerializeField] private Image image;
	private float timer;



	void Awake () {

	}

	// Use this for initialization
	void Start () {
		timer = 0f;
 		flag = false;
	}

	void Update(){
		timer += Time.deltaTime;
		if (timer <1f) {
			wave.transform.position += new Vector3(50f * Time.deltaTime,0,0);
		}else {if(timer >2f){
				timer = 0;}
			wave.transform.position += new Vector3(-50f * Time.deltaTime,0,0);
		}
		if (Input.GetKeyDown ("space")) {
			EnterGame ();
		}

		if (flag) {
			Color init, end;
			init = image.color;
			end = Color.black;
			image.color = Color.Lerp (init, end, Time.deltaTime);

		}
	}

	private void EnterGame ()
	{
		flag = true;
		Invoke ("loadScene", 1f);
	}

	void loadScene()
	{
		SceneManager.LoadScene ("jiaocheng");
	}
}