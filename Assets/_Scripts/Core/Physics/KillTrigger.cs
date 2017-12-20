using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Kill any IKillable instance on contact
/// <summary>
public class KillTrigger : MonoBehaviour
{

    #region Attributes
    [SerializeField]
    private List<string> tagToKill;

    [SerializeField]
	private bool killOnEnter = true;

	[SerializeField]
	private bool killOnExit = false;
    #endregion

    #region Core

    private void TryKill(GameObject other)
    {
        IKillable killable = other.GetComponent<IKillable>();
        if (killable != null)
        {
            if (tagToKill.Count > 0)
            {
                for (int i = 0; i < tagToKill.Count; i++)
                {
                    if (other.tag == tagToKill[i])
                    {
                        killable.Kill();
                        return;
                    }
                }
                return;
            }
            killable.Kill();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
		if (killOnEnter)
		{
			TryKill (other.gameObject);
		}
    }

	private void OnTriggerExit(Collider other)
	{
		if (killOnExit)
		{
			TryKill (other.gameObject);
		}
	}

    #endregion
}
