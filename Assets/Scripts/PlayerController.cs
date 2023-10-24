using System.ComponentModel;
using System.Timers;
using System;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   public float moveSpeed = 5f; // Velocidade de movimento do jogador.
    public float jumpForce = 10f; // Força do salto do jogador.
    public AudioClip jumpSound; // Som do salto.
    private Animator animator;   // Referência ao componente Animator.
    private SpriteRenderer spriteRenderer; // Referência ao componente SpriteRenderer.
    private bool isRunning = false; // Flag para verificar se o jogador está correndo.
    private Rigidbody2D rb; // Referência ao componente Rigidbody2D.
    private AudioSource audioSource; // Referência ao componente AudioSource.
    private bool isGrounded = false; // Flag para verificar se o jogador está no chão.
    private LayerMask groundLayer; // Layer para definir o que é considerado "chão."
public bool isTouchingWall = false;
    private RaycastHit2D wallHit;
 private Vector2 rayDirection;
    // Variável para armazenar o input horizontal.
    private float horizontalInput = 0f;
    // Variável para controlar se o jogador está no ar.
    private bool isJumping = false;
    public GameObject leftCollider;
    public GameObject rightCollider;
    public Collider2D rcollider;
    public Collider2D lcollider;

    void Start()
    {
        // Obtém a referência aos componentes.
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        groundLayer = LayerMask.GetMask("Ground");
    }
void FlipCharacter(bool isFacingLeft)
{
    spriteRenderer.flipX = isFacingLeft;
    // Ativa/desativa os Colliders com base na direção.
    leftCollider.SetActive(!isFacingLeft);
    rightCollider.SetActive(isFacingLeft);
}


    void Update()
    {
        // Obtém a entrada do jogador no eixo horizontal (esquerda/direita).
        horizontalInput = Input.GetAxis("Horizontal");
        // Calcula a velocidade final de movimento com base na entrada do jogador.
        Vector2 moveVelocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        
        

if (spriteRenderer.flipX)
        {
            rayDirection = Vector2.left;
        }
        else
        {
            rayDirection = Vector2.right;
        }
         // Verifica se o jogador está tocando uma parede.
        wallHit = Physics2D.Raycast(transform.position, rayDirection, 0.6f, LayerMask.GetMask("Wall"));
        isTouchingWall = wallHit.collider != null;

        // Aplica a velocidade ao Rigidbody2D do jogador, permitindo apenas movimento vertical no chão.
        if (isGrounded)
        {
            rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
            animator.SetBool("IsJumping", false);
            animator.SetBool("isGrounded", true);
            isJumping = false;
        }
        else
        {
              // Define o parâmetro "IsJumping" no Animator como verdadeiro.
            animator.SetBool("IsJumping", true);
            // Define a variável isJumping como verdadeira.
            isJumping = true;
            // Verifica se o jogador está tocando uma parede e pressionando a tecla de movimento contra ela.
            if (isTouchingWall && Mathf.Abs(horizontalInput) > 0.1f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y); // Pare de mover horizontalmente.
            }
            else if (!isTouchingWall) // Verifique se não está tocando em uma parede antes de aplicar a velocidade.
            {
                rb.velocity = moveVelocity;
            }
        }


        // Define o parâmetro "Speed" no Animator para controlar a animação de caminhada.
        float absoluteHorizontalInput = Mathf.Abs(horizontalInput);
        animator.SetFloat("Speed", absoluteHorizontalInput);

        // Verifica se o jogador está correndo.
        if (absoluteHorizontalInput > 0.1f)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        // Define o parâmetro "IsRunning" no Animator para controlar a transição entre animações.
        animator.SetBool("IsRunning", isRunning);

if (horizontalInput < 0)
{
    FlipCharacter(true); // Chame com true para virar para a esquerda.
}
else if (horizontalInput > 0)
{
    FlipCharacter(false); // Chame com false para virar para a direita.
}

        // Se o jogador estiver no chão e pressionar a tecla de salto (por exemplo, "Espaço"):
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // Aplica uma força vertical para fazer o jogador pular.
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            // Toca o som de salto.
            if (jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
           
        }
        if(isJumping)
    {
            animator.SetBool("isGrounded", false);
                    animator.SetBool("IsRunning", false);
    }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Verifica se o jogador está em contato com um objeto com a tag "Ground".
        if (other.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Verifica se o jogador deixou de estar em contato com um objeto com a tag "Ground".
        if (other.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
