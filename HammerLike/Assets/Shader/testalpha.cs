using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testalpha : MonoBehaviour
{
    // Start is called before the first frame update
    private Material material;
    
    void Start()
    {
        // ��Ƽ���� �ʱ�ȭ
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            OnHit();
        }
    }

    // ���� ���� �Լ�
    public void SetTransparency(float alpha)
    {
        if (material != null)
        {
            Color color = material.GetColor("_BaseColor");
            color.a = alpha; // ���� �� ����
            material.SetColor("_BaseColor", color);
        }
    }

    // �÷��̾ �ǰݵǾ��� �� ȣ��� �Լ�
    public void OnHit()
    {
        // ������ 0.5�� ����
        SetTransparency(0.5f);

        // �ʿ��� ��� ������ �ٽ� ������� �ǵ��� �� �ֽ��ϴ�.
        StartCoroutine(ResetTransparency());
    }

    // ���İ��� ������� �ǵ����� �ڷ�ƾ
    IEnumerator ResetTransparency()
    {
        // 1�� ��ٸ�
        yield return new WaitForSeconds(1);

        // ������ ���� ���� 1.0���� ����
        SetTransparency(1.0f);
    }
}
