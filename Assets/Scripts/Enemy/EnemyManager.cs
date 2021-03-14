using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string title;
        public Enemy enemyPrefab;
        public float spawnProbability;
    }


    public static EnemyManager Instance;
    public List<Enemy> activeEnemies;


    /* Fields for tuning how the difficulty changes overe time */
    public float levelChangeTime;
    public int currentLevel;
    public float levelSpeedFactor;
    public int maxEnemies = 5;
    public int enemyLevelDelta = 5;

    [SerializeField]
    public List<Transform> spawnLocations;

    //TODO: This will be a list of prefabs
    public List<EnemyType> enemyTypes;
    public List<float> enemySpawnPercent;

    public float lastSpawn;
    public float spawnTime;
    public int spawnCount = 1;
    public float levelSpawnProbDelta;

    public bool increaseLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = new List<Enemy>();
        currentLevel = 0;
        increaseLevel = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpawn > spawnTime && activeEnemies.Count <= maxEnemies)
        {
            // To make the game harder as we get furhter in,
            // increase number of enemies that get spawned at each tick
            for(int  i = 0; i < spawnCount; i++)
            {

            }
            SpawnEnemy();
            lastSpawn = Time.time;
        }
    }

    void SpawnEnemy()
    {

        Enemy enemyPrefab = null;
        // Grab the random enemy
        if (currentLevel < 3) enemyPrefab = enemyTypes[0].enemyPrefab;
        else
        {
            float r = Random.Range(0.0f, 1.0f);
            if (currentLevel > 3)
            {
                // Adding a value here makes it more likely that harder enemies will spawn
                r += currentLevel * levelSpawnProbDelta;
            }
            float t = enemyTypes[0].spawnProbability;
            for (int i = 0; i < enemyTypes.Count; i++)
            {
                if(r < t)
                {
                    enemyPrefab = enemyTypes[i].enemyPrefab;
                    break;
                } else
                {
                    t += enemyTypes[i].spawnProbability;
                }
            }
        }

        Vector3 pos = spawnLocations[Random.Range(0, spawnLocations.Count)].position;
        Enemy newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        newEnemy.maxHealth = newEnemy.maxHealthBase + (newEnemy.levelHealth * currentLevel);
        newEnemy.attackTime = newEnemy.attackTime - (newEnemy.levelAttackFactor * currentLevel);
        newEnemy.damage = newEnemy.damageBase + (newEnemy.levelDamage * currentLevel);
        activeEnemies.Add(newEnemy);
        newEnemy.transform.SetParent(this.transform);
    }

    public void OnEnemyDeath(Enemy e)
    {
        activeEnemies.Remove(e);
        GameStats.AddBug();
    }

    public void OnNewPlant(Plant newPlant)
    {
        foreach(Enemy e in activeEnemies)
        {
            if (e.target == null
                ||
                Vector2.Distance(e.transform.position, newPlant.transform.position)
                < Vector2.Distance(e.transform.position, e.target.transform.position))
            {
                e.UpdateTargetTo(newPlant);
            }
        }
    }


    public void IncreaseLevel()
    {
        currentLevel += 1;
        spawnTime *= levelSpeedFactor;
        maxEnemies += enemyLevelDelta; 
    }
}
