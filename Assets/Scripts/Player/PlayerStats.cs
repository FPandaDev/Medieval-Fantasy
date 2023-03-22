using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    // --- SERIALIZED FIELDS --- //
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;

    [SerializeField] private Image img;

    // --- UNITY METHODS --- //
    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        img.fillAmount = (float)health/maxHealth;
    }
}
