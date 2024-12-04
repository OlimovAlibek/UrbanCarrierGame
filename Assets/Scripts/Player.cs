using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Speed at which the player moves
    public float moveSpeed = 5f; 
    
    // Reference to the joystick for player movement
    public FixedJoystick joystick;

    // Rigidbody2D for physics-based movement
    public Rigidbody2D rb; 

    // Initial position of the player (used for respawning)
    private Vector3 initialPosition;

    // Maximum number of lives the player can have
    public int maxLives = 3;

    // Current number of lives, hidden from the Unity Inspector
    [HideInInspector]
    public int currentLives;

    // Reference to the health bar UI
    public HealthBar healthBar;

    // SpriteRenderer to control the player's appearance (e.g., flickering effect)
    public SpriteRenderer sprite;

    // Variables for controlling the flicker effect when hit
    private int flickerAmount = 6; // Number of times to flicker
    private float flickerDuration = 0.1f; // Duration of each flicker
    public bool canBeHit = true; // Whether the player can currently take damage

    // Buttons for receiving and delivering orders
    [SerializeField]
    private Button receiveButton, deliverButton;

    // Whether the player is carrying an order
    public bool carryingOrder = false;

    // Reference to the Animator for controlling animations
    public Animator animator;

    // Vector to store movement input
    Vector2 movement; 

    // Called once when the script is initialized
    void Start()
    {
        currentLives = maxLives; // Initialize lives
        healthBar.SetMaxHealth(maxLives); // Set health bar to full
        initialPosition = transform.position; // Save the player's starting position
    }

    // Called every frame to handle player input
    void Update() 
    {
        // Get movement input from the joystick
        movement.x = joystick.Horizontal; 
        movement.y = joystick.Vertical; 

        // Update animation parameters for movement
        animator.SetFloat("Horizontal", movement.x); 
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); // Set speed based on movement magnitude
    }

    // Called at fixed intervals to handle physics-based movement
    void FixedUpdate() 
    {
        // Move the player based on input and speed
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); 
    }

    // Triggered when the player enters a trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with a vehicle and can be hit
        if (collision.CompareTag("Vehicles") && canBeHit)
        {
            currentLives -= 1; // Reduce lives
            healthBar.SetHealth(currentLives); // Update the health bar
            transform.position = initialPosition; // Respawn the player at the initial position
            StartCoroutine(GetHitFlicker()); // Start flicker effect

            // If no lives are left, trigger game over
            if (currentLives <= 0)
            {
                FindObjectOfType<GamePlayManager>().gameOver();
            }
        }
    }

    // Triggered when the player collides with another object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with a shop
        if (collision.gameObject.CompareTag("Shop"))
        {
            if (collision.gameObject.GetComponent<Shop>().havingOrder)
            {
                receiveButton.gameObject.SetActive(true); // Show the receive button
            }
        }
        // Check if the player collided with a house while carrying an order
        else if (collision.gameObject.CompareTag("House") && carryingOrder)
        {
            if (collision.gameObject.GetComponent<House>().isDesination)
            {
                deliverButton.gameObject.SetActive(true); // Show the deliver button
            }
        }
    }

    // Triggered when the player exits a collision
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Hide buttons when leaving the collision area
        deliverButton.gameObject.SetActive(false);
        receiveButton.gameObject.SetActive(false);
    }

    // Coroutine for flickering effect when hit
    IEnumerator GetHitFlicker()
    {
        canBeHit = false; // Temporarily make the player invincible
        for (int i = 0; i < flickerAmount; i++)
        {
            sprite.color = new Color(1f, 1f, 1f, 0.5f); // Make the player semi-transparent
            yield return new WaitForSeconds(flickerDuration); // Wait
            sprite.color = Color.white; // Restore the player's color
            yield return new WaitForSeconds(flickerDuration); // Wait
        }
        canBeHit = true; // Allow the player to be hit again
    }
}
