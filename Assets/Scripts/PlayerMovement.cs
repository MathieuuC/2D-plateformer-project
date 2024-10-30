using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private float movement;
    private bool facingRight = true;
    private bool isGround = true;

    public Text coinText;
    public Text health;
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask attackLayer;
    public int currentCoin = 0;


    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform attackPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){

        if(FindObjectOfType<GameManager>().isGameActive == false) {
            return;
        }

        if (maxHealth <= 0) {
            
            Die();
        }

        coinText.text = currentCoin.ToString();
        health.text = maxHealth.ToString();
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

    public void AttackPlayer() {
        Collider2D collInfo2 = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo2){
            if (collInfo2.gameObject.GetComponent<PatrolEnnemy>() != null ) {
                collInfo2.gameObject.GetComponent<PatrolEnnemy>().TakeDamage(1);


            }
        }

    }

    private void OnDrawGizmos() {
        if(attackPoint == null) {
            return;
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int damage) {
        if(maxHealth <= 0){
            return; }


        maxHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Coin") {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);

        }
        
    }

    void Die() {
        Debug.Log("Player Die");
        FindObjectOfType<GameManager>().isGameActive = false;
        Destroy(this.gameObject);
    }
}
