using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class ghostPath : MonoBehaviour
{
    [Range(0f, 100f)] public float sailPercentage = 100f; // Default full speed
    [Range(-1f, 1f)] public float rudderRotation = 0f;    // Rudder rotation

    public float maxSpeed = 15f;   // Maximum speed of the enemy ship
    public float turnSpeed = 20f;  // Speed at which the ship can turn
    public float broadsideDistance = 50f; // Distance to initiate broadside maneuver
    public float minBroadsideDistance = 30f; // Minimum distance to maintain broadside

    public Transform enemyShip; // Reference to the player's ship
    private Rigidbody rb; // Rigidbody component
    private float currentSpeed = 0f;

    public GameObject player;

    // Water filling and sinking mechanics
    public float waterLevel = 0f;  // How much water the ship has taken on
    public float maxWaterLevel = 100f; // The maximum amount of water the ship can hold before sinking
    public float waterFillRate = 5f;  // How fast the water fills when hit
    private bool isSinking = false;   // To track if the ship is sinking

    // Variables for smooth sail adjustment
    public float sailChangeSpeed = 1f; // Speed at which sail percentage changes
    private float targetSailPercentage = 100f; // Desired sail percentage for smooth transition
    private float sailVelocity = 0f; // Used for SmoothDamp

    // Trigger colliders for hitbox and shooting zone
    public Collider hitboxTrigger;      // Collider for detecting cannonball hits
    public Collider shootingTrigger;    // Collider for detecting when to fire at player
    public Collider hitbox;

    public Transform island;
    public float circleRadius = 50f;
    public float circleSpeed = 2f;
    //Path to enemy ship

    void Start()
    {
        player = GameObject.Find("Player");
        island = GameObject.Find("center").transform;
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        FindNearestEnemy();
        
    }

    void Update()
    {
        if (enemyShip != null)
        {
            if(enemyShip.GetComponent<EnemyPath>().waterLevel >= maxWaterLevel)
            {
                sailPercentage = 0;
                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                enemyShip = null;
            }
        }
        
        // If the player's ship is set, navigate towards it
        if (!isSinking && enemyShip != null)
        {
            NavigateTowardsPlayer();
        }

        // Check if the water level is high enough to sink the ship
        if (waterLevel >= maxWaterLevel && !isSinking)
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(SinkShip());
                        
        }


    }
    void FindNearestEnemy()
    {
        // Find all enemies in the scene
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyShip");

        if (enemies.Length == 0)
        {
            enemyShip = null; // If no enemies, set to null
            return;
        }

        // Set initial reference point and distance
        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        // Loop through all enemies to find the nearest one
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        enemyShip = closestEnemy.transform;  // Set the nearest enemy
    }
    void FixedUpdate()
    {
        if(transform.position.y ==9)
        {
            Destroy(this);
        }
        if (enemyShip == null && !isSinking)
        {
            FindNearestEnemy();  // Find a new enemy if the current one is null (either no enemies or the previous one is dead)
        }
        if (enemyShip != null && !isSinking)
        {
            // Rotate the ship towards the player
            Quaternion targetRotation = Quaternion.LookRotation(enemyShip.transform.position);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime));

            // Set target sail speed to full to approach the player quickly
            targetSailPercentage = 100f;
        }
        if (enemyShip != null && !isSinking)
        {
            float distance = (enemyShip.position - transform.position).magnitude;
            if (!isSinking && enemyShip == null && distance > broadsideDistance)
            {
                Quaternion targetRotation = Quaternion.LookRotation(enemyShip.transform.position);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, turnSpeed * Time.deltaTime));
            }
        }
        
        if (!isSinking && enemyShip == null)
        {
            sailPercentage = 0;
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (!isSinking && enemyShip != null)
        {
            Debug.Log("moving like a dumbass");
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
        if (!isSinking && enemyShip == null)
        {
            Debug.Log("stopping movement");

            // Smoothly adjust the sail percentage towards the target value
            sailPercentage = Mathf.SmoothDamp(sailPercentage, 0, ref sailVelocity, sailChangeSpeed);

            // Calculate current speed based on sail percentage
            currentSpeed = (sailPercentage / 100f) * maxSpeed;

            // Reduce movement speed as the water level increases
            currentSpeed *= Mathf.Clamp01(1f - (waterLevel / maxWaterLevel));

            // Move the enemy ship forward
            Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            // Rotate the enemy ship based on rudder input
            Quaternion rotation = Quaternion.Euler(0f, 0 * turnSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * rotation);
        }
    }

    
    void NavigateTowardsPlayer()
    {
        if(enemyShip == null)
        {
            return;
        }
        Vector3 directionToPlayer = enemyShip.position - transform.position;
        directionToPlayer.y = 0; // Keep the movement on the horizontal plane

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > 100)
        {
            Vector3 trg = enemyShip.transform.position - transform.position;
            Quaternion target = Quaternion.LookRotation(trg);
            Debug.Log("rotating");
            // Rotate the ship towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, target , 3f * Time.deltaTime);

            // Set target sail speed to full to approach the player quickly
            targetSailPercentage = 10f;
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
        if (Mathf.Abs(angleToPlayer) > 80f && Mathf.Abs(angleToPlayer) < 100f)
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
        if (other.CompareTag("EnemyCannonball"))
        {
            waterLevel += waterFillRate;

            Destroy(other.gameObject);
            if (!isSinking)
            {
                float shipY = -1 * waterLevel * 0.01f;
                transform.position = new Vector3(transform.position.x, shipY, transform.position.z);
            }
        }

        // Check if this is the shooting trigger (detect player)
        if (other == shootingTrigger && other.CompareTag("enemyShip"))
        {
            FireAtPlayer();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("water")) { return; }
        // Check if this is the hitbox trigger for cannonball hits
        if (other == hitboxTrigger && other.CompareTag("EnemyCannonball"))
        {
            // Increase the water level when hit by a cannonball
            waterLevel += waterFillRate;

            // Log the impact to the console
            Debug.Log("Enemy ship hit by a cannonball! Water level: " + waterLevel);

            // Optionally, destroy the cannonball after it hits the ship
            if (!isSinking)
            {
                float shipY = -1 * waterLevel * 0.01f;
                transform.position = new Vector3(transform.position.x, shipY, transform.position.z);
            }
            other.GetComponent<cannonBall>().shotAt();
        }

        // Check if this is the shooting trigger (detect player)
        if (other == shootingTrigger && other.CompareTag("enemyShip"))
        {
            // Call method to fire cannons at the player
            FireAtPlayer();

            // Log when the player enters the shooting range
            Debug.Log("Player ship in range! Firing cannons!");
        }
    }

    private void FireAtPlayer()
    {
        // Logic to shoot cannons at the player
        Debug.Log("Firing cannons at the player!");
    }

    public IEnumerator SinkShip()
    {
        Debug.Log("sink my ship");
        GameObject.Find("Player").GetComponent<PlayerHealth>().mana++;
        isSinking = true;
        transform.GetComponentInChildren<Outline>().enabled = false;
        // Disable ship movement
        sailPercentage = 0f;
        currentSpeed = 0f;
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; // Stop physics-based movement

        //Disable Hitbox
        hitbox.isTrigger = true;


        // Sinking effect
        float sinkSpeed = 10f; // Adjust this for faster/slower sinking
        while (transform.position.y > -10f) // Adjust sinking depth
        {
            transform.position = new Vector3(transform.position.x,-9,transform.position.y);
            Destroy(gameObject);
            yield return null;
        }

        // Optionally, destroy or deactivate the ship after it sinks
        Destroy(gameObject);
    }
}
