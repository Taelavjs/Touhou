using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    [System.Serializable]
    public class PatternManager {
        public string selectedPattern;

        [Header("Shotgun Settings")]
        public int minShells;
        public int maxShells;
        public float timeToAppear;
        public float bulletOffsets;
        public float shotgunForce;
        public float timeBtwShots;
        public float projectileSpeed;


    }



    [Header("Game Objects")]
    public Transform bulletContainer;
    public Transform enemyContainer;
    public GameObject projectilePrefab;
    public Transform shotgunBarrel;
    public Transform player;
    public PatternManager[] patterns;

    [Header("Bullet Hell Settings")]
    public int numberOfProjectiles;

    private const float radius = 1f;
    private List<Vector3> graph1Spawns;
    public float graphDelay;
    public float graph1Force;
    public float timeBetweenWaves;



    // Start is called before the first frame update
    void Awake()
    {

        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bulletContainer = GameObject.FindGameObjectWithTag("Bullet Container").GetComponent<Transform>();
        enemyContainer = GameObject.FindGameObjectWithTag("Enemy Container").GetComponent<Transform>();
        gameObject.transform.SetParent(enemyContainer);
        //InvokeRepeating("FullCirclePattern", 1f, 5f);
        graph1Spawns = GenerateGraph1SpawnPoints();
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //StartCoroutine("ShotgunShot");
        }
    }

    /*private void FullCirclePattern(float speed)
    {
        Vector3 startPoint = transform.position;

        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i <= numberOfProjectiles; i++)
        {
            // Calculating angles to instantiate bullets at
            float projectileDirXPosition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector3 projectileVector = new Vector3(projectileDirXPosition, projectileDirYPosition, 0);
            Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * projectileSpeed;
            // Correcting rotation of bullets to face their momentum
            var relativePos = -projectileVector + transform.position;
            var angle1 = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.AngleAxis(angle1, Vector3.forward);

            GameObject tmpObj = Instantiate(projectilePrefab, startPoint, rotation);
            tmpObj.transform.SetParent(bulletContainer); //Adding bullet to container
            
            // Adding speed
            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector3(projectileMoveDirection.x, projectileMoveDirection.y, 0);

            angle += angleStep;
        }
    }
    */
    IEnumerator ShotgunShot(PatternManager currentPatt)
	{
        // Creating a random ammount of shells
        for(int j = 0; j < 7; j++)
        {
            List<GameObject> bullets = new List<GameObject>();
            for (int i = 0; i < Random.Range(currentPatt.minShells, currentPatt.maxShells); i++)
            {
                Vector3 randSpawnRange = new Vector3(shotgunBarrel.position.x + Random.Range(-2f, 2f), shotgunBarrel.position.y + Random.Range(-2f, 2f), 0);
                GameObject tmpObj = Instantiate(projectilePrefab, randSpawnRange, Quaternion.identity);
                iTween.FadeFrom(tmpObj, 0f, currentPatt.timeToAppear);
                bullets.Add(tmpObj);
            }
            yield return new WaitForSeconds(currentPatt.timeToAppear);
            foreach (GameObject obj in bullets)
            {
                Vector3 dir = (player.transform.position - shotgunBarrel.transform.position).normalized;
                dir = new Vector3(dir.x + Random.Range(-currentPatt.bulletOffsets, currentPatt.bulletOffsets), dir.y + Random.Range(-currentPatt.bulletOffsets, currentPatt.bulletOffsets), 0);
                //iTween.MoveTo(obj, player.transform.position, 2f);
                //iTween.LookTo(obj, player.position, 0f);
                obj.GetComponent<Rigidbody2D>().AddForce(dir * currentPatt.shotgunForce, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(currentPatt.timeBtwShots);
        }

    }

    private List<Vector3> GenerateGraph1SpawnPoints()
	{
        int t = 0;
        List<Vector3> spawnPoints = new List<Vector3>();
        float y;
        float x;

        for(t = 1; t < 360; t++)
		{
            y = (1 * Mathf.Sin(7 * t));
            x = (1 * Mathf.Sin(20 * t));

            spawnPoints.Add(new Vector3(x, y, 0));
            
        }
        return spawnPoints;
	}

    IEnumerator SpawnBulletsDelayed(float delayTime, List<Vector3> spawnPoints, PatternManager patt)
    {
        for (int k = 0; k < 3; k++) { 
            foreach (Vector3 posOffset in spawnPoints)
            {
                GameObject tmpObj = Instantiate(projectilePrefab, new Vector3(posOffset.x + gameObject.transform.position.x, posOffset.y + gameObject.transform.position.y, 0), Quaternion.identity);
                tmpObj.transform.parent = bulletContainer.transform;

                Vector3 dir = (tmpObj.transform.position - gameObject.transform.position).normalized;

                var relativePos = -dir + transform.position;
                var angle1 = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                var rotation = Quaternion.AngleAxis(angle1, Vector3.forward);

                tmpObj.GetComponent<DeleteBullet>().dir = dir;


                StartCoroutine(tmpObj.GetComponent<DeleteBullet>().DelaySprint(tmpObj, patt.projectileSpeed));
                //iTween.RotateTo(tmpObj, new Vector3(tmpObj.transform.eulerAngles.x, tmpObj.transform.eulerAngles.y, tmpObj.transform.eulerAngles.z + 90f), 1f);


                //tmpObj.GetComponent<Rigidbody2D>().AddForce(dir * graph1Force, ForceMode2D.Impulse);

                yield return new WaitForSeconds(delayTime);
            }
            yield return new WaitForSeconds(3f);
        }

	}

    void Firing(PatternManager wave)
	{
		switch (wave.selectedPattern)
		{
            case "Shotgun":
                StartCoroutine(ShotgunShot(wave));
                return;
            case "graph":
                StartCoroutine(SpawnBulletsDelayed(graphDelay, graph1Spawns, wave));
                return;
		}
	}

    IEnumerator SpawnWaves()
	{
        yield return new WaitForSeconds(3f);
		while (true)
		{
            foreach(PatternManager pat in patterns)
			{
                Firing(pat);
                yield return new WaitForSeconds(timeBetweenWaves);
			}
		}
	}

}
