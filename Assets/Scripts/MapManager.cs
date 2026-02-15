using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using System;

//This script manages everything related to the map
public class MapManager : MonoBehaviour
{
    [Header("Map")]
    public float MapLimitX = 10.0f;
    public float MapLimitZ = 6.0f;
    public List<Transform> walls;
    private HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

    [Header("Prefabs")]
    public GameObject foodPrefab;

    //=======================================================================
    //SpawnFood
    //=======================================================================
    public void Initialize()
    {
        //fill map position hash table
        foreach (Transform wall in walls) 
        {
            wallPositions.Add(
                new Vector2Int(
                    Mathf.RoundToInt(wall.position.z),
                    Mathf.RoundToInt(wall.position.z)
                )
            );
        }
    }

    //=======================================================================
    //SpawnFood
    //=======================================================================
    public void SpawnFood()
    {
        Vector3 spawnCoords = FoodSpawnPos();

        //Spawn food item
        Instantiate(foodPrefab, spawnCoords, Quaternion.identity);
    }

    //This method makes sure food is not spawned within another object
    private Vector3 FoodSpawnPos()
    {
        //Calculate spawn coordinates
        float coorX = 0.0f;
        float coorZ = 0.0f;

        while(true)
        {
            //try a random value
            coorX = (int)UnityEngine.Random.Range(-MapLimitX, MapLimitX );
            coorZ = (int)UnityEngine.Random.Range(-MapLimitZ, MapLimitZ );

            Vector2Int candidatePos = new Vector2Int((int)coorX, (int)coorZ);

            //If given positions are not contained in the map position hash table, return
            //otherwise try again.
            if (!wallPositions.Contains(candidatePos))
                return new Vector3(coorX, 0.0f, coorZ);
        }
    }

}
