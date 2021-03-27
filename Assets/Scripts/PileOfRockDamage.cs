using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileOfRockDamage : MonoBehaviour
{

    [SerializeField]
    private int health = 100;

    private Animator animator;
    private Activity activity;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        activity = GetComponent<Activity>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MakeDamage(int damage)
	{
        if (health > damage)
            health -= damage;
		else
		{
            health = 0;
            Fall();
		}
	}

    public void Fall()
	{
        animator.SetTrigger("Fall");
        activity.StopActivity();
	}

}
