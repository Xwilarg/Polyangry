using Assets.Scripts.Menu;
using RainbowJam2023.Prop;
using RainbowJam2023.SO;
using RainbowJam2023.VN;
using Unity.VisualScripting;
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

        private Vector2 _startPos;

        private bool _canClimb;

        private bool _canUseActionTarget;
        private Interactible _actionTarget;
        public Interactible ActionTarget
        {
            set
            {
                _actionTarget = value;
                _canUseActionTarget = true;

                if (_actionTarget == null)
                {
                    GameUIManager.Instance.RemoveAllHints();
                }
                else
                {
                    var colorListener = _actionTarget.GetComponent<AColorListener>();
                    if (colorListener != null)
                    {
                        if (GameManager.Instance.HasColor(colorListener.Color))
                        {
                            GameUIManager.Instance.DisplayActionHint();
                        }
                        else
                        {
                            GameUIManager.Instance.DisplayColorNotAvailableHint();
                            _canUseActionTarget = false;
                        }
                    }
                    else
                    {
                        GameUIManager.Instance.DisplayActionHint();
                    }
                }
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

        private void Start()
        {
            transform.position = GameObject.FindGameObjectWithTag("StartingPos").transform.position;
            _startPos = transform.position;
        }

        private void FixedUpdate()
        {
            if (VNManager.Instance.IsPlayingStory)
            {
                _rb.velocity = new(0f, _rb.velocity.y);
            }
            else
            {
                // Calculate speed
                Vector2 dir;
                if (_rb.gravityScale == 0f) // We are currently climbing
                {
                    dir = _mov.normalized * _info.Speed / 2f;
                    _anim.SetBool("MidAir", false);
                }
                else if (_canClimb && _mov.y != 0f) // We can climb and press up/down
                {
                    _rb.gravityScale = 0f;
                    dir = _mov.normalized * _info.Speed / 2f;
                    _anim.SetBool("MidAir", false);
                }
                else // On the floor
                {
                    dir = new(_mov.x * _info.Speed, _rb.velocity.y);
                    _anim.SetBool("MidAir", !IsOnFloor(out var _));
                }
                _rb.velocity = dir;

                _anim.SetBool("IsClimbing", _rb.gravityScale == 0f);

                if (_mov.x != 0f)
                {
                    _sr.flipX = _mov.x > 0f;
                }
                _anim.SetBool("IsRunning", _mov.magnitude != 0f);
            }

            if (transform.position.y < -5f)
            {
                transform.position = _startPos;
                _rb.velocity = Vector2.zero;
            }

            _anim.SetBool("IsGoingDown", _rb.velocity.y < 0f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                _ladderCount++;
                _canClimb = true;
            }
            else if (collision.CompareTag("EasterEgg"))
            {
                GameManager.Instance.IsInEasterEgg = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Ladder"))
            {
                _ladderCount--;
                if (_ladderCount == 0)
                {
                    _canClimb = false;
                    if (_rb.gravityScale == 0f)
                    {
                        _rb.gravityScale = 1f;
                        _rb.velocity = new(_rb.velocity.x, 0f);
                    }
                }
            }
            else if (collision.CompareTag("EasterEgg"))
            {
                GameManager.Instance.IsInEasterEgg = false;
            }
        }

        private bool IsOnFloor(out RaycastHit2D hit)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, 1.01f, ~(1 << 6));
            return hit.collider != null;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && !VNManager.Instance.IsPlayingStory)
            {
                if (IsOnFloor(out var _))
                {
                    _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                }
            }
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && ActionTarget != null && !VNManager.Instance.IsPlayingStory && _canUseActionTarget)
            {
                ActionTarget.InvokeAll();
                ActionTarget = null;
            }
        }

        private void OnEasterInternal(InputAction.CallbackContext value, int index)
        {
            if (value.performed)
            {
                GameManager.Instance.PlaySound(index);
            }
        }
        public void OnEaster1(InputAction.CallbackContext value) => OnEasterInternal(value, 0);
        public void OnEaster2(InputAction.CallbackContext value) => OnEasterInternal(value, 1);
        public void OnEaster3(InputAction.CallbackContext value) => OnEasterInternal(value, 2);
        public void OnEaster4(InputAction.CallbackContext value) => OnEasterInternal(value, 3);
        public void OnEaster5(InputAction.CallbackContext value) => OnEasterInternal(value, 4);
        public void OnEaster6(InputAction.CallbackContext value) => OnEasterInternal(value, 5);
        public void OnEaster7(InputAction.CallbackContext value) => OnEasterInternal(value, 6);
        public void OnEaster8(InputAction.CallbackContext value) => OnEasterInternal(value, 7);
        public void OnEaster9(InputAction.CallbackContext value) => OnEasterInternal(value, 8);
        public void OnEaster10(InputAction.CallbackContext value) => OnEasterInternal(value, 9);
        public void OnEaster11(InputAction.CallbackContext value) => OnEasterInternal(value, 10);
        public void OnEaster12(InputAction.CallbackContext value) => OnEasterInternal(value, 11);
    }
}
