using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public delegate void EmptyEvent();
    public event EmptyEvent OnHealthLost;
    public event EmptyEvent OnHealthGain;
    public event EmptyEvent OnDeath;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [Header("Optional common things")]
    [SerializeField] private ParticleSystem healthGainParticles;
    [SerializeField] private AudioClip healthGainAudio;

    [SerializeField] private ParticleSystem healthLostParticles;
    [SerializeField] private AudioClip healthLostAudio;

    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private AudioClip deathAudio;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    /// <summary>
    /// The health as a percentage value between 0 to 1
    /// </summary>
    public float NormalizedHealth => (currentHealth / maxHealth);

    private void Awake() {
        if (healthGainParticles != null || healthGainAudio != null) {
            OnHealthGain += HealthGained;
        }
        
        if (healthLostParticles != null || healthLostAudio != null) {
            OnHealthLost += HealthLost;
        }
        
        if (deathParticles != null || deathAudio != null) {
            OnDeath += Death;
        }
    }

    public void LoadSavedHealth(SavedHealth savedHealth) {
        currentHealth = savedHealth.currentHealth;
        maxHealth = savedHealth.maxHealth;
    }

    public void ModifyHealth(float amount) {
        float previousHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        // Basically an == but comparing floats as == is bad due to their precision
        if (Math.Abs(currentHealth - previousHealth) > 0.05f) {
            if (amount > 0) {
                OnHealthGain?.Invoke();
            } else {
                if (currentHealth > 0) {
                    OnHealthLost?.Invoke();
                } else {
                    OnDeath?.Invoke();
                }
            }
        }
    }

    private void HealthGained() {
        if (healthGainAudio != null) {
            AudioManager.Instance.PlaySound(healthGainAudio.name);
        }

        if (healthGainParticles != null) {
            
        }
    }

    private void HealthLost() {
        if (healthLostAudio != null) {
            AudioManager.Instance.PlaySound(healthLostAudio.name);
        }

        if (healthLostParticles != null) {
            
        }
    }

    private void Death() {
        if (deathAudio != null) {
            AudioManager.Instance.PlaySound(deathAudio.name);
        }

        if (deathParticles != null) {
            
        }
    }
}
