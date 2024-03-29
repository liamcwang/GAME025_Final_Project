using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveAttack : AIBehavior
{
    [SerializeField] float reactTimer;
    bool canAttack;
    Attack attack;
    

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();
        canAttack = true;    
    }

    public override void Act(Enemy e)
    {
        if (canAttack) {
            bool playerFound = attack.SimulateCast();
            if (playerFound)  {
                attack.Act();
                canAttack = false;
                StartCoroutine(ReactTimer());
            }
        }
        
    }

    IEnumerator ReactTimer() {
        yield return new WaitForSeconds(reactTimer);
        canAttack = true;
    }
}
