using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
	public Vector3 target = new Vector3();
    public bool selected = false;

	public void Shot(string player)
	{
		Destroy(gameObject);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		const float speed = 1.0f;

		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target, step);

        GetComponentInChildren<Selection>(true).gameObject.SetActive(selected);
    }

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag != tag) {
			//Debug.Log (collider.tag);
			AudioSource audio = GetComponent<AudioSource>();
			audio.Play();

			LineRenderer shot = GetComponent<LineRenderer>();
			shot.SetPosition(0, transform.position);
			shot.SetPosition(1, collider.transform.position);

			Color color = GetComponent<SpriteRenderer> ().color;
			shot.startColor = color;
			shot.endColor = color;

			collider.GetComponent<Base>().Shot (tag);
			Destroy(gameObject, 0.1f);
		}
	}
}
