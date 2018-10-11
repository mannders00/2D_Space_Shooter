using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponType {LaserChargeShot, ProjectileLaser};
public class Weapon_Manager : MonoBehaviour {

    public weaponType currentWeapon = weaponType.LaserChargeShot;

    public Animator animator;
    public GameObject spaceShip;
    public Transform cursor;

    public GameObject explosionPrefab;
    public GameObject laser;

    public GameObject projectileLaserPrefab;

    public void setWeapon(int weaponValue) {
        currentWeapon = (weaponType)weaponValue;
    }

    public void fire() {
        switch (currentWeapon) {
            case weaponType.LaserChargeShot: laserChargeShot(); break;
            case weaponType.ProjectileLaser: fireProjectileLaser(); break;
        }
    }

    public void laserChargeShot() {
        animator.SetTrigger("LaserChargeShot");
    }

    public void fireInstLaser() { //Invoked by the "LaserChargeShot" animation (easier this way)

        Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10F)));

        RaycastHit2D hit = Physics2D.Raycast(spaceShip.transform.position, mousePos - spaceShip.transform.position);
        Vector2 laserEnd;

        Debug.DrawRay(spaceShip.transform.position, mousePos);

        if (hit) {
            laserEnd = hit.point;
            //In the future plan to have different weapons sendmessage with damage parameter. Can make global to make balancing easier
            hit.transform.gameObject.SendMessage("hit");
        } else {
            laserEnd = (spaceShip.transform.position - new Vector3(mousePos.x, mousePos.y)) * -5F;
        }

        //Midpoint between ship and end of laser
        Vector3 midPoint = new Vector3((spaceShip.transform.position.x + laserEnd.x) / 2F,
                                        (spaceShip.transform.position.y + laserEnd.y) / 2F, 1F);

        laser.transform.position = midPoint;

        float laserAngle = (180 / Mathf.PI) * Mathf.Atan2(laserEnd.y - spaceShip.transform.position.y,
                                                          laserEnd.x - spaceShip.transform.position.x);

        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, laserAngle - 90F));

        laser.GetComponent<SpriteRenderer>().size = new Vector2(0.05F, Vector3.Distance(spaceShip.transform.position, laserEnd) + 0.25F);

    }

    public void fireProjectileLaser() {

        Vector3 mousePos = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10F)));
        Vector2 laserEnd = (spaceShip.transform.position - new Vector3(mousePos.x, mousePos.y)) * -5F;
        float laserAngle = (180 / Mathf.PI) * Mathf.Atan2(laserEnd.y - spaceShip.transform.position.y,
                                                          laserEnd.x - spaceShip.transform.position.x);

        Instantiate(projectileLaserPrefab, transform.parent.position, Quaternion.Euler(new Vector3(0, 0, laserAngle - 90F)));
    }
}
