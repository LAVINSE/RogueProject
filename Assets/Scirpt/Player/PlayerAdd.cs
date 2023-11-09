using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdd : MonoBehaviour
{
    #region 함수
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
    #endregion 함수
}
