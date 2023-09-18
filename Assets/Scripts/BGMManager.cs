using UnityEngine;

namespace RainbowJam2023
{
    public class BGMManager : MonoBehaviour
    {
        public static BGMManager Instance { private set; get; }

        [SerializeField]
        private AudioClip[] _bgms;

        private int _index;

        private AudioSource[] _source;

        private float _timer;
        private float _timerMax = 10f;

        private void Awake()
        {
            Instance = this;

            _source = GetComponents<AudioSource>();
            _timer = _timerMax;

            _source[1].clip = _bgms[0];
            _source[1].Play();
        }

        public void PlayNext()
        {
            _index++;
            _timer = 0f;

            var duration = _source[1].time;

            _source[0].clip = _bgms[_index - 1];
            _source[1].clip = _bgms[_index];

            _source[0].time = duration;
            _source[1].time = duration;

            _source[0].volume = 1f;
            _source[1].volume = 0f;

            _source[0].Play();
            _source[1].Play();
        }

        private void Update()
        {
            if (_timer < _timerMax)
            {
                _timer += Mathf.Clamp(Time.deltaTime, 0f, _timerMax);
                _source[1].volume = _timer / _timerMax;
                _source[0].volume = 1f - _source[1].volume;
            }
        }
    }
}
