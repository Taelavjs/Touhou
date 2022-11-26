using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp;
    public bool canTakeDamage;
    public float safetyTime;
    public HealthBar healthBar;
    public AudioSource sfxDeath;
    public AudioSource sfxHit;
    public GameObject scorePrefab;
    private GameObject powerUps;

    [SerializeField] float _degreesPerSecond = 30f;
    [SerializeField] Vector3 _axis = Vector3.forward;
    // Start is called before the first frame update
    void Start()
    {
        powerUps = GameObject.FindGameObjectWithTag("PowerUps");
        canTakeDamage = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageVal) 
    {

        if(canTakeDamage == false) { return; }
        StartCoroutine(Hit());
        hp -= damageVal;
        healthBar.SetHealth(hp);
        if (hp <= 0)
		{
            iTween.FadeTo(gameObject, 0f, 0.3f);
            sfxDeath.Play();
            GameObject tmpObj = Instantiate(scorePrefab, transform);
            
            tmpObj.transform.parent = powerUps.transform;

            Destroy(gameObject, 0.4f);
		}
    }




    private IEnumerator Hit()
    {
        sfxHit.Play();
        canTakeDamage = false;
        yield return new WaitForSeconds(safetyTime);
        canTakeDamage = true;
    }


}
