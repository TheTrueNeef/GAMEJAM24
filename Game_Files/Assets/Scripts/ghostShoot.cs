using UnityEngine;
using Random = System.Random;

public class ghostShoot : MonoBehaviour
{
    public GameObject cannonballPrefab; // Cannonball prefab
    public Transform[] leftCannons; // Array of transforms representing left-side cannons
    public Transform[] rightCannons; // Array of transforms representing right-side cannons
    Transform enemyShip; // Reference to the player ship
    public float fireInterval = 3f; // Time between cannon fire
    public float cannonballSpeed = 20f; // Speed of cannonballs

    public GameObject[] explosions;
    public Transform[] leftExplosions;  // Array of transforms for the left side cannons
    public Transform[] rightExplosions; // Array of transforms for the right side cannons
    Random random = new Random();

    private float nextFireTime = 0f;

    private void Start()
    {
        enemyShip = GetComponent<ghostPath>().enemyShip;
    }
    private void FixedUpdate()
    {
        enemyShip = GetComponent<ghostPath>().enemyShip;
    }

    private void Update()
    {
        // Check if it's time to fire at the player
        if (transform.position.y > 0 && enemyShip != null)
        {
            if (Time.time >= nextFireTime && IsBroadsideToPlayer())
            {
                FireBothSides();
                nextFireTime = Time.time + fireInterval; // Reset the fire timer
            }
        }
        
    }

    // Checks if the ship is broadside to the player (orthogonal to the player ship)
    private bool IsBroadsideToPlayer()
    {
        Vector3 directionToPlayer = enemyShip.position - transform.position;
        float angleToPlayer = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);

        // Fire only if the player is approximately 90 degrees to the left or right (broadside)
        return Mathf.Abs(angleToPlayer) > 75f && Mathf.Abs(angleToPlayer) < 105f;
    }

    // Fire both sides of cannons, same as the player's ship
    private void FireBothSides()
    {
        FireCannons(leftCannons);
        FireCannons(rightCannons);

        foreach (Transform explosion in leftExplosions)
        {
            explodeEffect(explosion);
        }

        // Fire all right-side cannons
        foreach (Transform explosion in rightExplosions)
        {
            explodeEffect(explosion);
        }
    }
    void explodeEffect(Transform explosion)
    {
        int index = random.Next(0, 6);
        GameObject explode = Instantiate(explosions[index], explosion.position, explosion.rotation);
        StartCoroutine(Camera.main.GetComponent<audioManagerCam>().cannonFire(explosion));
    }
    // Fire all the cannons from the given array (left or right side)
    private void FireCannons(Transform[] cannons)
    {
        foreach (Transform cannon in cannons)
        {
            // Instantiate a cannonball at the cannon's position
            GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, cannon.rotation);
            Rigidbody rb = cannonball.GetComponent<Rigidbody>();

            // Fire the cannonball in the forward direction of the cannon
            rb.linearVelocity = cannon.forward * cannonballSpeed;

            Debug.Log("Ghost Ship Fired Cannonball from: " + cannon.name);
        }
    }
}
