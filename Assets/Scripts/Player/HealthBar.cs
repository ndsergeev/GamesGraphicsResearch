using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Transform _mainCamera;
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Awake()
    {
        if (!(Camera.main is null)) _mainCamera = Camera.main.gameObject.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.forward);
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void ResetHealthToMax()
    {
        slider.value = slider.maxValue;
        fill.color = gradient.Evaluate(1f);
    }
}
