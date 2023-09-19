namespace RainbowJam2023.Prop
{
    public class ColorSwitch : Switch
    {
        private void Awake()
        {
            AwakeSwitch();

            _interactible.OnAction.AddListener(new(() =>
            {
                _source.Play();
                GameManager.Instance.SetCurrentColor(_color);
            }));
        }

        private void Start()
        {
            StartSwitch();
        }
    }
}
