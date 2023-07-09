using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameView
{
	Title,
	Playing,
	Result,
}
public class GameManager : MonoBehaviour
{

	public Sheep PlayerSheep;
	public Shepherd Shepherd;
	public GameObject BotSheepPrefab;
	public RectSpawnArea BotSpawnArea;
	public GameObject BushPrefab;
	public RectSpawnArea BushSpawnArea;
	public new CameraController camera;

	public TextMeshProUGUI timerText;
	public TextMeshProUGUI resultText;
	private string resultTemplate;

	public int botSheepCount;
	public int bushCount;

	public float finishedTime;

	public GameObject PlayingUILayer;
	public GameObject ResultUILayer;
	public GameObject TitleUILayer;
	public GameObject Playfield;
	public GameView initialView = GameView.Title;
	private GameView currentView = GameView.Title;
    public AudioSource AudioSource;
    public AudioClip SheepSound;
    public AudioClip CapturedSound;
    public AudioClip DestroyedSound;
	private List<Sheep> sheeps = new List<Sheep>();
	private List<Bush> bushes = new List<Bush>();

	private bool finished = false;

	private float timer = 0;
	private float finishedTimer = 0;

	public static GameManager Instance;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		resultTemplate = resultText.text;
	}

	// Start is called before the first frame update
	void Start()
	{
		SetGameView(initialView);
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
		if (currentView == GameView.Playing)
		{
			if (finished)
			{
				finishedTimer += Time.deltaTime;
				if (finishedTimer > finishedTime)
				{
					SetGameView(GameView.Result);
					int timeSeconds = System.Math.Max(0, Mathf.FloorToInt(timer));
					string result = $"{timeSeconds / 60:D2}:{timeSeconds % 60:D2}";
					resultText.text = string.Format(resultTemplate, result);
				}
			}
			else
			{
				timer += Time.deltaTime;
				int timeSeconds = System.Math.Max(0, Mathf.FloorToInt(timer));
				timerText.text = $"{timeSeconds / 60:D2}:{timeSeconds % 60:D2}";
				bool allCollected = true;
				foreach (var sheep in sheeps)
				{
					if (!sheep.collected)
					{
						allCollected = false;
						break;
					}
				}
				if (allCollected)
				{
					finished = true;
				}
			}
		}
		else
		{
			if (Keyboard.current.spaceKey.IsPressed())
			{
				SetGameView(GameView.Playing);
				Reset();
			}
		}
	}

	private void Reset()
	{
		foreach (var sheep in sheeps)
		{
			if (sheep != PlayerSheep)
			{
				Destroy(sheep.gameObject);
			}
		}
		foreach (var bush in bushes)
		{
			Destroy(bush.gameObject);
		}
		sheeps.Clear();
		bushes.Clear();
		PlayerSheep.Reset();
		Shepherd.Reset();
		sheeps.Add(PlayerSheep);
		for (int i = 0; i < botSheepCount; i++)
		{
			TryGenerateBotSheep();
		}
		for (int i = 0; i < bushCount; i++)
		{
			TryGenerateBush();
		}
		timer = 0;
		finishedTimer = 0;
		finished = false;
	}

	public Sheep FindLastSheep()
	{
		float xMin = float.PositiveInfinity;
		Sheep result = null;
		foreach (var sheep in sheeps)
		{
			if (sheep.collected)
			{
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

	void SetGameView(GameView view)
	{
		currentView = view;
		PlayingUILayer.SetActive(view == GameView.Playing);
		ResultUILayer.SetActive(view == GameView.Result);
		TitleUILayer.SetActive(view == GameView.Title);
		Playfield.SetActive(view != GameView.Title);
	}

    public void PlayAudio(AudioClip clip,float volume=1){
        AudioSource.PlayOneShot(clip,volume);
    }
}
