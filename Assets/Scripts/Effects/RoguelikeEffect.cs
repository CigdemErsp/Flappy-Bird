using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Roguelike/Effect")]
public class RoguelikeEffect : ScriptableObject
{
    #region serializefields
    [SerializeField] private string _effectName;
    [SerializeField] private string _description;
    #endregion

    public string EffectName => _effectName;
    public string Description => _description;
}