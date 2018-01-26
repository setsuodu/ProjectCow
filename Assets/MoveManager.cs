using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveManager : MonoBehaviour 
{
	public static MoveManager instance;

	public Vector3 spawnPos, endPos;
	public Text m_text; //打印
	public GameObject prefab; //预制体
	public Transform hand; //预制体
	public float speed; //控制全局速度
	public float timespan; //控制发射间隔
	float timer = 0;

	//public bool going;
	public Toggle going;

	void Awake () 
	{
		instance = this;
	}

	void Start()
	{
		//Spawn ();
	}

	void Update ()
	{
		//
		timer += Time.deltaTime;
		if (timer >= timespan && going.isOn)
		{
			Spawn ();
			timer = 0;
		}

		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			m_text.text = "";
		}

		if (Input.GetKey (KeyCode.Space)) 
		{
			//手的移动
			hand.localPosition = Vector3.Lerp (hand.localPosition, new Vector3 (0, 2, 0), 0.4f);

			//射线检测
			Ray ray = new Ray (transform.position, transform.up * 100);
			RaycastHit hitInfo;

			Debug.DrawLine (ray.origin, transform.up * 100, Color.green);

			if (Physics.Raycast (ray, out hitInfo)) //按下键时，射线检测到物体
			{
				switch (hitInfo.transform.name) 
				{
				case "prefab":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponent<Move> ().status)) {
						hitInfo.transform.GetComponent<Move> ().status = "good";
						Debug.Log ("good");
						m_text.text = "good";
					}
					break;
				case "mistake":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponentInParent<Move> ().status))
					{
						hitInfo.transform.GetComponentInParent<Move> ().status = "bad";
						Debug.Log ("bad");
						m_text.text = "bad";
					}
					break;
				}
			}
			else //按下键时，未检测到物体
			{
				//if(当前target还没有状态)
				if (m_text.text == "miss" || string.IsNullOrEmpty( m_text.text))
				{
					m_text.text = "miss";
				}
				//如果已经赋有状态，不变
			}
		}

		if (Input.GetKeyUp (KeyCode.Space))
		{
			hand.localPosition = new Vector3 (0, 0, 0);

			if (m_text.text == "miss") 
			{
				m_text.text = "";
			}
		}
	}

	[ContextMenu("Spawn")]
	void Spawn()
	{
		GameObject go = Instantiate (prefab);
		go.transform.position = spawnPos;
		go.name = "prefab";
	}
}
