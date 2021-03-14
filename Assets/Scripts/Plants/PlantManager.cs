using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * Manges active plants and keeps track of scores
 */
public class PlantManager : MonoBehaviour
{

    public static PlantManager Instance;
    public ArrayList activePlants;
    int score;
    public Text scoreText;

    // Maintain a map of Grid Position => Plant
    public Dictionary<Point, Plant> plants;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        plants = new Dictionary<Point, Plant>();
        activePlants = new ArrayList();

        // Plant starter daffodil
        PlantStarterPlant("daffodil", new Point(0, 0));
    }

    public void OnPlantDidActivate(Plant p, Point gridPos)
    {
        activePlants.Add(p);
        EnemyManager.Instance.OnNewPlant(p);
    }

    public void OnPlantPlanted(Plant p, Point loc)
    {
        plants.Add(loc, p);
        GameStats.AddPlant();
    }

    public bool PlantExistis(Point loc)
    {
        return plants.ContainsKey(loc) && plants[loc] != null;
    }

    public void OnPlantDeactivate(Plant p)
    {
        plants.Remove(p.gridPositon);
        activePlants.Remove(p);

        GameStats.AddPlantLost();

        if(activePlants.Count == 0)
        {
            GameManager.Instance.OnGameOver();
        }

    }

    public Plant GetRandomPlant()
    {
        if (activePlants.Count == 0) return null;
        return (Plant)activePlants[Random.Range(0, activePlants.Count)];
    }

    public void IncreaseDay()
    {
        foreach(Plant p in activePlants)
        {
            p.age += 1;
        }
    }


    private void PlantStarterPlant(string plantId, Point startPos)
    {
        PlantPrefab toPlant = PlayerPlanting.Instance.playerPlantingPrefabs.Find(p => p.id == plantId);
        Plant t = Instantiate<Plant>(toPlant.prefab, GameGrid.Instance.PosToWorldLocation(startPos) + new Vector3(0.0f, 0.15f, 0.0f), Quaternion.identity);
        t.gridPositon = startPos;
        OnPlantPlanted(t, startPos);
    }
}
