using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skeleton : Monster
{

    private void Awake()
    {
        base.Awake();
        moveSpeed = 2f;
        jumpPower = 5f;
    }


    void FixedUpdate()
    {

        if (!isHit)
        {
            Move();
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
