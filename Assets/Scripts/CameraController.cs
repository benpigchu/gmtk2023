using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Rect Restriction;
	void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(Restriction.center, Restriction.size);
	}
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position=transform.position;
        position.x=Mathf.Clamp(GameManager.Instance.PlayerSheep.transform.position.x,Restriction.xMin,Restriction.xMax);
        position.y=Mathf.Clamp(GameManager.Instance.PlayerSheep.transform.position.y,Restriction.yMin,Restriction.yMax);
        transform.position=position;
    }
}
