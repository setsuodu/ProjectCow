using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelManager : MonoBehaviour {

	public static WheelManager instace;
	public Wheel WHEEL;
	public Lvdai LVDAI;
//	public GameObject backGround;
//  public GameObject tempo;
	public float speed;

	//齿轮起始位置
	public int wheelPosiY;
	public int wheelPosiX;
	//履带起始位置
	public int lvPosiY;
	public int lvPosiX;

	private List<Wheel> wheel;
	private Lvdai lvdai;

	private int cntW;

	// Use this for initialization
	void Awake () {
		speed = 0.3f;
		instace = this;
		wheel = new List<Wheel>();
		lvdai = new Lvdai();

	}

	void Start(){
		wheelPosiX = -7;
		wheelPosiY = -1;
		lvPosiX = 0;
		lvPosiY = 0;
		cntW = 8;
	    
		//背景
//		GameObject bg = Instantiate (backGround, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		//for (int i = 0; i < 4; i++) {}  音符控件

		//齿轮
		for (int i = 0; i < cntW; i++) {
			Wheel inst = Instantiate (WHEEL, new Vector3 (wheelPosiX + 2 * i, wheelPosiY, 0), Quaternion.identity) as Wheel;
			wheel.Add(inst);
		}

		//履带
		Lvdai instan = Instantiate (LVDAI, new Vector3 (0, 0, -1), Quaternion.identity) as Lvdai;
		lvdai = instan;	


	}
	void Update(){
		//运动
		for (int i = 0; i < cntW; i++) {
			wheel [i].WMovement (speed);
		}
		lvdai.LMovement (speed);

	}
/*	public void ItemMovement(){
		//运动
		for (int i = 0; i < cntW; i++) {
			wheel [i].WMovement (speed);
		}
		lvdai.LMovement (speed);

	}	*/
}
