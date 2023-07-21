using System.Collections.Generic;
using UnityEngine;

public sealed class KingCtrl : ISlimeController
{
    private new void Awake()
    {
        base.Awake();
        // King Slime은 몬스터 스포너를 통해 직접 생성되지 않으므로 수동으로 ID 등록을 해 줄 필요가 있다.
        // 몬스터 스포너에 등록은 안합니다.
        // Update 및 FixedUpdate는 MonsterSpawner가 아닌 SlimeBehaviour을 사용합니다.
        MonsterList.Instance.KingPool.Alloc(new ObjectPool<ISlimeController>.PooledObject(this, gameObject.activeSelf));
    }

    private Timer spawnTimer = new Timer() { maxTime = 30f };
    private HashSet<ISlimeController> spawnedSlime = new HashSet<ISlimeController>();
    // 몬스터 인덱스/ID와 관련하여 지옥이 될 파트: 보스의 특별 코스: 스폰
    // 킹의 새싹 스폰은 오브젝트 풀이 오히려 비효율적일까라고 생각이 들긴 하는데...
    // 스폰되는 마릿수에 대한 제한+제약이 필요할 지도 모르겠다.
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
        // slime dead에 대하여 이벤트 처리로 변경을 하여 king에서도 핸들링 가능하도록 하기
    }
    /// <summary>
    /// islimecontroller(seedctrl)에서 return에서 호출
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
            Debug.LogError("이게 뭐야");
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