using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour {

	public static WheelManager instace;
	//public Wheel WHEEL;
	public Lvdai LVDAI;
	public niu NIU;
	public float speed;


	//齿轮起始位置
	public int wheelPosiY;
	public int wheelPosiX;
	//履带起始位置
	public int lvPosiY;
	public int lvPosiX;
	//牛的起始位置
	public int niuPosiY;
	public int niuPosiX;

	[SerializeField] private hand shou;
	[SerializeField] private SpriteRenderer Tong;//桶
	[SerializeField] private SpriteRenderer hearts;
	[SerializeField] private Text wenan;
	private List<string> dialogue;
	private int tttmp=1;
	//private List<Wheel> wheel;//滚轮
	[SerializeField]private Lvdai lvdai;	//履带
	private niu cow;		//牛的临时变量
	private int count;  	//计数器，需要连续计数三次
	private float timer; 	//计时器
	private Color tongColor;//临时存储桶的颜色
	private bool succMilk;  	// 成功挤到到牛奶
	private float timer2;
	private bool Iswenzi;
	[SerializeField] private Text congratulation;
	[SerializeField] private Text m_debugText;

	private int cntW;

	void Awake () {

		speed = 6f;
		instace = this;
//		wheel = new List<Wheel>();

		cow = new niu ();
		succMilk = false;
	}

	void Start(){
		timer = 0;
		timer2 = 0;
		dialogue = new List<string>();
//		wheelPosiX = -12;
//		wheelPosiY = -6;
		lvPosiX = 0;
		lvPosiY = -4;
		niuPosiX = -15;
		niuPosiY = -2;
		cntW = 8;
		count = 0;
		tongColor = Tong.color;
		Tong.color = Color.clear;
		dialogue.Add ("娇羞的母牛在滚动的传送带上出现，伸桶成功（卡在牛中间）");
		dialogue.Add ("看来你有跟奶牛做朋友的潜质，接下来试试挤奶吧！");
		dialogue.Add ("伸桶成功过之后按着空格键不放保持四个拍子。");

		//齿轮
		for (int i = 0; i < cntW; i++) {
//			Wheel inst = Instantiate (WHEEL, new Vector3 (wheelPosiX + 2 * i, wheelPosiY, 0), Quaternion.identity) as Wheel;
//			wheel.Add(inst);
		}

		//履带
		//Lvdai instan = Instantiate (LVDAI, new Vector3 (lvPosiX, lvPosiY, -1), Quaternion.identity) as Lvdai;

		lvdai = LVDAI;	

		cow = Instantiate (NIU, new Vector3 (niuPosiX, niuPosiY, -10), Quaternion.identity) as niu;
		wenan.text = dialogue [0];
		Iswenzi = true;
	}
	void Update(){
		if (Iswenzi) {
			timer2 += Time.deltaTime ;
			if (timer2 >= 3f) {
				timer2 = 0f;
				Iswenzi = false;
				wenan.text = " ";
			}
		}

		if (succMilk && Input.GetKeyDown (KeyCode.Space)) {
			speed = 0;
			EnterFormalGame ();
		}//场景切换
		//运动
		for (int i = 0; i < cntW; i++) {
//			wheel [i].WMovement (speed);
		}
		lvdai.LMovement (speed);
		cow.NMovement (speed);
		if (cow.transform.position.x > 18) {
			cow.transform.position = new Vector3 (niuPosiX, niuPosiY, -10);
		}
			if (count < 3) {
				timer += Time.deltaTime;
				if (timer > 0.5f) {
					huiShou ();
				}
				if (Input.GetKeyDown ("space")) {
					timer = 0;
					shenShou ();
					Ray ray = new Ray (new Vector3 (0, -6.5f, -10), shou.transform.up * 100);
					RaycastHit hitInfo;
					if (Physics.Raycast (ray, out hitInfo)) {
						if (hitInfo.transform.tag == "cow") {
							count++;
							Debug.Log ("good");
							shou.succ ();
							cow.AudioSucc ();
							//	}
						} else if (hitInfo.transform.tag == "error") {
							Debug.Log ("bad");
							cow.AudioFail ();
						}
					}
					if (count == 3) {
						Invoke ("huiShou", 0.5f);
					}
				}

			} else {
			if(count ==3)  Iswenzi = true;
			if (timer2 >= 2f) {
				speed = 0;
				if (count == 3) {
					timer2 = 0;
					wenan.text = dialogue [1];
					count++;
				} else if (count == 4) {
					timer2 = 0;
					wenan.text = dialogue [2];
					count++;
				} else{
					Iswenzi = false;
					wenan.text = " ";
					timer2 = 0;
					speed = 6;
				}
					
			}
				Tong.color = tongColor;
				if (Input.GetKeyDown ("space")) {
					timer = 0;
				}
				//加水
				if (Input.GetKey ("space")) {
					timer += Time.deltaTime;
					shenShou ();
					Ray ray = new Ray (new Vector3 (0, -6.5f, -10), shou.transform.up * 100);
					RaycastHit hitInfo;
					if (Physics.Raycast (ray, out hitInfo)) {
						if (hitInfo.transform.tag == "cow") {//加水
							m_debugText.text = "good " + timer.ToString ("f1");
							succMilk = true;
							speed = 0f;
							shou.succ ();
							shou.milk ();
							cow.jinai ();
							Debug.Log ("good");
							cow.AudioSucc ();
							//	}
						} else if (hitInfo.transform.tag == "error") {
							Debug.Log ("bad");
							cow.AudioFail ();
						}
					}
				}
				if (Input.GetKeyUp ("space")) {
					if (succMilk) {
						succMilk = false;
						if (timer >= 2.7 && 3.2f >= timer) {
							//通过 
							SpriteRenderer heart= Instantiate(hearts,new Vector3(5,2,2),Quaternion.identity)as SpriteRenderer;
							heart.transform.SetParent (cow.transform);
							congratulation.color = Color.Lerp (Color.clear, Color.black, 0.8f);
							succMilk = true;

						} else {
							cow.AudioFail ();
						}
					}
					speed = 6f;
					huiShou ();
				}

			}
		}
		
	void huiShou(){
		shou.transform.position = new Vector3 (-2, -6.5f, -10);
		shou.empty ();
	}
	void shenShou(){
		shou.transform.position = Vector3.Lerp (new Vector3 (-2, -6.5f, -10), new Vector3 (-2, -5f, -10), 0.4f);
	}

	public void EnterFormalGame()
	{
		SceneManager.LoadScene ("Game");
	}

}

