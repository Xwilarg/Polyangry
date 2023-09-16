using UnityEngine;

namespace RainbowJam2023.Prop
{
    public class ColorBlock : AColorListener
    {
        private BoxCollider2D _coll;

        public override bool IsOffBehaviorUsingTransparency => true;

        private void Awake()
        {
            InitColorDisplay(GetComponent<SpriteRenderer>());
            _coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            GameManager.Instance.RegisterListener(this);
        }

        public override void UpdateFromColor(GameManager.Color baseColor)
        {
            base.UpdateFromColor(baseColor);

            _coll.enabled = baseColor == _color;
        }
    }
}
