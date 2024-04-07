using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public int maxJumpCount; // �ִ� ���� Ƚ���� �����մϴ�.

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private int jumpCount; // ���� ���� Ƚ���� �����մϴ�.

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxJumpCount) // �ִ� ���� Ƚ�� �̳������� ���� ����
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0f); // ������ �� y �ӵ��� �ʱ�ȭ�մϴ�.
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                jumpCount++;
                anim.SetBool("isJumping", true);
            }
        }

        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        // Direction Sprite
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // Animation
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    void FixedUpdate()
    {
        // Move By Key Control
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y); // Right Max Speed
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y); // Left Max Speed

        // Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.5f)
                {
                    // ���� ����� ���� ���� Ƚ�� �ʱ�ȭ
                    jumpCount = 0;
                }
                anim.SetBool("isJumping", false);
            }
        }
    }
}
