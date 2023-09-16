using Assets.Scripts.Menu;
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
        private Animator _anim;
        private SpriteRenderer _sr;

        private Interactible _actionTarget;
        public Interactible ActionTarget
        {
            set
            {
                _actionTarget = value;

                GameUIManager.Instance.ToggleActionHint(_actionTarget != null);
            }
            get
            {
                return _actionTarget;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _sr = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
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

                if (_movX != 0f)
                {
                    _sr.flipX = _movX > 0f;
                }
                _anim.SetBool("IsRunning", _movX != 0f);
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && !VNManager.Instance.IsPlayingStory)
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down, 1.01f, ~(1 << 6));
                if (hit.collider != null)
                {
                    _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                }
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && ActionTarget != null && !VNManager.Instance.IsPlayingStory)
            {
                ActionTarget.InvokeAll();
                ActionTarget = null;
            }
        }
    }
}
