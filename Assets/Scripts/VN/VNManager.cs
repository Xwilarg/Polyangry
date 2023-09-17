using Ink.Runtime;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RainbowJam2023.VN
{
    public class VNManager : MonoBehaviour
    {
        public static bool QuickRetry = false;
        public static VNManager Instance { private set; get; }

        [SerializeField]
        private TextDisplay _display;

        private Story _story;

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private TextAsset _intro;

        [SerializeField]
        private GameObject _namePanel;

        [SerializeField]
        private TMP_Text _nameText;

        private bool _isSkipEnabled;
        private float _skipTimer;
        private float _skipTimerRef = .1f;

        private Action _onDone;

        private void Awake()
        {
            Instance = this;
        }

        public bool IsPlayingStory => _container.activeInHierarchy;

        private void Start()
        {
            ShowStory(_intro, null);
        }

        private void Update()
        {
            if (_isSkipEnabled)
            {
                _skipTimer -= Time.deltaTime;
                if (_skipTimer < 0)
                {
                    _skipTimer = _skipTimerRef;
                    DisplayNextDialogue();
                }
            }
        }

        public void ShowStory(TextAsset asset, Action onDone)
        {
            Debug.Log($"[STORY] Playing {asset.name}");
            _onDone = onDone;
            _story = new(asset.text);
            _isSkipEnabled = false;
            DisplayStory(_story.Continue());
        }

        private void DisplayStory(string text)
        {
            _container.SetActive(true);
            _namePanel.SetActive(false);
            foreach (var tag in _story.currentTags)
            {
                var s = tag.Split(' ');
                var content = string.Join(' ', s.Skip(1));
                switch (s[0])
                {
                    // TODO
                }
            }
            _display.ToDisplay = text;
        }

        public void DisplayNextDialogue()
        {
            if (!_container.activeInHierarchy)
            {
                return;
            }
            if (!_display.IsDisplayDone)
            {
                // We are slowly displaying a text, force the whole display
                _display.ForceDisplay();
            }
            else if (_story.canContinue && // There is text left to write
                !_story.currentChoices.Any()) // We are not currently in a choice
            {
                DisplayStory(_story.Continue());
            }
            else if (!_story.canContinue && !_story.currentChoices.Any())
            {
                _container.SetActive(false);
                _onDone?.Invoke();
            }
        }

        public void ToggleSkip(bool value)
            => _isSkipEnabled = value;

        public void OnNextDialogue(InputAction.CallbackContext value)
        {
            if (value.performed)
            {
                DisplayNextDialogue();
            }
        }
    }
}
