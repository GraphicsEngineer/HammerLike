using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static ToonyColorsPro.ShaderGenerator.Enums;

public class FloatingItem : MonoBehaviour
{
    public ItemManager itemManager; // ItemManager ����
    public int itemId; // �� �������� ID

    private Rigidbody rb;
    private float originalY;
    private float maxY;
    private float speed = 1f;
    private bool isMovingUp = false;
    private float timeToKinematic = 2f; // Kinematic���� ��ȯ�ϱ� �� ��� �ð�
    private float timer = 0f; // Ÿ�̸�
    private bool isDropped = false;
    private float obtainableDistance = 8f; // �������� �÷��̾ ���� �� �ִ� �Ÿ�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalY = transform.position.y + 0.5f;
        maxY = originalY + 1f;
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse); // �ʱ� �� �߰�
    }

    void Update()
    {
        // �÷��̾���� �Ÿ��� ���
        float distanceToPlayer = Vector3.Distance(itemManager.player.transform.position, transform.position);
        /// ����� ��� �������� �� �ڼ�ó�� �ٰ������� ��常 �ڵ����� ������� �ٸ� ��������
        /// ZŰ�� �����߸� ������� �ؾ��Ѵ�
        /// ���� boolean������ �ϳ� �� ���� ȹ���ߴ��� ��ȹ���ߴ����� �Ǵ��ϰ� �ϴ°� �´°Ű���
        if (distanceToPlayer < obtainableDistance)
        {
            // �������� �÷��̾�� ��ܿ�
            rb.isKinematic = false;
            Vector3 directionToPlayer = (itemManager.player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + directionToPlayer * 10f * Time.deltaTime);
        }

        if (isDropped && distanceToPlayer > obtainableDistance)
        {
            timer += Time.deltaTime;
            if (timer >= timeToKinematic)
            {
                rb.isKinematic = true; // ������ �ð� ���Ŀ� isKinematic�� true�� ����
                isDropped = false; // �� ���� �����ǵ���
            }
        }

        if (isMovingUp && !isDropped)
        {
            // ���� �̵�
            if (transform.position.y < maxY)
            {
                float newY = Mathf.MoveTowards(transform.position.y, maxY, speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                isMovingUp = false; // �ִ� ���� ���� �� �̵� ���� ����
            }
        }
        else
        {
            // ���� ��ġ�� �̵�
            if (transform.position.y > originalY)
            {
                float newY = Mathf.MoveTowards(transform.position.y, originalY, speed * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }
            else
            {
                isMovingUp = true; // ���� ��ġ�� ���� �� �̵� ���� ����
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Room"))
        {
            isDropped = true;
            isMovingUp = true;
            // ���⼭�� isKinematic�� �������� ����
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� ���� �� �������� �κ��丮�� �߰��ϰ� ��ü �ı�
            itemManager.GiveItemToPlayer(itemId);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // �÷��̾�� ���� �� �������� �κ��丮�� �߰��ϰ� ��ü �ı�
            itemManager.GiveItemToPlayer(itemId);
            Destroy(gameObject);
        }
    }
}
