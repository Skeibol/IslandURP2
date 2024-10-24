using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public float radius;
    public float spawnFrequency;
    public GameObject enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startSpawning());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 PickRandomBorderPoint()
    {
        var randomCirclePoint = Random.Range(0, Mathf.PI * 2f);
        var circleX = Mathf.Sin(randomCirclePoint);
        var circleY = Mathf.Cos(randomCirclePoint);

        return new Vector3(circleX * radius, circleY * radius, 0);


    }

    IEnumerator startSpawning()
    {
        var spawn = PickRandomBorderPoint();
        Instantiate(enemy, transform.TransformPoint(spawn), quaternion.identity, transform.parent);
        yield return new WaitForSeconds(spawnFrequency);

        StartCoroutine(startSpawning());
    }
}
