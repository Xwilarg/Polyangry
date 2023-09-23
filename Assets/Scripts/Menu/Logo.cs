using UnityEngine.EventSystems;

namespace UnityEngine.Menu
{
    public class Logo : MonoBehaviour, IPointerClickHandler
    {
        private float _timer;

        [SerializeField]
        private AudioClip[] _clips;

        private bool _dir;

        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_timer <= 0f)
            {
                _timer = 360f;
                if (Random.Range(0, 2) == 0)
                {
                    _dir = true;
                    _source.PlayOneShot(_clips[0], .5f);
                }
                else
                {
                    _dir = false;
                    _source.PlayOneShot(_clips[1], .5f);
                }
            }
        }

        private void Update()
        {
            if (_timer > 0f)
            {
                _timer -= Time.deltaTime * 250f;
                if (_dir) transform.rotation = Quaternion.Euler(0f, 0f, 360f - Mathf.Clamp(_timer, 0f, 360f));
                else transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(_timer, 0f, 360f));
            }
        }
    }
}
