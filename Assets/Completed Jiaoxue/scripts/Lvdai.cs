using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvdai : MonoBehaviour {
	private MeshRenderer mesh;
	private Vector2 tmpPosi;

	void Awake () {
		//transform.Rotate(0,0,90);
		mesh = GetComponent<MeshRenderer> ();
		tmpPosi = mesh.material.GetTextureOffset ("_MainTex");
	}
	//public lvDaiMovement () 
	public void LMovement(float speed)
	{
		tmpPosi += new Vector2 (-speed * Time.deltaTime * 0.2f, 0);
		mesh.material.SetTextureOffset ("_MainTex",  tmpPosi);
	}


}
