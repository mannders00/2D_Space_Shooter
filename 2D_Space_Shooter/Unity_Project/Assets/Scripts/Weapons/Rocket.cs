using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    public float projectileSpeed;

    void Start () {
        Object.Destroy(gameObject, 10);
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * transform.up * projectileSpeed;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        other.transform.gameObject.SendMessage("hit", SendMessageOptions.DontRequireReceiver);
        Object.Destroy(gameObject);
    }
}
