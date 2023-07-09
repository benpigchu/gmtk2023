using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
	public new Collider2D collider;
	void Awake()
	{
		collider = GetComponent<Collider2D>();
	}

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
	internal void OnCapture()
	{
		collider.enabled=false;
	}

	internal void OnHit()
	{
		gameObject.SetActive(false);
	}
}
