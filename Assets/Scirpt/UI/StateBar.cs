using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateBar : MonoBehaviour
{
    #region 변수
    [SerializeField] private List<Image> SKillImgList = new List<Image>();
    [SerializeField] private List<TMP_Text> SkillCoolTextList = new List<TMP_Text>();
    [SerializeField] private List<TMP_Text> SkillKeyTextList = new List<TMP_Text>();

    [SerializeField] private Image HpBarImg = null;
    [SerializeField] private Image ManaBarImg = null;
    [SerializeField] private Image UserImg = null;

    [SerializeField] private TMP_Text LevelText = null;
    #endregion // 변수

    #region 프로퍼티
    public static StateBar Instance { get; private set; }
    #endregion // 프로퍼티

    #region 함수
    /** 초기화 */
    private void Awake()
    {
        Instance = this;
    }

    /** 초기화 => 상태를 갱신한다 */
    private void Update()
    {
        
    }

    /** 쿨타임을 체크한다 */
    public IEnumerator CheckCoolTime(int SkillNumber, float CoolTime)
    {
        // TODO : enum으로 스킬Number 수정
        float CurrentTime = 0f;
        CurrentTime = CoolTime;

        while (CurrentTime > 0.0f)
        {
            CurrentTime -= Time.deltaTime;

            SKillImgList[SkillNumber].fillAmount = (CurrentTime / CoolTime);

            string Span = TimeSpan.FromSeconds(CurrentTime).ToString("s\\:ff");
            string[] Tokens = Span.Split(':');

            SkillCoolTextList[SkillNumber].text = string.Format("{0}:{1}", Tokens[0], Tokens[1]);

            yield return new WaitForFixedUpdate();
        }

        GameManager.Inst.IsBasicAttack = true;
    }

    /** 상태바 세팅 */
    public void UpdateStateBar(float PlayerMaxHp, float PlayerCurrentHp,
        float PlayerMaxMana, float PlayerCurrentMana, int PlayerLevel, SpriteRenderer PlayerSprite)
    {
        HpBarImg.fillAmount = PlayerCurrentHp / PlayerMaxHp;
        ManaBarImg.fillAmount = PlayerCurrentMana / PlayerMaxMana;
        LevelText.text = PlayerLevel.ToString();
        UserImg.sprite = PlayerSprite.sprite;

        SkillKeyTextList[0].text = KeySetting.Keys[UserKeyAction.Skill_Q].ToString();
        SkillKeyTextList[1].text = string.Empty;
        SkillKeyTextList[2].text = string.Empty;
        SkillKeyTextList[3].text = string.Empty;
        SkillKeyTextList[4].text = string.Empty;
    }
    #endregion // 함수
}
