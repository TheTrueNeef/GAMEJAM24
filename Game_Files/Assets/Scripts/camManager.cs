using UnityEngine;
using Unity.Cinemachine;
public class camManager : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject ship;
    public GameObject shopPos;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q)) //Camera Left
        {
            toggle();
            cam1.GetComponent<CinemachineCamera>().Priority = 1;
        }
        if (Input.GetKey(KeyCode.E)) //Camera Right
        {
            toggle();
            cam2.GetComponent<CinemachineCamera>().Priority = 1;
        }
        if (Input.GetKey(KeyCode.V)) 
        {
            toggle();
            cam3.GetComponent<CinemachineCamera>().Priority = 1;
        }
    }
    
    public void leaveShop()
    {
        cam1.GetComponent<CinemachineCamera>().Priority = 0;
        cam2.GetComponent<CinemachineCamera>().Priority = 0;
        cam3.GetComponent<CinemachineCamera>().Priority = 1;
        cam4.GetComponent<CinemachineCamera>().Priority = 0;
    }

    void toggle()
    {
        cam1.GetComponent<CinemachineCamera>().Priority = 0;
        cam2.GetComponent<CinemachineCamera>().Priority = 0;
        cam3.GetComponent<CinemachineCamera>().Priority = 0;
        cam4.GetComponent<CinemachineCamera>().Priority = 0;

    }
    public void win()
    {
        cam1.GetComponent<CinemachineCamera>().Priority = 0;
        cam2.GetComponent<CinemachineCamera>().Priority = 0;
        cam3.GetComponent<CinemachineCamera>().Priority = 0;
        cam4.GetComponent<CinemachineCamera>().Priority = 1;
    }
    public void shopView()
    {
        ship.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        ship.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        ship.GetComponent<ShipMovement>().sailPercentage = 0;
        ship.GetComponent<ShipMovement>().rudderRotation = 0;
        ship.transform.position = shopPos.transform.position;
        ship.transform.rotation = shopPos.transform.rotation;
        toggle();
        cam4.GetComponent<CinemachineCamera>().Priority = 1;
    }
}
