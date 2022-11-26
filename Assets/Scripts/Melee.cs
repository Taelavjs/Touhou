using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public PlayerControls player;
    public Transform playerTrans;
    public float knockbackForce;
    public float hitDelay;
    private float countdown;
    private Animator animator;
    public int dmg;

    // Start is called before the first frame update
    void Start()
    {
        countdown = hitDelay;
        animator = GetComponent<Animator>();
        animator.SetBool("canSwing", false);

    }

    // Update is called once per frame
    void Update()
    {


    }

    public IEnumerator SwingBat()
	{
        animator.SetBool("canSwing", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("canSwing", false);

    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if(!collision.CompareTag("Enemy")) { return; }
        countdown = hitDelay;
        player.HitDetected(collision, knockbackForce);
        collision.GetComponent<Enemy>().TakeDamage(dmg);
	}




}
