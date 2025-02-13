using UnityEngine;

public class escapeMenu : MonoBehaviour
{
    public GameObject scapeMenu;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            scapeMenu.SetActive(true);
        }
    }
}
