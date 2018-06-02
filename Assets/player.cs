using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {


	int speed = 20;
	float Cameraspeed = 2.8f;
	void Start () {
		float b = GameObject.Find ("Terrain").GetComponent<TerrainGenerator> ().TerSize * 0.3f;
		transform.position = new Vector3 (b, Mathf.Abs (GameObject.Find ("Terrain").GetComponent<TerrainGenerator> ().maximumheight) +50, b);
	}
	void Update () {
		transform.eulerAngles += new Vector3 (Input.GetAxis("Mouse Y") * Cameraspeed , Input.GetAxis ("Mouse X") * Cameraspeed,  0);

		float aaa = (Mathf.Abs(GameObject.Find ("Terrain").GetComponent<TerrainGenerator> ().TerSize)/ 2) -2;
		if (transform.position.x > aaa) {
			var temp = transform.position;
			temp.x = aaa;
			transform.position = temp;
		}
		if (transform.position.x < -aaa) {
			var temp = transform.position;
			temp.x = -aaa;
			transform.position = temp;
		}
		if (transform.position.z < -aaa) {
			var temp = transform.position;
			temp.z = -aaa;
			transform.position = temp;
		}
		if (transform.position.z > aaa) {
			var temp = transform.position;
			temp.z = aaa;
			transform.position = temp;
		}
		int a = 0;
		if (Input.GetKey (KeyCode.W)) {
				transform.position -= transform.forward * Time.deltaTime * speed;
		}
		if (Input.GetKey (KeyCode.S)) {
				transform.position += transform.forward * Time.deltaTime * speed;
		}

		if (Input.GetKey (KeyCode.A)) {
				transform.position += transform.right * Time.deltaTime * speed;
		}
			if (Input.GetKey (KeyCode.D)) {
				transform.position -= transform.right * Time.deltaTime * speed;
		}
		if (Input.GetKey (KeyCode.Q)) {
			a++;
			transform.Rotate (0, 0, a);
		}
		if (Input.GetKey (KeyCode.E)) {
			a--;
			transform.Rotate (0, 0, a);
		}
	}
}
