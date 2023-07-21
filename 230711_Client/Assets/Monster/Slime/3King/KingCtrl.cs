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
}