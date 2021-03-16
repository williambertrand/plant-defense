using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PlantPrefab
{
    public string id;
    public Plant prefab;
}

public class PlayerPlanting : MonoBehaviour
{

    public static PlayerPlanting Instance;

    [SerializeField]
    public List<PlantPrefab> playerPlantingPrefabs;
    TopDownCharacterController playerController;

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
        playerController = GetComponent<TopDownCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlantSeedling(string plantId)
    {
        PlantPrefab toPlant = playerPlantingPrefabs.Find(p => p.id == plantId);
        Point? location = playerController.focusPoint;
        if (location == null) return;

        if (PlantManager.Instance.PlantExistis((Point)location)) return;

        if (Player.Instance.seedCount < toPlant.prefab.seedCost)
        {
            PlantMenuSystem.Instance.Notify("You don't have enough seeds for that plant!", 1.5f);
            return; 
        }

        Vector3 delta = new Vector3(0.0f, 0.15f, 0.0f);
        Plant t = Instantiate<Plant>(toPlant.prefab, GameGrid.Instance.PosToWorldLocation((Point)location) + delta, Quaternion.identity);
        t.gridPositon = (Point)location;
        Player.Instance.OnSeedSpend(toPlant.prefab.seedCost);
        PlantManager.Instance.OnPlantPlanted(t, (Point)location);
    }
}
