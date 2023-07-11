#pragma warning disable IDE0051 // for Unity Magic Methods (ex: Update)
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public sealed class UIFadeAnim : MonoBehaviour
{
    [HideInInspector] public ObjectPool pool;
    [SerializeField] private AnimationCurve _anim = default;
    [SerializeField] private float _duration = default;
    private float _elapsed;
    private Graphic _graphic;

    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
    }

    private void OnEnable()
    {
        _elapsed = 0f;
    }

    private void LateUpdate()
    {
        _elapsed += Time.deltaTime;
        if (_elapsed >= _duration)
        {
            if (pool != null)
            {
                pool.Return(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
            return;
        }
        float alpha = _anim.Evaluate(_elapsed / _duration);
        Color c = _graphic.color;
        c.a = alpha;
        _graphic.color = c;
    }
}