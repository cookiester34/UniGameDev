using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMoveScript : MonoBehaviour
{
    public float speed;
    public GameObject impactPrefab;
    public List<GameObject> trails;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

        if(speed != 0)
        {
            rb.position += transform.forward * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;
        //if (collision.collider.tag == "Building" && collision.collider.name != "QueenBeeBuilding(Clone)")
        //{
        //    GameObject dissolver = new GameObject("dissolver", typeof(Dissolver));
        //    dissolver.GetComponent<Dissolver>().Setup(collision.transform.gameObject);
        //    Object.Destroy(collision.transform.gameObject, 0.2f);
        //    AudioManager.Instance.PlaySound("DestroyBuilding");
        //}
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
        foreach (var hitCollider in hitColliders)
        {
            Health health = hitCollider.GetComponent<Health>();
            if(health != null)
            {
                health.ModifyHealth(-2);
            }
        }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if(impactPrefab != null)
        {
            var impactVFX = Instantiate(impactPrefab, pos, rot) as GameObject;
            Destroy(impactVFX, 5);
        }

        if(trails.Count > 0)
        {
            for(int i = 0; i< trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>();
                if(ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }

        Destroy(gameObject);
    }
}
