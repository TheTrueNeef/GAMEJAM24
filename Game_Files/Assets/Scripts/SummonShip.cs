using UnityEngine;

public class SummonShip : MonoBehaviour
{
    public GameObject ghostShipPrefab;  // Reference to your ghost ship prefab
    public float summonHeightStart = -10f;  // Initial y position of ghost ship
    public float summonHeightEnd = 0f;     // Final y position of ghost ship
    public float summonSpeed = 50f;           // Speed of ghost ship rising

    public bool isSummoning = false;
    public bool canSummon;
    void Update()
    {
        if (GetComponent<PlayerHealth>().mana == 0) 
        {
            canSummon = false;
        }
        else if(!isSummoning)
        {
            canSummon = true;
        }

        // Check if the player presses 'F' and ghost ship hasn't been summoned yet
        if (Input.GetKeyDown(KeyCode.F) && canSummon)
        {
            GetComponent<PlayerHealth>().mana--;
            SummonGhostShipNextToPlayer();
        }

        // If the ghost ship is in the process of summoning, move it upwards
        
    }

    void SummonGhostShipNextToPlayer()
    {
        // Instantiate the ghost ship at the correct position (next to player, at y = -125)
        Vector3 spawnPosition = new Vector3(transform.position.x + 10f, summonHeightStart, transform.position.z);  // Adjust x or z as needed
        GameObject ghostShipInstance = Instantiate(ghostShipPrefab, spawnPosition, Quaternion.identity);
        ghostShipInstance.GetComponent<Collider>().enabled = false;
        float speed = this.GetComponent<ShipMovement>().maxSpeed;
        float turnspeed = this.GetComponent<ShipMovement>().turnSpeed;
        float force = this.GetComponent<ShipCannon>().fireForce / 100;
        float fire = this.GetComponent<ShipCannon>().fireRate;

        ghostShipInstance.GetComponent<ghostPath>().maxSpeed = speed;
        ghostShipInstance.GetComponent<ghostPath>().turnSpeed = turnspeed;
        ghostShipInstance.GetComponent<ghostShoot>().cannonballSpeed = force;
        ghostShipInstance.GetComponent<ghostPath>().broadsideDistance = 100;
        ghostShipInstance.GetComponent<ghostPath>().waterFillRate = 25;
        ghostShipInstance.GetComponent<ghostShoot>().fireInterval = fire;
        // Start the summoning process
        isSummoning = true;
    }
}
