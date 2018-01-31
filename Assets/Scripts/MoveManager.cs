using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveManager : MonoBehaviour
{
	public static MoveManager instance;

	public bool isPlaying;
	public float speed; //控制全局速度
	public float timespan; //控制发射间隔
	public GameObject current; //当前目标
	[SerializeField] private Vector3 spawnPos, endPos;
	[SerializeField] private Text m_debugText; //打印
	[SerializeField] private Text m_scoreText; //计分板
	[SerializeField] private float timer = 0;
	[SerializeField] private GameObject scoreBoard;
	[SerializeField] private GameObject nodeCol;
	[SerializeField] private GameObject[] prefab; //预制体
	[SerializeField] private Transform bucket;
	[SerializeField] private RectTransform milk; //桶mask
	[SerializeField] private Transform target; //当前目标
    [SerializeField] private GameObject effectPrefab; //effect
    [SerializeField] private Transform node4; //effect pos
    [SerializeField] private float holdTime = 0;
	public Toggle going;
    private int cowCount = 0; //牛的数量
    [SerializeField] private int cowMax = 10; //牛的总量，cowCount等于cowMax时停止Spawn，结算
	[SerializeField] private int jCount = 0; //挤的次数
	public int bucketSuccess = 0; //伸桶成功的数量
	public int milkSuccess = 0; //挤奶成功的数量

	//结算
	[SerializeField] private Button restartButton;
	[SerializeField] private GameObject[] results;

	void Awake () 
	{
		instance = this;
		going.onValueChanged.AddListener ((bool value) => OnStartGame(value));
		restartButton.onClick.AddListener (RestartGame);
	}

	void Start()
	{
		scoreBoard.SetActive (false);
		going.isOn = true;
		isPlaying = true;
	}

	[Space(10), Header("声音")]
	public AudioSource audioSource;
	public AudioClip setBucketClip;

	void Update ()
	{
		//计分
		//Score ();

		//获取状态
		timer += Time.deltaTime;
		if (timer >= timespan && going.isOn && current == null)
		{
			if (cowCount >= cowMax) 
			{
				PushScoreBoard ();
			} 
			else
			{
				current = Spawn();
			}
		}

		//操作
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if (restartButton.gameObject.activeInHierarchy)
			{
				restartButton.image.color = new Color (0.5f, 0.5f, 0.5f, 1);
			}

			m_debugText.text = "";
			holdTime = 0;
			if (timer > 2 && current != null)
			{
				Move script = current.GetComponent<Move> ();
				script.ChangeStatus (3);
			}
			audioSource.clip = setBucketClip;
			audioSource.Play ();

			Ray ray = new Ray (transform.position + new Vector3(0,0,2), transform.up * 100);
			RaycastHit hitInfo;
			if (Physics.Raycast (ray, out hitInfo)) //按下键时，射线检测到物体
			{	
				switch (hitInfo.transform.name) 
				{
				case "cow1":
				case "cow2":
						jCount += 1;
						SoundMilk.instance.PlaySound ();
						break;
				}
			}
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
						Move script = hitInfo.transform.GetComponent<Move> ();
						script.status = "good";
						script.Stop ();
						target = hitInfo.transform;

                        //Debug.Log ("good"); //伸桶成功 bucketSuccess
                        Transform ef = Instantiate(effectPrefab, node4.position, Quaternion.identity).transform;
                        ef.SetParent(node4);

                        //计分
                        script.bucket = 1;
						script.ChangeStatus (1);
					}
					if(hitInfo.transform.GetComponent<Move> ().status == "good")
					{
						holdTime += Time.deltaTime;
						if (milk.localPosition.y < 0) 
						{
							milk.localPosition += new Vector3 (0, 1.5f, 0); //0.01f 根据sprite大小，装载时间更改
						}
						m_debugText.text = "good " + holdTime.ToString("f1");
						going.isOn = false;

						if (holdTime > 2.3f) 
						{
							//音效
							SoundCows.instance.PlayClip(0); //失败

							//hold太久了，按过头
							m_debugText.text = "too much";
							Move script = hitInfo.transform.GetComponent<Move> ();
							script.Continue();
							script.ChangeStatus (3);
							script.milk = 0;
							going.isOn = true;
							target = null;
						}
					}
					break;

				case "cow2": //巨大
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponent<Move> ().status)) 
					{
						Move script = hitInfo.transform.GetComponent<Move> ();
						script.status = "good";
						script.Stop ();
						target = hitInfo.transform;

                        //Debug.Log ("good"); //伸桶成功 bucketSuccess
                        Transform ef = Instantiate(effectPrefab, node4.position, Quaternion.identity).transform;
                        ef.SetParent(node4);

                        //计分
                        script.bucket = 1;
						script.ChangeStatus (1);

						//音效
						SoundCows.instance.PlayClip(1); //成功
					}
					if(hitInfo.transform.GetComponent<Move> ().status == "good")
					{
						holdTime += Time.deltaTime;
						if (milk.localPosition.y < 0) 
						{
							milk.localPosition += new Vector3 (0, 2f, 0); //0.01f 根据sprite大小，装载时间更改
						}
						m_debugText.text = "good " + holdTime.ToString("f1");
						going.isOn = false;
						if (holdTime > 2.3f) 
						{
							//音效
							SoundCows.instance.PlayClip(0); //失败

							//hold太久了，按过头
							Debug.Log("too much");
							m_debugText.text = "too much";
							Move script = hitInfo.transform.GetComponent<Move> ();
							script.Continue();
							script.ChangeStatus (3);
							script.milk = 0;
							going.isOn = true;
							target = null;
						}
					}
					break;

				case "cow3": //公牛
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponent<Move> ().status))
					{
						Move script = hitInfo.transform.GetComponent<Move> ();
						script.status = "bad";
						script.bucket = 0;
						script.milk = 0;
						script.ChangeStatus (1);

						//音效
						SoundCows.instance.PlayClip(0); //失败
					}
					break;

				case "mistake":
					if (string.IsNullOrEmpty (hitInfo.transform.GetComponentInParent<Move> ().status))
					{
						Move script = hitInfo.transform.GetComponentInParent<Move> ();
						script.status = "bad";
						Debug.Log ("bad");
						m_debugText.text = "bad";
						script.ChangeStatus (3);

						//音效
						SoundCows.instance.PlayClip(0); //失败
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
			if (restartButton.gameObject.activeInHierarchy)
			{
				restartButton.image.color = new Color (1, 1, 1, 1);
				RestartGame ();
			}

			milk.localPosition = new Vector3 (0, -200f, 0);
			bucket.localPosition = new Vector3 (0, 0, 0);
			//Debug.Log (holdTime);

			//hold时间不够
			if (target != null) {
				Move script = target.GetComponent<Move> ();
				if (target.name == "cow3") 
				{
					script.bucket = 0;
					script.milk = 0;
				}
				else 
				{
					script.Continue ();

					if (holdTime < 1.8f)
					{
						//时间不够
						m_debugText.text = "not enough";
						Debug.Log ("not enough");
						script.ChangeStatus (3);

						//音效
						SoundCows.instance.PlayClip(0); //失败
					} 
					else if (holdTime > 2.3f)
					{ 
						//不会在这里执行too much
					}
					else 
					{
						//正好
						m_debugText.text = "good";
						//Debug.Log ("good");

						//计分
						script.milk = 1;
						script.ChangeStatus (2);

						//音效
						SoundCows.instance.PlayClip(1); //成功
					}

					going.isOn = true;
					target = null;
					timer = 0;
				}

				if (m_debugText.text == "miss")
				{
				
				}
			}
		}
	}

	public List<int> weightList = new List<int> ()
	{
		0, 2, 0, 0, 1, 0, 0, 1, 1, 0, 1, 1,
	};


	//刷下一个牛
	GameObject Spawn()
	{
		//创建一个音符碰撞体
		Instantiate (nodeCol);
		
		//int id = UnityEngine.Random.Range (0, 3);
		//int index = Random.Range (0,idList.Count);
		///int id = idList[index];
		//Debug.Log ("index: " + index + ", id:" + id);
		int id = weightList [Random.Range (0, weightList.Count)];

		GameObject go = Instantiate (prefab[id]);
		go.transform.position = spawnPos;
		go.name = "cow" + (id + 1); //随机
		//Debug.Log(go.name);
		cowCount += 1;
		timer = 0;

		return go;
	}

	//计分函数
	public void Score()
	{
		m_scoreText.text = "牛数量: " + cowCount
			+ "\n伸桶成功: " + bucketSuccess + " 成功率: " + ((cowCount == 0)? 0 : (float)bucketSuccess *100 / (float)cowCount).ToString("f0") + "%"
			+ "\n挤奶成功: " + milkSuccess + " 成功率: " + ((cowCount == 0)? 0 : (float)milkSuccess * 100 / (float)jCount).ToString("f0") + "%";
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
		if (scoreBoard.activeInHierarchy == false) 
		{
			scoreBoard.SetActive (true);
			results [ShowResult()].SetActive (true);
			isPlaying = false;

			BGMManager.instance.EndMusic ();
		}
	}

	int ShowResult()
	{
		float bucketRate = (float)bucketSuccess *100 / (float)cowCount;
		float milkRate = (float)bucketSuccess * 100 / (float)jCount;

		if (bucketRate < 60)
		{
			SoundScoreboard.instance.PlayClip (0);
			return 0;
		}
		else if(bucketRate >= 60 && milkRate < 60)
		{
			SoundScoreboard.instance.PlayClip (0);
			return 1;
		}
		else if(bucketRate >= 80 && milkRate < 80 && milkRate >= 60)
		{
			SoundScoreboard.instance.PlayClip (1);
			return 2;
		}
		else if(bucketRate >= 60 && bucketRate < 80 &&milkRate >=60)
		{
			SoundScoreboard.instance.PlayClip (1);
			return 3;
		}
		else if(bucketRate >= 80 && milkRate>=80)
		{
			SoundScoreboard.instance.PlayClip (2);
			return 4;
		}

		return 0; //0-4
	}

	//重新游戏
	void RestartGame()
	{
		Invoke ("LoadScene" , 1);
	}

	void LoadScene()
    {
        //SceneManager.LoadScene ("Game"); //重复玩，测试
        SceneManager.LoadScene ("Welcome");
	}
}
