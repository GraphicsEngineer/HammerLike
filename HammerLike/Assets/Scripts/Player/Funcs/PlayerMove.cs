
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f; // �÷��̾��� �̵� �ӵ��� �����մϴ�.
    public Camera cam; // ���� ī�޶� �Ҵ��ϱ� ���� �����Դϴ�.

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // ���� ������ �Է��� �޽��ϴ�.
        float vertical = Input.GetAxis("Vertical"); // ���� ������ �Է��� �޽��ϴ�.

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized; // �Է°��� �������� �̵� ������ �����մϴ�.

        // ���콺 ��ư�� ������ �ִ� ���
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ�κ��� Ray�� ����ݴϴ�.
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Ray�� ��ü�� �浹�� ���
            {
                Vector3 targetPosition = hit.point; // Ray�� ��ü�� �浹�� ��ġ�Դϴ�.
                targetPosition.y = transform.position.y; // y ��ǥ�� �÷��̾��� y ��ǥ�� �����մϴ�.
                transform.LookAt(targetPosition); // �÷��̾ ���콺 Ŀ���� ������ �ٶ󺸰� �մϴ�.
            }
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World); // �̵� ����� �ӵ��� �������� �÷��̾ �̵���ŵ�ϴ�.
    }
}
