using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskwave : MonoBehaviour {
	public float speed=1f;
	// Use this for initialization
	public void WaveMovement(){
		if (transform.position.x > -40) {
			transform.position += new Vector3(speed * Time.deltaTime,0,0);
		}else if (transform.position.x <40){
			transform.position += new Vector3(-speed * Time.deltaTime,0,0);
		}
	}
}
