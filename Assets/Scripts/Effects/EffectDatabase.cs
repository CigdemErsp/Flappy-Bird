using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectDatabase", menuName = "Roguelike/Effect Database")]
public class EffectDatabase : ScriptableObject
{
    #region serializefields
    [SerializeField] private List<RoguelikeEffect> _effects;
    #endregion
}