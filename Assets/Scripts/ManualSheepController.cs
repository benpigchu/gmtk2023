using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ManualController", menuName = "ScriptableObjects/ManualSheepController")]
public class ManualSheepController : SheepController
{
	public InputAction move;
	public override void Init(Sheep sheep)
	{
		move.Enable();
	}
	public override Vector2? GetTargetDirection()
	{
		Vector2 value = move.ReadValue<Vector2>();
		if (value.SqrMagnitude() > 0.5)
		{
			return value.normalized;
		}
		return null;
	}
}