using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] public int enemiesToSpawn;
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnRate;
}
