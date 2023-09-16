using UnityEngine;
using UnityEngine.Events;

namespace RainbowJam2023.Player
{
    public class Interactible : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onAction;

        public UnityEvent OnAction => _onAction;

        public bool CanBeUsed { set; get; } = true;

        public void InvokeAll()
        {
            _onAction?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && CanBeUsed)
            {
                collision.GetComponent<PlayerController>().ActionTarget = this;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var pc = collision.GetComponent<PlayerController>();
                if (pc.ActionTarget != null && pc.ActionTarget.GetInstanceID() == GetInstanceID())
                {
                    pc.ActionTarget = null;
                }
            }
        }
    }
}
