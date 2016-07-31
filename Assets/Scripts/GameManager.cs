using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Transform enemy;
	private List<Transform> spawnPoints = new List<Transform>(); 
	public List<Transform> walls = new List<Transform> ();
	public GameObject PauseDisplay;
	public GameObject PauseFade;
	public float fightRoundTime;
	public float buildRoundTime;
	public float spawnFrequency;
	private GameObject buildMenu;
	private float lastSpawnTime = 0.0f;
	private float roundStartTime = 0.0f;
	private bool spawnEnabled = true;
	private int roundNum = 1;
	public int numAlive = 0;
	private bool paused = false;
	private bool buildEnabled = false;

	// Use this for initialization
	void Start () {

		// Keep track of spawn points
		getSpawnPoints();

		//Find build menu, set inactive
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (Input.GetKeyDown (KeyCode.Space))
			paused = !paused;

		if (paused) {
			Time.timeScale = 0.0f;
			PauseDisplay.SetActive (true);
			PauseFade.SetActive (true);
		} 

		else {
			Time.timeScale = 1.0f;
			PauseDisplay.SetActive (false);
			PauseFade.SetActive (false);
		}

		if (!paused) {

			if (buildEnabled)
				buildRoundBehavior ();
			else
				roundBehavior ();
		}
	}

	void getSpawnPoints()
	{
		for (int i = 0; i < transform.childCount; i++) 
		{
			Transform child = transform.GetChild (i).transform;
			if (child.tag == "SpawnPoints") 
			{
				for (int j = 0; j < child.childCount; j++)
					spawnPoints.Add (child.GetChild (j));
			}
		}
	}

	void roundBehavior()
	{
		Debug.Log ("Fight Round: " + (Time.time - roundStartTime));
		if ((Time.time - lastSpawnTime) >= spawnFrequency && spawnEnabled) 
		{
			for(int i = 0; i < roundNum; i++)
				SpawnEnemy ();
			
			lastSpawnTime = Time.time;
		}

		if ((Time.time - roundStartTime) >= fightRoundTime) 
			spawnEnabled = false;

		if (!spawnEnabled && numAlive == 0) {
			roundStartTime = Time.time;
			buildEnabled = true;
		}
			
	}

	void SpawnEnemy ()
	{
		Transform spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Count)];
		Transform newEnemy = Instantiate (enemy);
		newEnemy.position = spawnPoint.position;
		numAlive += 1;
	}

	void buildRoundBehavior()
	{
		Debug.Log ("Build Round: " + (Time.time - roundStartTime));
		GameObject.Find ("Player").GetComponent<PlayerHealth> ().currentHealth = 100;

		// Set build menu to be active

		if (Time.time - roundStartTime > buildRoundTime) 
		{
			roundNum++;
			roundStartTime = Time.time;
			spawnEnabled = true;
			buildEnabled = false;
		}
	}
}
