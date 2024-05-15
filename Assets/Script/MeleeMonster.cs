using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class MeleeMonster : Monster
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
        moveSpeed = 2f;
        jumpPower = 5f;

        StartCoroutine(FSM());
    }


    IEnumerator FSM()
    {
        while (true)
        {
            Debug.Log("Current State: " + currentState);
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
            Move();
        }
        yield return null;
    }

    IEnumerator Hit()
    {
        Debug.Log("Entering Hit state");

        MyAnimSetTrigger("Hit");

        yield return new WaitForSeconds(0.5f); // Hit �ִϸ��̼� ��� �ð�

        currentState = State.Run;
    }

    IEnumerator Death()
    {
        Debug.Log("Entering Death state");
        MyAnimSetTrigger("Death");

        // Death �ִϸ��̼� ��� �ð���ŭ ���
        yield return new WaitForSeconds(2f);

        // ���� ó�� ���� (��: ������Ʈ ����)
        Destroy(gameObject);
    }

    //void FixedUpdate()
    //{

    //    if (!isHit)
    //    {
    //        Move();
    //    }

    //    if (currentHp <= 0)
    //        MyAnimSetTrigger("Death");
    //}

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


    protected void OnTriggerEnter2D(Collider2D collision) // �÷��̾�� �ε����� ���� ��ȯ
    {
        base.OnTriggerEnter2D(collision);
        if (collision.transform.CompareTag("PlayerHitBox"))
        {
            Debug.Log("PlayerHitBox hit, flipping monster");
            MonsterFlip();
            Debug.Log("Monster flipped");
        }
    }


}
