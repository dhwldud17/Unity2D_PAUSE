using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    GameObject[] weaponInventory;  // ���� ���� ��, ���� �Ŵ����� ���� ���� �����ؾ� ��(�Լ� ���).
    public int playerDamage; // ���� ���� ��, ���� �Ŵ����� ���� ���� �����ؾ� ��(�Լ� ���).

    Rigidbody2D rigid;
    Collider2D col;


    //[SerializeField] private Sprite defaultWeaponImage; // �⺻ ���� �̹���
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        weaponInventory = GameManager.Instance.GetWeaponInventory();
        playerDamage = GameManager.Instance.GetPlayerDamage();
        /*
        // �ʱ� ���� �� �⺻ ���� �̹����� ����
        GameObject inventoryImage = GameObject.Find("ui").transform.Find("inventory").Find("WeaponImage").gameObject;
        inventoryImage.GetComponent<Image>().sprite = defaultWeaponImage;
        inventoryImage.SetActive(true);
        */
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("weapon") && Input.GetButtonDown("Interaction"))
        {
            /*
            GameManager.Instance.SetWeaponInventory(collision);

            GameObject inventoryImage = GameObject.Find("ui").transform.Find("inventory").Find("WeaponImage").gameObject;
            inventoryImage.GetComponent<Image>().sprite = collision.transform.GetComponent<SpriteRenderer>().sprite;
            inventoryImage.SetActive(true);

            int newBulletIndex = collision.GetComponent<weapon>().weaponId;
            GetComponent<PlayerMove>().ChangeBullet(newBulletIndex);
            */
        }
    }


    // ����
    public void SetWeaponInventory(Collider2D collision)
    {
        if (collision == null)
            return;

        bool isFull = true;
        int index = -1;
        for (int i = 0; i < weaponInventory.Length; i++)
        {
            if (weaponInventory[i] == null)
            {
                index = i;
                isFull = false;
                break;
            }
        }

        if (isFull) // �κ��丮 �� ĭ�̶� ������ �ӽ� �ڵ�
        {
            collision.gameObject.SetActive(false);
            collision.transform.parent = transform;
            Destroy(weaponInventory[0]);
            weaponInventory[0] = collision.gameObject;
        }
        else
        {
            collision.gameObject.SetActive(false);
            collision.transform.parent = transform;
            weaponInventory[index] = collision.gameObject;
        }

        // ���� �������� ������Ʈ
        playerDamage = collision.GetComponent<weapon>().weaponDamage;
        GameManager.Instance.SetPlayerDamage(playerDamage);
    }
}
