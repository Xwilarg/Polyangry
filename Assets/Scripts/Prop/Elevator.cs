﻿using UnityEngine;

namespace RainbowJam2023.Prop
{
    public class Elevator : AColorListener
    {
        [SerializeField]
        private Transform[] _nodes;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private SpriteRenderer _colorDisplay;

        private AudioSource _source;

        private int _nodeIndex;

        private const float _minDist = .1f;

        private Rigidbody2D _rb;

        public override bool IsOffBehaviorUsingTransparency => false;

        private bool _isActive;

        public override void UpdateFromColor(GameManager.Color baseColor)
        {
            base.UpdateFromColor(baseColor);
            _isActive = baseColor == _color;
            if (_isActive)
            {
                _source.Play();
            }
            else
            {
                _source.Stop();
            }

            if (!_isActive)
            {
                _rb.velocity = Vector2.zero;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _source = GetComponent<AudioSource>();
            InitColorDisplay(_colorDisplay);
        }

        private void Start()
        {
            GameManager.Instance.RegisterListener(this);
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                _rb.velocity = (_nodes[_nodeIndex].position - transform.position).normalized * _speed;

                if (Vector2.Distance(transform.position, _nodes[_nodeIndex].position) < _minDist)
                {
                    if (_nodeIndex == _nodes.Length - 1)
                    {
                        _nodeIndex = 0;
                    }
                    else
                    {
                        _nodeIndex++;
                    }
                }
            }
        }
    }
}
