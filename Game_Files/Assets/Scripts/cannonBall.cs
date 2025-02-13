using System;
using UnityEngine;

public class cannonBall : MonoBehaviour
{
    public GameObject Splash;
    public GameObject Splinter;
    private void Start()
    {
        // Destroy the projectile after a specified lifetime
        Destroy(gameObject, 5f);
        if (transform.position.y < 0 )
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.y < 0)
        {
            GameObject splasheffect = Instantiate(Splash, transform.position, Quaternion.identity);
            splasheffect.transform.position = transform.position;
            DestroyImmediate(gameObject);
        }
    }
    
    public void shotAt()
    {
        GameObject splint = Instantiate(Splinter, transform.position, Quaternion.identity);
        GameObject splint1 = Instantiate(Splinter, transform.position, Quaternion.identity);
        GameObject splint2 = Instantiate(Splinter, transform.position, Quaternion.identity);
        GameObject splint3 = Instantiate(Splinter, transform.position, Quaternion.identity);
        StartCoroutine(Camera.main.GetComponent<audioManagerCam>().cannonImpactSound(transform));
        GetComponent<Collider>().enabled = false;
    }
}
