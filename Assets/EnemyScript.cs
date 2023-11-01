using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rb;
    private float previousXVel = 0;
    private float previousYVel = 0;
    public GameObject player;
    public GameObject flashlightLink;
    public CircleCollider2D playerCollider;
    public EdgeCollider2D flashlightCollider;

    //how many walls the enemy has to hit before searching;
    int  scurryingWallsLeft = 0;
    //
    bool isScurrying = false;
    //public BoxCollider2D flashlightCollider;
    //local value of rotation in radians
    public float angle = 0;

    //distance between the player and enemy at which the enemy would charge directly at the player;
    [SerializeField]
    private float attackDistanceTrigger = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movements();
    }
    void Movements()
    {
        float xSpeed = Time.deltaTime * moveSpeed * Mathf.Cos(angle);
        float ySpeed = Time.deltaTime * moveSpeed * Mathf.Sin(angle);
        Vector2 moveDirection = new Vector2(xSpeed, ySpeed);
        rb.AddForce(moveDirection, ForceMode2D.Impulse);
        previousXVel = rb.velocity.x;
        previousYVel = rb.velocity.y;
        if (!isScurrying)
        {
            float lookingAngle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x));
            transform.rotation  = Quaternion.Euler(0,0, lookingAngle / Mathf.PI * 180 + 90);
        }
        else if (isScurrying)
        {
            float lookingAngle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x));
            transform.rotation = Quaternion.Euler(0, 0, lookingAngle / Mathf.PI * 180 - 90);
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        float xDifference = coll.GetContact(0).point.x - coll.gameObject.transform.position.x;
        float yDifference = coll.GetContact(0).point.y - coll.gameObject.transform.position.y;
        if (coll.collider == flashlightCollider && !isScurrying)
        {
            Debug.Log("Hit by Light");
            angle += Mathf.PI;
            rb.AddForce(new Vector2(rb.velocity.x * -2, rb.velocity.y * -2), ForceMode2D.Impulse);
            scurryingWallsLeft = 5;
            isScurrying = true;
        }
        else if (coll.collider == playerCollider)
        {
            Debug.Log("Ladies and gentlemen, we got him.");
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < attackDistanceTrigger && scurryingWallsLeft == 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x * -1, rb.velocity.y * -1), ForceMode2D.Impulse);
            angle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x));
            Debug.Log("I'm chasing the player");
        }
        else if (Vector3.Distance(player.transform.position, transform.position) > attackDistanceTrigger*3 && scurryingWallsLeft == 0) 
        {
            angle = Mathf.Atan2((player.transform.position.y - transform.position.y), (player.transform.position.x - transform.position.x));
            respawnClose();
        }
        else
        {
            Debug.Log("I'm looking around.");
            rb.AddForce(new Vector2(rb.velocity.x / -2, rb.velocity.y / -2), ForceMode2D.Impulse);
            angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x);
            if (scurryingWallsLeft > 0)
            {
                scurryingWallsLeft -= 1;
            }
            isScurrying = false;
        }
    }
    void respawnClose() 
    {
        float closestDistance = 1000000000000;
        int index = 0;
        for (int x = 0; x < 8; x++) {
            if (Vector3.Distance(player.transform.position, GameObject.Find("EnemyRespawn" + x).transform.position) < closestDistance) 
            {
                index = x;
                closestDistance = Vector3.Distance(player.transform.position, transform.position);
            }
        }
        GameObject respawnNode = GameObject.Find("EnemyRespawn" + index);
        transform.position = respawnNode.transform.position;

    }
}