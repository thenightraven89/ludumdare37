using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{
    [SerializeField]
    private ParticleSystem _ps;

    [SerializeField]
    private int _ammoCapacity;

    [SerializeField]
    private float _loadTime;

    [SerializeField]
    private float _recoilMagnitude;

    private float _currentLoadTime;

    private int _ammo;

    public float Use()
    {
        if (_ammo > 0 && _currentLoadTime == 0)
        {
            _currentLoadTime = _loadTime;
            _ammo--;
            _ps.Emit(1);
            return _recoilMagnitude;
        }

        return 0f;
    }

    public void Reload(int ammo)
    {
        _ammo = Mathf.Clamp(_ammo + ammo, 0, _ammoCapacity);
    }

    private void Update()
    {
        _currentLoadTime = Mathf.Clamp(_currentLoadTime - Time.deltaTime, 0f, _loadTime);
    }
}