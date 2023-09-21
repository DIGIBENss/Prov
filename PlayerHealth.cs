using UnityEngine;
using Mirror;
using System;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    public Action<int,int> OnValueChanged;
    public int Health => _health;
    [SerializeField,SyncVar(hook = nameof(OnHealthChanged))] private int _health = 100;
    [SerializeField]private int _maxHealth = 100;

    public virtual void TakeDamage(int damage)
    {
        if (!isServer) return;
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        OnValueChanged?.Invoke(_health,_maxHealth);
    }

    public void TakeHeal(int value)
    {
        if (!isServer) return;
        if(_health + value <= _maxHealth){
            _health += value;
        }
        else _health = _maxHealth;
    }
}
