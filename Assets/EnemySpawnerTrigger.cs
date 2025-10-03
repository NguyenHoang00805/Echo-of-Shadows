using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTrigger : MonoBehaviour
{
    [Header("Enemies to Activate")]
    public List<GameObject> enemies;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null) enemy.SetActive(true);
            }
            Destroy(gameObject);
        }
    }
}
