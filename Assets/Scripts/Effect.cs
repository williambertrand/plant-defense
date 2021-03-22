using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{

    public float duration;
    float spawnAt;

    public void OnSpawn() {
        spawnAt = Time.time;
    }    

    // Update is called once per frame
    void Update()
    {

        if(Time.time - spawnAt >= duration)
        {
            gameObject.SetActive(false);
        }
        
    }
}
