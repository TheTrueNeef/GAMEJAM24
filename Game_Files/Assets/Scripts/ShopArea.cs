using UnityEngine;
using UnityEngine.UI;

public class ShopArea : MonoBehaviour
{
    public GameObject shopButton; // Reference to the button UI
    public GameObject shopMenu;   // Reference to the shop menu UI panel
    public string playerTag = "PlayerShip"; // The player's tag to identify the player

    private void Start()
    {
        // Make sure the button and shop menu are hidden at the start
        shopButton.SetActive(true);
        shopMenu.SetActive(false);
    }

    // When the player enters the shop area (trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            // Show the shop button when the player enters the area
            shopButton.SetActive(true);
        }
    }
    

    // When the player exits the shop area (trigger)
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            // Hide the shop button when the player exits the area
            shopButton.SetActive(false);
            // Hide the shop menu if it's open
            shopMenu.SetActive(false);
            other.GetComponent<camManager>().leaveShop();
        }
    }

    // Call this method when the shop button is clicked
    public void OpenShopMenu()
    {
        // Show the shop menu
        shopMenu.SetActive(true);
    }

    // Call this method to close the shop menu (optional, add a close button in UI)
    public void CloseShopMenu()
    {
        // Hide the shop menu
        shopMenu.SetActive(false);
    }
}
