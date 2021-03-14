using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPlant : Plant
{

    public float pointRate;

    // Start should be called from parent

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (currentState == PlantState.GROWING) return;

        pointRate = basePointRate * (pointMultiplier * age);
        float points = pointRate * Time.deltaTime;
        currentPointVal += points;
        newPoints += points;
    }
}
