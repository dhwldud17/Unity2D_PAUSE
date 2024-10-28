using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Monster
{
    public enum State
    {
        Run,
        Hit,
        Death,
    };

    public State currentState = State.Run;


    private void Awake()
    {
        base.Awake();

        atkCoolTime = 3f;
        atkCoolTimeCalc = atkCoolTime;


        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Run:
                    yield return StartCoroutine(Run());
                    break;
                case State.Hit:
                    yield return StartCoroutine(Hit());
                    break;
                case State.Death:
                    yield return StartCoroutine(Death());
                    break;
            }
        }
    }

    IEnumerator Run()
    {
        if (!isHit)
        {
            MyAnimSetTrigger("Run");
            Move();
        }

        yield return null;
    }

    IEnumerator Hit()
    {
        MyAnimSetTrigger("Hit");

        yield return new WaitForSeconds(0.5f); // Hit �ִϸ��̼� ��� �ð�

        currentState = State.Run;
    }

    IEnumerator Death()
    {
        MyAnimSetTrigger("Death");

        capsuleCollider.enabled = false;

        // Death �ִϸ��̼� ��� �ð���ŭ ���
        yield return new WaitForSeconds(2f);

        // ���� ó�� ���� (��: ������Ʈ ����)
        Destroy(gameObject);
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (currentHp <= 0)
        {
            currentState = State.Death;
        }
        else
        {
            currentState = State.Hit;
        }
    }

    public override void Move()
    {
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);

        if (MonsterDirLeft == true)
            moveDir = -0.5f;
        else
            moveDir = 0.5f;

        Vector2 currentPos = transform.position; // ���� ��ġ ����
        Vector2 frontVec = new Vector2(currentPos.x + transform.localScale.x, currentPos.y - 0.3f); // �� ����
        Vector2 downVec = new Vector2(transform.position.x + moveDir, transform.position.y);

        Debug.DrawRay(frontVec, MonsterDirLeft ? Vector3.right : Vector3.left, new Color(0, 1, 0));
        Debug.DrawRay(downVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D front = Physics2D.Raycast(frontVec, MonsterDirLeft ? Vector3.right : Vector3.left, 1, LayerMask.GetMask("Platform"));

        if (front.collider != null) // �� ������ȯ
            MonsterFlip();

        GroundCheck();

        if (isGround)
        {
            RaycastHit2D down = Physics2D.Raycast(downVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (down.collider == null)
                MonsterFlip();
        }
    }
}
