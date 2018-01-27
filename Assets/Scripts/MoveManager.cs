using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveManager : MonoBehaviour 
{
	public static MoveManager instance;

	public float speed; //控制全局速度
	public float timespan; //控制发射间隔
	[SerializeField] private Vector3 spawnPos, endPos;
	[SerializeField] private Text m_debugText; //打印
	[SerializeField] private Text m_scoreText; //计分板
	[SerializeField] private float timer = 0;
	[SerializeField] private GameObject scoreBoard;
	[SerializeField] private GameObject prefab; //预制体
	[SerializeField] private Transform bucket;
	[SerializeField] private RectTransform milk; //桶mask
	[SerializeField] private Transform target; //当前目标
	private float holdTime = 0;
	public Toggle going;
	[SerializeField] private int cowMax = 10; //牛的总量，cowCount等于cowMax时停止Spawn，结算
	/*[SerializeField]*/ private int cowCount = 0; //牛的数量
	/*[SerializeField]*/ private int bucketSuccess = 0; //伸桶成功的数量
	/*[SerializeField]*/ private int milkingSuccess = 0; //挤奶成功的数量

	void Awake () 
	{
		instance = this;
		going.onValueChanged.AddListener ((bool value) => OnStartGame(value));
	}

	void Start()
	{
		scoreBoard.SetActive (false);
	}

	void Update ()
	{
		//计分
		Score ();

		//获取状态
		timer += Time.deltaTime;
		if (timer >= timespan && going.isOn)
		{
			if (cowCount > cowMax) 
			{
				PushScoreBoard ();
			} 
			else
			{
				Spawn ();
				timer = 0;
			}
		}

		//操作
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			m_debugText.text = "";
			holdTime = 0;
		}
		if (Input.GetKey (KeyCode.Space)) 
		{
			//手的移动
			bucket.localPosition = Vector3.Lerp (bucket.localPosition, new Vector3 (0, 2, 0), 0.4f);

			//射线检测
			Ray ray = new Ray (transform.position + new Vector3(0,0,2), transform.up * 100);
			RaycastHit hitInfo;

			Debug.DrawLine (ray.origin, transform.up * 100, Color.green);

			if (Physics.Raycast (ray, out hitInfo)) //按下键时，射线检测到物体
			{
				switch (hitInfo.transform.name) 
				{
				case "cow1":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponent<Move> ().status)) 
					{
						hitInfo.transform.GetComponent<Move> ().status = "good";
						hitInfo.transform.GetComponent<Move> ().Stop ();
						target = hitInfo.transform;
						Debug.Log ("good"); //伸桶成功 bucketSuccess
						bucketSuccess += 1;
					}

					if(hitInfo.transform.GetComponent<Move> ().status == "good")
					{
						holdTime += Time.deltaTime;
						if (milk.localPosition.y < 0) 
						{
							milk.localPosition += new Vector3 (0, 1f, 0); //0.01f 根据sprite大小，装载时间更改
						}
						m_debugText.text = "good " + holdTime.ToString("f1");
						going.isOn = false;
						if (holdTime > 4) 
						{
							//hold太久了，按过头
							Debug.Log("too much");
							m_debugText.text = "too much";
							hitInfo.transform.GetComponent<Move> ().Continue();
							going.isOn = true;
							target = null;
							//timer = 0;
						}
					}
					break;
				case "mistake":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponentInParent<Move> ().status))
					{
						hitInfo.transform.GetComponentInParent<Move> ().status = "bad";
						Debug.Log ("bad");
						m_debugText.text = "bad";
					}
					break;
				}
			}
			else //按下键时，未检测到物体
			{
				//if(当前target还没有状态)
				if (m_debugText.text == "miss" || string.IsNullOrEmpty (m_debugText.text)) 
				{
					m_debugText.text = "miss";
				}
				//如果已经赋有状态，不变
			}
		}
		if (Input.GetKeyUp (KeyCode.Space))
		{
			milk.localPosition = new Vector3 (0, -200f, 0);
			bucket.localPosition = new Vector3 (0, 0, 0);
			Debug.Log (holdTime);

			//hold时间不够
			if (target != null) 
			{
				target.GetComponent<Move> ().Continue();

				if (holdTime < 3)
				{
					//时间不够
					m_debugText.text = "not enough";
					Debug.Log("not enough");
				} 
				else if (holdTime > 4) //不会在这里执行too much
				{
					//hold太久了
					Debug.Log("too much");
					m_debugText.text = "too much";
				}
				else 
				{
					//正好
					m_debugText.text = "good";
					Debug.Log("good");
					milkingSuccess += 1; //挤奶成功
				}

				going.isOn = true;
				target = null;
				timer = 0;
			}

			if (m_debugText.text == "miss")
			{
				//m_debugText.text = "";
			}
		}
	}

	//刷下一个牛
	void Spawn()
	{
		GameObject go = Instantiate (prefab);
		go.transform.position = spawnPos;
		go.name = "cow1"; //随机
		cowCount += 1;
	}

	//计分函数
	void Score()
	{
		m_scoreText.text = "牛数量: " + cowCount
			+ "\n伸桶成功: " + bucketSuccess + " 成功率: " + ((cowCount == 0)? 0 : (float)bucketSuccess *100 / (float)cowCount).ToString("f0") + "%"
			+ "\n挤奶成功: " + milkingSuccess + " 成功率: " + ((cowCount == 0)? 0 : (float)milkingSuccess * 100 / (float)cowCount).ToString("f0") + "%";
	}

	//游戏开始开关
	void OnStartGame(bool active)
	{
		if (active) 
		{
			timer = 0;
		}
	}

	//弹出计分板
	void PushScoreBoard()
	{
		scoreBoard.SetActive (true);
	}
}
