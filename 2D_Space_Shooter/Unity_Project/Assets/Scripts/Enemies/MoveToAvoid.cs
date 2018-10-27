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

    public float chaseDistance;
    public float maxEnemyVelocity;
    float enemyVelocity;

    public GameObject enemy;

    public float magnitude = 2F;
    float sideLength = .75F;
    float width = 0.25F;
    float rayAngle = 45F;

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

        enemy.transform.position = transform.position;
        transform.position = enemy.transform.position;

        
        if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance) {
            enemyVelocity = Mathf.Lerp(enemyVelocity, maxEnemyVelocity, Time.deltaTime * 2);
        } else {
            enemyVelocity = Mathf.Lerp(enemyVelocity, 0, Time.deltaTime * 2);
        }
        transform.position += -transform.up * Time.deltaTime * enemyVelocity;

        setupRays();
        /*undo comments if curious what the rays look like
        Debug.DrawRay(transform.position, rayLeftSide);
        Debug.DrawRay(rayLeft, rayLeftAngle); //Left Angle
        Debug.DrawRay(rayLeft, rayMiddle); //Left Straight
        Debug.DrawRay(transform.position, rayMiddle * 1.1F); //Middle
        Debug.DrawRay(rayRight, rayMiddle); //Right Straight
        Debug.DrawRay(rayRight, rayRightAngle); //Right Angle
        Debug.DrawRay(transform.position, rayRightSide);
        */


        bool hasToAvoid = false;
        RaycastHit2D hit = new RaycastHit2D();

        Vector3 hitTangent = Vector3.zero;

        

        if (Physics2D.Raycast(transform.position, rayLeftSide, sideLength)) { // print("rayLefSide");

            hasToAvoid = true;
            hit = Physics2D.Raycast(transform.position, rayLeftSide, sideLength);
            hitTangent = Vector3.Cross(hit.normal, transform.forward);

            Vector3 safePos = transform.position + (new Vector3(hit.normal.x, hit.normal.y) * sideLength);
            transform.position = Vector3.Lerp(transform.position, safePos, Time.deltaTime);
            // Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(transform.position, rayLeftSide, sideLength).normal, transform.forward), Color.red);
        } 
        else if (Physics2D.Raycast(transform.position, rayRightSide, sideLength)) { // print("rayRightSide");

            hasToAvoid = true;
            hit = Physics2D.Raycast(transform.position, rayRightSide, sideLength);
            hitTangent = Vector3.Cross(hit.normal, -transform.forward);

            Vector3 safePos = transform.position + (new Vector3(hit.normal.x, hit.normal.y) * sideLength);
            transform.position = Vector3.Lerp(transform.position, safePos, Time.deltaTime);
            // Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude).normal, transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude)) { // print("rayLeftAngle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude).normal, transform.forward);
            //Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayLeft, rayLeftAngle, magnitude).normal, transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayRight, rayRightAngle, magnitude)) { // print("rayRightAngle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayRight, rayRightAngle, magnitude).normal, -transform.forward);
            //Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayRight, rayRightAngle, magnitude).normal, -transform.forward), Color.red);
        } 
        else if (Physics2D.Raycast(rayLeft, rayMiddle, magnitude)) { // print("rayLeft");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayLeft, rayMiddle, magnitude).normal, transform.forward);
            //Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayLeft, rayMiddle, magnitude).normal, transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(rayRight, rayMiddle, magnitude)) { // print("rayRight");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(rayRight, rayMiddle, magnitude).normal, -transform.forward);
            //Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(rayRight, rayMiddle, magnitude).normal, -transform.forward), Color.red);
        }
        else if (Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F)) { // print("rayMiddle");

            hasToAvoid = true;
            hitTangent = Vector3.Cross(Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F).normal, -transform.forward);
            //Debug.DrawRay(hit.point, Vector3.Cross(Physics2D.Raycast(transform.position, rayMiddle, magnitude * 1.1F).normal, -transform.forward), Color.red);
        }

        float rotZ = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        if (!hasToAvoid) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, rotZ + 90)), Time.deltaTime * 2);
        }else{
            transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.FromToRotation(-transform.up, hitTangent) * transform.rotation), Time.deltaTime * 2);
        }

        enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, Quaternion.Euler(0, 0, rotZ + 90), Time.deltaTime * 2);
    }

    public void hit() {
        //    Instantiate(explosionPrefab, laserEnd, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
