#pragma warning disable IDE0051
#pragma warning disable CS0162
using System;
using UnityEngine;
using URandom = UnityEngine.Random;

[RequireComponent(typeof(TriggerArea))]
[RequireComponent(typeof(SphereCollider))]
public class MonsterSpawner : MonoBehaviour
{
	public static Vector2 Vector2GetRandomPoint(float radius)
	{
		return URandom.insideUnitCircle * radius;
	}

	[SerializeField] private int maxCount = 10;
	[SerializeField] private float coolTimePlayer = 30;
	[SerializeField] private float coolTimeMonster = 30;
	private float elapsedPlayer = 0;
	private float elapsedMonster = 0;
	[HideInInspector] public TriggerArea area;
	[HideInInspector] public SphereCollider _collider;

	[Serializable]
	public struct Prob
	{
		public enum MonsterType
		{
			seed, rabbit
		}
		[Min(1)] public int count;
		public MonsterType type;
	}
	[SerializeField]private Prob[] probs;
	/// <summary>
	/// 소환될 몬스터의 유형을 계산하는 메서드
	/// </summary>
	/// <returns></returns>
	private Prob.MonsterType GetRandomType()
	{
		int sum = 0;
		for (int i = 0; i < probs.Length; i++)
		{
			sum += probs[i].count;
		}
		for (int i = 0; i < probs.Length; i++)
		{
			Prob prob = probs[i];
			if (URandom.Range(0, sum) < prob.count)
			{
				return prob.type;
			}
			sum -= prob.count;
		}
		throw new Exception("WTF");
	}

	[SerializeField]private GameObject monster1; // seed
	[SerializeField]private GameObject monster2; // rabbit
	public ObjectPool monsters1;
	public ObjectPool monsters2;
	public ObjectPoolBehaviour dmgTextPool;

	private void Awake()
	{
		area = GetComponent<TriggerArea>();
		_collider = GetComponent<SphereCollider>();
		monsters1 = new ObjectPool(monster1, this.transform, typeof(ISlimeController));
		monsters2 = new ObjectPool(monster2, this.transform, typeof(ISlimeController));
	}

	/// <summary>
	/// 영역 내의 랜덤한 좌표를 계산하여 반환한다.
	/// </summary>
	public Vector3 GetRandomPoint()
	{
		Vector2 rp = Vector2GetRandomPoint(_collider.radius);
		return transform.localToWorldMatrix.MultiplyPoint(new Vector3(rp.x, 0, rp.y));
	}

	/// <summary>
	/// 몬스터(슬라임)을 소환하는 메서드, 성공 시 ret에 몬스터의 스크립트가 담기며 true가 반환되며, 그렇지 않을 경우 null이 담기며 false가 반환된다.
	/// </summary>
	public bool Spawn(out ISlimeController ret)
	{
		Vector3 point = GetRandomPoint();
		if (Physics.Raycast(new Vector3(point.x, 1e+10f, point.z), Vector3.down, out RaycastHit hit))
		{
			Prob.MonsterType type = GetRandomType();
			ISlimeController gObj;
			switch (type)
			{
				case Prob.MonsterType.seed:
					gObj = (ISlimeController)monsters1.Get();
					break;
				default:
					gObj = null;
					break;
			}
			gObj.transform.SetPositionAndRotation(hit.point, Quaternion.Euler(0f, URandom.Range(0f, 360f), 0f));
			ret = gObj.GetComponent<ISlimeController>();
			ret.spawnPoint = gObj.transform.position;
			ret.dmgTextPool = this.dmgTextPool;
			return true;
		}
		ret = null;
		return false;
	}
	/// <summary>
	/// 몬스터가 죽었을 때 호출되는 메서드, ISlimeController에서 호출이 된다.
	/// </summary>
	public void OnSlimeDead(ISlimeController slime)
	{
		if (slime is SeedCtrl)
		{
			monsters1.Return(slime);
		}
		else if (slime is HareCtrl)
		{
			monsters2.Return(slime);
		}
		else
		{
			if (monsters1.tObj.Contains(slime))
			{
				monsters1.Return(slime);
			}
			else if (monsters2.tObj.Contains(slime))
			{
				monsters2.Return(slime);
			}
			else
			{
				Debug.LogError("MonsterSpawner: Invalid Type");
			}
		}
	}

	private void FixedUpdate()
	{
		int count = monsters1.tObj.Count + monsters2.tObj.Count;
		if (area.EnteredPlayer.Count == 0)
		{
			elapsedPlayer += Time.fixedDeltaTime;
			if (elapsedPlayer >= coolTimePlayer)
			{
				monsters1.ReturnAll();
				monsters2.ReturnAll();
				elapsedMonster = 0;
				return;
			}
		}
		else
		{
			elapsedPlayer = 0;
			if (MONSTER_SETTINGS.SPAWN == 1)
			{
				// test
				if (count == 0)
				{
					elapsedMonster = coolTimeMonster;
				}
			}

            // 몬스터 생성
            if (count < maxCount)
            {
                if (MONSTER_SETTINGS.SPAWN == 2)
                {
                    elapsedMonster += (maxCount - count) * 0.5f / coolTimeMonster;
                    while (elapsedMonster >= 1f)
                    {
                        elapsedMonster -= 1f;
                        Spawn(out ISlimeController slime);
                        if (slime != null)
                        {
                            count++;
                            slime.spawner = this;
                            if (count == maxCount)
                            {
                                elapsedMonster = 0f;
                                return;
                            }
                        }
                    }
                }
                else if (MONSTER_SETTINGS.SPAWN == 1)
                {
                    elapsedMonster += Time.fixedDeltaTime;
                    if (elapsedMonster >= coolTimeMonster)
                    {
                        elapsedMonster = 0f;
                        uint c = (uint)maxCount - (uint)count;
                        if (c % 2 == 1) { c++; } // 올림을 위해
                        c = c >> 1;
                        while (c > 0)
                        {
                            Spawn(out ISlimeController temp);
                            if (temp != null)
                            {
                                c--;
                                count++;
                                temp.spawner = this;
                            }
                        }
                    }
                }
            }
        }

		// 리스트를 돌 때 리스트가 변경되도 상관 없게 방향을 역방향으로 수정
		// 다만 버그의 위험이 없진 않다.
		var list = monsters1.tObj;
		for (int i = list.Count; i > 0;)
		{
			ISlimeController ctrl = (ISlimeController)list[--i];
			ctrl.OnFixedUpdate();
		}
	}

	private void Update()
	{
		// 리스트를 돌 때 리스트가 변경되도 상관 없게 방향을 역방향으로 수정
		// 다만 버그의 위험이 없진 않다.
		var list = monsters1.tObj;
		for (int i = list.Count; i > 0;)
		{
			ISlimeController ctrl = (ISlimeController)list[--i];
			ctrl.OnUpdate();
		}
	}
}