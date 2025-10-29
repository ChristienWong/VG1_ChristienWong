using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceShooter;
namespace SpaceShooter{
public class Projectile : MonoBehaviour
{

    Rigidbody2D _rb;
    Transform target;
    
    void Start(){
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        float acceleration = 1f;
        float maxSpeed = 2f;
       
        ChooseNearestTarget();
        
        if (target != null){
            Vector2 directionToTarget = target.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            _rb.MoveRotation(angle);
        }

        _rb.AddForce(transform.right * acceleration);
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);
    }

    void ChooseNearestTarget(){
       float closestDistance = 9999f;
       Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

       for(int i =0; i<asteroids.Length; i++){
        Asteroid asteroid = asteroids[i];

        if (asteroid.transform.position.x > transform.position.x){
            Vector2 directionToTarget = asteroid.transform.position - transform.position;
        
            if (directionToTarget.sqrMagnitude < closestDistance){
            closestDistance = directionToTarget.sqrMagnitude;
            target = asteroid.transform;
            }
       }
       }
    }

    void OnCollisionEnter2D(Collision2D other){
        HandleHit(other.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other){
        HandleHit(other.gameObject);
    }

    void HandleHit(GameObject otherGameObject){
        if (!otherGameObject.TryGetComponent<Asteroid>(out _)){
            return;
        }

        GameObject explosionPrefab = GameController.instance != null
            ? GameController.instance.explosionPrefab
            : null;

        if (explosionPrefab != null){
            GameObject explosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
            Destroy(explosion, 0.25f);
        }

        Destroy(otherGameObject);
        Destroy(gameObject);
    }
}
}
