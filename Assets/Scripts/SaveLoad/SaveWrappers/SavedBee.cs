using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class SavedBee {
    public SavedTransform transform;
    public SavedHealth health;
    public int id;

    public const string BeePrefab = "Bee";

    public SavedBee(Bee bee) {
        transform = new SavedTransform(bee.transform);
        health = new SavedHealth(bee.GetComponent<Health>());
        id = bee.Id;
    }

    public Bee Instantiate() {
        GameObject go = (GameObject)Object.Instantiate(Resources.Load(BeePrefab), transform.Position, transform.Rotation);
        Health healthComponent = go.GetComponent<Health>();
        healthComponent.LoadSavedHealth(health);
        Bee bee = go.GetComponent<Bee>();
        bee.Id = id;
        return bee;
    }
}
