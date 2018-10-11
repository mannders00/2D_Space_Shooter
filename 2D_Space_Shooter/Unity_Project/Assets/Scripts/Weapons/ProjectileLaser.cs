using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLaser : MonoBehaviour {

    public float projectileSpeed;

    private void Start() {
        Object.Destroy(gameObject, 5);
    }

    void Update () {
        transform.position += Time.deltaTime * transform.up * projectileSpeed;
	}

    private void OnTriggerEnter2D(Collider2D other) {
        other.transform.gameObject.SendMessage("hit");
    }
}
