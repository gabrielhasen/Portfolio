using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDeath : MonoBehaviour
{
    public UnityEvent Event_OnDeath;
    public Character character;
    private bool Stop;
    void Start()
    {
        Stop = false;
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (Stop) {
            return;
        }

        if (character.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead)
        {
            Stop = true;
            Event_OnDeath.Invoke();
        }

    }
}
