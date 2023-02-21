using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Knight
{
    // Start is called before the first frame update
    protected override void DoHit(Transform nearest)
    {
        nav.speed = 0f;
        if (currHitTime > hitTime)
        {
            hitTime = Random.Range(1f, maxHitTime);
            currHitTime = 0f;
            anim.SetTrigger("attack");
            nearest.gameObject.GetComponent<Knight>().GetHit();
        }
    }
}
