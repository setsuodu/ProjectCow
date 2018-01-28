using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeAnim : MonoBehaviour 
{
	public Sprite[] sps;
	private Image image;
	public GameObject curCol;

	void Awake () 
	{
		image = GetComponent<Image> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "node") 
		{
			//Debug.Log (col.name + " is enter");
			curCol = col.gameObject;
			image.sprite = sps [1];
		}
	}


	void OnTriggerExit(Collider col)
	{
		if (col.tag == "node")
		{
			//Debug.Log (col.name + " is exit");
			curCol = null;
			image.sprite = sps [0];
		}
	}

	void Update()
	{
		if(curCol == null)
		{
			//image.sprite = sps [0];
		}
	}
}
