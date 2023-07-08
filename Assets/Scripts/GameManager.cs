using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Sheep playerSheep;
    private List<Sheep> sheeps=new List<Sheep>();

    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
        sheeps.Add(playerSheep);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sheep FindLastSheep(){
        float xMin=float.PositiveInfinity;
        Sheep result=null;
        foreach (var sheep in sheeps)
        {
            if(sheep.transform.position.x<xMin){
                xMin=sheep.transform.position.x;
                result=sheep;
            }
        }
        return result;
    }
}
