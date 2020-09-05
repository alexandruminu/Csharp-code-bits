using System.Collections.Generic;
using UnityEngine;

public class TabTargeting : MonoBehaviour
{
    public Transform targetArrow;
    public List<Targetable> availableTargets = new List<Targetable>();
    public Targetable currentTarget;
    public int currentTargetIndex = 0;
    public KeyCode changeTargetKey;

    private void Update()
    {
        if (Input.GetKeyDown(changeTargetKey))
        {
            if(availableTargets.Count > 0)
            {
                currentTargetIndex++;
                currentTarget = availableTargets[currentTargetIndex % availableTargets.Count];
                targetArrow.position = currentTarget.transform.position;
                targetArrow.gameObject.SetActive(true);
            }
            else
            {
                targetArrow.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Targetable targetable = other.GetComponent<Targetable>();
        if (targetable)
        {
            if (!availableTargets.Contains(targetable))
            {
                availableTargets.Add(targetable);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Targetable targetable = other.GetComponent<Targetable>();
        if (targetable)
        {
            availableTargets.Remove(targetable);
        }
    }
}
