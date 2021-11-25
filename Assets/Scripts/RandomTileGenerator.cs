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
    private float tileLength = 50;
    private int car_per_tile = 2;  // сколько спавнить машин на одном тайле

    [SerializeField] private Transform player;
    private int startTiles = 6;

    public GameObject player_object;
    private PlayerController player_controller;

    void Start()
    {
        player_controller = player_object.GetComponent<PlayerController>();

        for (int i = 0; i < startTiles; i++)
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
        }
    }

    void Update()
    {
        if (player.position.z - 0.6 * tileLength > spawnPos - (startTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], transform.forward * spawnPos, transform.rotation);
        activeTiles.Add(nextTile);

        // спавним, например, три машины:
        // чтобы машины не пересекались спавним машины по длине тайла так: первая посередине (spawnPos), вторая сдвинута вперед на треть длины тайла (spawnPos + tileLength/3),
        // третья сдвинута назад на треть длины тайла (spawnPos - tileLength/3)
        // еще пример для четырех: spawnPos - 2 * tileLength / 4, spawnPos - 1 * tileLength / 4, spawnPos - 0 * tileLength / 4, spawnPos + 1 * tileLength / 4
        for (int i = 0; i < car_per_tile; i++)
        {
            int car_shift = Random.Range(-1, 2);  // random shift -1(left), 0(center), +1(right)
            int forward_shift = i - (int)(car_per_tile / 2);  // деление начело (без остатка): (int)(3 / 2) = 1, (int)(4 / 2) = 2
            GameObject nextCar = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], transform.forward * (spawnPos + forward_shift * tileLength / car_per_tile) + car_shift * transform.right * player_controller.lineDistance, transform.rotation);
            activeCars.Add(nextCar);
        }

        spawnPos += tileLength;
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);

        Destroy(activeCars[0]);
        activeCars.RemoveAt(0);
    }
}
