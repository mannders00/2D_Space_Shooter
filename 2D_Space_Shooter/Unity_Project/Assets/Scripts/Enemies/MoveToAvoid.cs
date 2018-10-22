using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAvoid : MonoBehaviour {

    public GameObject player;

    Vector3 rayLeftSide;
    Vector3 rayLeftAngle;
    Vector3 rayLeft;
    Vector3 rayMiddle;
    Vector3 rayRight;
    Vector3 rayRightAngle;
    Vector3 rayRightSide;

    float magnitude = 1.5F;
    float sideLength = 0.75F;
    float width = 0.25F;
    float rayAngle = 50F;
    float rotateSensitivity = 40;

    private void setupRays() {
        Vector3 pos = transform.position;

        rayLeftSide = -transform.right * sideLength;
        rayLeftAngle = Quaternion.Euler(new Vector3(0, 0, -rayAngle)) * -transform.up * magnitude;
        rayLeft = transform.position + (-transform.right * width);
        rayMiddle = -transform.up * magnitude;
        rayRight = transform.position + (transform.right * width);
        rayRightAngle = Quaternion.Euler(new Vector3(0, 0, rayAngle)) * -transform.up * magnitude;
        rayRightSide = transform.right * sideLength;
    }

    // Update is called once per frame
    void Update () {

        transform.position += -transform.up * Time.deltaTime * 2F;

        setupRays();
        Debug.DrawRay(transform.position, rayLeftSide);
        Debug.DrawRay(rayLeft, rayLeftAngle); //Left Angle
        Debug.DrawRay(rayLeft, rayMiddle); //Left Straight
        Debug.DrawRay(transform.position, rayMiddle * 1.1F); //Middle
        Debug.DrawRay(rayRight, rayMiddle); //Right Straight
        Debug.DrawRay(rayRight, rayRightAngle); //Right Angle
        Debug.DrawRay(transform.position, rayRightSide);

        bool hasToAvoid = false;
        RaycastHit2D hit = new RaycastHit2D();

        Vector3 hitTangent = Vector3.zero;

        /*if(Physics2D.Raycast(rayRight, rayRightAngle, magnitude)) {  //print("rayMiddle");
            hit = Physics2D.Raycast(rayRight, rayRightAngle, magnitude);
            Vector3 norm = hit.normal;
            //avoidRot = Quaternion.Euler(new Vector3(norm.x, norm.y, norm.z - 90));
            Debug.DrawRay(hit.point, Vector3.Cross(norm, -transform.forward), Color.red);
        }*/

        if (Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F)) { // print("rayLeftAngle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F).normal, -transform.forward);

            Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F).normal, -transform.forward), Color.red);
        }
        if (Physics2D.Raycast(rayLeft, rayMiddle, magnitude)) { // print("rayLeft");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayLeft, rayMiddle, magnitude).normal, transform.forward);

            Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayLeft, rayMiddle, magnitude).normal, transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude)) { // print("rayLeftAngle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude).normal, transform.forward);

            Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude).normal, transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayRight, rayMiddle, magnitude)) { // print("rayRight");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayRight, rayMiddle, magnitude).normal, -transform.forward);

            Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayRight, rayMiddle, magnitude).normal, -transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayRight, rayRightAngle, magnitude)) { // print("rayRightAngle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayRight, rayRightAngle, magnitude).normal, -transform.forward);

            Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayRight, rayRightAngle, magnitude).normal, -transform.forward), Color.red);
        }

        if (Physics2D.Raycast(transform.position, rayLeftSide, sideLength) || Physics2D.Raycast(transform.position, rayRightSide, sideLength)) { //if there is something touching the side raycast, it shoudln't look towards the player it should go forward
            hasToAvoid = true;
        }
        
        //Debug.DrawRay(transform.position, -transform.up, Color.green);

        if (!hasToAvoid) {
            float rotZ = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, rotZ + 90)), Time.deltaTime);
        }else{
            transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.FromToRotation(-transform.up, hitTangent) * transform.rotation), Time.deltaTime);
        }
    }
}
