using UnityEngine;
using UnityEngine.SceneManagement;

namespace RainbowJam2023.Menu
{
    public class MainMenu : MonoBehaviour
    {
        public void Play(string levelData)
        {
            LevelData = levelData;
            SceneManager.LoadScene("Main");
        }

        public static string LevelData = null;
    }
}
