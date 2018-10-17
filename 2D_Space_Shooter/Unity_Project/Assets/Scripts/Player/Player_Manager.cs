using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {

    [Header("Movement")]
    public float lookAtSpeed = 1;

    public GameObject cursor;

    private Rigidbody2D rb2d;
    private float shipBounds;

    public Animator uiAnimate;

    [Header("Scripts")]
    public Weapon_Manager wepManager;
    public UIManager uiManager;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();

        Camera cam = Camera.main; //87.5
        shipBounds = Mathf.Abs(cam.ScreenToWorldPoint(new Vector3(50F, 0)).x);


    }

    float lastPointAngle;
    void Update () {

        //Setting cursor position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = new Vector3(worldPos.x, worldPos.y, 0);

        //Setting camera position
        Camera.main.transform.position = new Vector3(0, transform.position.y, -10);

        //Clamping the bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -shipBounds, shipBounds);
        transform.position = pos;

        //Using trig to find the angle between two points (spaceship and mouse cursor)
        float adj = Input.mousePosition.x - (Camera.main.WorldToScreenPoint(transform.position).x);
        float opp = Input.mousePosition.y - (Camera.main.WorldToScreenPoint(transform.position).y);

        float pointAngle = (180 / Mathf.PI) * Mathf.Atan2(opp, adj);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, pointAngle - 90F), Time.deltaTime * lookAtSpeed);

        //WS + Mouse
        /*
        if (Input.GetKey(KeyCode.W)) {
            rb2d.AddForce(transform.up * 5F);
        }
        if (Input.GetKey(KeyCode.S)) {
            rb2d.AddForce(Vector2.down * 5F);
        }*/

        //WASD Movement
        if (Input.GetKey(KeyCode.W)) {
            rb2d.AddForce(Vector2.up * 5F);
        }
        if (Input.GetKey(KeyCode.A)) {
            rb2d.AddForce(Vector2.left * 5F);
        }
        if (Input.GetKey(KeyCode.S)) {
            rb2d.AddForce(Vector2.down * 5F);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb2d.AddForce(Vector2.right * 5F);
        }

        //Fire
        if (Input.GetKeyDown(KeyCode.Mouse0)) { //InstLaser
            wepManager.fire();
        }

        //UI Switching
        if (Input.GetKeyDown(KeyCode.E)) {
            uiManager.shiftRight();
            wepManager.setWeapon(uiManager.getSelectedWeapon() - 1);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            uiManager.shiftLeft();
            wepManager.setWeapon(uiManager.getSelectedWeapon() - 1);
        }
    }
}
