#pragma warning disable IDE0032 // auto
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public sealed class SlimeMov : MonoBehaviour
{
    [SerializeField] private string jumpStateName;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isJumping;
    public bool IsJumping => isJumping;
    private bool doJump;

    public void Init(Animator animator, NavMeshAgent agent)
    {
        this.animator = animator;
        this.agent = agent;
    }
    private void Awake()
    {
        var animator = GetComponent<Animator>();
        var agent = GetComponent<NavMeshAgent>();
        Init(animator, agent);
    }

    public void AnimEvent(int i)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(jumpStateName))
        {
            switch ((ISlimeController.AnimEventType)i)
            {
                case ISlimeController.AnimEventType.Start:
                    isJumping = true;
                    break;
                case ISlimeController.AnimEventType.End:
                    isJumping = false;
                    animator.SetFloat("Speed", 0f);
                    break;
            }
        }
    }

    private Vector3 prev;
    private Vector3 prevDir;
    public void Jump(Vector3 dest)
    {
        if (!doJump)
        {
            prev = transform.position;
        }
        if (!isJumping)
        {
            agent.destination = dest;
            NavMeshPath path = agent.path;
            Vector3[] corners = path.corners;
            if (corners.Length >= 2)
            {
                Vector3 dir = corners[1] - corners[0];
                dir.y = 0;
                dir.Normalize();

                Vector3 dir2 = transform.forward;
                dir2.y = 0;
                dir2.Normalize();

                float dot = Vector3.Dot(dir, dir2); // cos theta

                Vector3 dir3 = dest - prev;
                dir3.y = 0;
                dir3.Normalize();

                Vector4 dir4 = dest - transform.position;
                dir4.y = 0;
                dir4.Normalize();

                if (dot > 0.98f)
                {
                    doJump = true;
                    isJumping = true;
                    animator.SetFloat("Speed", 1f);
                    prevDir = Vector3.zero;
                }
                else if (doJump = true && Vector3.Dot(dir3, prevDir) > 0.01 && Vector3.Dot(dir4, prevDir) < -0.01)
                //else if (doJump && dot < 0)
                {
                    doJump = false;
                }

                prevDir = prev;
            }
        }
    }

    private bool input;
    private Vector3 dest;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                input = true;
                dest = hit.point;
            }
        }
    }
    private void FixedUpdate()
    {
        if (input)
        {
            Jump(dest);
        }
    }

    private void OnAnimatorMove()
    {
        // apply root motion to AI
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        agent.nextPosition = transform.position = position;
    }
}