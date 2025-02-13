using UnityEngine;

public class summonedShip : MonoBehaviour
{
    public float summonSpeed = 15f;           // Speed of ghost ship rising
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

// Update is called once per frame
void Update()
    {
        if (transform.position.y < 0)
        {
            // Move the ghost ship upwards until it reaches the target y position
            float step = summonSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position;
            newPosition.y = Mathf.MoveTowards(newPosition.y, 0, step);
            transform.position = newPosition;

            // Stop summoning when target y position is reached
            if (newPosition.y >= 0)
            {
                GetComponent<Collider>().enabled = true;
                GameObject.Find("Player").GetComponent<SummonShip>().isSummoning = false;
            }
        }
    }
}
