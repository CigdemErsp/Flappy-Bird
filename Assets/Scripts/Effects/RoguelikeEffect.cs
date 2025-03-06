using System;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Roguelike/Effect")]
public class RoguelikeEffect : ScriptableObject
{
    #region actions
    public static event Action<RoguelikeEffect> OnEffectApplied;
    #endregion

    #region serializefields
    [SerializeField] private TMP_Text _effectName;
    [SerializeField] private TMP_Text _description;
    #endregion

    public TMP_Text EffectName 
    {  
        get 
        { 
            return _effectName; 
        }
        set
        {
            _effectName = value;
        }
    }

    public TMP_Text Description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
        }
    }

    public virtual void ApplyEffect()
    {
        Debug.Log(this.EffectName);
        OnEffectApplied?.Invoke(this);
    }
}