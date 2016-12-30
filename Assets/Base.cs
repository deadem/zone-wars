using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : MonoBehaviour {
	public Rigidbody Bot;

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
			Rigidbody bot = (Rigidbody)Instantiate (Bot, transform.position, Quaternion.identity);
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
}
