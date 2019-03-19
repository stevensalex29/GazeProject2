using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour
{
    // Awake is called first
    private void Awake()
    {
        // Destroy menu music if this is first level
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");
        if (objs != null) Destroy(objs[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
