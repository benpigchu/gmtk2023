using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shepherd : MonoBehaviour
{

	public new Rigidbody2D rigidbody;
	public SpriteRenderer sprite;

    public float speed;
	private Sheep target;
	private float timer=float.PositiveInfinity;

    public float restTimeout;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
	}
    // Start is called before the first frame update
    void Start()
    {
        if(rigidbody.velocity.x>0){
            sprite.flipX=false;
        }

        if(rigidbody.velocity.x<0){
            sprite.flipX=true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
	void FixedUpdate()
	{
        if(target==null){
            rigidbody.velocity=Vector2.zero;
            timer+=Time.fixedDeltaTime;
            if(timer>restTimeout){
                target=GameManager.Instance.FindLastSheep();
            }
        }
        if(target!=null){
            rigidbody.velocity=speed*(target.transform.position-transform.position).normalized;
        }
	}

    public void HitSheep(Sheep sheep){
        if(sheep==target){
            sheep.Hit();
            target=null;
            timer=0;
        }
    }
}
