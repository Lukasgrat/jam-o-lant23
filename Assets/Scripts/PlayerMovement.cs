using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    private float horizontalInput = 0;
    private float verticalInput = 0;
    public int movementSpeed = 0;
    public int rotationSpeed = 0;
    public int respawnIndex = 0; 
    public int endIndex = 0;
    public GameObject respawnSection;
    public GameObject endSection;
    public CircleCollider2D playerCollider;

    void Start()
    {
        respawnIndex = Random.Range(0, 7);
        endIndex = Random.Range(0, 7);
        if (respawnIndex == endIndex)
        {
            endIndex = (2 * respawnIndex) % 8;
        }
        respawnSection = GameObject.Find("Respawn" +  respawnIndex);
        endSection = GameObject.Find("Respawn" + endIndex);
        rb = GetComponent<Rigidbody2D>();
        transform.position = respawnSection.transform.position;
    }

    void Update()
    {
        GetPlayerInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        rb.velocity = transform.up * Mathf.Clamp01(verticalInput) * movementSpeed;
    }

    private void RotatePlayer()
    {
        float rotation = horizontalInput * rotationSpeed;
        transform.Rotate(Vector3.forward * rotation);
    }
    private void OnCollisionEnter2D(Collision2D coll) 
    {
        Debug.Log(coll.otherCollider);
        if (coll.gameObject.tag == "Respawn" && coll.otherCollider == playerCollider)
            Debug.Log("here");
        {
            if (coll.gameObject.name == endSection.name) 
            {
                Debug.Log("You win!");
            }
        }
    }
}