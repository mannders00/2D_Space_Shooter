using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {

    [Header("Movement")]
    public float lookAtSpeed = 1;

    public GameObject cursor;

    private void Start() {
    //   print(Camera.main.WorldToScreenPoint(transform.position).x);
    }

    void Update () {

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(worldPos.x, worldPos.y, 0);

        //  Here I just imagine a triangle between the middle of the screen and the mouse
        //  I use trig to find the angle and change that to the rotation, so that the
        //  middle sprite points towards the mouse
        float adj = Input.mousePosition.x - (Camera.main.WorldToScreenPoint(transform.position).x);
        float opp = Input.mousePosition.y - (Camera.main.WorldToScreenPoint(transform.position).y);

        float pointAngle = (180 / Mathf.PI) * Mathf.Atan2(opp, adj);

        //The goal is to make it overshoot a little but bounce back
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, pointAngle), Time.deltaTime * lookAtSpeed);

	}
}
