using RainbowJam2023.Player;
using UnityEngine;

namespace RainbowJam2023.Prop
{
    [RequireComponent(typeof(Interactible))]
    public class Switch : AColorListener
    {
        [SerializeField]
        private GameObject _onPos, _offPos;

        [SerializeField]
        private SpriteRenderer _colorDisplay;

        private Interactible _interactible;

        public override bool IsOffBehaviorUsingTransparency => false;

        public override void UpdateFromColor(GameManager.Color baseColor)
        {
            base.UpdateFromColor(baseColor);

            var amITarget = baseColor == _color;

            _onPos.SetActive(amITarget);
            _offPos.SetActive(!amITarget);
            _interactible.CanBeUsed = !amITarget;
        }

        private void Awake()
        {
            _onPos.SetActive(false);

            InitColorDisplay(_colorDisplay);

            _interactible = GetComponent<Interactible>();
            _interactible.OnAction.AddListener(new(() =>
            {
                GameManager.Instance.SetCurrentColor(_color);
            }));
        }

        private void Start()
        {
            GameManager.Instance.RegisterListener(this);
        }
    }
}
