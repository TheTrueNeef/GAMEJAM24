using UnityEngine;

public class ShipRocking : MonoBehaviour
{
    public float rollAmount = 5f; // Maximum tilt angle (side to side)
    public float pitchAmount = 2f; // Maximum pitch angle (forward/backward)
    public float rollSpeed = 0.5f; // Speed of side-to-side rocking
    public float pitchSpeed = 0.3f; // Speed of forward/backward rocking

    private float rollOffset; // Offset for the roll angle
    private float pitchOffset; // Offset for the pitch angle

    void Start()
    {
        // Initialize offsets with random values for variation in motion
        rollOffset = Random.Range(0f, Mathf.PI * 2f);
        pitchOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        // Calculate the rocking (side-to-side roll)
        float rollAngle = rollAmount * Mathf.Sin(Time.time * rollSpeed + rollOffset);

        // Calculate the pitching (forward/backward pitch)
        float pitchAngle = pitchAmount * Mathf.Sin(Time.time * pitchSpeed + pitchOffset);

        // Apply the rotation to the ship
        transform.localRotation = Quaternion.Euler(pitchAngle, 0f, rollAngle);
    }
}
