using Mirror;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerHealth))]
public class HealthUI : NetworkBehaviour 
{
    [SerializeField]private Image _healthBar;
    private PlayerHealth _target;

    private void Start() {
        _target = GetComponent<PlayerHealth>();
        _target.OnValueChanged += Render;
    }

    private void Render(int currentHealth,int maxHealth){
        _healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }

}