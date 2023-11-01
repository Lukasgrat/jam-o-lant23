using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public Sprite startSprite;
    public Sprite endSprite;

    private float horizontalInput = 0;
    private float verticalInput = 0;
    public int movementSpeed = 0;
    public int rotationSpeed = 0;
    public int respawnIndex = 0; 
    public int endIndex = 0;
    public GameObject respawnSection;
    public GameObject endSection;
    public GameObject enemy;
    public CircleCollider2D playerCollider;
    public EdgeCollider2D flashLightCollider;
    public TMP_Text distanceText;

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
        SpriteRenderer respawnRenderer = respawnSection.GetComponent<SpriteRenderer>();
        respawnRenderer.sprite = startSprite;
        respawnRenderer.color = Color.white;
        SpriteRenderer endRenderer = endSection.GetComponent<SpriteRenderer>();
        endRenderer.sprite = endSprite;
        endRenderer.color = Color.white;
        rb = GetComponent<Rigidbody2D>();
        transform.position = respawnSection.transform.position;
    }

    void Update()
    {
        GetPlayerInput();
        SetDistanceText();
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
        if (coll.gameObject.tag == "Respawn" && !(coll.otherCollider == flashLightCollider))
        {
            Debug.Log("here");
            if (coll.gameObject.name == endSection.name) 
            {
                Debug.Log("You win!");
                Time.timeScale = 0;
            }
        }
    }
    void SetDistanceText()
    {
        int totalDistance = (int)Vector3.Distance(enemy.transform.position, transform.position);
        distanceText.text = "The clown is: " + totalDistance.ToString() + " meters away!";
    }
}
