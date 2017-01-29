﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
	public Vector3 target = new Vector3();
    public bool selected = false;
	public bool isActive = true;

	public void Shot(string player)
	{
		isActive = false;
		Destroy(gameObject, 0.1f);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (!isActive) {
			return;
		}
		const float speed = 1.0f;

		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target, step);

        GetComponentInChildren<Selection>(true).gameObject.SetActive(selected);
    }

	void OnTriggerEnter2D(Collider2D collider) {
		if (!isActive) {
			return;
		}
		if (collider.tag != tag) {
			Base enemyBase = collider.GetComponent<Base>();
			if (enemyBase) {
				enemyBase.Shot (tag);

				isActive = false;
				Destroy(gameObject, 0.1f);
			} else {
				Bot bot = collider.GetComponent<Bot> ();
				if (bot) {
					if (!bot.isActive) {
						return;
					}
					bot.Shot (tag);

					isActive = false;
					Destroy(gameObject, 0.1f);
				}
			}

			AudioSource audio = GetComponent<AudioSource>();
			audio.Play();

			LineRenderer shot = GetComponent<LineRenderer>();
			shot.SetPosition(0, transform.position);
			shot.SetPosition(1, collider.transform.position);

			Color color = GetComponent<SpriteRenderer> ().color;
			shot.startColor = color;
			shot.endColor = color;
		}
	}
}
