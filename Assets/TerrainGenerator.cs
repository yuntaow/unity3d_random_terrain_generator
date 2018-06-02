using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
		// details is the richness of the ground
		public int details = 128;
		// tersize give the size of the ground
		public float TerSize = 128;
		// mHeight determine the maximum height of the terrain 
		public float mHeight = 50;
		// used for recording the initial height
	 	public float maximumheight;
		// give an array for vertex
		Vector3[] vertexArray;
		int numberofVertex;
	void Start () {
		MeshFilter terMesh = this.gameObject.AddComponent<MeshFilter> ();
		terMesh.mesh = this.DSalgo  ();
		MeshRenderer render = this.gameObject.AddComponent<MeshRenderer> ();
		render.material.shader = Shader.Find ("Tim/full color");
		MeshCollider coder = this.gameObject.AddComponent<MeshCollider> ();
		coder.convex = false;
		coder.sharedMesh = terMesh.mesh;
		coder.skinWidth = 0.01f;
		}

		Mesh DSalgo (){
		// a_details created for reuse 
		int a_details = details + 1;
		// create a mesh function
		Mesh m = new Mesh ();
			// firstly determine how large the vertex array is going to be,
			// the will determine the richness of the terrain
			// adding 1 to detains because detains is how many the partition is
			numberofVertex = (a_details) * (a_details);
			// creating the array for mesh
		    vertexArray = new Vector3[numberofVertex];
			// creatin the triangle
			int[] triangles = new int[details*details*6];
			// half is half of the Tersize
			float Half = TerSize * 0.5f;
			// used for mesh.triangle indexing
			int index = 0;
			
			for(int i = 0; i < a_details; i++){
				for(int ii = 0; ii < a_details; ii++){
				// this determine which vertex we are evaluating
				int whichindex = ii + i * a_details;
				// firstly this goes to the top right,
				// the right to the top right 
				// the right to the right to the top right
				// .... reach the end of the first xaxis, moving on the the next xaxis
				// the density depends on the Tersize / details
				vertexArray [whichindex] = new Vector3 (ii * TerSize / details - Half , 0.0f, - i * TerSize / details + Half);
				// preparing values for mesh.triangle
				if ( i < details && ii< details){
						
						int upleft = i * (a_details) + ii;
						int downleft = (i+1) * (a_details) + ii;

						triangles [index++] = upleft;
						triangles [index++] = upleft+1;
						triangles [index++] = downleft+1;

						triangles [index++] = upleft;
						triangles [index++] = downleft+1;
						triangles [index++] = downleft;

					}
				}
			}

		// initial value for one corner
		vertexArray [0].y = Random.Range (-mHeight, mHeight);
		// initial value for one corner
		vertexArray[details].y = Random.Range (-mHeight, mHeight);
		// initial value for one corner
		vertexArray[vertexArray.Length-1].y = Random.Range (-mHeight, mHeight);
		// initial value for one corner
		vertexArray[vertexArray.Length-1-details].y = Random.Range (-mHeight, mHeight);


		// this part contains code from Youtube. 
		// Reference details in Readme.txt
		// creating xaxis and col index for diamond square algo
		int xaxis, col;
		// how many iteration these algo needs
		int iterations = (int)Mathf.Log (details, 2);
		// initial square num for the plane
		// currrently just 1 
		int squaresInPlane = 1;
		// size of these small squares after each iteration
		int smallsquare = details;
		maximumheight = mHeight;
		// this part contains code from Youtube. 
		// Reference details in Readme.txt

		// precalculated iteration for the algo 
		for (int i = 0; i < iterations; i++) {
			xaxis = 0;
			// iteration 
			for (int j = 0; j < squaresInPlane; j++) {
				col = 0; 
				// iteration 
				for (int k = 0; k < squaresInPlane; k++) {
					// a function determines the height for the vertex
					SquareDiamond (xaxis, col, smallsquare, mHeight);
					col += smallsquare;
					if (col == details) {
						col = 0;
					}
				}
				xaxis += smallsquare;
				if (xaxis == details) {
					break;
				}
			}
			mHeight *= 0.5f;
			squaresInPlane *= 2;
			smallsquare /= 2;
		}
		m.vertices = vertexArray;
		m.triangles = triangles;
		float rate = 0f; 

		int total_height = 0;
		// coloring for different height
		Color[] colors = new Color[m.vertices.Length];
		for (int i = 0; i < colors.Length; i++) {
			rate = m.vertices [i].y / maximumheight;

			if (rate < 0) {
				colors [i] = new Color (0.7f, 0.535f, 0.22f, 0f);
			} else if (m.vertices [i].y < 0.2 * maximumheight) {
				colors [i] = new Color (0.06f, 0.8f, 0.35f, 0);
			} else if (m.vertices [i].y < 0.7 * maximumheight) {
				colors [i] = new Color (0.06f * rate * 5, 0.2f, 0.35f, 0f);
			} else if (m.vertices [i].y < 0.9 * maximumheight) {
				colors [i] = new Color (0.9f, 1f, 1f, 0f);
			} else {
				colors [i] = new Color (1f, 1f, 1f, 0f);
			}
			total_height = total_height + (int)m.vertices[i].y;
		}
		m.RecalculateBounds ();

		m.colors = colors;

		m.RecalculateNormals ();
		return m;
	}



	// this part may contain code from Youtube. 
	// Reference details in Readme.txt
	void SquareDiamond(int xaxis, int zaxis, int size, float randomvalues)

	{
		int a_details = details + 1;

		int upleft = xaxis * (a_details) + zaxis;
		int downleft = (xaxis + size) * (a_details) + zaxis;
		// half of the Tersize
		int Half = (int)(size * 0.5f);

		int diamondstep = (int)(xaxis + Half) * (a_details) + (int)(zaxis + Half);
		// 
		int mid = (int)(xaxis + Half) * (a_details) + (int)(zaxis + Half);
		// random value associated with each vertex.
		float rand = Random.Range (-randomvalues, randomvalues);
		// this is the diamond step. Calculating the center point by finding average of four corner height with a random value
		vertexArray [diamondstep].y = (vertexArray [downleft].y + vertexArray [upleft + size].y + vertexArray [upleft].y + vertexArray[downleft+size].y)* 0.25f + rand;
		// square step : calculating the top of the center , the right of the center, the left of the center and the bottton of the center

		// this part may contain code from Youtube. 
		// Reference details in Readme.txt
		// mid top 
		vertexArray [upleft + Half].y =  (vertexArray [upleft].y + vertexArray [upleft + size].y + vertexArray [mid].y) / 3 +rand;
		// left
		vertexArray [mid - Half].y = (vertexArray [upleft].y + vertexArray [downleft].y + vertexArray [mid].y)/3 + rand;
		// right 
		vertexArray [mid + Half].y = (vertexArray [upleft + size].y + vertexArray [downleft + size].y + vertexArray [mid].y) / 3 + rand;
		// mid down
		vertexArray [downleft + Half].y = (vertexArray [downleft].y + vertexArray [downleft + size].y + vertexArray [mid].y) / 3 + rand;
	
	}



		
}