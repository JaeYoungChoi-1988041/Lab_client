// 씨앗뱉기만 구현하면 완료 (그 외의 기능(돌격, 이동 등... )은 ISlimeController에 구현되어 있음)
// 씨앗뱉기: Skill1
using UnityEngine;

public sealed class SeedCtrl : ISlimeController
{
    [SerializeField] private GameObject seedProjectile;
    public TriggerArea attackRange2; // 공격범위의 3배에 이내 1배 이외에선 씨앗뱉기를 한다.

    #region State<Skill1>
    private void Shoot(Vector3 pos, Vector3 dir)
    {
        Quaternion q = Quaternion.LookRotation(dir);
        GameObject gObj = Instantiate(seedProjectile, pos, q);
    }

    private bool Skill1_started = false;
    private Timer Skill1_attackTimer = new Timer() { maxTime = 0.125f };
    private void Skill1_OnEnter()
    {
        Skill1_started = false;
        Skill1_attackTimer.elapsed = 0f;
        if (MONSTER_SETTINGS.FACE)
        {
            this._face.BaseTexture = this._face.face.GetTexture(SlimeFace.Type.ATK);
        }
        _agent.isStopped = true;

        Shoot(transform.position, transform.forward);
    }
    private void Skill1_OnFixedUpdate()
    {
        if (Skill1_started)
        {
            if (Skill1_attackTimer.Update(Time.fixedDeltaTime, out float _))
            {
                _agent.isStopped = false;
                NextAttack();
            }
        }
        else
        {
            if (_target.ST == StateType.Die)
            {
                Targeting(null);
            }
            else
            {
                bool entered = attackRange2.EnteredPlayer.Contains(_target);

                this._agent.destination = this._target.transform.position;
                Vector3 v1 = this.transform.forward;
                Vector3 v2 = (this._target.transform.position - this.transform.position);
                Vector2 v3 = new Vector2(v1.x, v1.z).normalized;
                Vector2 v4 = new Vector2(v2.x, v2.z).normalized;
                float view = Vector2.Dot(v3, v4);
                if (entered)
                {
                    if (view > 0.98f)
                    {
                        //Debug.Log("Shoot!");
                        Skill1_started = true;
                    }
                    else
                    {
                        // in Complex: v4 / v3 = v4 * 공액(v3) = (x4 + y4i)(x3 - y3i)
                        // = x4x3 + y4y3 + y4x3i - y3x4i
                        v4 = new Vector2(v4.x * v3.x + v4.y * v3.y, v4.y * v3.x - v3.y * v4.x);
                        float theta = Mathf.Atan2(v4.y, v4.x);
                        this.transform.Rotate(0, Mathf.Sign(theta) * Time.fixedDeltaTime * -120, 0);
                    }
                }
                else
                {
                    this.ChangeState(StateEnum.Walk);
                }
            }
        }
    }
    #endregion

    protected override void NextAttack()
    {
        Player player;
        GetNextTarget(out player);
        if (player == _target)
        {
            if (_attackRange.EnteredPlayer.Contains(_target))
            {
                ChangeState(StateEnum.Attack);
            }
            else if (_attackRange.EnteredPlayer.Contains(_target))
            {
                ChangeState(StateEnum.Skill1);
            }
        }
        else
        {
            Targeting(player);
        }
    }

    public override void Attack_WhenExit()
    {
        if (attackRange2.EnteredPlayer.Contains(_target))
        {
            ChangeState(StateEnum.Skill1);
        }
        else
        {
            base.Attack_WhenExit();
        }
    }
}