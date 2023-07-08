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

    public bool collected{get;private set;}
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
        controller=Instantiate<SheepController>(controller);
		controller.Init(this);
        collected=false;
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
        if(rigidbody.velocity.x>0){
            sprite.flipX=false;
        }

        if(rigidbody.velocity.x<0){
            sprite.flipX=true;
        }
	}
	void FixedUpdate()
	{
        controller?.Update();
		Vector2 TargetDirection = Vector2.right;
        if(!collected){
            TargetDirection=controller?.GetTargetDirection()?.normalized ?? Vector2.zero;
        }
        Vector2 TargetVelocity = TargetDirection*movement.Speed;
        Vector2 DeltaVelocity = TargetVelocity-rigidbody.velocity;
        if(DeltaVelocity.SqrMagnitude()==0){
            return;
        }
        Vector2 DeltaDirection = DeltaVelocity.normalized;
        Vector2 ResultDeltaVelocity = DeltaDirection*Mathf.Min(movement.MaxAccelerator*Time.fixedDeltaTime,DeltaVelocity.magnitude);
        rigidbody.velocity+=ResultDeltaVelocity;
	}

    void OnTriggerStay2D(Collider2D col)
    {
        Shepherd shepherd=col.GetComponent<Shepherd>();
        if(shepherd!=null){
            shepherd.HitSheep(this);
        }

        FinishArea finishArea=col.GetComponent<FinishArea>();
        if(finishArea!=null){
            collected=true;
        }
    }

    public void Hit(){
        rigidbody.velocity=new Vector2(movement.HitSpeed,0);
    }
}
