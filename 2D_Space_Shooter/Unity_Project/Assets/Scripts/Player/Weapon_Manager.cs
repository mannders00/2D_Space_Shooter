using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponType {LaserChargeShot, ProjectileLaser, Rocket};
public class Weapon_Manager : MonoBehaviour {

    public weaponType currentWeapon = weaponType.LaserChargeShot;

    public Animator animator;
    public GameObject spaceShip;
    public Transform cursor;
    bool readyToFire = true;

    public GameObject explosionPrefab;

    public GameObject laser;
    public GameObject projectileLaserPrefab;
    public GameObject rocketPrefab;

    public void setWeapon(int weaponValue) {
        currentWeapon = (weaponType)weaponValue;
    }

    public void fire() {
        if (readyToFire) {
            switch (currentWeapon) {
                case weaponType.LaserChargeShot: laserChargeShot(); break;
                case weaponType.ProjectileLaser: fireProjectileLaser(); break;
                case weaponType.Rocket: fireRocket(); break;
            }
        }
    }

    public void laserChargeShot() {
        readyToFire = false;
        animator.SetTrigger("LaserChargeShot");
    }

    public Quaternion getAngleToMouse() {

        /*Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10F)));
        Vector2 laserEnd = (spaceShip.transform.position - new Vector3(mousePos.x, mousePos.y));

        float laserAngle = (180 / Mathf.PI) * Mathf.Atan2(laserEnd.y - spaceShip.transform.position.y,
                                                          laserEnd.x - spaceShip.transform.position.x);*/

        Vector2 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10F)));
        Vector2 shipPos = spaceShip.transform.position;

        float laserAngle = (180 / Mathf.PI) * Mathf.Atan2(mousePos.y - shipPos.y, mousePos.x - shipPos.x);
        if(laserAngle < 0) {
            laserAngle += 360F;
        }

        return Quaternion.Euler(new Vector3(0, 0, laserAngle - 90F));
    }

    public void fireInstLaser() { //Invoked by the "LaserChargeShot" animation (easier this way)
        readyToFire = true;

        Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10F)));

        RaycastHit2D hit = Physics2D.Raycast(spaceShip.transform.position, mousePos - spaceShip.transform.position);
        Vector3 laserEnd;

        if (hit) {
            laserEnd = hit.point;
            //In the future plan to have different weapons sendmessage with damage parameter. Can make global to make balancing easier
            hit.transform.gameObject.SendMessage("hit", SendMessageOptions.DontRequireReceiver);
        } else {
            laserEnd = transform.position + (Vector3.Normalize(mousePos - transform.position) * 30F);
        }
        
        //Midpoint between ship and end of laser
        Vector3 midPoint = new Vector3((spaceShip.transform.position.x + laserEnd.x) / 2F,
                                        (spaceShip.transform.position.y + laserEnd.y) / 2F, 1F);

        laser.transform.position = midPoint;

        laser.transform.rotation = getAngleToMouse();

        laser.GetComponent<SpriteRenderer>().size = new Vector2(0.05F, Vector3.Distance(spaceShip.transform.position, laserEnd) + 0.25F);

    }

    public void fireProjectileLaser() {

        Vector3 instPos = transform.parent.position;
        Instantiate(projectileLaserPrefab, new Vector3(instPos.x, instPos.y, 3), getAngleToMouse());
    }

    public void fireRocket() {

        Vector3 instPos = transform.parent.position;
        Instantiate(rocketPrefab, new Vector3(instPos.x, instPos.y, 3), getAngleToMouse());
    }
}
