using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
	public Vector3 target = new Vector3();
	public bool selected = false;
	public bool isActive = true;
	public float speed = 1.0f;
	public float energy = 2;
	public List<Collider2D> enemies = new List<Collider2D>();
	public float shotTime = 0;

	public void Shot(string player)
	{
		isActive = false;
		if (--energy <= 0) {
			isActive = false;
			Destroy(gameObject, 0.1f);
		}
	}

	public void Attack(Vector2 coordinates)
	{
		float angle = Random.value * Mathf.PI * 2;
		target = coordinates + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.value;
	}

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target, step);

		GetComponentInChildren<Selection>(true).gameObject.SetActive(selected);

		shotTime += Time.deltaTime;
		if (shotTime > 0.05) {
			GetComponent<LineRenderer>().numPositions = 0;
		}

		Collider2D lastMob = enemies.Find(x => x && x.tag != tag);

		if (shotTime > 1 && lastMob) {
			shotTime = 0;

			Base enemyBase = lastMob.GetComponent<Base>();
			if (enemyBase) {
				enemyBase.Shot(tag);

				//isActive = false;
				//Destroy(gameObject, 0.1f);
			} else {
				Bot bot = lastMob.GetComponent<Bot>();
				if (bot) {

					if (!bot.isActive) {
						//return;
					}
					bot.Shot(tag);

					//isActive = false;
					//Destroy(gameObject, 0.1f);
				}
			}

			GameManager.instance.Shot();

			LineRenderer shot = GetComponent<LineRenderer>();
			shot.numPositions = 2;
			shot.SetPosition(0, transform.position);
			shot.SetPosition(1, lastMob.transform.position);

			Color color = GetComponent<SpriteRenderer>().color;
			shot.startColor = color;
			shot.endColor = color;
		}
	}
		

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag != tag) {
			enemies.Add(collider);
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag != tag) {
			enemies.Remove(collider);
		}
	}
}
