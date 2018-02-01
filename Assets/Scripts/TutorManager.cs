using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorManager : MonoBehaviour
{
	public static TutorManager instance;

	public float speed; //控制全局速度
	public float timespan; //控制发射间隔
    public Toggle isStart;
    public GameObject current; //当前目标
    /*[HideInInspector]*/ public Sprite[] bucketSprites;
    /*[HideInInspector]*/ public Sprite[] maskSprites;
    [SerializeField] private Vector3 spawnPos, endPos;
	[SerializeField] private float timer = 0;
    [SerializeField] private float holdTime = 0;
	[SerializeField] private int bCount = 0; //伸的成功
    [SerializeField] private int jCount = 0; //挤的成功
	[SerializeField] private GameObject nodeCol;
    [SerializeField] private GameObject effectPrefab; //effect
	[SerializeField] private GameObject[] prefab; //预制体
    [SerializeField] private SpriteMask bucketMask;
    [SerializeField] private SpriteRenderer bucket;
    [SerializeField] private SpriteRenderer milk;
    [SerializeField] private Transform node4; //effect pos
    private RaycastHit hitInfo;
    [SerializeField] private bool textTime; //文字时间
    [SerializeField] private int subid = -1;
    [SerializeField] private int step = 0;
    private List<string> subList = new List<string>()
    {
        "朋友，奶牛们可是很喜欢音乐的。",
        "节奏感强的人才能成为合格的挤奶工。",
        "在第四拍的时候敲空格键伸出奶桶试试。",
        "别墨迹，我已经准备好了！（按Space进入）",
        "娇羞的母牛在滚动的传送带上出现，3次伸手成功\n（卡在牛中间）！",
        "看来你有跟奶牛做朋友的潜质，接下来试试挤奶吧！",
        "伸桶成功过之后按着空格键不放保持四个拍子。",
        "",
        "有点样子了，来正式的试炼吧，期待你成为一名骄傲的挤奶工！\n按下空格正式进入游戏。"
    };
    [SerializeField] private Text m_subText;
    [SerializeField] private Image m_fadeImage;
    [SerializeField] private Button m_skipButton;
    [SerializeField] private GameObject tipImage;
    [Space(10), Header("声音")]
    public AudioSource audioSource;
    public AudioClip setBucketClip;

    void Awake () 
	{
		instance = this;
        m_fadeImage.color = new Color(0, 0, 0, 1);
        isStart.onValueChanged.AddListener ((bool value) => OnStartGame(value));
        m_skipButton.onClick.AddListener(LoadScene);
    }

	void Start()
	{
		isStart.isOn = false;
        textTime = true;
        NextSub();
        bucket.enabled = false;
        milk.enabled = false;
    }

    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //m_skipButton.image.color = new Color(1, 1, 1, 1);
            LoadScene();
        }

        if(m_fadeImage.color.a > 0)
        {
            m_fadeImage.color = Color.Lerp(m_fadeImage.color, new Color(0, 0, 0, 0), 0.1f);
        }

        if (textTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextSub();
            }
        }
        else
        {
            timer += Time.deltaTime;
            if (isStart.isOn && current == null && timer >= timespan && step < 3)
            {
                //如果教学未完成，一直刷
                current = Spawn();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                holdTime = 0;
                if (timer > 2 && current != null)
                {
                    Move script = current.GetComponent<Move>();
                    script.ChangeExpression(3);
                }
                audioSource.clip = setBucketClip;
                audioSource.Play();

                Ray ray = new Ray(transform.position + new Vector3(0, 0, 2), transform.up * 100);
                if (Physics.Raycast(ray, out hitInfo)) //按下键时，射线检测到物体
                {
                    switch (hitInfo.transform.name)
                    {
                        case "cow4":
                            Move script = hitInfo.transform.GetComponent<Move>();
                            script.status = "good";
                            script.Stop();
                            Transform ef = Instantiate(effectPrefab, node4.position, Quaternion.identity).transform; //特效
                            ef.SetParent(node4);
                            bucket.enabled = true;
                            milk.enabled = true;

                            bCount += 1; //成功3次
                            SoundMilk.instance.PlaySound(1); //挤奶
                            if (bCount == 3)
                            {
                                //成功3次，再进文字
                                NextSub();
                            }
                            break;
                    }
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                bucket.transform.localPosition = Vector3.Lerp(bucket.transform.localPosition, new Vector3(0, 2, 0), 0.4f); //手的移动

                if (step >= 2)
                {
                    Ray ray = new Ray(transform.position + new Vector3(0, 0, 2), transform.up * 100);
                    Debug.DrawLine(ray.origin, transform.up * 100, Color.green);
                    if (Physics.Raycast(ray, out hitInfo)) //按下键时，射线检测到物体
                    {
                        switch (hitInfo.transform.name)
                        {
                            case "cow4":
                                if (hitInfo.transform.GetComponent<Move>().status == "good")
                                {
                                    Move script = hitInfo.transform.GetComponent<Move>();
                                    holdTime += Time.deltaTime;
                                    if (milk.transform.localPosition.y < 0)
                                    {
                                        milk.transform.localPosition += new Vector3(0, 0.012f, 0); //装牛奶
                                    }
                                    isStart.isOn = false;
                                    if (holdTime > script.holdTime + 0.5f)
                                    {
                                        //hold太久了，按过头
                                        if (step == 1)
                                        {
                                            SoundCows.instance.PlayClip(1); //成功
                                            script.ChangeExpression(2);
                                        }
                                        else if (step == 2)
                                        {
                                            SoundCows.instance.PlayClip(0); //失败
                                            script.ChangeExpression(3);
                                        }

                                        script.Continue();
                                        script.mSuccess = 0;
                                        isStart.isOn = true;

                                        //再看教程
                                        subid = 3;
                                        textTime = true;
                                        NextSub();
                                    }
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
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (SoundMilk.instance.audioSource.isPlaying)
                {
                    SoundMilk.instance.StopSound();
                }

                milk.transform.localPosition = new Vector3(0, -1.5f, 0);
                bucket.transform.localPosition = new Vector3(0, 0, 0);
                bucket.enabled = false;
                milk.enabled = false;

                if (hitInfo.transform != null && current == hitInfo.transform.gameObject)
                {
                    current.GetComponent<BoxCollider>().enabled = false;  //只许点一次
                    Move script = hitInfo.transform.GetComponent<Move>();
                    {
                        script.Continue();

                        if (holdTime < 1.8f)
                        {
                            //不够，失败
                            Debug.Log("not enough");

                            if (step == 1)
                            {
                                script.ChangeExpression(2);
                                SoundCows.instance.PlayClip(1);
                            }
                            else if (step == 2)
                            {
                                //再看教程
                                subid = 3;
                                textTime = true;
                                NextSub();
                                script.ChangeExpression(3);
                                SoundCows.instance.PlayClip(0);
                            }
                        }
                        else if (holdTime > 2.3f)
                        {
                            //在GetKey中执行too much
                        }
                        else
                        {
                            //正好
                            script.ChangeExpression(2); //表情
                            SoundCows.instance.PlayClip(1); //成功
                            jCount += 1; //挤奶成功，进入正式
                            NextSub();
                        }
                        isStart.isOn = true;
                        timer = 0;
                    }
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

    //下一页文字
    void NextSub()
    {
        if (subid < subList.Count - 1)
        {
            subid += 1;
            m_subText.text = subList[subid];
            if (subid == 3)
            {
                isStart.isOn = true;
                m_subText.text = "";
                tipImage.SetActive(false);
                step = 1;
                textTime = false;
            }
            else if (subid >= 4 && subid <= 6)
            {
                tipImage.SetActive(true);
                textTime = true;
            }
            else if (subid == 7)
            {
                isStart.isOn = true;
                m_subText.text = "";
                tipImage.SetActive(false);
                step = 2;
                textTime = false;
            }
            else if (subid == 8)
            {
                textTime = true;
                tipImage.SetActive(true);
                subid += 1;
            }
        }
        else
        {
            LoadScene();
        }
        Debug.Log("NextSub " + subid);
    }

    //刷下一个牛
    GameObject Spawn()
	{
		//创建一个音符碰撞体
		GameObject col = Instantiate (nodeCol);
        col.GetComponent<NodeColMove>().speed = this.speed;

        bucket.sprite = bucketSprites[0]; //根据不同牛，用不同桶
        bucketMask.sprite = bucketSprites[0];
        milk.sprite = maskSprites[0];

        GameObject go = Instantiate (prefab[0]); //教学牛
		go.transform.position = spawnPos;
		go.name = "cow4";
        go.GetComponent<Move>().speed = this.speed;

		timer = 0;

		return go;
	}

    void LoadScene()
    {
        SceneManager.LoadScene("Game");
    }
}
