using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public float duration;
    public float createdAt;

    private void Start()
    {
        createdAt = Time.time;
    }

    private void Update()
    {
        if(Time.time - createdAt > duration)
        {
            Destroy(gameObject);
        }
    }
}
