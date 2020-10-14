using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float timeToSpawn;
    public float spawnDelay = 2f;
    public GameObject cubePrefab;

    List<GameObject> allCubes = new List<GameObject>();
    int maxCubesToSpawn = 20;


    /// <summary>
    /// Instantiates ALL the cubes that are going to form the cube/prefab pool that is going to be used throughout the game.
    /// </summary>
    void InitializeCubePool()
    {
        for (int i = 0; i < maxCubesToSpawn; i++)
        {
            GameObject newCube = Instantiate(cubePrefab, this.transform);
            newCube.GetComponent<CubeSpawn>().InitializeCubeParts();
            newCube.SetActive(false);
            allCubes.Add(newCube);
        }
    }


    private void Start()
    {
        InitializeCubePool();
    }
    void Update()
    {
        if (CubeGameManager.Instance.gameHasStarted)
        {
            SpawnCubes();
        }
    }
    /// <summary>
    /// Spawns the cubes.
    /// </summary>
    void SpawnCubes()
    {
        if (Time.time > timeToSpawn && CubeGameManager.Instance.gameHasStarted)
        {
            if (GetCubeFromPool())
            {
                timeToSpawn = Time.time + spawnDelay;
                GameObject randomCube = GetCubeFromPool();
                randomCube.SetActive(true);
                randomCube.transform.position = this.transform.position;
                randomCube.GetComponent<CubeSpawn>().Randomize();
            }

        }

    }


    /// <summary>
    /// Returns the first inactive cube prefab that finds inside the pool.
    /// </summary>
    /// <returns>The cube from pool.</returns>
    GameObject GetCubeFromPool()
    {
        for (int i = 0; i < allCubes.Count; i++)
        {
            if (!allCubes[i].gameObject.activeInHierarchy)
            {
                return allCubes[i].gameObject;
            }

        }

        return null;
    }
}
