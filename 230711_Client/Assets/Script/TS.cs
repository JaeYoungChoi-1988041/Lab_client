using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TS : MonoBehaviour
{
    public int damage = 10;

    private Player player;
    public GameObject play;
    public GameObject wbtn;
    public GameObject abtn;
    public GameObject mbtn;

    private void Update()
    {
        if (play == null)
        {
            play = GameObject.FindWithTag("Player"); //작용할 테스트 플레이어 오브젝트
            player = play.GetComponentInChildren<Player>();
        }
    }

    public void PlusHp()
    {
        player.stats.maxHp += 50;
        Debug.Log(player.stats.maxHp);
    }
    public void MinusHp()
    {
        player.stats.maxHp -= 50;
        Debug.Log(player.stats.maxHp);
    }
}
