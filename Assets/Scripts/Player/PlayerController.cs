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

        private Vector2 _mov;
        private Rigidbody2D _rb;
        private Animator _anim;
        private SpriteRenderer _sr;

        private int _ladderCount;

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
                _rb.velocity = new(_mov.x * _info.Speed, _rb.gravityScale == 0f ? _mov.y * _info.Speed : _rb.velocity.y);

                if (_mov.x != 0f)
                {
                    _sr.flipX = _mov.x > 0f;
                }
                _anim.SetBool("IsRunning", _mov.x != 0f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                _ladderCount++;
                _rb.gravityScale = 0f;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                _ladderCount--;
                if (_ladderCount == 0)
                {
                    _rb.gravityScale = 1f;
                    _rb.velocity = new(_rb.velocity.x, 0f);
                }
            }
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
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
