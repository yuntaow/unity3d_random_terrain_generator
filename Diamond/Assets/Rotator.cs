using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rotator : MonoBehaviour {

//	public GameObject fater;
	// Use this for initialization
	void Start () {
		float a = GameObject.Find ("Terrain").GetComponent<TerrainGenerator> ().TerSize;
		transform.position = new Vector3( 0 , a , - a - 10);
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, new Vector3 (0,0,1) * 40, 1);
	}
}
