using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class ShipCannon : MonoBehaviour
{
    public Transform[] leftCannons;  // Array of transforms for the left side cannons
    public Transform[] rightCannons; // Array of transforms for the right side cannons
    public Transform[] leftExplosions;  // Array of transforms for the left side cannons
    public Transform[] rightExplosions; // Array of transforms for the right side cannons
    public GameObject cannonballPrefab; // Prefab for the cannonball
    public GameObject[] explosions;
    public float fireForce = 1000f;   // The force applied to the cannonball when fired
    public float fireRate = 10f;       // Delay between cannon shots
    public bool canFire = true;      // Used to control the firing rate
    Random random = new Random();
    public bool isrepairing = false;
    public GameObject reloadAnim;
    private void Start()
    {
        
    }
    void Update()
    {
        // Fire cannons when Space key is pressed and canFire is true
        if (Input.GetKeyDown(KeyCode.Space) && canFire && !isrepairing)
        {
            StartCoroutine(FireCannons());
        }
    }

    IEnumerator FireCannons()
    {
        // Prevent firing until the next shot is ready
        canFire = false;
        reloadAnim.SetActive(true);
        // Fire all left-side cannons
        foreach (Transform cannon in leftCannons)
        {
            FireCannon(cannon);
        }

        // Fire all right-side cannons
        foreach (Transform cannon in rightCannons)
        {
            FireCannon(cannon);
        }

        foreach (Transform explosion in leftExplosions)
        {
            explodeEffect(explosion);
        }

        // Fire all right-side cannons
        foreach (Transform explosion in rightExplosions)
        {
            explodeEffect(explosion);
        }
        // Wait for fire rate duration
        yield return new WaitForSeconds(fireRate);

        // Allow firing again after delay
        canFire = true;
        reloadAnim.SetActive(false);

    }
    void explodeEffect(Transform explosion)
    {
        int index = random.Next(0, 6);
        GameObject explode = Instantiate(explosions[index], explosion.position, explosion.rotation);
    }
    void FireCannon(Transform cannon)
    {
        // Instantiate a cannonball at the position of the cannon
        GameObject cannonball = Instantiate(cannonballPrefab, cannon.position, cannon.rotation);
        // Get the Rigidbody component of the cannonball
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        // Apply force to the cannonball in the forward direction of the cannon
        rb.AddForce(cannon.forward * fireForce);
        StartCoroutine(Camera.main.GetComponent<audioManagerCam>().cannonFire(cannon));
    }
}
