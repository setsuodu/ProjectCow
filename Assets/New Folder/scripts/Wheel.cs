using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
	
	// Update is called once per frame
	public void WMovement(float speed){
		transform.Rotate (0, 0, -speed*250 * Time.deltaTime);
	}
}
