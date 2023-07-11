using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    [SerializeField]
    private Text goldInfo;
    private int gold;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// "changedInt" 만큼 골드를 변동시킨다.
    /// 골드 변동되어 -가 될시 return 함
    /// </summary>
    /// <param name="changedInt"></param>
    /// <returns></returns>
    public void ChangeGold(int changedInt)
    {
        if (gold + changedInt < 0) return;
        gold += changedInt;
        goldInfo.text = gold.ToString()+"Gold";
    }

    public int GetGold()
    {
        return gold;
    }
}
