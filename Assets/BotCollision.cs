using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCollision : MonoBehaviour {

	void OnCollisionExit(Collision collisionInfo) {
		Debug.Log("No longer in contact with " + collisionInfo.transform.name);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		//foreach (ContactPoint contact in collision.contacts) {
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}
		Debug.Log ("collide 2d");

		//if (collision.relativeVelocity.magnitude > 2) {
			//audio.Play();
		//}

	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log ("collide");
	}

	void OnTriggerEnter2D(Collider2D collider) {
		Debug.Log (collider.transform.name);
	}
}
