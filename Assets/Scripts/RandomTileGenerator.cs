using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTileGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject[] carPrefabs;
    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> activeCars = new List<GameObject>();
    private float spawnPos = 0;
    public float tileLength = 50;
    public int car_per_tile = 3;  // how many cars to spawn on one tile
    public int startTiles = 6;

    public GameObject player_object;
    private Transform player_transform;
    private PlayerController player_controller;

    void Start()
    {
        player_transform = player_object.GetComponent<Transform>();
        player_controller = player_object.GetComponent<PlayerController>();

        SpawnEmptyTile(0);

        for (int i = 0; i < startTiles; i++)
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    void Update()
    {
        if (player_transform.position.z - 0.6 * tileLength > spawnPos - (startTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    private void SpawnEmptyTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
        activeTiles.Add(nextTile);

        spawnPos += tileLength;
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
        activeTiles.Add(nextTile);

        // let's spawn, for example, three cars:
        // so that the machines do not intersect, let's spawn the machines along the length of the tile like this: the first one is in the middle (spawnPos), the second one is shifted forward by a third of the tile length (spawnPos + tileLength/3),
        // third one is shifted back by a third of the tile length (spawnPos - tileLength / 3)
        // another example for four: spawnPos - 2 * tileLength / 4, spawnPos - 1 * tileLength / 4, spawnPos - 0 * tileLength / 4, spawnPos + 1 * tileLength / 4
        for (int i = 0; i < car_per_tile; i++)
        {
            int car_shift = Random.Range(-1, 2);  // random shift -1(left), 0(center), +1(right)
            int forward_shift = i - (int)(car_per_tile / 2);  // (int)(3 / 2) = 1, (int)(4 / 2) = 2
            GameObject nextCar = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], transform.forward * (spawnPos + forward_shift * tileLength / car_per_tile) + car_shift * transform.right * player_controller.lineDistance, transform.rotation);
            nextCar.transform.rotation = transform.rotation;
            activeCars.Add(nextCar);
        }

        spawnPos += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);

        for (int i = 0; i < car_per_tile; i++)
        {
            Destroy(activeCars[0]);
            activeCars.RemoveAt(0);
        }
    }
}
