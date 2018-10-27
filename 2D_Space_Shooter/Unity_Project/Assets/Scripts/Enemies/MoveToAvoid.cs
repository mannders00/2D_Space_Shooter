using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAvoid : MonoBehaviour {

    public GameObject player;

    Vector3 posLeftSide;
    Vector3 posLeftAngle;
    Vector3 posLeft;
    Vector3 posMiddle;
    Vector3 posRight;
    Vector3 posRightAngle;
    Vector3 posRightSide;

    public float chaseDistance;
    public float maxEnemyVelocity;
    float enemyVelocity;

    public GameObject enemy;

    public float magnitude = 2F;
    float sideLength = .75F;
    float width = 0.25F;
    float rayAngle = 45F;

    private void setupPos() {
        Vector3 pos = transform.position;

        posLeftSide = -transform.right * sideLength;
        posLeftAngle = Quaternion.Euler(new Vector3(0, 0, -rayAngle)) * -transform.up * magnitude;
        posLeft = transform.position + (-transform.right * width);
        posMiddle = -transform.up * magnitude;
        posRight = transform.position + (transform.right * width);
        posRightAngle = Quaternion.Euler(new Vector3(0, 0, rayAngle)) * -transform.up * magnitude;
        posRightSide = transform.right * sideLength;
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

        /*undo comments if curious what the rays look like
        Debug.DrawRay(transform.position, posLeftSide);
        Debug.DrawRay(posLeft, posLeftAngle); //Left Angle
        Debug.DrawRay(posLeft, posMiddle); //Left Straight
        Debug.DrawRay(transform.position, posMiddle * 1.1F); //Middle
        Debug.DrawRay(posRight, posMiddle); //Right Straight
        Debug.DrawRay(posRight, posRightAngle); //Right Angle
        Debug.DrawRay(transform.position, posRightSide);
        */


        bool hasToAvoid = false;
        RaycastHit2D hit = new RaycastHit2D();
        Vector3 hitTangent = Vector3.zero;


        setupPos(); //This sets up the starting points of ecah ray that we will be casting now

        //This sets up all the rays. I do it here because I use them multiple times in the if statements,
        //And it's more concise to define them here. It should be easier to visualize as well.
        RaycastHit2D rayLeftSide = Physics2D.Raycast(transform.position, posLeftSide, sideLength);
        RaycastHit2D rayRightSide = Physics2D.Raycast(transform.position, posRightSide, sideLength);
        RaycastHit2D rayLeftAngle = Physics2D.Raycast(posLeft, posLeftAngle, magnitude);
        RaycastHit2D rayRightAngle = Physics2D.Raycast(posRight, posRightAngle, magnitude);
        RaycastHit2D rayLeft = Physics2D.Raycast(posLeft, posMiddle, magnitude);
        RaycastHit2D rayRight = Physics2D.Raycast(posRight, posMiddle, magnitude);
        RaycastHit2D rayMiddle = Physics2D.Raycast(transform.position, posMiddle, magnitude * 1.1F);

        if (rayLeftSide) {

            hasToAvoid = true;
            hit = rayLeftSide;
            hitTangent = Vector3.Cross(hit.normal, transform.forward);

            Vector3 safePos = transform.position + (new Vector3(hit.normal.x, hit.normal.y) * sideLength);
            transform.position = Vector3.Lerp(transform.position, safePos, Time.deltaTime);
        } 
        else if (rayRightSide) {

            hasToAvoid = true;
            hit = rayRightSide;
            hitTangent = Vector3.Cross(hit.normal, -transform.forward);

            Vector3 safePos = transform.position + (new Vector3(hit.normal.x, hit.normal.y) * sideLength);
            transform.position = Vector3.Lerp(transform.position, safePos, Time.deltaTime);
        }
        if (rayLeftAngle) {

            hasToAvoid = true;
            hitTangent = Vector3.Cross(rayLeftAngle.normal, transform.forward);
        }
        else if (rayRightAngle) {

            hasToAvoid = true;
            hitTangent = Vector3.Cross(rayRightAngle.normal, -transform.forward);
        } 
        else if (rayLeft) {

            hasToAvoid = true;
            hitTangent = Vector3.Cross(rayLeft.normal, transform.forward);
        }
        else if (rayRight) {

            hasToAvoid = true;
            hitTangent = Vector3.Cross(rayRight.normal, -transform.forward);
        }
        else if (rayMiddle) {

            hasToAvoid = true;
            hitTangent = Vector3.Cross(rayMiddle.normal, -transform.forward);
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
