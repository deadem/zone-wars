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
	LineRenderer shot;

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
		shot = GetComponent<LineRenderer>();

		Color color = GetComponent<SpriteRenderer>().color;
		shot.startColor = color;
		shot.endColor = color;

		shotTime = Random.value;
	}
	
	// Update is called once per frame
	void Update()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector2.MoveTowards(transform.position, target, step);

		GetComponentInChildren<Selection>(true).gameObject.SetActive(selected);

		shotTime += Time.deltaTime;
		if (shotTime > 0.05) {
		  shot.numPositions = 0;
		}
			
		if (shotTime > 1) {
			enemies.RemoveAll(x => !x);
			Collider2D lastMob = enemies.Find(x => x && x.tag != tag);

			if (lastMob) {
				shotTime = 0;

				Base enemyBase = lastMob.GetComponent<Base>();
				if (enemyBase) {
					enemyBase.Shot(tag);
				} else {
					Bot bot = lastMob.GetComponent<Bot>();
					if (bot) {
						bot.Shot(tag);
					}
				}

				GameManager.instance.Shot();

				shot.numPositions = 2;
				shot.SetPosition(0, transform.position);
				shot.SetPosition(1, lastMob.transform.position);
			}
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
		enemies.Remove(collider);
	}
}
