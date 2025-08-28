using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private AudioClip hitSFX;

    private List<GameObject> hits;
    void Start()
    {
        hitSFX = Resources.Load<AudioClip>("SFX/shieldHit");
        hits = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
            Destroy(other.gameObject);
            
        if (other.CompareTag("Tower"))
        {
            for(int i = 0; i < hits.Count; i++)
            {
                if(hits[i] != null && hits[i] == other.gameObject)
                    return;
            }
            hits.Add(other.gameObject);

            LevelManager.instance.PlaySound(hitSFX, 0.4f);
            other.gameObject.GetComponent<TowerController>().takeDamage(66, false);

            if(other.gameObject.GetComponent<Shooter>() != null)
            {
                other.gameObject.GetComponent<Shooter>().Disable(5);
            }
        }
    }
}
