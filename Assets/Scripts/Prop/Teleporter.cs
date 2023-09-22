using RainbowJam2023;
using RainbowJam2023.Prop;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Teleporter : AColorListener
{
    [SerializeField]
    private TeleporterInternal[] _teleporters;
    private ParticleSystem[] _pSystems;

    private Color _colorRGB;

    public bool CanBeUsed { set; get; } = true;
    public bool IsRightColor { set; get; } = false;
    public IEnumerator Reload()
    {
        CanBeUsed = false;
        yield return new WaitForSeconds(1f);
        CanBeUsed = true;
    }

    public override bool IsOffBehaviorUsingTransparency => false;

    private void Awake()
    {
        _pSystems = _teleporters.Select(x => x.GetComponent<ParticleSystem>()).ToArray();
        int i = 0;
        foreach (var t in _teleporters)
        {
            t.Teleporter = this;
            _teleporters[i].Next = (i == _teleporters.Length - 1 ? _teleporters[0] : _teleporters[i + 1]).transform;
            i++;
        }
        foreach (var ps in _pSystems)
        {
            _colorRGB = GameManager.ColorToRGB(_color);
            ps.startColor = new(
                r: _colorRGB.r,
                g: _colorRGB.g,
                b: _colorRGB.b,
                a: .2f
            );
        }
    }

    private void Start()
    {
        GameManager.Instance.RegisterListener(this);
    }

    public override void UpdateFromColor(GameManager.Color baseColor)
    {
        if (_color == baseColor)
        {
            foreach (var ps in _pSystems)
            {
                ps.startColor = _colorRGB;
            }
            IsRightColor = true;
        }
        else
        {
            foreach (var ps in _pSystems)
            {
                ps.startColor = new(
                    r: _colorRGB.r,
                    g: _colorRGB.g,
                    b: _colorRGB.b,
                    a: .2f
                );
            }
            IsRightColor = false;
        }
    }
}
