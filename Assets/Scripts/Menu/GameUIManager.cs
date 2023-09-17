using UnityEngine;

namespace Assets.Scripts.Menu
{
    public class GameUIManager : MonoBehaviour
    {
        public static GameUIManager Instance { private set; get; }

        [SerializeField]
        private GameObject _pressEHint, _colorNotAvailableText;

        private void Awake()
        {
            Instance = this;
        }

        public void DisplayActionHint()
        {
            _colorNotAvailableText.SetActive(false);
            _pressEHint.SetActive(true);
        }

        public void DisplayColorNotAvailableHint()
        {
            _colorNotAvailableText.SetActive(true);
            _pressEHint.SetActive(false);
        }

        public void RemoveAllHints()
        {
            _colorNotAvailableText.SetActive(false);
            _pressEHint.SetActive(false);
        }
    }
}
