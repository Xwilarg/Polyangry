using System.Linq;
using UnityEngine;

namespace RainbowJam2023.Prop
{
    public class ColorSwitch : Switch
    {
        private void Awake()
        {
            AwakeSwitch();

            _interactible.OnAction.AddListener(new(() =>
            {
                if (_clips.Any())
                {
                    _source.clip = _clips[Random.Range(0, _clips.Length)];
                    _source.Play();
                }
                GameManager.Instance.SetCurrentColor(_color);
            }));
        }

        private void Start()
        {
            StartSwitch();
        }
    }
}
