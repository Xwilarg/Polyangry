using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager Instance { private set; get; }

        [SerializeField]
        private GameObject _pressEHint;

        private void Awake()
        {
            Instance = this;
        }

        public void ToggleActionHint(bool value)
        {
            _pressEHint.SetActive(value);
        }
    }
}
