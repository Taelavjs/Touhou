using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBullet : MonoBehaviour
{
    private float timeToDetonate = 10f;
    public int damage;
    public float speed;
    public float rotationSpeed;
    public Vector3 dir;
    public bool canStart = false;
     //LineRenderer laser;
    // Start is called before the first frame update
    void Start()
    {
        /*
        laser = GetComponent<LineRenderer>();
        laser.startWidth = .1f;
        laser.endWidth = .1f;
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, dir * 30);
        */
        }

    // Update is called once per frame
    void Update()
    {
        if(canStart == true)
		{
            dir = new Vector3(dir.x, dir.y, dir.z);
            transform.Translate(dir * speed * Time.deltaTime);
		}

        timeToDetonate -= Time.deltaTime;
        if(timeToDetonate < 0)
		{
            Destroy(gameObject);
		}
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
            collision.gameObject.GetComponent<PlayerHealth>().ExternalHealthUpdate(damage);
            Destroy(gameObject);
		} 
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator DelaySprint(GameObject tmpObj, float speedy)
    {

        speed = speedy;
        yield return new WaitForSeconds(0.5f);
        canStart = true;
        //laser.enabled = false;
    }
}
