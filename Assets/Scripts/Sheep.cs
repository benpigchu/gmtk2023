using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SheepController : ScriptableObject
{
	public virtual void Init(Sheep sheep) { }
	public abstract Vector2? GetTargetDirection();
}

public class Sheep : MonoBehaviour
{
	public new Rigidbody2D rigidbody;
	public SheepController controller;
    public SheepMovementConfig movement;
	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		controller.Init(this);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	void FixedUpdate()
	{
		Vector2 TargetDirection = controller?.GetTargetDirection() ?? Vector2.zero;
        Vector2 TargetVelocity = TargetDirection*movement.Speed;
        Vector2 DeltaVelocity = TargetVelocity-rigidbody.velocity;
        if(DeltaVelocity.SqrMagnitude()==0){
            return;
        }
        Vector2 DeltaDirection = DeltaVelocity.normalized;
        Vector2 ResultDeltaVelocity = DeltaDirection*Mathf.Min(movement.MaxAccelerator*Time.fixedDeltaTime,DeltaVelocity.magnitude);
        rigidbody.velocity+=ResultDeltaVelocity;
	}
}
