using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : MonoBehaviour {
	public Rigidbody Bot;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; ++i) {
			float angle = Random.value * Mathf.PI * 2;
			Rigidbody bot = (Rigidbody) Instantiate(Bot, transform.position, Quaternion.identity);

			Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle)) * 5f;
			bot.GetComponent<Bot> ().target = transform.position + position;

			bot.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
