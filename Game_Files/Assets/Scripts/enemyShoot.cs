using UnityEngine;
using Random = System.Random;

public class enemyShoot : MonoBehaviour
{
    public GameObject cannonballPrefab; // Cannonball prefab
    public Transform[] leftCannons; // Array of transforms representing left-side cannons
    public Transform[] rightCannons; // Array of transforms representing right-side cannons
    public Transform playerShip; // Reference to the player ship
    public float fireInterval = 3f; // Time between cannon fire
    public float cannonballSpeed = 20f; // Speed of cannonballs

    public Transform[] leftExplosions;  // Array of transforms for the left side cannons
    public Transform[] rightExplosions; // Array of transforms for the right side cannons
    public GameObject[] explosions;

    public float nextFireTime = 0f;
    Random random = new Random();


    private void Start()
    {
        playerShip = GetComponent<EnemyPath>().playerShip;
    }

    // Fire both sides of cannons, same as the player's ship
    public void FireBothSides()
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

        }
    }
}
