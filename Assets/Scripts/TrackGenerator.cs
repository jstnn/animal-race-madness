using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : MonoBehaviour {

	public GameObject prefab;
	//float gridX = 3f;
	//float gridY = 3f;
	float side = 50f;

    int[,] gridA = { { 0, 0, 0 }, { 1, 1, 1 }, { 0, 0, 0 } };
    int[,] gridB = { { 0, 1, 0 }, { 0, 1, 0 }, { 0, 1, 0 } };
    int[,] gridC = { { 0, 1, 0 }, { 1, 1, 0 }, { 0, 0, 0 } };
    int[,] gridD = { { 0, 0, 0 }, { 1, 1, 0 }, { 0, 1, 0 } };

	void Start() {
        for (int y = 0; y < gridC.GetLength(0); y++) {
            for (int x = 0; x < gridC.GetLength(1); x++) {
                string s = gridC[x, y].ToString();
                if(gridC[x, y]==1) {
                    
                    Vector3 pos = new Vector3(x, 0, y) * side;

                    Instantiate(prefab, pos, Quaternion.identity);
                }
				
			}
		}
	} 
}
