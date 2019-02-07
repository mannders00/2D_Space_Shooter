using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfind : MonoBehaviour {

    public float resolution = 1F;
    private List<Vector3> points = new List<Vector3>();
    public GameObject target;
    public float reachDistance = 1;

    private float radius = 1;

    Vector3 currentPos;

    Vector3 lastSelfPos;
    Vector3 lastTargetPos;

    void Start() {
        radius = GetComponent<CircleCollider2D>().radius;
        currentPos = transform.position;
    }

    private void Update() {
        if((lastSelfPos != transform.position) || (lastTargetPos != target.transform.position)) {
            findPath();
        }

        for (int i = 1; i < points.Count; i++) {
            Debug.DrawLine(points[i-1], points[i]);
        }
    }

    bool found = false;
    void findPath() {

        points.Clear();
        currentPos = transform.position;

        found = false;
        int attempts = 0;

        while (!found) {

            attempts++;
            Vector3[] testPoints = new Vector3[8];
            for (int i = 0; i < testPoints.Length; i++) {

                testPoints[i] = currentPos + (Vector3.Normalize(Quaternion.Euler(0, 0, (float)360 * i / testPoints.Length) * currentPos) * resolution);
            }

            int minIndex = 0;
            bool accessible = false;

            if(!Physics2D.OverlapCircle(testPoints[0], radius * 2)) {
                accessible = true;
            }

            for (int i = 0; i < testPoints.Length; i++) {
                if (Vector3.Distance(testPoints[i], target.transform.position) < Vector3.Distance(testPoints[minIndex], target.transform.position)) {

                    if(!Physics2D.OverlapCircle(testPoints[i], radius * 2)) {
                        minIndex = i;
                        accessible = true;
                    }
                }
            }

            if (Vector3.Distance(testPoints[minIndex], target.transform.position) > reachDistance) {
                points.Add(testPoints[minIndex]);
                currentPos = testPoints[minIndex];
            } else {
                found = true;
            }

            lastSelfPos = transform.position;
            lastTargetPos = target.transform.position;

            if(attempts > 1000) {
                found = true;
                print("RIP");
            }
        }
    }
}
