using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Monster
{
    public enum State
    {
        Run,
        Attack,
        Hit,
        Death,
    };

    public State currentState = State.Run;

    public Transform genPoint;
    public GameObject Bullet;


    void Awake()
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
                case State.Attack:
                    yield return StartCoroutine(Attack());
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
        float runTime = Random.Range(2f, 4f);
        while (runTime >= 0f)
        {
            if (currentState == State.Hit || currentState == State.Death)
                yield break;

            runTime -= Time.deltaTime;
            MyAnimSetTrigger("Run");

            if (!isHit)
            {
                Move();

                if (canAtk && IsPlayerDir())
                {
                    if (Vector2.Distance(transform.position, GameManager.Instance.player.transform.position) < 15f)
                    {
                        currentState = State.Attack;
                        yield break;
                    }
                }
            }
            yield return null;
        }

        if (currentState != State.Attack)
        {
            MonsterFlip();
        }

    }

    IEnumerator Attack()
    {
        yield return null;

        canAtk = false;

        MyAnimSetTrigger("Attack");

        float attackDuration = Random.Range(0.5f, 1f);
        float timer = 0f;
        while (timer < attackDuration)
        {
            timer += Time.deltaTime;
            if (currentState == State.Hit || currentState == State.Death)
                yield break;

            yield return null;
        }

        currentState = State.Run;
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

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    void Fire()
    {
        GameObject bulletClone = Instantiate(Bullet, genPoint.position, genPoint.rotation);
        if (bulletClone != null)
        {
            bulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * transform.localScale.x * 5f;
            Physics2D.IgnoreCollision(bulletClone.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
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
