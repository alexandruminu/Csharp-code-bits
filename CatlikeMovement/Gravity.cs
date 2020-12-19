using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    [SerializeField] private SO_Float gravity;
    private Rigidbody rbComponent;
	float floatDelay;

	private void Awake()
    {
        rbComponent = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
		if (rbComponent.IsSleeping())
		{
			floatDelay = 0f;
			return;
		}

		if (rbComponent.velocity.sqrMagnitude < 0.0001f)
		{
			floatDelay += Time.deltaTime;
			if (floatDelay >= 1f)
			{
				return;
			}
		}
		else
		{
			floatDelay = 0f;
		}
		rbComponent.AddForce(Vector3.down * gravity.value);
    }
}
