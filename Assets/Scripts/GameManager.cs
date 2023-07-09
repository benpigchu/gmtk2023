using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	public Sheep PlayerSheep;
	public GameObject BotSheepPrefab;
	public RectSpawnArea BotSpawnArea;
	public GameObject BushPrefab;
	public RectSpawnArea BushSpawnArea;
	public new CameraController camera;

	public TextMeshProUGUI timerText;
    public int botSheepCount;
    public int bushCount;
	private List<Sheep> sheeps = new List<Sheep>();
	private List<Bush> bushes = new List<Bush>();

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
		for (int i = 0; i < botSheepCount; i++)
		{
			TryGenerateBotSheep();
		}
		for (int i = 0; i < bushCount; i++)
		{
			TryGenerateBush();
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

	private void TryGenerateBush()
	{
		Vector2? position = BushSpawnArea.SelectSpawnPosition(Vector2.one);
		if (position != null)
		{
			GameObject bushGameObject = Instantiate(BushPrefab, position.Value, Quaternion.identity);
			Bush bush = bushGameObject.GetComponent<Bush>();
			bushes.Add(bush);
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
