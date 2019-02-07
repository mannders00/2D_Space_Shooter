using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour {

    public double nodeDensity = 1;

    Vector3[,] nodes;

    public Transform target;
    public GameObject testObj;

    private void Start() {
        generateGrid();

        for(int i = 0; i < nodes.GetLength(0); i++) {
            for(int j = 0; j < nodes.GetLength(1); j++) {
                Instantiate(testObj, nodes[i, j], Quaternion.identity);
            }
        }
    }
    private void generateGrid() {

        Vector3 pos = transform.position;

        float deltaX = pos.x - target.position.x;
        int xCount = (int)(Mathf.Abs(deltaX) / nodeDensity);
        float deltaY = pos.y - target.position.y;
        int yCount = (int)(Mathf.Abs(deltaY) / nodeDensity);

        nodes = new Vector3[xCount+1,yCount+1];

        for(int i = 0; i <= xCount; i++) {

            for(int j = 0; j <= yCount; j++) {

                nodes[i,j] = new Vector3(pos.x - (i * (deltaX / xCount)), pos.y - (j * (deltaY / yCount)));
            }
        }
    }
}
