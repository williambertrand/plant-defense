using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBSHealthChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RequestHandler.SendHealthCheck());
    }

}
