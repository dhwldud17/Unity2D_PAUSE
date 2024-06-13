using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skeleton : Monster
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

        monsterHitBox.enabled = false;
        // Death �ִϸ��̼� ��� �ð���ŭ ���
        yield return new WaitForSeconds(1f);

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

}
