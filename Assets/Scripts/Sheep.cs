using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SheepController : ScriptableObject
{
	public virtual void Init(Sheep sheep) { }
	public virtual void Update() { }
	public abstract Vector2? GetTargetDirection();
}

public class Sheep : MonoBehaviour
{
	public new Rigidbody2D rigidbody;
	public SpriteRenderer sprite;
	public SheepController controller;
	public SheepMovementConfig movement;

	public bool collected { get; private set; }

	private Bush capturer;
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		controller = Instantiate<SheepController>(controller);
		controller.Init(this);
		collected = false;
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (rigidbody.velocity.x > 0)
		{
			sprite.flipX = false;
		}

		if (rigidbody.velocity.x < 0)
		{
			sprite.flipX = true;
		}
	}
	void FixedUpdate()
	{
		if (capturer != null)
		{
			rigidbody.velocity = Vector2.zero;
			return;
		}
		controller?.Update();
		Vector2 TargetDirection = Vector2.right;
		if (!collected)
		{
			TargetDirection = controller?.GetTargetDirection()?.normalized ?? Vector2.zero;
		}
		Vector2 TargetVelocity = TargetDirection * movement.Speed;
		Vector2 DeltaVelocity = TargetVelocity - rigidbody.velocity;
		if (DeltaVelocity.SqrMagnitude() == 0)
		{
			return;
		}
		Vector2 DeltaDirection = DeltaVelocity.normalized;
		Vector2 ResultDeltaVelocity = DeltaDirection * Mathf.Min(movement.MaxAccelerator * Time.fixedDeltaTime, DeltaVelocity.magnitude);
		rigidbody.velocity += ResultDeltaVelocity;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (collected)
		{
			return;

		}

		Shepherd shepherd = col.GetComponent<Shepherd>();
		if (shepherd != null)
		{
			shepherd.HitSheep(this);
		}

		Bush bush = col.GetComponent<Bush>();
		if (bush != null)
		{
			CapturedBy(bush);
		}

		FinishArea finishArea = col.GetComponent<FinishArea>();
		if (finishArea != null)
		{
			collected = true;
		}
	}

	private void CapturedBy(Bush bush)
	{
		capturer = bush;
		rigidbody.position = bush.transform.position;
        bush.OnCapture();
	}

	public void Hit()
	{
        if(capturer!=null){
            capturer.OnHit();
            capturer=null;
        }else{
    		rigidbody.velocity = new Vector2(movement.HitSpeed, 0);
        }
	}
}
