using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class DataSaveManager
{
    public float PlayerMaxHp = 20f;
    public float PlayerMaxMana = 20f;
    public float PlayerAtk = 5f;
    public int PlayerGold = 10;
    public int PlayerLevel = 0;
    public float PlayerBasicAtkCoolTime = 2f;

    public List<ItemInfoTable> PlayerItemList = new List<ItemInfoTable>();
}

public class DataManager : CSingleton<DataManager>
{
    #region 변수
    public string oPath;
    #endregion // 변수

    #region 함수
    private void Start()
    {
        oPath = Path.Combine(Application.dataPath + "/Data/", "SaveFile");
        JsonLoad();
    }

    /** 유니티가 종료될때 */
    private void OnApplicationQuit()
    {
        JsonSave();
    }

    /** 데이터 로드 */
    public void JsonLoad()
    {
        DataSaveManager SaveData = new DataSaveManager();

        if(!File.Exists(oPath))
        {
            // 처음 시작하는 데이터 정보 쓰기
            Debug.Log("불러올 데이터 없음");

            // 현재 데이터 저장
            JsonSave();
        }
        else
        {
            string LoadJson = File.ReadAllText(oPath);
            SaveData = JsonUtility.FromJson<DataSaveManager>(LoadJson);

            if(SaveData != null)
            {
                GameManager.Inst.oPlayerMaxHp = SaveData.PlayerMaxHp;
                GameManager.Inst.oPlayerMaxMana = SaveData.PlayerMaxMana;
                GameManager.Inst.oPlayerGold = SaveData.PlayerGold;
                GameManager.Inst.oPlayerAtk = SaveData.PlayerAtk;
                GameManager.Inst.oPlayerLevel = SaveData.PlayerLevel;
                GameManager.Inst.oPlayerBasicAtkCoolTime = SaveData.PlayerBasicAtkCoolTime;
                GameManager.Inst.oPlayerItemList = SaveData.PlayerItemList;
            }
        }
    }

    /** 데이터 저장 */
    public void JsonSave()
    {
        DataSaveManager SaveData = new DataSaveManager();

        SaveData.PlayerMaxHp = GameManager.Inst.oPlayerMaxHp;
        SaveData.PlayerMaxMana = GameManager.Inst.oPlayerMaxMana;
        SaveData.PlayerGold = GameManager.Inst.oPlayerGold;
        SaveData.PlayerAtk = GameManager.Inst.oPlayerAtk;
        SaveData.PlayerLevel = GameManager.Inst.oPlayerLevel;
        SaveData.PlayerBasicAtkCoolTime = GameManager.Inst.oPlayerBasicAtkCoolTime;
        SaveData.PlayerItemList = GameManager.Inst.oPlayerItemList;

        string Json = JsonUtility.ToJson(SaveData, true);

        File.WriteAllText(oPath, Json);
    }
    #endregion // 함수
}
