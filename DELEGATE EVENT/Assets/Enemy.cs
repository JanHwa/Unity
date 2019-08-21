using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Player.onEnemyHit += Damage;
    }

    void Damage(Color color)
    {
        transform.GetComponent<Renderer>().material.color = color;

    }

    private void OnDisable()
    {
        Player.onEnemyHit -= Damage;
    }
}
