﻿using RainbowJam2023.Menu;
using RainbowJam2023.Prop;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RainbowJam2023
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Color CurrentColor { private set; get; } = Color.BLACK;

        private readonly List<AColorListener> _colorListeners = new();
        private readonly List<Color> _availableColors = new() { Color.RED };

        public bool IsInEasterEgg { set; get; }

        private AudioSource _source;

        [SerializeField]
        private AudioClip[] _easterSounds;

        public bool HasColor(Color c)
            => _availableColors.Contains(c);

        public void UnlockColor(Color c)
        {
            _availableColors.Add(c);
        }

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            Instance = this;
            SceneManager.LoadScene(MainMenu.LevelData ?? "SampleMap", LoadSceneMode.Additive);
        }

        public void PlaySound(int index)
        {
            if (IsInEasterEgg)
            {
                _source.PlayOneShot(_easterSounds[index]);
            }
        }

        public void RegisterListener(AColorListener l)
        {
            _colorListeners.Add(l);
            l.UpdateFromColor(CurrentColor);
        }

        public void SetCurrentColor(Color c)
        {
            CurrentColor = c;
            foreach (var cl in _colorListeners)
            {
                cl.UpdateFromColor(c);
            }
        }

        public static UnityEngine.Color ColorToRGB(Color c)
        {
            return c switch
            {
                Color.NONE => UnityEngine.Color.white,
                Color.RED => UnityEngine.Color.red,
                Color.YELLOW => UnityEngine.Color.yellow,
                Color.BLUE => UnityEngine.Color.blue,
                Color.BLACK => UnityEngine.Color.black,
                Color.GREY => UnityEngine.Color.grey,
                _ => throw new NotImplementedException()
            };
        }

        public enum Color
        {
            NONE,
            RED,
            YELLOW,
            BLUE,
            BLACK,
            GREY
        }
    }
}
