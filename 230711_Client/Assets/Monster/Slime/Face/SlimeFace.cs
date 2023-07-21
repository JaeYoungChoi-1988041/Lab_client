#pragma warning disable IDE0044 // Add readonly modifier
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Monster/Slime/Face")]
public sealed class SlimeFace : ScriptableObject
{
    [SerializeField]
    private Texture IDL = default,
        MOV = default,
        JMP = default,
        ATK = default,
        DMG = default;

    public enum Type
    {
        Idle,
        Walk,
        Jump,
        ATK,
        DMG
    }
    public Texture GetTexture(Type type)
    {
        switch (type)
        {
            case Type.Idle:
                return IDL;
            case Type.Walk:
                return MOV;
            case Type.Jump:
                return JMP;
            case Type.ATK:
                return ATK;
            case Type.DMG:
                return DMG;
            default:
#if DEBUG
                Debug.LogError("SlimeFace::GetTexture(Type type) - [param]Type is invalid");
#endif
                return null;
        }
    }
}