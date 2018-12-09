using System;
using FlyBattle.Interface;
using UnityEngine;
using FlyBattle.Utils;

public class Bullet : MonoBehaviour, IHealthChange, IParentKnower
{
    [Header("Bullet params")] [Tooltip("Target layers")] [SerializeField]
    private LayerMask targetList;
    private LayerMask _parent;

    [Tooltip("Bullet speed")] [SerializeField]
    private float _speed = 20.0F;

    [Tooltip("Damage, a negative value")] [SerializeField]
    private float _damage = -1f;

    [SerializeField] private GameObject _hitEffect;

    public float DamageCount => _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var colLayer = collision.gameObject.layer;

        if (((targetList.value ^ _parent.value) & 1 << colLayer) == 0) return;
        
        var target = collision.gameObject.GetComponent<IHealth>();
        if (target != null)
        {
            Damage(target);
        }

        var position = transform.position;
        _hitEffect.Spawn(position, Quaternion.FromToRotation(Vector3.up, position));
        gameObject.Recycle();
    }

    public void Damage(IHealth target)
    {
        target.OnTakeDamage(this);
    }

    public void SetParent(GameObject parent)
    {
        if (parent != null) _parent = 1 << parent.layer;
    }
}