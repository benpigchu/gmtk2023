using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "ScriptableObjects/SheepMovementConfig")]
public class SheepMovementConfig : ScriptableObject
{
	public float Speed;
	public float MaxAccelerator;
	public float HitSpeed;
}