using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpawnManager : MonoBehaviour
{

    #region Singleton
    public static SeedSpawnManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public GameObject seedPrefab;

    // Max seeds to be on the ground at any time (TODO)
    public int maxSpawn;


    // Handle timing of seed spawns
    public float nextSpawnTime;
    public float minSpawnDelay;
    public float maxSpawnDelay;


    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + minSpawnDelay;   
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawnTime)
        {

            SpawnSeed();
            nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void SpawnSeed()
    {
        Point p = new Point(
            Random.Range(-GameGrid.Instance.boardWidth, GameGrid.Instance.boardWidth),
            Random.Range(-GameGrid.Instance.boardWidth, GameGrid.Instance.boardWidth)
        );

        GameObject seed = Instantiate(seedPrefab, GameGrid.Instance.PosToWorldLocation(p), Quaternion.identity, transform);
        seed.transform.SetParent(transform);
    }
}
