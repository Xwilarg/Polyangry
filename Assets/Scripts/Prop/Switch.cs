using RainbowJam2023.Player;
using UnityEngine;

namespace RainbowJam2023.Prop
{
    [RequireComponent(typeof(Interactible))]
    public class Switch : MonoBehaviour
    {
        [SerializeField]
        private GameObject _onPos, _offPos;

        [SerializeField]
        private SpriteRenderer _colorDisplay;

        [SerializeField]
        private GameManager.Color _color;

        private Color _colorOn, _colorOff;

        private Interactible _interactible;

        public void UpdateFromColor(GameManager.Color baseColor)
        {
            var amITarget = baseColor == _color;

            _onPos.SetActive(amITarget);
            _offPos.SetActive(!amITarget);
            _colorDisplay.color = amITarget ? _colorOn : _colorOff;
            _interactible.CanBeUsed = !amITarget;
        }

        private void Awake()
        {
            _onPos.SetActive(false);

            _colorOn = GameManager.ColorToRGB(_color);
            _colorOff = new(
                r: Mathf.Clamp01(_colorOn.r - .2f),
                g: Mathf.Clamp01(_colorOn.g - .2f),
                b: Mathf.Clamp01(_colorOn.b - .2f)
            );

            _colorDisplay.color = _colorOff;

            _interactible = GetComponent<Interactible>();
            _interactible.OnAction.AddListener(new(() =>
            {
                GameManager.Instance.SetCurrentColor(_color);
            }));
        }

        private void Start()
        {
            GameManager.Instance.RegisterSwitch(this);
        }
    }
}
