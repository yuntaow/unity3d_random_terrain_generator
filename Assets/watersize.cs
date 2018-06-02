using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watersize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float a = GameObject.Find("Terrain").GetComponent<TerrainGenerator>().TerSize;
		this.transform.localScale = new Vector3(a/10,1,a/10);
	}
}
