using UnityEngine;

public class SlimeFaceManager : MonoBehaviour
{
    private Material mat;
    private void Awake()
    {
        mat = GetComponentInChildren<SkinnedMeshRenderer>().materials[1];
    }

    public SlimeFace face;
    private Texture baseTex;
    private Texture overrideTex;
    [HideInInspector] public float duration;
    private float elapsed;

    /// <summary>
    /// 베이스 표정을 지정한다.
    /// </summary>
    public Texture BaseTexture
    {
        get => this.baseTex;
        set
        {
            if (this.baseTex != value)
            {
                this.baseTex = value;
                if (this.overrideTex == null)
                {
                    mat.mainTexture = this.baseTex;
                }
            }
        }
    }
    /// <summary>
    /// 오버라이드 표정을 지정한다. null이 아니면 이 표정이 우선적으로 출력이 된다.
    /// </summary>
    public Texture OverrideTexture
    {
        get => this.overrideTex;
        set
        {
            if (this.overrideTex != value)
            {
                this.overrideTex = value;
                if (this.overrideTex != null)
                {
                    mat.mainTexture = this.overrideTex;
                    elapsed = 0f;
                }
                else
                {
                    mat.mainTexture = this.baseTex;
                }
            }
        }
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            OverrideTexture = null;
        }
    }
}