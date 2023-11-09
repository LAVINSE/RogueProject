using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeightDropTable : ScriptableObject
{
    [System.Serializable]
    public class DropTable
    {
        public ItemInfoTable ItemTable;
        public float Weight;
    }

    #region 변수
    public float Total = 0;
    public List<DropTable> DropTableList = new List<DropTable>();
    #endregion // 변수

    #region 함수
    private ItemInfoTable PickItem()
    {
        var Rand = Mathf.Floor(Total * Random.Range(0.0f, 1.0f));
        float Percent = 0;

        for (int i = 0; i < DropTableList.Count; i++)
        {
            Percent += DropTableList[i].Weight;

            if (Rand <= Percent)
            {
                return DropTableList[i].ItemTable;
            }
        }

        return null;
    }

    public ItemInfoTable ItemDrop()
    {
        var ItemPick = PickItem();
        /*
        if (ItemPick == null)
        {
            return null;
        }
        */

        return ItemPick;
    }
    #endregion // 함수
}
