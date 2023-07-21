using System.Collections.Generic;
using UnityEngine;

public sealed class KingCtrl : ISlimeController
{
    private new void Awake()
    {
        base.Awake();
        // King Slime�� ���� �����ʸ� ���� ���� �������� �����Ƿ� �������� ID ����� �� �� �ʿ䰡 �ִ�.
        // ���� �����ʿ� ����� ���մϴ�.
        // Update �� FixedUpdate�� MonsterSpawner�� �ƴ� SlimeBehaviour�� ����մϴ�.
        MonsterList.Instance.KingPool.Alloc(new ObjectPool<ISlimeController>.PooledObject(this, gameObject.activeSelf));
    }

    private Timer spawnTimer = new Timer() { maxTime = 30f };
    private HashSet<ISlimeController> spawnedSlime = new HashSet<ISlimeController>();
    // ���� �ε���/ID�� �����Ͽ� ������ �� ��Ʈ: ������ Ư�� �ڽ�: ����
    // ŷ�� ���� ������ ������Ʈ Ǯ�� ������ ��ȿ�����ϱ��� ������ ��� �ϴµ�...
    // �����Ǵ� �������� ���� ����+������ �ʿ��� ���� �𸣰ڴ�.
    private void SpawnSeed(Vector3 pos, Quaternion rot)
    {
        var seedPool = MonsterList.Instance.SeedPool;
        var pooledMonster = seedPool.Get();
#if DEBUG
        if (!pooledMonster.active || !pooledMonster.@object.gameObject.activeSelf)
        {
            Debug.LogError("Monster is disabled");
        }
#endif
        var monster = pooledMonster.@object;
        monster.transform.SetPositionAndRotation(pos, rot);
        // slime dead�� ���Ͽ� �̺�Ʈ ó���� ������ �Ͽ� king������ �ڵ鸵 �����ϵ��� �ϱ�
    }
    /// <summary>
    /// islimecontroller(seedctrl)���� return���� ȣ��
    /// </summary>
    public void OnSeedDead(ISlimeController seed)
    {
        if (seed == null) { return; }
        spawnedSlime.Remove(seed);
        if (seed is SeedCtrl)
        {
            MonsterList.Instance.SeedPool.Return(seed);
        }
        else
        {
            Debug.LogError("�̰� ����");
        }
    }
    private void ReturnAll()
    {
        var set = spawnedSlime;
        HashSet<ISlimeController>.Enumerator @enum = set.GetEnumerator();
        while(@enum.MoveNext())
        {
            var seed = @enum.Current;
            MonsterList.Instance.SeedPool.Return(seed);
        }
        @enum.Dispose();
        set.Clear();
    }
}