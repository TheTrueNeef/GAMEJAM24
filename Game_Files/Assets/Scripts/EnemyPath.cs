using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [Range(0f, 100f)] public float sailPercentage = 100f; // Default full speed
    [Range(-1f, 1f)] public float rudderRotation = 0f;    // Rudder rotation

    public float maxSpeed = 15f;   // Maximum speed of the enemy ship
    public float turnSpeed = 20f;  // Speed at which the ship can turn
    public float broadsideDistance = 50f; // Distance to initiate broadside maneuver
    public float minBroadsideDistance = 30f; // Minimum distance to maintain broadside

    public Transform playerShip; // Reference to the player's ship
    private Rigidbody rb; // Rigidbody component
    private float currentSpeed = 0f;

    // Water filling and sinking mechanics
    public float waterLevel = 0f;  // How much water the ship has taken on
    public float maxWaterLevel = 100f; // The maximum amount of water the ship can hold before sinking
    public float waterFillRate = 5f;  // How fast the water fills when hit
    public bool isSinking = false;   // To track if the ship is sinking

    // Variables for smooth sail adjustment
    public float sailChangeSpeed = 1f; // Speed at which sail percentage changes
    private float targetSailPercentage = 100f; // Desired sail percentage for smooth transition
    private float sailVelocity = 0f; // Used for SmoothDamp

    // Trigger colliders for hitbox and shooting zone
    public Collider hitboxTrigger;      // Collider for detecting cannonball hits
    public Collider shootingTrigger;    // Collider for detecting when to fire at player
    public Collider hitbox;
    void Start()
    {
        playerShip = GameObject.Find("Player").transform;
        Physics.IgnoreCollision(playerShip.GetComponent<Collider>(), hitbox);
        Physics.IgnoreCollision(GameObject.Find("Terrain").GetComponent<Collider>(), hitbox);


        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If the player's ship is set, navigate towards it
        if (!isSinking && playerShip != null)
        {
            NavigateTowardsPlayer();
        }

        // Check if the water level is high enough to sink the ship
        if (waterLevel >= maxWaterLevel && !isSinking)
        {
            playerShip.GetComponent<PlayerHealth>().Kills++;
            StartCoroutine(SinkShip());
        }


        
    }

    void FixedUpdate()
    {
        if (!isSinking)
        {
            // Smoothly adjust the sail percentage towards the target value
            sailPercentage = Mathf.SmoothDamp(sailPercentage, targetSailPercentage, ref sailVelocity, sailChangeSpeed);

            // Calculate current speed based on sail percentage
            currentSpeed = (sailPercentage / 100f) * maxSpeed;

            // Reduce movement speed as the water level increases
            currentSpeed *= Mathf.Clamp01(1f - (waterLevel / maxWaterLevel));

            // Move the enemy ship forward
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            // Rotate the enemy ship based on rudder input
            Quaternion rotation = Quaternion.Euler(0f, rudderRotation * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * rotation);
        }
    }

    void NavigateTowardsPlayer()
    {
        Vector3 directionToPlayer = playerShip.position - transform.position;
        directionToPlayer.y = 0; // Keep the movement on the horizontal plane

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > broadsideDistance)
        {
            // Rotate the ship towards the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // Set target sail speed to full to approach the player quickly
            targetSailPercentage = 100f;
        }

        // If the ship is far from the player, move directly towards the player
        if (distanceToPlayer > broadsideDistance)
        {
            // Rotate the ship towards the player
            
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // Set target sail speed to full to approach the player quickly
            targetSailPercentage = 100f;
        }
        else if (distanceToPlayer <= broadsideDistance && distanceToPlayer > minBroadsideDistance)
        {
            // Once in broadside distance, attempt to turn broadside to the player
            PerformBroadsideManeuver(directionToPlayer);
        }
        else
        {
            // If the enemy is too close, reduce forward speed and try to maintain distance
            PerformBroadsideManeuver(directionToPlayer);
            targetSailPercentage = 30f; // Slow down when too close to avoid sailing away
        }
    }

    void PerformBroadsideManeuver(Vector3 directionToPlayer)
    {
        // Get the angle between the ship's forward direction and the direction to the player
        float angleToPlayer = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);

        // If the angle is not already broadside (i.e., 90 degrees), turn accordingly
        if (Mathf.Abs(angleToPlayer) > 70f && Mathf.Abs(angleToPlayer) < 100f)
        {
            // Close enough to broadside, keep rudder centered
            rudderRotation = 0f;
            // Slow down or stop forward movement to avoid sailing away
            targetSailPercentage = 10f; // Minimal forward movement to keep position
        }
        else if (angleToPlayer > 0)
        {
            // Rotate left to face broadside
            rudderRotation = -1f;
        }
        else
        {
            // Rotate right to face broadside
            rudderRotation = 1f;
        }

        // Adjust sail speed for maneuvering (slow down for better control)
        targetSailPercentage = 50f;
    }

    // Trigger collision with cannonball (hitbox)
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("water")) { return; }
        // Check if this is the hitbox trigger for cannonball hits
        if (other.CompareTag("Cannonball"))
        {
            waterLevel += waterFillRate;

            other.GetComponent<cannonBall>().shotAt();
            if (!isSinking)
            {
                float shipY = -1 * waterLevel * 0.01f;
                transform.position = new Vector3(transform.position.x, shipY, transform.position.z);
            }
        }

        // Check if this is the shooting trigger (detect player)
        if (other.CompareTag("PlayerShip") || (other.CompareTag("GhostShip")))
        {
            FireAtPlayer();
            if (other.CompareTag("GhostShip"))
            {
                Debug.Log("Ghost Ship in Range");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GhostShip"))
        {
            Debug.Log("Ghost Ship in Range");
        }
        if (other.CompareTag("water")) { return; }
        // Check if this is the hitbox trigger for cannonball hits
        if (other == hitboxTrigger && other.CompareTag("Cannonball"))
        {
            // Increase the water level when hit by a cannonball
            waterLevel += waterFillRate;


            // Optionally, destroy the cannonball after it hits the ship
            if (!isSinking)
            {
                float shipY = -1 * waterLevel * 0.01f;
                transform.position = new Vector3(transform.position.x, shipY, transform.position.z);
            }
            Destroy(other.gameObject);
        }

        // Check if this is the shooting trigger (detect player)
        if (other == shootingTrigger && (other.CompareTag("PlayerShip") || (other.CompareTag("GhostShip"))))
        {
            FireAtPlayer();
        }
    }

    private void FireAtPlayer()
    {
        if (Time.time >= GetComponent<enemyShoot>().nextFireTime && !isSinking)
        {
            Debug.Log("Player ship in range! Firing cannons!");
            GetComponent<enemyShoot>().FireBothSides();
            GetComponent<enemyShoot>().nextFireTime = Time.time + GetComponent<enemyShoot>().fireInterval; // Reset the fire timer
        }
    }

    IEnumerator SinkShip()
    {
        isSinking = true;
        this.GetComponent<enemyShoot>().fireInterval = 100000;
        transform.GetComponentInChildren<Outline>().enabled = false;
        // Disable ship movement
        sailPercentage = 0f;
        currentSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // Stop physics-based movement

        //Disable Hitbox
        hitbox.isTrigger = true;


        // Sinking effect
        float sinkSpeed = 0.5f; // Adjust this for faster/slower sinking
        while (transform.position.y > -10f) // Adjust sinking depth
        {
            transform.position += Vector3.down * sinkSpeed * Time.deltaTime;
            yield return null;
        }

        // Optionally, destroy or deactivate the ship after it sinks
        Destroy(gameObject);
    }
}
