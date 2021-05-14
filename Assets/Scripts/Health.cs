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
    [SerializeField] private bool shouldDestroyOnDeath = false;

    [Header("Optional common things")]
    [SerializeField] private GameObject healthGainParticles;
    [SerializeField] private AudioSource healthGainAudio;

    [SerializeField] private GameObject healthLostParticles;
    [SerializeField] private AudioSource healthLostAudio;

    [SerializeField] private GameObject deathParticles;
    [SerializeField] private AudioSource deathAudio;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    /// <summary>
    /// The health as a percentage value between 0 to 1
    /// </summary>
    public float NormalizedHealth => (currentHealth / maxHealth);

    public bool regen = false;
    private float regenTimeIntival = 1f;

    private void Awake() {
        OnHealthGain += HealthGained;
        OnHealthLost += HealthLost;
        OnDeath += Death;
    }

    private void Update()
    {
        if (regen)
        {
            if (regenTimeIntival > 0)
                regenTimeIntival -= Time.deltaTime;
            else
            {
                regenTimeIntival = 1f;
                ModifyHealth(0.005f);
            }
        }
    }

    public void LoadSavedHealth(SavedHealth savedHealth) {
        currentHealth = savedHealth.currentHealth;
        maxHealth = savedHealth.maxHealth;
    }

    public void HealForCost(float amount, ResourcePurchase resourcePurchase)
    {
        if (ResourceManagement.Instance.CanUseResource(resourcePurchase))
        {
            ModifyHealth(amount);
        }
    }

    public void ModifyHealth(float amount) {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (amount > 0)
        {
            OnHealthGain?.Invoke();
        }
        else
        {
            if (currentHealth > 0.1f)
            {
                OnHealthLost?.Invoke();
            }
            else
            {
                OnDeath?.Invoke();
            }
        }
    }

    public void SetHealth(float amount)
    {
        maxHealth = amount;
        currentHealth = amount;
    }

    public void ModulateSound(AudioSource source) {
        float pitchOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        float volumeOffset = UnityEngine.Random.Range(-0.05f, 0.05f);
        source.pitch = 1 + pitchOffset;
        source.volume = 1 + volumeOffset;
    }

    private void HealthGained() {
        if (healthGainAudio != null) {
            AudioManager.Instance.ModulateAudioSource(healthGainAudio);
            healthGainAudio.Play();
        }

        if (healthGainParticles != null) {
            CreateParticles(healthGainParticles);
        }
    }

    private void HealthLost() {
        if (healthLostAudio != null) {
            AudioManager.Instance.ModulateAudioSource(healthLostAudio);
            healthLostAudio.Play();
        }

        if (healthLostParticles != null) {
            CreateParticles(healthLostParticles);
        }
    }

    private void Death() {
        if (deathAudio != null) {
            AudioManager.Instance.ModulateAudioSource(deathAudio);
            deathAudio.Play();
        }

        if (deathParticles != null) {
            CreateParticles(deathParticles);
        }

        if (shouldDestroyOnDeath) {
            GameObject dissolver = new GameObject("dissolver", typeof(Dissolver));
            dissolver.GetComponent<Dissolver>().Setup(gameObject);
            Destroy(gameObject);
        }
    }

    private void CreateParticles(GameObject particlePrefab) {
        GameObject go = Instantiate(particlePrefab, transform.position, transform.rotation);
        ParticleSystem particles = go.GetComponent<ParticleSystem>();
        particles.Play();
        Destroy(particles.gameObject, particles.time);
    }
}
