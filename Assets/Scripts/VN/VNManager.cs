﻿using Ink.Runtime;
using RainbowJam2023.SO;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RainbowJam2023.VN
{
    public class VNManager : MonoBehaviour
    {
        public static bool QuickRetry = false;
        public static VNManager Instance { private set; get; }

        [SerializeField]
        private TextDisplay _display;

        [SerializeField]
        private VNCharacterInfo[] _characters;
        private VNCharacterInfo _currentCharacter;

        [SerializeField]
        private AudioSource _source;

        private Story _story;

        [SerializeField]
        private GameObject _container;

        [SerializeField]
        private TextAsset _intro;

        [SerializeField]
        private GameObject _namePanel;

        [SerializeField]
        private Image _bustImage;

        [SerializeField]
        private TMP_Text _nameText;

        private bool _isSkipEnabled;
        private float _skipTimer;
        private float _skipTimerRef = .1f;

        private Action _onDone;

        private bool _backToMenu;

        private void Awake()
        {
            Instance = this;
            _display.OnDisplayDone += (sender, e) =>
            {
                _source.Stop();
            };
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
            _backToMenu = false;
            _currentCharacter = null;
            _onDone = onDone;
            _story = new(asset.text);
            _isSkipEnabled = false;
            DisplayStory(_story.Continue());
        }

        private void DisplayStory(string text)
        {
            _source.Play();
            _container.SetActive(true);
            _namePanel.SetActive(false);
            foreach (var tag in _story.currentTags)
            {
                var s = tag.Split(' ');
                var content = string.Join(' ', s.Skip(1)).ToUpperInvariant();
                switch (s[0])
                {
                    case "speaker":
                        if (content == "none") _currentCharacter = null;
                        else _currentCharacter = _characters.FirstOrDefault(x => x.Name.ToUpperInvariant() == content);

                        Debug.Log($"[STORY] Speaker set to {_currentCharacter?.Name}");
                        break;

                    case "menu":
                        _backToMenu = true;
                        break;

                    default:
                        Debug.LogError($"Unknown story key: {s[0]}");
                        break;
                }
            }
            _display.ToDisplay = text;
            if (_currentCharacter == null)
            {
                _namePanel.SetActive(false);
                _bustImage.gameObject.SetActive(false);
            }
            else
            {
                _namePanel.SetActive(true);
                _bustImage.gameObject.SetActive(true);
                _nameText.text = _currentCharacter.DisplayName;
                _bustImage.sprite = _currentCharacter.Sprite;
            }
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
                if (_backToMenu)
                {
                    SceneManager.LoadScene("MainMenu");
                }
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
