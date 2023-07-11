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
    /// "changedInt" ��ŭ ��带 ������Ų��.
    /// ��� �����Ǿ� -�� �ɽ� return ��
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
