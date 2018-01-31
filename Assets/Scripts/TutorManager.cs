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
    public Toggle going;
    public GameObject current; //当前目标
    [SerializeField] private Vector3 spawnPos, endPos;
	[SerializeField] private float timer = 0;
    [SerializeField] private float holdTime = 0;
	[SerializeField] private int bCount = 0; //伸的成功
    [SerializeField] private int jCount = 0; //挤的成功
	[SerializeField] private GameObject nodeCol;
	[SerializeField] private GameObject prefab; //预制体
    [SerializeField] private GameObject effectPrefab; //effect
    [SerializeField] private RectTransform milk; //桶mask
    [SerializeField] private Transform bucket;
	[SerializeField] private Transform target; //当前目标
    [SerializeField] private Transform node4; //effect pos

    [Space(10), Header("声音")]
    public AudioSource audioSource;
    public AudioClip setBucketClip;

    [SerializeField] private int step = 0;
    [SerializeField] private Image m_fadeImage;
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
    [SerializeField] private int subid = -1;
    [SerializeField] private GameObject tipImage;
    [SerializeField] private bool textTime;

    void Awake () 
	{
		instance = this;
        m_fadeImage.color = new Color(0, 0, 0, 1);
        going.onValueChanged.AddListener ((bool value) => OnStartGame(value));
	}

	void Start()
	{
		going.isOn = false;
        textTime = true;
        NextSub();
    }

    public RaycastHit hitInfo;
    void Update ()
    {
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
            if (going.isOn && current == null && timer >= timespan && step < 3)
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
                    script.ChangeStatus(3);
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
                            target = hitInfo.transform;
                            Transform ef = Instantiate(effectPrefab, node4.position, Quaternion.identity).transform; //特效
                            ef.SetParent(node4);

                            bCount += 1; //成功3次
                            SoundMilk.instance.PlaySound(); //挤奶
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
                bucket.localPosition = Vector3.Lerp(bucket.localPosition, new Vector3(0, 2, 0), 0.4f); //手的移动

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
                                    holdTime += Time.deltaTime;
                                    if (milk.localPosition.y < 0)
                                    {
                                        milk.localPosition += new Vector3(0, 1.5f, 0); //装牛奶
                                    }
                                    going.isOn = false;
                                    if (holdTime > 2.3f)
                                    {
                                        //hold太久了，按过头
                                        SoundCows.instance.PlayClip(0); //失败
                                        Move script = hitInfo.transform.GetComponent<Move>();
                                        script.Continue();
                                        script.ChangeStatus(3);
                                        script.milk = 0;
                                        going.isOn = true;
                                        target = null;

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
                                    script.ChangeStatus(3);
                                    SoundCows.instance.PlayClip(0); //失败
                                }
                                break;
                        }
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (current != null && hitInfo.transform.gameObject == current) //只许点一次
                {
                    Debug.Log(hitInfo.transform);
                    current.GetComponent<BoxCollider>().enabled = false;
                }

                milk.localPosition = new Vector3(0, -200f, 0);
                bucket.localPosition = new Vector3(0, 0, 0);
                if (target != null)
                {
                    Move script = target.GetComponent<Move>();
                    {
                        script.Continue();

                        if (holdTime < 1.8f)
                        {
                            //不够，失败
                            Debug.Log("not enough");

                            if (step == 2)
                            {
                                //再看教程
                                subid = 3;
                                textTime = true;
                                NextSub();
                            }

                            script.ChangeStatus(3);
                            SoundCows.instance.PlayClip(0);
                        }
                        else if (holdTime > 2.3f)
                        {
                            //在GetKey中执行too much
                        }
                        else
                        {
                            //正好
                            script.ChangeStatus(2); //表情
                            SoundCows.instance.PlayClip(1); //成功
                            jCount += 1; //挤奶成功，进入正式
                            NextSub();
                        }
                        going.isOn = true;
                        target = null;
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

    void NextSub()
    {
        if (subid < subList.Count - 1)
        {
            subid += 1;
            m_subText.text = subList[subid];
            if (subid == 3)
            {
                going.isOn = true;
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
                going.isOn = true;
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
            SceneManager.LoadScene("Game");
        }
        Debug.Log("NextSub " + subid);
    }

    //刷下一个牛
    GameObject Spawn()
	{
		//创建一个音符碰撞体
		GameObject col = Instantiate (nodeCol);
        col.GetComponent<NodeColMove>().speed = this.speed;

		GameObject go = Instantiate (prefab); //教学牛
		go.transform.position = spawnPos;
		go.name = "cow4";
        go.GetComponent<Move>().speed = this.speed;

		timer = 0;

		return go;
	}
}
