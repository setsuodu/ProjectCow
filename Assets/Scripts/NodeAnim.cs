using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeAnim : MonoBehaviour 
{
	public Sprite[] sps;
	public GameObject curCol;
    private SpriteRenderer render;

    [SerializeField] private float radian = 0; //弧度
    private float perRadian = 0.03f; //每次变化的弧度
    private float radius = 0.8f; //半径
    private Vector3 oldPos;

    void Awake () 
	{
        render = GetComponent<SpriteRenderer> ();
        oldPos = transform.position;
    }

    void Start()
    {

    }

    void LateUpdate()
    {
        radian += perRadian; //弧度每次加0.03  
        float dy = Mathf.Cos(radian) * radius; //dy定义的是针对y轴的变量，也可以使用sin，找到一个适合的值就可以  
        transform.position = oldPos + new Vector3(0, dy, 0);
    }

    void OnTriggerEnter(Collider col)
	{
		if (col.tag == "node") 
		{
			//Debug.Log (col.name + " is enter");
			curCol = col.gameObject;
            render.sprite = sps [1];
		}
	}
    
	void OnTriggerExit(Collider col)
	{
		if (col.tag == "node")
		{
			//Debug.Log (col.name + " is exit");
			curCol = null;
            render.sprite = sps [0];
		}
	}
}
