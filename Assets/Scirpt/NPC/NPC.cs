using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    /** 초기화 => 처음 접촉했을 때 */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 접촉중일 경우
        if(collision.gameObject.CompareTag("Player"))
        {
            var PlayerComponent = collision.gameObject.GetComponent<Player>();
            
            if(PlayerComponent != null )
            {
                PlayerComponent.oNPC = this;
            }
        }
    }

    /** 초기화 => 처음 접촉이 끝났을 경우 */
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void ASD()
    {
        Debug.Log("asdf");
    }
}
