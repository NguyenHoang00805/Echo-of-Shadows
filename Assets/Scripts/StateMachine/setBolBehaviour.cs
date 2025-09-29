using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setBolBehaviour : StateMachineBehaviour
{ 
    public string boolname;
    public bool updateOnState;
    public bool updateOnStateMachine;
    public bool valueOnEnter, valueOnExit;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            if (updateOnStateMachine)
                animator.SetBool(boolname, valueOnEnter);
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateOnState)
        {
            if (updateOnStateMachine)
                animator.SetBool(boolname, valueOnExit);
        }
    }

    //OnStateMachineEnter is called when entering a state machine via its Entry Node
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine)
            animator.SetBool(boolname, valueOnEnter);
    }

    //OnStateMachineExit is called when exiting a state machine via its Exit Node
    override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        if (updateOnStateMachine) 
            animator.SetBool(boolname, valueOnExit);
    }
}
