using System;
using System.Collections;
using FlyBattle.Utils;
using FlyBattle.Interface;
using UnityEngine;

public class PlaneCannon : MonoBehaviour, IWeapon
{
    [Header("Типы снарядов")] public Bullet[] BulletType;

    [Header("Выбранный тип снаряда")] [SerializeField]
    private Bullet _currentBullet;

    [SerializeField] private Transform _bulletSpawnPoint;
    private bool _canShooting = false;
    [SerializeField]
    private bool _reLoad;
    
    [SerializeField]
    private float timeReload = 2f;
    
    public event Action Shoot = delegate { };
    public bool GetReload => _reLoad;

    private void Start()
    {
        ChangeBulletType(0);
    }

    private IEnumerator Reload(float time)
    {
        _reLoad = true;
        yield return new WaitForSeconds(time);
        _reLoad = false;
    }

    public void ChangeBulletType(int value)
    {
        var num = Mathf.Clamp(value, 0, BulletType.Length);
        _currentBullet = BulletType[num];
    }

    public void SetShootActive(bool flag)
    {
        _canShooting = flag;
    }

    public void OnShoot()
    {
        if (!_canShooting) return;
        if (_reLoad) return;
        var rb2DBullet = _currentBullet.gameObject.Spawn(_bulletSpawnPoint.position).GetComponent<Rigidbody2D>();
        rb2DBullet.GetComponent<IParentKnower>().SetParent(gameObject);
        rb2DBullet.AddForce(_bulletSpawnPoint.right * 1.2f, ForceMode2D.Impulse);
        

        Shoot?.Invoke();
        StartCoroutine(Reload(timeReload));
    }

    private void OnDestroy()
    {
        Shoot = null;
    }
}