﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour 
{
	void Start () {
		
	}
	
	void Update () 
	{
		transform.Rotate (-Vector3.forward * 10f, Space.Self);
	}
}
