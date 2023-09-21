using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
public class DummyHealth : MonoBehaviour
{
    private PlayerHealth _target;

    private void Start() {
        _target = GetComponent<PlayerHealth>();
        _target.OnValueChanged += RegisterHeal;
    }

    private void RegisterHeal(int current, int max){
        _target.TakeHeal(max - current);
    }

    private void OnDestroy() {
        _target.OnValueChanged -= RegisterHeal;
    }
}
