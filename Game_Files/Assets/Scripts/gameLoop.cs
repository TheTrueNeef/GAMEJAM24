using System.Collections.Generic;
using UnityEngine;

public class gameLoop : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab; // Reference to the enemy prefab to spawn
    public Vector2 topRightCorner = new Vector2(500, 500);  // Top right corner of the spawn zone
    public Vector2 bottomLeftCorner = new Vector2(-500, -500); // Bottom left corner of the spawn zone
    public int maxRounds = 100; // Set the maximum number of rounds

    public int currentRound = 1; // Current round, starting from 1
    private int baseEnemies = 2;  // Number of enemies in the first round
    private bool isWaitingForUser = true; // Bool to check if waiting for the user to start next round
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject winScreen;
    public GameObject lostScreen;
    public GameObject btn;
    void Update()
    {
        if (!isWaitingForUser && enemies.Count == 0) 
        {
            btn.SetActive(true);
            EndRound();
        }
        // Check if the user has clicked to start the next round
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i].GetComponent<EnemyPath>().waterLevel >= 100)
            {
                enemies.RemoveAt(i);
            }
        }
        if(player.GetComponent<PlayerHealth>().waterLevel >= 100) 
        {
            lostScreen.SetActive(true);
        }
    }
    public void startRound()
    {
        if (isWaitingForUser)
        {
            btn.SetActive(false);
            isWaitingForUser = false; // Start the next wave when user clicks the button (Space in this case)
            StartNextRound();
        }
    }
    void StartNextRound()
    {
        player.GetComponent<PlayerHealth>().gold = 100+5*Mathf.Pow(Mathf.Pow(11000000f / 5, 1f / 99f), currentRound - 1);
        // Check if the maximum round has been reached
        if (currentRound > maxRounds)
        {
            player.GetComponent<camManager>().win();
            winScreen.SetActive(true);
            return; // Stop further rounds from being generated
        }

        // Calculate the number of enemies to spawn in this round
        int enemiesToSpawn = baseEnemies + (currentRound - 1) + (currentRound / 5);

        // Spawn enemies at random positions within the defined zone
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyPath>().maxSpeed = Mathf.Lerp(10f, 70f, (float)(currentRound - 1) / (100 - 1));
            enemy.GetComponent<EnemyPath>().turnSpeed = Mathf.Lerp(15f, 75f, (float)(currentRound - 1) / (100 - 1));
            enemy.GetComponent<enemyShoot>().cannonballSpeed = Mathf.Lerp(100f, 200f, (float)(currentRound - 1) / (100 - 1));
            enemy.GetComponent<EnemyPath>().broadsideDistance = Mathf.Lerp(50f, 200f, (float)(currentRound - 1) / (100 - 1));
            enemy.GetComponent<EnemyPath>().waterFillRate = Mathf.Lerp(25f, 1f, (float)(currentRound - 1) / (100 - 1));
            enemy.GetComponent<enemyShoot>().fireInterval = Mathf.Lerp(5f, 0.1f, (float)(currentRound - 1) / (100 - 1));
            enemies.Add(enemy);
        }

        Debug.Log("Wave " + currentRound + " started with " + enemiesToSpawn + " enemies!");

        // Increase round number after starting the round
        currentRound++;

    }

    // Method to get a random spawn position within the defined zone
    Vector3 GetRandomSpawnPosition()
    {
        float xPos = Random.Range(bottomLeftCorner.x, topRightCorner.x);
        float zPos = Random.Range(bottomLeftCorner.y, topRightCorner.y);

        // Assuming enemies spawn at ground level (y = 0). You can change this if needed.
        return new Vector3(xPos, 0f, zPos);
    }

    void EndRound()
    {
        // When the round is over, wait for user to press the button to continue
        isWaitingForUser = true;
        btn.SetActive(true);
    }
}
