namespace RainbowJam2023.Prop
{
    public class ColorSwitch : Switch
    {
        private void Awake()
        {
            AwakeSwitch();

            _interactible.OnAction.AddListener(new(() =>
            {
                GameManager.Instance.SetCurrentColor(_color);
            }));
        }

        private void Start()
        {
            StartSwitch();
        }
    }
}
