using Runtime.Data.ValueObjects;
using UnityEngine;

namespace Runtime.Data.UnityObjects
{
    [CreateAssetMenu(fileName = "CD_Input", menuName = "Picker3DExample/CD_Input", order = 0)]
    public class CD_Input : ScriptableObject
    {
        public InputData data;
    }
}