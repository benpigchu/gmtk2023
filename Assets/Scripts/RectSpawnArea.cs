using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectSpawnArea : MonoBehaviour
{
	public float width;
	public float height;
	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
	}

	public Vector2? SelectSpawnPosition(Vector2 objectSize)
	{
		float xMin = transform.position.x - width / 2;
		float xMax = transform.position.x + width / 2;
		float yMin = transform.position.y - height / 2;
		float yMax = transform.position.y + height / 2;
		int attemptedTime = 0;
		float x = 0;
		float y = 0;
		while (true)
		{
			x = Random.Range(xMin, xMax);
			y = Random.Range(yMin, yMax);
			var castBoxResult = Physics2D.BoxCast(new Vector2(x, y), objectSize, 0, Vector2.up, 0);
			if (castBoxResult.collider == null)
			{
				break;
			}
			attemptedTime++;
			if (attemptedTime >= 5)
			{
				return null;
			}
		}
		return new Vector2(x,y);
	}
}