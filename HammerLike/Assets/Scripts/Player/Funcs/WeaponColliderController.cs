using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColliderController : MonoBehaviour
{
    private PlayerAtk playerAtk;
    // Start is called before the first frame update
    void Start()
    {
        playerAtk = FindObjectOfType<PlayerAtk>();
        if (playerAtk == null)
        {
            Debug.LogError("PlayerAtk ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableWeaponCollider()
    {
        playerAtk.weaponCollider.enabled = true;
        Debug.Log("enable Weaaawwpon");
    }
    public void DisableWeaponCollider()
    {
        playerAtk.weaponCollider.enabled = false;
        Debug.Log("disable Weapon");
    }
}
