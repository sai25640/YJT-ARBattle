using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ARBattle
{
    public class CharacterAttr:MonoBehaviour
    {
        public string Name;
        public int MaxHP;
        public float MoveSpeed;
        public int Attack;
        public float AtkRange;
        public float AtkRate;
        public string PrefabName;
        public int CurrentHP;
        public bool IsKilled = false;
    }
}
