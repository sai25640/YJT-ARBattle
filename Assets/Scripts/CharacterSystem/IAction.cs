using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattle
{
    public interface IAction
    {
        void Attack(BaseCharacter target);

        void UnderAttack(BaseCharacter target);

        void Killed();

        void Move(BaseCharacter target);

    }
 }
