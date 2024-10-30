using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private float movement;
    private bool facingRight = true;
    private bool isGround = true;
    public Rigidbody2D rb;
    public Animator animator;


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float jumpHeight = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){

        if (maxHealth <= 0) {
            Die();
        }
        {
            
        }
        movement = Input.GetAxis("Horizontal");

        if (movement < 0f && facingRight){
            transform.eulerAngles = new Vector3(0f,-180f,0f);
            facingRight = false;
        } else if (movement > 0f && !facingRight){
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        if (Input.GetKey(KeyCode.Space) && isGround) {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);

        }

        if(Mathf.Abs(movement) > .1f) {
            animator.SetFloat("Run", 1f);
        } else if(movement < .1f) {
            animator.SetFloat("Run", 0f);

        }

        if(Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate() {

        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * moveSpeed;
    }

    void Jump() {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag == "Ground"){
            isGround = true;
            animator.SetBool("Jump", false);
        }

    }

    public void TakeDamage(int damage) {
        if(maxHealth <= 0){
            return; }


        maxHealth -= damage;
    }

    void Die() {
        Debug.Log("Player Die");
    }
}
