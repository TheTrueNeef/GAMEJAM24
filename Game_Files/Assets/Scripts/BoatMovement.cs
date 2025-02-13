using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(Rigidbody))]
public class BoatMovementPhysics : MonoBehaviour
{
    // Amplitude and speed of up and down bobbing
    public float bobbingForceAmplitude = 10f;    // Force applied upwards (affects how high/low the boat moves)
    public float bobbingSpeed = 0.3f;            // How fast the boat bobs

    // Amplitude and speed of tilting side to side
    public float tiltSideForceAmplitude = 1f;    // Torque for tilting side to side
    public float tiltSideSpeed = 0.2f;           // Speed of side-to-side tilting

    // Amplitude and speed of tilting forward and backward
    public float tiltForwardForceAmplitude = 1f; // Torque for tilting forward/backward
    public float tiltForwardSpeed = 0.1f;        // Speed of forward/backward tilting

    private Rigidbody rb;
    private Vector3 startPos;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.Find("Terrain").GetComponent<Collider>());

        // Store the initial position to offset forces correctly
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        ApplyBobbingForce();
        ApplyTiltingForce();
    }

    void ApplyBobbingForce()
    {
        // Calculate upward bobbing force using a sine wave
        float bobbingForce = Mathf.Sin(Time.time * bobbingSpeed) * bobbingForceAmplitude;

        // Apply an upward or downward force to simulate bobbing (affects Rigidbody)
        rb.AddForce(Vector3.up * bobbingForce, ForceMode.Acceleration);
    }

    void ApplyTiltingForce()
    {
        // Tilting side to side using a sine wave
        float tiltSide = Mathf.Sin(Time.time * tiltSideSpeed) * tiltSideForceAmplitude;

        // Tilting forward and backward using a sine wave
        float tiltForward = Mathf.Sin(Time.time * tiltForwardSpeed) * tiltForwardForceAmplitude;

        // Apply torque to rotate the boat side to side (around z-axis)
        rb.AddTorque(Vector3.forward * tiltSide, ForceMode.Acceleration);

        // Apply torque to rotate the boat forward and backward (around x-axis)
        rb.AddTorque(Vector3.right * tiltForward, ForceMode.Acceleration);
    }
}
