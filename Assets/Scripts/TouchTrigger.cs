using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTrigger : MonoBehaviour
{
    private enum TriggerType {DEATH, WIN, ITEM, DAMAGE};
    public bool isActive = true;
    [SerializeField] private TriggerType triggerType;
    [SerializeField] int damage;
    [SerializeField] Vector2 bounceForce = new Vector2(10f, 10f);
    [SerializeField] float bounceTimer = 0.5f;

    Animator anim;

    private void Start()
    {
        switch (triggerType)
        {
            case TriggerType.ITEM:
            anim = GetComponent<Animator>();
            break;
            default:
            break;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        

        Player p = other.gameObject.GetComponent<Player>();
        if (p != null) {
            switch (triggerType)
            {
                case TriggerType.DEATH:
                    p.Die();
                    break;
                case TriggerType.WIN:
                    GameManager.Victory();
                    break;
                case TriggerType.ITEM:
                    ItemGet();
                    break;
                case TriggerType.DAMAGE:
                    p.hurtbox.takeDamageBounce(damage, transform.position, bounceForce, bounceTimer);
                    break;
                default:
                    break;
            }
        }    
    }

    private void ItemGet() {
        isActive = false;
        anim.Play(anim.name + "Open");
        
    }

}
