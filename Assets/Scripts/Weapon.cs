using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int powerVal;
	public float launchVal;
	public AudioSource sfxHit;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
            collision.gameObject.GetComponent<Enemy>().TakeDamage(powerVal);
			iTween.ShakePosition(collision.gameObject, new Vector3(0.3f, 0.3f, 0), 0.5f);
			if (!sfxHit.isPlaying)
			{
				sfxHit.Play();
			}
		}
    }

	public void launchWeapon(Vector2 dir)
	{
		
		Debug.DrawLine(gameObject.transform.position, (Vector3)  dir * 100, Color.white, 10f, false);
		dir = dir.normalized;
		Debug.Log(dir);

		transform.Translate(dir * launchVal * Time.deltaTime);

	}

}
