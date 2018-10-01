using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {

	public void hit() {
        Destroy(gameObject);
    }
}
