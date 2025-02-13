using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class mistDamage : MonoBehaviour
{
    public float increaseRate = 1f; // How much to increase the value per second
    private float valueToIncrease = 0f; // The value that will be increased
    private bool isInMist = false; // Check if the object is in the mist area

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the mist has a specific tag (optional)
        if (other.CompareTag("PlayerShip")) // Change "Player" to your object's tag
        {
            isInMist = true;
            GameObject exitingObject = other.gameObject;
            StartCoroutine(IncreaseValuePlayer(exitingObject));
        }
        if (other.CompareTag("EnemyShip")) // Change "Player" to your object's tag
        {
            isInMist = true;
            GameObject exitingObject = other.gameObject;
            StartCoroutine(IncreaseValueEnemy(exitingObject));
        }
        if (other.CompareTag("GhostShip")) // Change "Player" to your object's tag
        {
            isInMist = true;
            GameObject exitingObject = other.gameObject;
            StartCoroutine(IncreaseValueGhost(exitingObject));
        }

    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the mist has a specific tag (optional)
        if (other.CompareTag("PlayerShip")) // Change "Player" to your object's tag
        {
            isInMist = false;
            GameObject exitingObject = other.gameObject;
            StopCoroutine(IncreaseValuePlayer(exitingObject));
        }
        if (other.CompareTag("EnemyShip")) // Change "Player" to your object's tag
        {
            isInMist = false;
            GameObject exitingObject = other.gameObject;
            StopCoroutine(IncreaseValueEnemy(exitingObject));
        }
        if (other.CompareTag("GhostShip")) // Change "Player" to your object's tag
        {
            isInMist = false;
            GameObject exitingObject = other.gameObject;
            StopCoroutine(IncreaseValueGhost(exitingObject));
        }
    }


    private IEnumerator IncreaseValuePlayer(GameObject ship)
    {
        while (isInMist)
        {
            ship.GetComponent<PlayerHealth>().waterLevel += 5 * Time.deltaTime; // Increase the value
            yield return null; // Wait until the next frame
        }
    }
    private IEnumerator IncreaseValueEnemy(GameObject ship)
    {
        while (isInMist)
        {
            ship.GetComponent<EnemyPath>().waterLevel += 25 * Time.deltaTime; // Increase the value
            yield return null; // Wait until the next frame
        }
    }
    private IEnumerator IncreaseValueGhost(GameObject ship)
    {
        while (isInMist)
        {
            ship.GetComponent<ghostPath>().waterLevel += 25 * Time.deltaTime; // Increase the value
            yield return null; // Wait until the next frame
        }
    }
}
