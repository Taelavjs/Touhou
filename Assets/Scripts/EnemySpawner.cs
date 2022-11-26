using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float speedForce;
    Transform[] spawnPoints;
	// Start is called before the first frame update

	private void Awake()
	{

        spawnPoints = gameObject.GetComponentsInChildren<Transform>();
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
