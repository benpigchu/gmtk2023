using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public Sheep PlayerSheep;
	public GameObject BotSheepPrefab;
	public RectSpawnArea BotSpawnArea;
	public new CameraController camera;

	public TextMeshProUGUI timerText;
	private List<Sheep> sheeps = new List<Sheep>();

    private bool finished=false;

    private float timer=0;

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
		sheeps.Add(PlayerSheep);
		for (int i = 0; i < 2; i++)
		{
			TryGenerateBotSheep();
		}
	}

	private void TryGenerateBotSheep()
	{
		Vector2? position = BotSpawnArea.SelectSpawnPosition(Vector2.one);
		if (position != null)
		{
			GameObject sheepGameObject = Instantiate(BotSheepPrefab, position.Value, Quaternion.identity);
			Sheep sheep = sheepGameObject.GetComponent<Sheep>();
			sheeps.Add(sheep);
		}
	}

	// Update is called once per frame
	void Update()
	{
        if(!finished){
            timer+=Time.deltaTime;
			int timeRemainSeconds = System.Math.Max(0, Mathf.FloorToInt(timer));
			timerText.text = $"{timeRemainSeconds / 60}:{timeRemainSeconds % 60:D2}";
            bool allCollected=true;
            foreach (var sheep in sheeps)
            {
                if(!sheep.collected){
                    allCollected=false;
                    break;
                }
            }
            if(allCollected){
                finished=true;
                Debug.Log("Finished");
            }
        }
	}

	public Sheep FindLastSheep()
	{
		float xMin = float.PositiveInfinity;
		Sheep result = null;
		foreach (var sheep in sheeps)
		{
            if(sheep.collected){
                continue;
            }
			if (sheep.transform.position.x < xMin)
			{
				xMin = sheep.transform.position.x;
				result = sheep;
			}
		}
		return result;
	}
}
