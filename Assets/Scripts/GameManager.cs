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

        public Color CurrentColor { private set; get; }

        private readonly List<AColorListener> _colorListeners = new();

        private void Awake()
        {
            Instance = this;
            SceneManager.LoadScene("Map", LoadSceneMode.Additive);
        }

        public void RegisterListener(AColorListener l)
        {
            _colorListeners.Add(l);
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
                _ => throw new NotImplementedException()
            };
        }

        public enum Color
        {
            NONE,
            RED,
            YELLOW
        }
    }
}
