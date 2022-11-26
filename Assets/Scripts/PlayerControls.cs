using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [Header("Getting Player Parts")]
    public List<Rigidbody2D> playerParts = new List<Rigidbody2D>();
    private Rigidbody2D[] list;

    public Weapon weaponCode;
    public Rigidbody2D currentPlayer;
    public TrailRenderer dashTrail;

    [Header("Player Movement")]
    public float normalSpeed;
    public float maxSpeed;
    public float moveSpeed;
    public float angleRotation;
    public float minDist;
    public float maxDist;
    public float dashLength;
    public float dashSpeed;
    public Color dashColor;
    public Color normalColor;

    private float dashCounter;
    private float dashCoolCounter;
    public float dashCooldown;
    public KeyCode dash;
    public KeyCode shield;
    public GameObject shieldObj;
    private bool isDashing;
    private float activeSpeed;
    public AudioSource sfxDash;


    [Header("weapon")]
    
    public KeyCode attack;
    public GameObject weapon;



    // Start is called before the first frame update
    void Start()
    {
        normalColor = currentPlayer.GetComponent<SpriteRenderer>().color;

		list = gameObject.GetComponentsInChildren<Rigidbody2D>();
        foreach(Rigidbody2D player in list)
		{
            playerParts.Add(player);
		}


        currentPlayer = playerParts[0].GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        PlayerDash();
        PlayerMovement();
        DashingTrail();
        SwingCheck();
        StartCoroutine(UseShield());
    }

    private void FixedUpdate()
    {

    }
    
    IEnumerator UseShield()
	{
        if(Input.GetKeyDown(shield) == true)
		{
            iTween.ScaleTo(shieldObj, new Vector3(8, 8, 1), 1f);
            yield return new WaitForSeconds(2f);
            iTween.ScaleTo(shieldObj, new Vector3(0.1f, 0.1f, 1), 1f);
        }
        


    }

    private void PlayerMovement()
	{
        if(currentPlayer == null) { return; }
        if (isDashing == false) { activeSpeed = normalSpeed; }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float hori = (h * activeSpeed * Time.deltaTime) + currentPlayer.transform.position.x;
        float vert = (v * activeSpeed * Time.deltaTime) + currentPlayer.transform.position.y;
        currentPlayer.transform.position = new Vector2(hori, vert);
    }

    private void SwingCheck()
	{
		if (Input.GetKeyDown(attack))
		{
            StartCoroutine(weapon.GetComponent<Melee>().SwingBat());
		}
	}

    private void MinimuimDistance(Rigidbody2D rotatioObj, Rigidbody2D playerRb, float minDist, float maxDist)
	{
        if (Vector3.Distance(rotatioObj.transform.position, playerRb.transform.position) < minDist)
		{
            rotatioObj.transform.position = (rotatioObj.transform.position - playerRb.transform.position).normalized * minDist + playerRb.transform.position;
        } else if (Vector3.Distance(rotatioObj.transform.position, playerRb.transform.position) > maxDist)
        {
            rotatioObj.transform.position = (rotatioObj.transform.position - playerRb.transform.position).normalized * maxDist + playerRb.transform.position;
        }
    }

    private void PlayerDash()
	{
		if (Input.GetKeyDown(dash) && dashCoolCounter <= 0 && dashCounter <= 0)
		{
            activeSpeed = dashSpeed;
            dashCounter = dashLength;

            isDashing = true;
            sfxDash.Play();
            currentPlayer.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(DisableNoclip());
        }

        if(dashCounter > 0)
		{
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)
			{
                Debug.Log("error");
                activeSpeed = normalSpeed;
                dashCoolCounter = dashCooldown;
                isDashing = false;
			}
		}

        if(dashCoolCounter > 0)
		{
            dashCoolCounter -= Time.deltaTime;
		}
	}

    IEnumerator DisableNoclip()
	{
        yield return new WaitForSeconds(dashLength + 0.3f);
        currentPlayer.GetComponent<BoxCollider2D>().enabled = true;

    }

    private void DashingTrail()
	{
        if(isDashing == true)
		{
            dashTrail.enabled = true;
            currentPlayer.GetComponent<SpriteRenderer>().color = dashColor;

        } else
		{
            currentPlayer.GetComponent<SpriteRenderer>().color = normalColor;
            dashTrail.enabled = false;
		}
	}

    public void HitDetected(Collider2D triggeredObj, float force)
	{
        currentPlayer.AddForce((currentPlayer.transform.position - triggeredObj.transform.position).normalized * force, ForceMode2D.Impulse);
        Debug.Log("tesz");
	}


}
