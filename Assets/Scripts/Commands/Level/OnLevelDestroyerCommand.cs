using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Commands.Level
{
    public class OnLevelDestroyerCommand
    {
        private Transform _levelHolder;

        public OnLevelDestroyerCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        public void Execute()
        {
            if(_levelHolder.childCount<=0) return;
            Object.Destroy(_levelHolder.GetChild(0));
        }
    }
}