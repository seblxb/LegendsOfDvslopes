using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField] GameObject player;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] powerUpSpawns;

    [SerializeField] GameObject tanker;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject ranger;
    [SerializeField] GameObject arrow;

    [SerializeField] GameObject healthPowerUp;
    [SerializeField] GameObject speedPowerUp;
    [SerializeField] int maxPowerUps = 4;

    [SerializeField] Text levelText;
    [SerializeField] Text endGameText;  

    private int currentLevel;
    [SerializeField] int finalLevel = 10;
    private float generatedSpawnTime = 1f;
    private float currentSpawnTime = 0f;
    private float powerUpSpawnTime = 60f;
    private float currentPowerUpSpawnTime = 0f;

    private int powerUps = 0;

    private GameObject newEnemy;
    private GameObject newPowerUp;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killedEnemies = new List<EnemyHealth>();

    public void RegisterEnemy (EnemyHealth enemy) {
        enemies.Add(enemy);
    }

    public void KillEnemy (EnemyHealth enemy) {
        killedEnemies.Add(enemy);
    }

    public void RegisterPowerUp() {
        powerUps++;
    }

    private bool gameOver = false;
    
    public bool GameOver 
    {
        get {return gameOver;}
    }

	public GameObject Player
	{
		get { return player; }
	}

    public GameObject Arrow
    {
        get { return arrow; }
    }

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject); 
    }

    // Use this for initialization
    void Start () {

        endGameText.GetComponent<Text>().enabled = false;
        currentLevel = 1;
        StartCoroutine(Spawn());
        StartCoroutine(PowerUpSpawn());
	}
	
	// Update is called once per frame
	void Update () {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
	}

    public void PlayerHit (int currentHP) {
        if (currentHP > 0) {
            gameOver = false;
        } else {
            gameOver = true;
            StartCoroutine(endGame("Defeat"));
        }
    }


    IEnumerator Spawn() {
        // check spawn time > current time
        // if less enemies than current level
        // randomly sollect spawn point and spawn enemy
        // if we have killed the same number of enemies of the current level, 
        // clear out the enemies  and killed enemies arrays, increment current 
        // level by one, start again

        if (currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0f;
            if (enemies.Count < currentLevel) {
                int randomNum = Random.Range(0, spawnPoints.Length - 1);
                int spawnPoint = randomNum;
                GameObject spawnLocation = spawnPoints[spawnPoint];
                int randomEnemy = Random.Range(0, 3);
                //print(randomEnemy); 
                if (randomEnemy == 0) {
                    newEnemy = Instantiate(soldier);
                } else if (randomEnemy == 1) {
                    newEnemy = Instantiate(ranger);
                } else  if  (randomEnemy == 2) {
                    newEnemy = Instantiate(tanker);
                }

                newEnemy.transform.position = spawnLocation.transform.position;
            }
        }

        if (killedEnemies.Count == currentLevel && currentLevel != finalLevel) 
        {
            enemies.Clear();
            killedEnemies.Clear();
            yield return new WaitForSeconds(3f);
            currentLevel++;
            levelText.text = "level " + currentLevel;
               
        }

        if (killedEnemies.Count == finalLevel) {
            StartCoroutine(endGame("Victory!"));
        }
        yield return null;
        StartCoroutine(Spawn());
    }

    IEnumerator endGame(string outcome) {
        endGameText.text = outcome;
        endGameText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Game Menu"); 
    }

    IEnumerator PowerUpSpawn() {
		if (currentPowerUpSpawnTime > powerUpSpawnTime) {
            print("powerup generating"); 
            currentPowerUpSpawnTime = 0;

            if (powerUps < maxPowerUps)
            {
				int randomNum = Random.Range(0, powerUpSpawns.Length - 1);
				int spawnPoint = randomNum;
				GameObject spawnLocation = powerUpSpawns[spawnPoint];

				int randomPowerUp = Random.Range(0, 2);
				//print(randomEnemy); 
				if (randomPowerUp == 0)
				{
                    newPowerUp = Instantiate(healthPowerUp) as GameObject;
				}
				else if (randomPowerUp == 1)
				{
                    newPowerUp = Instantiate(speedPowerUp) as GameObject;

				}

				newPowerUp.transform.position = spawnLocation.transform.position;
            }


        }

		yield return null;
		StartCoroutine(PowerUpSpawn());
    }
}
