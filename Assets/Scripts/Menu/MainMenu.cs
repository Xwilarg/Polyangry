using UnityEngine;
using UnityEngine.SceneManagement;

namespace RainbowJam2023.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
