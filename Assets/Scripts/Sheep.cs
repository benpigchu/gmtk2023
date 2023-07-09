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

	private Vector3 initPosition;

	private Bush capturer;
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		controller = Instantiate<SheepController>(controller);
		controller.Init(this);
		collected = false;
		initPosition = transform.position;
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
            rigidbody.bodyType=RigidbodyType2D.Static;
			return;
		}
        rigidbody.bodyType=RigidbodyType2D.Dynamic;
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

    void OnCollisionStay2D(Collision2D collision)
	{
        if(collision.relativeVelocity.magnitude>movement.Speed*1.1){
            Release();
        }
    }

	private void CapturedBy(Bush bush)
	{
		GameManager.Instance.PlayAudio(GameManager.Instance.CapturedSound,0.25f);
		capturer = bush;
        transform.position=bush.transform.position;
		rigidbody.position = bush.transform.position;
        rigidbody.bodyType=RigidbodyType2D.Static;
        bush.OnCapture();
	}

    void Release(){
        if(capturer!=null){
            capturer.OnHit();
            rigidbody.bodyType=RigidbodyType2D.Dynamic;
            capturer=null;
        }
    }

	public void Hit()
	{
		GameManager.Instance.PlayAudio(GameManager.Instance.SheepSound,0.25f);
        if(capturer!=null){
            Release();
        }else{
    		rigidbody.velocity = new Vector2(movement.HitSpeed, 0);
        }
	}

	internal void Reset()
	{
		transform.position=initPosition;
		rigidbody.position=initPosition;
		rigidbody.velocity=Vector2.zero;
		rigidbody.bodyType=RigidbodyType2D.Dynamic;
		collected=false;
		capturer=null;
	}
}
