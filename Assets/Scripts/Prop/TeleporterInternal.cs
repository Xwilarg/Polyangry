using UnityEngine;

namespace RainbowJam2023.Prop
{
    public class TeleporterInternal : MonoBehaviour
    {
        public Teleporter Teleporter { set; get; }
        public Transform Next { set; get; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && Teleporter.IsRightColor && Teleporter.CanBeUsed)
            {
                StartCoroutine(Teleporter.Reload());
                collision.transform.position = Next.position;
            }
        }
    }
}
