using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{

    float speed = 40; 
    Animator animator;
    public Transform ball;
    public Transform aimTarget; 

    public Transform[] targets; // array of targets 

    float force = 13; 
    Vector3 targetPosition; 

    ShotManager shotManager; 


    void Start()
    {
        targetPosition = transform.position; 
        animator = GetComponent<Animator>(); 
        shotManager = GetComponent<ShotManager>(); 
    }

    void Update()
    {
        Move(); 
    }

    void Move()
    {
        targetPosition.x = ball.position.x; 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime); 
    }

    Vector3 PickTarget() // picks a random target 
    {
        int randomValue = Random.Range(0, targets.Length); // get a random value from 0 to length of our targets array-1
        return targets[randomValue].position; 
    }

    Shot PickShot() // picks a random shot 
    {
        int randomValue = Random.Range(0, 2); // pick a random value 0 or 1 
        if (randomValue == 0) // 上旋球
            return shotManager.topSpin;
        else                   // 平击球
            return shotManager.flat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if it collides ball
        {
            Shot currentShot = PickShot(); // random shot 

            Vector3 dir = PickTarget() - transform.position; // get the direction 
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0); // set force to the ball

            Vector3 ballDir = ball.position - transform.position; // get the ball direction from the bot's position
            if (ballDir.x >= 0) // right
            {
                animator.Play("forehand"); // play a forehand animation
            }
            else
            {
                animator.Play("backhand"); // play a backhand animation
            }

            ball.GetComponent<Ball>().hitter = "bot";
        }
    }
}