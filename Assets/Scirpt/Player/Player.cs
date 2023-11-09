using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region 변수
    [SerializeField] private float BasicAtkCoolTime = 0f;

    private float BasicAtkCurrentTime;
    #endregion // 변수

    #region 함수
    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        
    }

    /** 초기화 => 접촉중인 상태일때 */
    private void OnCollisionStay2D(Collision2D collision)
    {
        // 태그가 Item 이면
        if (collision.gameObject.CompareTag("Item"))
        {
            ItemAdd GetItem = collision.gameObject.GetComponent<ItemAdd>();

            // Z 키를 눌렀을 때, 아이템이 존재 할때
            if (Input.GetKey(KeyCode.Z) && GetItem != null)
            {
                ItemInfoTable Item = GetItem.ItemAdd();
                var AddItem = Inventory.Instance.AddItem(Item);

                // 아이템을 인벤토리에 넣었으면 
                if (AddItem == true)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }

    private void BasicAtk()
    {
        // 쿨타임
        if (BasicAtkCurrentTime <= 0)
        {
            // Q 키를 눌렀을 때
            if(Input.GetKey(KeyCode.Q))
            {
                // 공격
            }
        }
        else
        {
            BasicAtkCurrentTime -= Time.deltaTime;
        }
    }
    #endregion // 함수
}
