using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour {

    [Header("Stats")]
    public int health = 100;

    [Header("Movement")]
    public float moveSpeed = 1F;
    public float lookAtSpeed = 1;

    public Texture2D cursor;
    private Rigidbody2D rb2d;

    private Vector3 velocity;

    public Animator uiAnimate;

    [Header("Scripts")]
    public Weapon_Manager wepManager;
    public UIManager uiManager;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();

        Camera cam = Camera.main; //87.5

        Cursor.SetCursor(cursor, new Vector2(72, 72), CursorMode.Auto);
    }


    float lastPointAngle;
    void Update () {

        //Slerping camera position
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, targetPos, Time.deltaTime * 5);

        /* Don't use this method anymore, at the moment do not have side bounds but may re-implement in the future
        Clamping the bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -shipBounds, shipBounds);
        transform.position = pos;
        */

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
            rb2d.AddForce(Vector2.up * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A)) {
            rb2d.AddForce(Vector2.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S)) {
            rb2d.AddForce(Vector2.down * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb2d.AddForce(Vector2.right * moveSpeed);
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
