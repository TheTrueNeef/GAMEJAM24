using UnityEngine;

public class shop : MonoBehaviour
{
    public void buySpeed()
    {
        if (GetComponent<PlayerHealth>().gold >= Mathf.Round(Mathf.Pow(1.4f,GetComponent<ShipMovement>().maxSpeed -10)) && GetComponent<ShipMovement>().maxSpeed < 50)
        {
            GetComponent<ShipMovement>().maxSpeed += 1;
            GetComponent<ShipMovement>().turnSpeed += 1;
            GetComponent<PlayerHealth>().gold -= Mathf.Round(Mathf.Pow(1.4f, GetComponent<ShipMovement>().maxSpeed - 10));
        }
    }
    public void buyRange()
    {
        if (GetComponent<PlayerHealth>().gold >= Mathf.Round(Mathf.Pow(1.4f, (GetComponent<ShipCannon>().fireForce / 100) - 10)/2) && GetComponent<ShipCannon>().fireForce < 8000)
        {
            GetComponent<ShipCannon>().fireForce += 200;
            GetComponent<PlayerHealth>().gold -= Mathf.Round(Mathf.Pow(1.4f, (GetComponent<ShipCannon>().fireForce / 100) - 10)/2);
        }
    }

    public void buyRepair()
    {
        if (GetComponent<PlayerHealth>().gold >= Mathf.Round(Mathf.Pow(1.4f, (GetComponent<ShipRepair>().repairRate) - 5)) && GetComponent<ShipRepair>().repairRate < 30)
        {
            GetComponent<ShipRepair>().repairRate += 1;
            GetComponent<PlayerHealth>().waterFillRate -= 0.2f;
            GetComponent<PlayerHealth>().gold -= Mathf.Round(Mathf.Pow(1.4f, (GetComponent<ShipRepair>().repairRate) - 5));
        }
    }

    public void buyReload()
    {
        if (GetComponent<PlayerHealth>().gold >= 1000 && GetComponent<ShipCannon>().fireRate > 1)
        {
            GetComponent<ShipCannon>().fireRate -= 0.5f;
            GetComponent<PlayerHealth>().gold -= 1000;
        }
    }

    public void buyShips()
    {
        if (GetComponent<PlayerHealth>().gold >= 10000 && GetComponent<PlayerHealth>().mana < 20)
        {
            GetComponent <PlayerHealth>().mana++;
            GetComponent<PlayerHealth>().maxShips++;
            GetComponent<PlayerHealth>().gold -= 10000;
        }
    }
}
