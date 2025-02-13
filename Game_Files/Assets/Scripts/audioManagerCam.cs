using System.Collections;
using UnityEngine;

public class audioManagerCam : MonoBehaviour
{
    public AudioClip cannonImpact; // Assign your sound clip in the Inspector
    public AudioClip cannonFire1; // Assign your sound clip in the Inspector
    public AudioClip cannonFire2; // Assign your sound clip in the Inspector

    public IEnumerator cannonImpactSound(Transform targetObject)
    {
        // Generate a random delay between 0.5 and 1.0 seconds
        float randomDelay = Random.Range(0.05f, 0.1f);
        yield return new WaitForSeconds(randomDelay); // Wait for the delay
        AudioSource.PlayClipAtPoint(cannonImpact, targetObject.position);
    }
    public IEnumerator cannonFire(Transform targetObject)
    {
        // Generate a random delay between 0.5 and 1.0 seconds
        float randomDelay = Random.Range(0.05f, 0.1f);
        yield return new WaitForSeconds(randomDelay); // Wait for the delay
        int number = Random.Range(1, 3);
        if (number == 1)
        {
            AudioSource.PlayClipAtPoint(cannonFire1, targetObject.position);

        }
        else
        {
            AudioSource.PlayClipAtPoint(cannonFire2, targetObject.position);

        }
    }

}
