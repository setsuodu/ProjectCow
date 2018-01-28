using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour 
{
	public List<int> weightList = new List<int> (){0, 1, 2, 2, 2, 2};
	public int value;

	void Start ()
	{
		
	}

	[ContextMenu("Output")]
	void Output ()
	{
		Debug.Log (weightList [Random.Range (0, weightList.Count)]);
		value = Random.Range (0, 3);
	}
}
