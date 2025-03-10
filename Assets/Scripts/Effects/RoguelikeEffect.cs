using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Roguelike/Effect")]
public class RoguelikeEffect : ScriptableObject
{
    #region actions
    public event Action<RoguelikeEffect> OnEffectSelected;
    #endregion

    #region serializefields
    [SerializeField] private string _effectName;
    [SerializeField] private string _description;
    #endregion

    public string EffectName => _effectName;
    public string Description => _description;

    public virtual void OnClick()
    {
        ApplyEffect();
    }

    public void ApplyEffect()
    {
        Debug.Log(this.EffectName);
        OnEffectSelected?.Invoke(this);
    }
}