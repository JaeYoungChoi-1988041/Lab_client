using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    //장비중인 아이템
    public Item equipItem;
    //현재 아이템
    public Item invenItem;
    public Inventory Equipment;
    public List<Item> items;
    private Image image;
    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    void Awake()
    {
        FreshSlot();
    }

    /// <summary>
    /// 아이템 정렬
    /// </summary>
    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
        foreach(Slot slot in slots)
        {
            slot.transform.gameObject.AddComponent<RightClick>();
        }
    }

    public void AddItem(Item item)
    {
        // 포션일시 포션스택(99)개 까지 겹쳐지는지 확인 겹치고 나머지 Additem실행
        if (items.Count < slots.Length)
        {
            items.Add(item);
            FreshSlot();
        }
        else
        {
            Debug.Log("아이템이 가득차있습니다");
        }
    }

    public void DiscardItem(int i)
    {
        if (items.Count == 0) return;
        items.RemoveAt(i);
        FreshSlot();
    }

    /// <summary>
    /// 장비를 우클릭 할시 실행 (인벤)
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public void EquipItem(Item item)
    {
        Item tempItem = null;
        //종류체크 (Item.type helmet, hp, weapon, mp)
        switch (item.type)
        {
            case Item.itemType.helmet:
                tempItem = Equipment.items[0];
                Equipment.items[0] = item;
                break;
            case Item.itemType.hpPotion:
                tempItem = Equipment.items[1];
                Equipment.items[1] = item;
                break;
            case Item.itemType.weapon:
                tempItem = Equipment.items[2];
                Equipment.items[2] = item;
                break;
            case Item.itemType.mpPotion:
                tempItem = Equipment.items[3];
                Equipment.items[3] = item;
                break;
            default:
                break;
        }
        if(tempItem!=null) AddItem(tempItem);
        Equipment.FreshSlot();
    }

    /// <summary>
    /// 장비창 아이템 우클릭했을때 해제 (장비)
    /// </summary>
    public void UnequipmentItem(Item item)
    {
        if (items.Count < slots.Length)
        {
            Equipment.DiscardItem((int)item.type);
            AddItem(item);
            item = null;
        }
        else return;

    }
}