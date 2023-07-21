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
}