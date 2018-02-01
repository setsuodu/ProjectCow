using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveManager : MonoBehaviour
{
	public static MoveManager instance;

    [Header("声音")]
    public AudioSource audioSource;
    public AudioClip setBucketClip;
    [Space(10)]
    public float speed = 0.1f; //控制全局速度
	public float timespan = 2; //控制刷新间隔
    public bool isGame;
    public Toggle isStart;
    public GameObject current; //当前目标
    /*[HideInInspector]*/ public Sprite[] bucketSprites;
    /*[HideInInspector]*/ public Sprite[] maskSprites;
    [SerializeField] private float timer = 0; 
    [SerializeField] private float holdTime = 0;
    [SerializeField] private Vector3 spawnPos, endPos;
	[SerializeField] private GameObject scoreBoard;
	[SerializeField] private GameObject nodeCol;
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject[] prefab;
    [SerializeField] private SpriteMask bucketMask;
    [SerializeField] private SpriteRenderer bucket;
    [SerializeField] private SpriteRenderer milk;
    [SerializeField] private Transform node4; //effect位置
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject[] results;
    [SerializeField] private int cowMax = 10; //牛的总量
    public int cowCount = 0; //当前刷牛数
    public int bSuccess = 0; //伸桶成功数
    public int mCount = 0;   //挤奶尝试数
    public int mSuccess = 0; //挤奶成功数
    private List<int> weightList = new List<int>() { 0, 2, 0, 0, 1, 0, 0, 1, 1, 0, 1 };
    private GameObject heart = null;
    private RaycastHit hitInfo;

    void Awake () 
	{
		instance = this;

		isStart.onValueChanged.AddListener ((bool value) => OnStartGame(value));
		restartButton.onClick.AddListener (RestartGame);
	}

	void Start()
	{
		scoreBoard.SetActive (false);
		isStart.isOn = true;
		isGame = true;
        bucket.enabled = false;
        milk.enabled = false;
    }

    void Update()
    {
        if (isGame)
        {
            timer += Time.deltaTime;
            if (timer >= timespan && isStart.isOn && current == null)
            {
                if (cowCount >= cowMax)
                {
                    PushScoreBoard();
                }
                else
                {
                    current = Spawn(); //刷出一个新牛
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (restartButton.gameObject.activeInHierarchy)
            {
                restartButton.image.color = new Color(0.5f, 0.5f, 0.5f, 1); //计分板按钮
            }
            holdTime = 0; //重置长按
            audioSource.clip = setBucketClip; 
            audioSource.Play(); //播放音效

            Ray ray = new Ray(transform.position + new Vector3(0, 0, 2), transform.up * 100);
            if (Physics.Raycast(ray, out hitInfo)) //按下键时，射线检测到物体
            {
                switch (hitInfo.transform.name)
                {
                    case "cow1":
                    case "cow2":
                        Move script = hitInfo.transform.GetComponent<Move>();
                        script.status = "good";
                        script.bSuccess = 1; //伸桶成功+1
                        script.mCount = 1; //尝试挤奶+1
                        script.ChangeExpression(1);
                        script.Stop(); //停下挤奶
                        int id = int.Parse(current.name.Substring(3));
                        SoundMilk.instance.PlaySound(id); //巨大奶牛要播两遍
                        Transform ef = Instantiate(effectPrefab, node4.position, Quaternion.identity).transform;
                        ef.SetParent(node4);
                        bucket.enabled = true;
                        milk.enabled = true;
                        break;
                    case "cow3":
                        //伸桶+0
                        script = hitInfo.transform.GetComponent<Move>();
                        script.mCount = 1; //挤公牛，尝试+1，成功+0
                        bucket.enabled = true;
                        milk.enabled = true;
                        break;
                }
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            bucket.transform.localPosition = Vector3.Lerp(bucket.transform.localPosition, new Vector3(0, 2, 0), 0.4f); //手的移动

            Ray ray = new Ray(transform.position + new Vector3(0, 0, 2), transform.up * 100);
            Debug.DrawLine(ray.origin, transform.up * 100, Color.green);
            if (Physics.Raycast(ray, out hitInfo)) //按下键时，射线检测到物体
            {
                switch (hitInfo.transform.name)
                {
                    case "cow1":
                        if (hitInfo.transform.GetComponent<Move>().status == "good")
                        {
                            if (heart == null)
                            {
                                heart = Instantiate(heartPrefab);
                            }

                            Move script = hitInfo.transform.GetComponent<Move>();
                            holdTime += Time.deltaTime;
                            if (milk.transform.localPosition.y < 0f)
                            {
                                milk.transform.localPosition += new Vector3(0, 0.012f, 0); //1.6/200
                            }
                            isStart.isOn = false;
                            if (holdTime > script.holdTime + 0.5f)
                            {
                                SoundCows.instance.PlayClip(0); //按过头，失败
                                script.ChangeExpression(3);
                                script.Continue();
                                script.mSuccess = 0;
                                isStart.isOn = true;
                            }
                        }
                        break;
                    case "cow2": //巨大
                        if (hitInfo.transform.GetComponent<Move>().status == "good")
                        {
                            if (heart == null)
                            {
                                heart = Instantiate(heartPrefab);
                            }

                            Move script = hitInfo.transform.GetComponent<Move>();
                            holdTime += Time.deltaTime;
                            if (milk.transform.localPosition.y < 0f)
                            {
                                milk.transform.localPosition += new Vector3(0, 0.006f, 0); //0.8/200涨慢一些
                            }
                            isStart.isOn = false;
                            if (holdTime > script.holdTime + 0.5f)
                            {
                                Debug.Log("too much");
                                SoundCows.instance.PlayClip(0); //按过头，失败
                                script.ChangeExpression(3);
                                script.Continue();
                                script.mSuccess = 0;
                                isStart.isOn = true;
                            }
                        }
                        break;
                    case "cow3": //公牛
                        if (string.IsNullOrEmpty(hitInfo.transform.GetComponent<Move>().status))
                        {
                            Move script = hitInfo.transform.GetComponent<Move>();
                            script.status = "bad";
                            script.ChangeExpression(1);
                            SoundCows.instance.PlayClip(0); //失败
                        }
                        break;
                    case "mistake":
                        if (string.IsNullOrEmpty(hitInfo.transform.GetComponentInParent<Move>().status))
                        {
                            Debug.Log("bad");
                            Move script = hitInfo.transform.GetComponentInParent<Move>();
                            script.status = "bad";
                            script.ChangeExpression(3);
                            SoundCows.instance.PlayClip(0); //失败
                        }
                        break;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (SoundMilk.instance.audioSource.isPlaying)
            {
                SoundMilk.instance.StopSound();
            }

            //Debug.Log (holdTime);
            if (restartButton.gameObject.activeInHierarchy)
            {
                restartButton.image.color = new Color(1, 1, 1, 1);
                RestartGame();
            }
            if (heart != null)
            {
                Destroy(heart);
                heart = null;
            }

            milk.transform.localPosition = new Vector3(0, -1.5f, 0);
            bucket.transform.localPosition = new Vector3(0, 0, 0);
            bucket.enabled = false;
            milk.enabled = false;

            if (hitInfo.transform != null && current == hitInfo.transform.gameObject)
            {
                current.GetComponent<BoxCollider>().enabled = false;
                Move script = current.GetComponent<Move>();
                switch (current.name)
                {
                    case "cow1":
                    case "cow2":
                        script.Continue();
                        if (holdTime < script.holdTime)
                        {
                            Debug.Log("not enough");
                            script.ChangeExpression(3);
                            SoundCows.instance.PlayClip(0); //时间不够，失败
                        }
                        else if (holdTime > script.holdTime + 0.5f)
                        {
                            //不会在这里执行too much
                        }
                        else
                        {
                            //正好
                            script.mSuccess = 1;
                            script.ChangeExpression(2);
                            SoundCows.instance.PlayClip(1); //成功
                        }
                        isStart.isOn = true;
                        timer = 0;
                        break;
                    case "cow3":
                        script.bSuccess = 0;
                        script.mCount = 0;
                        script.mSuccess = 0;
                        break;
                }
            }
        }
    }

    //游戏开始开关
    void OnStartGame(bool active)
    {
        if (active)
        {
            timer = 0;
        }
    }

    //刷下一个牛
    GameObject Spawn()
    {
        timer = 0;

        //创建一个音符碰撞体
        GameObject col = Instantiate(nodeCol);
        col.GetComponent<NodeColMove>().speed = this.speed;

        int id = weightList [Random.Range (0, weightList.Count)];
        Debug.Log("Spawn " + id);
        bucket.sprite = bucketSprites[id]; //根据不同牛，用不同桶
        bucketMask.sprite = bucketSprites[id];
        milk.sprite = maskSprites[id];

        GameObject go = Instantiate (prefab[id]);
		go.transform.position = spawnPos;
		go.name = "cow" + (id + 1); //随机
        go.GetComponent<Move>().speed = this.speed;

        return go;
	}

	//弹出计分板
	void PushScoreBoard()
	{
		if (scoreBoard.activeInHierarchy == false) 
		{
			scoreBoard.SetActive (true);
			results [ShowResult()].SetActive (true);
			isGame = false;
			BGMManager.instance.EndMusic ();
		}
	}

    //判断结果
	int ShowResult()
    {
        float bRate = (float)bSuccess * 100 / (float)cowCount; //伸桶成功率=伸的成功数/当前刷牛数
        float jRate = (float)mSuccess * 100 / (float)mCount; //挤奶成功率=挤的成功数/伸的成功数

        if (bRate < 60)
		{
			SoundScoreboard.instance.PlayClip (0);
			return 0;
		}
		else if(bRate >= 60 && jRate < 60)
		{
			SoundScoreboard.instance.PlayClip (0);
			return 1;
		}
		else if(bRate >= 80 && jRate < 80 && jRate >= 60)
		{
			SoundScoreboard.instance.PlayClip (1);
			return 2;
		}
		else if(bRate >= 60 && bRate < 80 && jRate >= 60)
		{
			SoundScoreboard.instance.PlayClip (1);
			return 3;
		}
		else if(bRate >= 80 && jRate >= 80)
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

    public void Score()
    {
        float bRate = (float)bSuccess * 100 / (float)cowCount; //伸桶成功率=伸的成功数/当前刷牛数
        float jRate = (float)mSuccess * 100 / (float)mCount; //挤奶成功率=挤的成功数/挤奶尝试数
        Debug.Log("[伸桶]" + bRate + "%\n[挤奶]" + jRate + "%");
    }
}
