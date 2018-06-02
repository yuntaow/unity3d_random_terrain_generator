using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreateWater : MonoBehaviour {
	public GameObject waterp;
	// Use this for initialization 
	void Start(){
			this.transform.position = new Vector3 (0, 0, 0);
			GameObject.Instantiate (waterp as GameObject, new Vector3 (0, this.transform.position.y, 0), Quaternion.identity);

	}
}
