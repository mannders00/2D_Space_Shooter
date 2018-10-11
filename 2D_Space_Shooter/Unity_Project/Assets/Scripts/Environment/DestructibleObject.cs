using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {

    //GameObject explosionPrefab;

    public void hit() {
    //    Instantiate(explosionPrefab, laserEnd, Quaternion.identity);
        Destroy(gameObject);
    }
}
