using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavedHealth {
    public float currentHealth;
    public float maxHealth;
    
    public SavedHealth(Health health) {
        currentHealth = health.CurrentHealth;
        maxHealth = health.MaxHealth;
    }
}
