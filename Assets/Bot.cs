using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : ManagedUpdateBehavor
{
	public Vector3 target = new Vector3();
	public bool isActive = true;
	public float speed = 1.0f;
	public float energy = 2;
	public List<Collider> enemies = new List<Collider>();
	private Transform transformation;
	public float shotTime = 0;
	LineRenderer shot;
	private GameObject selectionObject;

	public bool IsSelected()
	{
		return selectionObject && selectionObject.activeSelf;
	}

	public void Select(bool selected)
	{
		if (selectionObject) {
			selectionObject.SetActive(selected);
		}
	}

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
	public override void Start()
	{
		shot = GetComponent<LineRenderer>();

		Color color = GetComponent<SpriteRenderer>().color;
		shot.startColor = color;
		shot.endColor = color;

		shotTime = Random.value;
		transformation = transform;
		selectionObject = GetComponentInChildren<Selection>(true).gameObject;

		gameObject.layer = LayerMask.NameToLayer(CompareTag("Player") ? "Player" : "Enemy");

		base.Start();
	}
	
	public override void ManagedUpdate()
	{
		transformation.position = Vector3.MoveTowards(transformation.position, target, speed * Time.deltaTime);
		shotTime += Time.deltaTime;

		base.ManagedUpdate();
	}
		
	void FixedUpdate()
	{
		if (shotTime > 0.05 && shot.positionCount != 0) {
			shot.positionCount = 0;
		}
		if (shotTime > 1 && enemies.Count != 0) {
			enemies.RemoveAll(x => !x);
			Collider lastMob = enemies.Find(x => x && !x.CompareTag(tag));

			if (lastMob) {
				shotTime = Random.value;

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

				shot.positionCount = 2;
				shot.SetPosition(0, transformation.position);
				shot.SetPosition(1, lastMob.transform.position);
			}
		}
	}
		

	void OnTriggerEnter(Collider collider)
	{
		if (!collider.CompareTag(tag)) {
			enemies.Add(collider);
		}
	}

	void OnTriggerExit(Collider collider)
	{
		enemies.Remove(collider);
	}
}
