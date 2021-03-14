using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlant : Plant
{
    public float seedTime;
    public float lastSeed;
    public float seedCount;

    public GameObject showPlusSeedPrefab;
    private GameObject showPlusSeedInstance;
    private Animator seedAnimator;


    protected override void Start()
    {
        base.Start();
        showPlusSeedInstance = Instantiate(showPlusSeedPrefab, this.transform);
        seedAnimator = showPlusSeedInstance.GetComponent<Animator>();
        showPlusSeedInstance.transform.position = this.transform.position + new Vector3(0, 0.25f, 0);
        showPlusSeedInstance.SetActive(false);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (currentState == PlantState.GROWING) return;
        if (Time.time - lastSeed >= seedTime)
        {
            GenerateSeed();
        }
    }

    public void GenerateSeed()
    {
        showPlusSeedInstance.SetActive(true);
        seedAnimator.Play("SeedShow", -1, 0.0f);
        Player.Instance.OnSeedCollect(false);
        lastSeed = Time.time;
    }
}
