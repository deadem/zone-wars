﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour {
	public Vector3 target = new Vector3();
    public bool selected = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		const float speed = 1.0f;

		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target, step);

        GetComponentInChildren<Selection>(true).gameObject.SetActive(selected);
    }
}
