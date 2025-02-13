using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Range(0f, 100f)] public float sailPercentage = 0f; // Sail percentage (0% = no speed, 100% = full speed)
    [Range(-1f, 1f)] public float rudderRotation = 0f;  // Rudder rotation (-1 = full left, 1 = full right)

    public float maxSpeed = 20f;  // Maximum speed of the ship
    public float turnSpeed = 30f; // Speed at which the ship turns

    private Rigidbody rb; // Rigidbody component
    private float currentSpeed = 0f;

    public GameObject cameraCine;
    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get user input for sail and rudder control
        UpdateSailInput();
        UpdateRudderInput();
    }

    void FixedUpdate()
    {
        // Calculate current speed based on sail percentage
        currentSpeed = (sailPercentage / 100f) * maxSpeed;

        // Move the ship forward based on its current speed
        Vector3 movement = transform.forward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate the ship based on rudder input
        Quaternion rotation = Quaternion.Euler(0f, rudderRotation * turnSpeed/2 * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }

    void UpdateSailInput()
    {
        // Increase sail percentage
        if (Input.GetKey(KeyCode.W))
        {
            sailPercentage += 40f * Time.deltaTime; // Adjust sail lowering/raising speed
            sailPercentage = Mathf.Clamp(sailPercentage, 0f, 100f);
        }

        // Decrease sail percentage
        if (Input.GetKey(KeyCode.S))
        {
            sailPercentage -= 40f * Time.deltaTime;
            sailPercentage = Mathf.Clamp(sailPercentage, 0f, 100f);
        }
        cameraCine.GetComponent<CinemachineThirdPersonFollow>().CameraDistance = 10 * (sailPercentage / 100);
    }

    void UpdateRudderInput()
    {
        // Adjust rudder rotation
        if (Input.GetKey(KeyCode.A))
        {
            rudderRotation = Mathf.MoveTowards(rudderRotation, -1f, Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rudderRotation = Mathf.MoveTowards(rudderRotation, 1f, Time.deltaTime);
        }
        else
        {
            // Automatically return rudder to center if no input
            rudderRotation = Mathf.MoveTowards(rudderRotation, 0f, Time.deltaTime);
        }
    }
}
