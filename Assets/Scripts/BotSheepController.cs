using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "BotController", menuName = "ScriptableObjects/BotSheepController")]
public class BotSheepController : SheepController
{
	public float restTimeout;
	public float moveTimeout;
	private float timer;
	private Vector2? direction;
	public override Vector2? GetTargetDirection()
	{
		return direction;
	}
	public override void Update() {
		timer+=Time.deltaTime;
		if(direction==null){
			if(timer>restTimeout){
				timer=0;
				float angle=Random.value*Mathf.PI*2;
				direction=new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));
			}
		}else{
			if(timer>moveTimeout){
				timer=0;
				direction=null;
			}
		}
	}
}