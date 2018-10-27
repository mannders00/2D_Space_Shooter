using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	void FixedUpdate () {
        transform.position = Mathfx.Bounce(transform.position);
	}
}
