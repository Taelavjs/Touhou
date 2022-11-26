using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{
    private Transform playerPos;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float x = Random.Range(-1f, 1f); 
        float y = Random.Range(0, 1f);
        Vector3 dir = new Vector3(transform.position.x + x, transform.position.y + y, 0);
        iTween.MoveTo(gameObject, dir, 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(moveToPlayer());
    }

    private IEnumerator moveToPlayer()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().enabled = true;
        Vector3 dir = playerPos.position - gameObject.transform.position;
        dir *= Time.deltaTime * speed;
        transform.Translate(dir.x, dir.y, 0);
    }

}
