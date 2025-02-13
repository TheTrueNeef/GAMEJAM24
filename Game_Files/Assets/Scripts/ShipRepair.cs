using UnityEngine;

public class ShipRepair : MonoBehaviour
{
    // Reference to the cannons (assuming they are scripts or GameObjects)
    public GameObject[] cannons; // Array of cannon GameObjects
    public GameObject repairAnim;
    
    public float repairRate = 10f;        // How much water to remove per second (speed of repair)

    // Flag to check if repair is happening
    private bool isRepairing = false;

    void Update()
    {
        // If the player presses 'R', start the repair process
        if (Input.GetKeyDown(KeyCode.R) && !isRepairing)
        {
            StartRepair();
        }

        // If the ship is repairing, continue draining water
        if (isRepairing)
        {
            RepairShip();
        }
    }

    // Start the repair process: disable cannons and begin draining water
    void StartRepair()
    {
        isRepairing = true;
        repairAnim.SetActive(true);
        // Disable all cannons
        GetComponent<ShipCannon>().isrepairing = true;
    }

    // Repair the ship by draining water over time
    void RepairShip()
    {
        if (GetComponent<PlayerHealth>().waterLevel > 0f)
        {
            // Gradually decrease the water level (repairing the ship)
            GetComponent<PlayerHealth>().waterLevel -= repairRate * Time.deltaTime;

            // Clamp water level to avoid going below 0
            GetComponent<PlayerHealth>().waterLevel = Mathf.Clamp(GetComponent<PlayerHealth>().waterLevel, 0f, GetComponent<PlayerHealth>().sinkingThreshold);
        }
        else
        {
            // When fully repaired (water drained), stop repairing and enable the cannons
            StopRepair();
        }
    }

    // Stop the repair process and re-enable the cannons
    void StopRepair()
    {
        isRepairing = false;
        repairAnim.SetActive(false);
        GetComponent<ShipCannon>().isrepairing = false;
    }
}
