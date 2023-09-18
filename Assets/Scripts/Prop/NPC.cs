using RainbowJam2023.VN;
using UnityEngine;

namespace RainbowJam2023.Prop
{
    public class NPC : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _story;

        [SerializeField]
        private GameManager.Color _color;

        public void UnlockNPC()
        {
            BGMManager.Instance.PlayNext();
            VNManager.Instance.ShowStory(_story, () =>
            {
                GameManager.Instance.UnlockColor(_color);
                Destroy(gameObject);
            });
        }
    }
}
