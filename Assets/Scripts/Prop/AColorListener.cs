using UnityEngine;

namespace RainbowJam2023.Prop
{
    public abstract class AColorListener : MonoBehaviour
    {
        private SpriteRenderer _colorRepresentationTarget;

        private Color _colorOn, _colorOff;

        [SerializeField]
        protected GameManager.Color _color;

        public abstract bool IsOffBehaviorUsingTransparency { get; }

        public virtual void UpdateFromColor(GameManager.Color baseColor)
        {
            _colorRepresentationTarget.color = baseColor == _color ? _colorOn : _colorOff;
        }

        public void InitColorDisplay(SpriteRenderer sr)
        {
            _colorRepresentationTarget = sr;


            _colorOn = GameManager.ColorToRGB(_color);

            if (IsOffBehaviorUsingTransparency)
            {
                _colorOff = new(
                    r: _colorOn.r,
                    g: _colorOn.g,
                    b: _colorOn.b,
                    a: .5f
                );
            }
            else
            {
                _colorOff = new(
                    r: Mathf.Clamp01(_colorOn.r - .2f),
                    g: Mathf.Clamp01(_colorOn.g - .2f),
                    b: Mathf.Clamp01(_colorOn.b - .2f)
                );
            }

            _colorRepresentationTarget.color = _colorOff;
        }
    }
}
