using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeSceneBtn : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void EasyLevel()
    {
        SceneManager.LoadScene("EasyLevel");
    }
    public void MediumLevel()
    {
        SceneManager.LoadScene("MediumLevel");
    }
    public void HardLevel()
    {
        SceneManager.LoadScene("HardLevel");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
