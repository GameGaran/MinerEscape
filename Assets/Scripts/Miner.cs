using Michsky.UI.ModernUIPack;

using System;

using UnityEngine;

public class Miner : MonoBehaviour
{

	[SerializeField]
	private int waterResource = 20;

	[SerializeField]
	private int energyResource = 20;

	[SerializeField]
	private int foodResource = 20;

	[SerializeField]
	private int damageAmount = 10;

	[SerializeField]
	private int workWaterDrainage = 3;

	[SerializeField]
	private int workEnergyDrainage = 3;

	[SerializeField]
	private int workFoodDrainage = 3;

	[SerializeField]
	private int pumpWaterDrainage = -10;

	[SerializeField]
	private int pumpEnergyDrainage = 2;

	[SerializeField]
	private int pumpFoodDrainage = 1;

	[SerializeField]
	private int gardeningWaterDrainage = 1;

	[SerializeField]
	private int gardeningEnergyDrainage = 1;

	[SerializeField]
	private int gardeningFoodDrainage = -10;

	[SerializeField]
	private int sleepWaterDrainage = 1;

	[SerializeField]
	private int sleepEnergyDrainage = -10;

	[SerializeField]
	private int sleepFoodDrainage = 1;

	[SerializeField]
	private int activityDuration = 5;

	[SerializeField]
	private float atSpotThreshold = 0.1f;

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private Activity currentActivity;

	[SerializeField]
	private Transform activitySpot;

	[SerializeField]
	private ProgressBar energyProgressBar;

	private bool atSpot = false;
	private Animator animator;

	// Start is called before the first frame update
	void Start()
	{
		this.animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if (!atSpot && activitySpot != null)
		{
			GoToSpot();
		}

	}

	private void DoActivity()
	{
		if (currentActivity == null)
			return;
		switch (currentActivity.Type)
		{
			case Activity.ActivityType.PileOfRock:
				PileOfRockActivity();
				break;
			case Activity.ActivityType.WaterPump:
				PumpActivity();
				break;
			case Activity.ActivityType.Gardening:
				GardeningActivity();
				break;
			case Activity.ActivityType.Sleeping:
				SleepActivity();
				break;
			default:
				break;
		}

	}

	private void StartAnimation()
	{
		animator.ResetTrigger("StartPickaxe");
		animator.ResetTrigger("Walk");

		if (!atSpot)
		{
			animator.SetTrigger("Walk");
			return;
		}
		switch (currentActivity.Type)
		{
			case Activity.ActivityType.PileOfRock:
				animator.SetTrigger("StartPickaxe");
				break;
			case Activity.ActivityType.WaterPump:
				break;
			case Activity.ActivityType.Gardening:
				break;
			case Activity.ActivityType.Sleeping:
				break;
			default:
				break;
		}
	}

	private void GoToSpot()
	{
		var pos = new Vector2(transform.position.x, transform.position.y);
		var spotPos = new Vector2(activitySpot.position.x, activitySpot.position.y);
		var dist = (pos - spotPos).magnitude;
		if (dist < atSpotThreshold)
		{
			atSpot = true;
			StartAnimation();
			return;
		}
		if (dist <= speed * Time.deltaTime)
		{
			transform.position = activitySpot.position;
			StartAnimation();
			atSpot = true;
		}
		var direction = (spotPos - pos).normalized;
		var localScale = transform.localScale;
		var xScale = Mathf.Abs(localScale.x);
		if(direction.x < 0)
		{
			xScale = -xScale;
		}
			transform.localScale = new Vector3(xScale, localScale.y, localScale.z);
		transform.position = transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
	}

	public bool SetActivity(Activity activity)
	{
		if (activity == null)
		{
			activitySpot = null;
			currentActivity = null;
			atSpot = false;
			StartAnimation();
			return true;
		}
		if(currentActivity != null)
		{
			currentActivity.FreeSpot(activitySpot);
		}
		var spot = activity.GetFreeSpot(this);
		if (spot == null)
			return false;
		currentActivity = activity;
		activitySpot = spot;
		atSpot = false;
		StartAnimation();
		return true;
	}

	private void PileOfRockActivity()
	{
		currentActivity.gameObject.GetComponent<PileOfRockDamage>().MakeDamage(damageAmount);
		this.waterResource -= this.workWaterDrainage;
		this.energyResource -= this.workEnergyDrainage;
		this.foodResource -= this.workFoodDrainage;
	}

	private void PumpActivity()
	{
		this.waterResource -= this.pumpWaterDrainage;
		this.energyResource -= this.pumpEnergyDrainage;
		this.foodResource -= this.pumpFoodDrainage;
	}

	private void GardeningActivity()
	{
		this.waterResource -= this.gardeningWaterDrainage;
		this.energyResource -= this.gardeningEnergyDrainage;
		this.foodResource -= this.gardeningFoodDrainage;
	}

	private void SleepActivity()
	{
		this.waterResource -= this.sleepWaterDrainage;
		this.energyResource -= this.sleepEnergyDrainage;
		this.foodResource -= this.sleepFoodDrainage;
	}

}
