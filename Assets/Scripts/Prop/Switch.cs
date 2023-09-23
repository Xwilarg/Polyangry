using RainbowJam2023.Player;
using UnityEngine;

namespace RainbowJam2023.Prop
{
    [RequireComponent(typeof(Interactible))]
    public abstract class Switch : AColorListener
    {
        [SerializeField]
        private GameObject _onPos, _offPos;

        [SerializeField]
        private SpriteRenderer _colorDisplay;

        [SerializeField]
        protected AudioClip[] _clips;

        protected Interactible _interactible;
        protected AudioSource _source;

        public override bool IsOffBehaviorUsingTransparency => false;

        public override void UpdateFromColor(GameManager.Color baseColor)
        {
            base.UpdateFromColor(baseColor);

            var amITarget = baseColor == _color;

            _onPos.SetActive(amITarget);
            _offPos.SetActive(!amITarget);
            _interactible.CanBeUsed = !amITarget;
        }

        protected void AwakeSwitch()
        {
            _onPos.SetActive(false);

            InitColorDisplay(_colorDisplay);

            _interactible = GetComponent<Interactible>();
            _source = GetComponent<AudioSource>();
        }

        protected void StartSwitch()
        {
            GameManager.Instance.RegisterListener(this);
        }
    }
}
