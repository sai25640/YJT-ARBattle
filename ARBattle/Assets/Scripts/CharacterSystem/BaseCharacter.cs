using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattle
{
    [RequireComponent(typeof(CharacterAttr))]
    public abstract class BaseCharacter : MonoBehaviour
    {
        public CharacterAttr Attr;
        protected FSMSystem mFSMSystem;

        public virtual void Awake()
        {
            Attr = GetComponent<CharacterAttr>();
        }

        public abstract void Attack(BaseCharacter target);

        public abstract void Killed();

        public abstract void Move(BaseCharacter target);

        public abstract void UnderAttack(BaseCharacter target);
    
        public virtual void PerformTransition(Transition trans)
        {
            mFSMSystem.PerformTransition(trans);
        }
    }
}
