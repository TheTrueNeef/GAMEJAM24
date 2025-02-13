using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public int mana = 1;
    public int maxShips = 1;
    public Text manaText;
    public Text manaCost;

    public float waterLevel = 0f; // Current water level in the ship
    public float waterFillRate = 0.1f; // Rate at which water fills when hit
    public float sinkingThreshold = 20f; // Water level threshold to start sinking
    public float sinkingSpeed = 0.2f; // Speed at which the ship sinks
    public float tiltAngle = 10f; // Tilt angle for sinking

    public float gold = 5;
    public Text goldText;
    public Text killText;
    public Text killText2;

    public Text speedText;
    public Text speedCost;
    public float speed;
    public Text rangeText;
    public Text rangeCost;
    public float range;
    public Text repairText;
    public Text repairCost;
    public int repair;
    public Text weaponsText;
    public Text weaponsCost;
    public int weapons;
    public GameObject cam;
    public Slider shipSlider;
    public Slider healthSlider;

    private bool isSinking = false; // Is the ship sinking?
    public Collider hitbox;
    public int Kills = 0;

    public Text[] roundsText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCannonball"))
        {
            Debug.Log("Player Ship Hit by Cannonball!");
            waterLevel += waterFillRate; // Increase water level
            Destroy(other.gameObject); // Destroy the cannonball

            // Check if the water level has reached the sinking threshold
            if (waterLevel >= sinkingThreshold)
            {
                isSinking = true;
                Debug.Log("Player Ship is Sinking!");
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyCannonball"))
        {
            waterLevel += waterFillRate; // Increase water level
            other.GetComponent<cannonBall>().shotAt();
            StartCoroutine(Camera.main.GetComponent<audioManagerCam>().cannonImpactSound(transform));
            // Check if the water level has reached the sinking threshold
            if (waterLevel >= sinkingThreshold)
            {
                isSinking = true;
            }

        }
    }

    private void Update()
    {

        healthSlider.value = (sinkingThreshold - waterLevel + 0.0f) / (sinkingThreshold + 0.0f);
        shipSlider.value = (mana + 0.0f)/(maxShips + 0.0f);
        killText.text = "Sinks: " + Kills + "";
        killText2.text = "Sinks: " + Kills + "";
        int rounds = GameObject.Find("Game Loop").GetComponent<gameLoop>().currentRound;
        roundsText[0].text = "Rounds: " + rounds;
        roundsText[1].text = "Rounds: " + rounds;

        if (Mathf.Round(gold) >= 0)
        {
            goldText.text = "Gold: " + Mathf.Round(gold);
        }
        else
        {
            goldText.text = "Gold: " + 0;
            gold = 0;
        }
        //Range
        rangeText.text = "Range: " + GetComponent<ShipCannon>().fireForce/100;
        rangeCost.text = "" + Mathf.Round(Mathf.Pow(1.4f, (GetComponent<ShipCannon>().fireForce / 100) - 10));
        //Speed
        speedText.text = "Speed: " + GetComponent<ShipMovement>().maxSpeed;
        speedCost.text = "" + Mathf.Round(Mathf.Pow(1.4f, GetComponent<ShipMovement>().maxSpeed - 10));
        //Ghosts
        repairText.text = "Repair: " + GetComponent<ShipRepair>().repairRate;
        repairCost.text = "" + Mathf.Round(Mathf.Pow(1.4f, GetComponent<ShipRepair>().repairRate - 5));
        //Weapons
        weaponsText.text = "Reload: " + GetComponent<ShipCannon>().fireRate;
        weaponsCost.text = "" + 1000;
        //Mana
        manaText.text = "Ships: " + maxShips;
        manaCost.text = "" + 10000;
        // If the ship is sinking, apply sinking logic
        if (isSinking)
        {
            cam.GetComponent<CinemachineCamera>().Priority = 10;
            SinkShip();
        }
    }

    private void SinkShip()
    {
        hitbox.isTrigger = true;
        // Move the ship downwards
        transform.position += Vector3.down * sinkingSpeed * Time.deltaTime;

        // Tilt the ship upwards
        Quaternion tiltRotation = Quaternion.Euler(tiltAngle, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, tiltRotation, Time.deltaTime * 4f); // Smooth tilt

        // Optionally, stop sinking after a certain position
        if (transform.position.y < -5f) // Adjust as needed
        {
            isSinking = false;
            // Add any additional game-over logic here
        }
    }
}
