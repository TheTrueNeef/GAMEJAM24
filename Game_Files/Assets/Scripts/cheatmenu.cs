using UnityEngine;

public class cheatmenu : MonoBehaviour
{
    public GameObject cheatsMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J)) 
        {
            cheatsMenu.SetActive(true);
        }
    }
}
