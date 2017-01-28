﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : MonoBehaviour {
	public Bot botPrefab;
	public double power = 100;

	private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color> {
		{ "Player", Color.yellow },
		{ "EnemyGreen", Color.green },
		{ "EnemyRed", Color.red },
		{ "", Color.gray }
	};

	private Color getColor() {
		foreach(KeyValuePair<string, Color> color in colors) {
			if (color.Key == tag || color.Key == "") {
				return color.Value;
			}
		}

		return Color.cyan;
	}

	// Use this for initialization
	void Start() {
		InvokeRepeating ("CloneBot", 1f, 1f);
	}

	void CloneBot() {
		if (getColor() != Color.gray) {
			float angle = Random.value * Mathf.PI * 2;
			Bot bot = (Bot)Instantiate (botPrefab, transform.position, Quaternion.identity);
			bot.GetComponent<SpriteRenderer> ().color = getColor ();
			bot.gameObject.tag = tag;

			Vector3 position = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle)) * 1.5f * (Random.value + 1f);
			bot.GetComponent<Bot> ().target = transform.position + position;

			bot.gameObject.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update() {
		GetComponent<SpriteRenderer>().color = getColor();
	}

	void OnGUI()
	{
		float barDisplay = 5;
		Texture2D progressBarEmpty = new Texture2D(10, 10);
		Texture2D progressBarFull = new Texture2D(10, 10);

		Vector2 size = new Vector2(60, 5);
		Vector3 position = Camera.main.WorldToScreenPoint (transform.position);

		Rect pos = new Rect ();

		pos.x = position.x - size.x / 2;
		pos.y = position.y + 20;
		pos.width = size.x;
		pos.height = size.y;

		GUI.color = getColor ();
		GUI.DrawTexture (pos, progressBarEmpty);
		GUI.color = Color.white;
	} 

}
