using RainbowJam2023.SO;
using RainbowJam2023.VN;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RainbowJam2023.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private float _movX;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (VNManager.Instance.IsPlayingStory)
            {
                _rb.velocity = new(0f, _rb.velocity.y);
            }
            else
            {
                _rb.velocity = new(_movX * _info.Speed, _rb.velocity.y);
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
        }
    }
}
