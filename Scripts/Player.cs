using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform aimTarget; // target 
    float speed = 3f; 
    float force = 13; 

    bool hitting; // 是否打到球

    public Transform ball; // 球
    Animator animator;

    Vector3 aimTargetInitialPosition; 

    ShotManager shotManager; 
    Shot currentShot; 

    [SerializeField] Transform serveRight;
    [SerializeField] Transform serveLeft;
    bool servedRight = true;
    private void Start()
    {
        animator = GetComponent<Animator>(); 
        aimTargetInitialPosition = aimTarget.position; 
        shotManager = GetComponent<ShotManager>(); 
        currentShot = shotManager.topSpin; 
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); 
        float v = Input.GetAxisRaw("Vertical"); 

        if (Input.GetKeyDown(KeyCode.F)) 
        {
            hitting = true; 
            currentShot = shotManager.topSpin; // 上旋
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; 
        }                    

        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; 
            currentShot = shotManager.flat; // 平击球
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false;
        }



        if (hitting)  // if we are trying to hit the ball
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime); //translate the aiming gameObject on the court horizontallly
        }


        if ((h != 0 || v != 0) && !hitting) // if we want to move and we are not hitting the ball
        {
            transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime); // move on the court
        }


        ball.GetComponent<Ball>().hitter = "player";
        ball.GetComponent<Ball>().playing = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // if we collide with the ball 
        {
            Vector3 dir = aimTarget.position - transform.position; // get the direction to where we want to send the ball
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0);
            //add force to the ball plus some upward force according to the shot being played

            Vector3 ballDir = ball.position - transform.position; // get the direction of the ball compared to us to know if it is
            if (ballDir.x >= 0)                                   // on out right or left side 
            {
                animator.Play("forehand");                        // play a forhand animation if the ball is on our right
            }
            else                                                  // otherwise play a backhand animation 
            {
                animator.Play("backhand");
            }
            ball.GetComponent<Ball>().hitter = "player";
            aimTarget.position = aimTargetInitialPosition; // reset the position of the aiming gameObject to it's original position ( center)

        }
    }
    public void Reset()
    {
        if(servedRight)
            transform.position = serveLeft.position;
        else
            transform.position = serveRight.position;

        servedRight = !servedRight;
    }

}