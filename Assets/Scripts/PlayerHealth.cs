using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHp;
    public HealthBar healthBar;
    public AudioSource sfxDeath;
    public float invunerableTime;    // Start is called before the first frame update
    void Start()
    {
        health = maxHp;
        healthBar.SetMaxHealth(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator HealthUpdate(int dmgTaken)
	{
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        health = Mathf.Min(maxHp, health - dmgTaken);
        healthBar.SetHealth(health);
        if (health <= 0)
		{
            if (!sfxDeath.isPlaying) { sfxDeath.Play(); }
            iTween.ShakePosition(gameObject, new Vector3(0.2f, 0.2f, 0), 1f);
            iTween.FadeTo(gameObject, 0f, 2f);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
        yield return new WaitForSeconds(invunerableTime);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        
	}

	private void OnDestroy()
	{
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void ExternalHealthUpdate(int dmgTaken)
	{
        StartCoroutine(HealthUpdate(dmgTaken));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Points"))
        {
            Debug.Log("Destroyed");
            Destroy(collision.gameObject);
        }
    }
}
