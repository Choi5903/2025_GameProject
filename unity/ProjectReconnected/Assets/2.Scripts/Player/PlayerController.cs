using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private Transform currentPlatform = null;
    private Vector3 lastPlatformPosition;

    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.1f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool canMove = true;

    private Animator animator;
    [Header("치트 기능 (Z키)")]
    public GameObject guardObject; // 가드 오브젝트 연결
    private bool cheatGuardActive = false;
    private float fixedYPosition;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private float stepTimer = 0f;
    public float stepInterval = 0.4f; // 발걸음 간격 조절

    private void Update()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive) return;
        if (!canMove) return;

        float move = Input.GetAxisRaw("Horizontal");

        if (isGrounded && Mathf.Abs(move) > 0.01f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                SFXManager.Instance.PlayRandomStepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        // 이동 처리
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // 애니메이션 제어
        if (animator != null)
        {
            bool isWalking = Mathf.Abs(move) > 0.01f;
            animator.SetBool("isWalking", isWalking);
            animator.SetBool("isJumping", !isGrounded); // 점프 상태 처리
        }

        // 방향 반전 처리
        if (move != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(move) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            SFXManager.Instance.PlayJumpSound();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        // Z 키 치트 - 가드 오브젝트 토글 + Y축 고정
        if (Input.GetKeyDown(KeyCode.Z))
        {
            cheatGuardActive = !cheatGuardActive;

            if (guardObject != null)
                guardObject.SetActive(cheatGuardActive);

            if (cheatGuardActive)
            {
                fixedYPosition = transform.position.y;
                Debug.Log("🛡️ 가드 활성화 + Y축 고정");
            }
            else
            {
                Debug.Log("❌ 가드 비활성화 + Y축 고정 해제");
            }
        }

    }
    private void FixedUpdate()
    {
        CheckGrounded();
        // 플랫폼 이동 보정
        if (isGrounded && currentPlatform != null)
        {
            Vector3 delta = currentPlatform.position - lastPlatformPosition;
            rb.position += new Vector2(delta.x, delta.y); // 위치에 직접 더하기
        }
        if (currentPlatform != null)
        {
            lastPlatformPosition = currentPlatform.position;
        }

        if (cheatGuardActive)
        {
            rb.position = new Vector2(rb.position.x, fixedYPosition);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                currentPlatform = collision.transform;
                lastPlatformPosition = currentPlatform.position;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    private void CheckGrounded()
    {
        if (groundCheck == null) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        if (!enabled)
        {
            rb.velocity = Vector2.zero;
        }
    }
}
