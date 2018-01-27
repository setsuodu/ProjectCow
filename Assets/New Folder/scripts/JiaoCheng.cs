using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JiaoCheng : MonoBehaviour {
	public Text wenzi;
	private List<string> welcome;
	private int cnt;
	// Use this for initialization
	void Start () {
		cnt = 0;
		welcome = new List<string> ();	
		welcome.Add ("碰友，奶牛们可是很喜欢音乐的。");
		welcome.Add ("节奏感强的人才能成为合格的挤奶工。");
		welcome.Add ("在第四拍的时候敲空格键伸出奶桶试试。");
		welcome.Add ("别墨迹，我已经准备好了！（按回车进入）.");

	}
	
	// Update is called once per frame
	void Update () {
		if (cnt<4&&Input.GetKeyDown ("space")) {
			wenzi.text = welcome [cnt++];
		}
		if (cnt >= 4&&Input.GetKeyDown(KeyCode.Return)) {
			loadScene ();
		}
	}
	void loadScene()
	{
		SceneManager.LoadScene ("background");
	}
}
