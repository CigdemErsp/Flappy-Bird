using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDatabase", menuName = "Roguelike/Effect Database")]
public class EffectDatabase : ScriptableObject
{
    #region serializefields
    [SerializeField] private List<RoguelikeEffect> _effects;
    #endregion

    public List<RoguelikeEffect> Effects { get { return _effects; } }
}