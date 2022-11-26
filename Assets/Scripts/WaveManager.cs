using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum SpawnState
	{
        SPAWNING,
        WAITING,
        COUNTING,
        COMPLETE
	}

    [System.Serializable]
    public class Wave
	{
        public string name;
        public Transform[] enemy;
        public GameObject spawnPoints;
        public int count;
        public float spawnRate;
        public Transform globalSP;


    }

    public Wave[] waves;


    public float timeBetweenWaves = 1f;
    public float waveCountDown;

    private int nextWave = 0;
    private float searchCountDown = 1f;
    private SpawnState state = SpawnState.COUNTING;

	private void Start()
	{
        waveCountDown = timeBetweenWaves;
	}

	private void Update()
	{
        if(state == SpawnState.WAITING)
		{
			if (!EnemiesAlive()) 
            {
                //Begin new round
                WaveCompleted();
                
            } else
			{
                return;
			}
		}

		if(waveCountDown <= 0)
		{
            if(state != SpawnState.SPAWNING)
			{
                // Start Spawning Wave
                StartCoroutine(SpawnWave(waves[nextWave]));
			}
		}
		else
		{
            waveCountDown -= Time.deltaTime;
		}

        if(state == SpawnState.COMPLETE)
		{

		}
	}

    void WaveCompleted()
	{
        Debug.Log("Wave Complete");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if(nextWave+1 > waves.Length - 1) { state = SpawnState.COMPLETE; return; }

        nextWave++;
	}

    bool EnemiesAlive()
	{
        searchCountDown -= Time.deltaTime;

        if(searchCountDown > 0f) { return true; }
        searchCountDown = 1f;
		if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0) { return true; }
        return false;
	}

    IEnumerator SpawnWave(Wave _wave)
	{
        Debug.Log("Spawning Wave : " + _wave.name);
        state = SpawnState.SPAWNING;

        //Spawn Shit
        for (int i = 0; i < _wave.count + 1; i++)
		{
            Transform[] points = _wave.spawnPoints.GetComponentsInChildren<Transform>();
            Debug.Log(points[i].name);
            if(points[i].CompareTag("Enemy Spawn"))
			{
                int k = 1;

                foreach (Transform en in _wave.enemy)
				{
                    spawnEnemy(en.transform, k, points, _wave.globalSP);
                    k++;
                }
                yield return new WaitForSeconds(1f);
            }
            
        }


        state = SpawnState.WAITING;

        yield break;
	}

    void spawnEnemy(Transform _enemy, int spawnNo, Transform[] _spawnPoints, Transform sp)
	{
        var newEnemy = Instantiate(_enemy, sp.position, transform.rotation);
        iTween.MoveTo(newEnemy.gameObject, _spawnPoints[spawnNo].position, 3f);
        newEnemy.transform.SetParent(gameObject.transform);
	}
}
