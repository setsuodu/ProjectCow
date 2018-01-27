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
	public Transform hand; //预制体prefab
	public RectTransform milk; //预制体prefab
	public float speed; //控制全局速度
	public float timespan; //控制发射间隔
	float timer = 0;
	float holdTime = 0;
	public Toggle going;
	public Transform target; //当前目标体

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
			holdTime = 0;
		}

		if (Input.GetKey (KeyCode.Space)) 
		{
			//手的移动
			hand.localPosition = Vector3.Lerp (hand.localPosition, new Vector3 (0, 2, 0), 0.4f);

			//射线检测
			Ray ray = new Ray (transform.position + new Vector3(0,0,2), transform.up * 100);
			RaycastHit hitInfo;

			Debug.DrawLine (ray.origin, transform.up * 100, Color.green);

			if (Physics.Raycast (ray, out hitInfo)) //按下键时，射线检测到物体
			{
				switch (hitInfo.transform.name) 
				{
				case "prefab":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponent<Move> ().status)) 
					{
						hitInfo.transform.GetComponent<Move> ().status = "good";
						hitInfo.transform.GetComponent<Move> ().Stop ();
						target = hitInfo.transform;
						Debug.Log ("good");
					}
					if(hitInfo.transform.GetComponent<Move> ().status == "good")
					{
						holdTime += Time.deltaTime;
						if (milk.localPosition.y < 0) 
						{
							milk.localPosition += new Vector3 (0, 0.5f, 0); //0.01f 根据sprite大小，装载时间更改
						}
						m_text.text = "good " + holdTime.ToString("f1");
						going.isOn = false;
						if (holdTime > 4) 
						{
							//hold太久了，按过头
							Debug.Log("too much");
							m_text.text = "too much";
							hitInfo.transform.GetComponent<Move> ().Continue();
							going.isOn = true;
							target = null;
						}
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
				if (m_text.text == "miss" || string.IsNullOrEmpty (m_text.text)) 
				{
					m_text.text = "miss";
				}
				//如果已经赋有状态，不变
			}
		}

		if (Input.GetKeyUp (KeyCode.Space))
		{
			milk.localPosition = new Vector3 (0, -100f, 0);
			hand.localPosition = new Vector3 (0, 0, 0);
			Debug.Log (holdTime);

			//hold时间不够
			if (target != null) 
			{
				target.GetComponent<Move> ().Continue();

				if (holdTime < 3)
				{
					//时间不够
					m_text.text = "not enough";
					Debug.Log("not enough");
				} 
				else if (holdTime > 4) //不会在这里执行too much
				{
					//hold太久了
					Debug.Log("too much");
					m_text.text = "too much";
				}
				else 
				{
					//正好
					m_text.text = "正好";
					Debug.Log("正好");
				}

				going.isOn = true;
				target = null;
			}

			if (m_text.text == "miss") 
			{
				//m_text.text = "";
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
