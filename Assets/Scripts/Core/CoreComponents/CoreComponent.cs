using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlant
{
    public class CoreComponent : MonoBehaviour
    {
        protected Core _core;
        protected virtual void Awake()
        {
            _core = transform.parent.GetComponent<Core>();
            if (_core == null)
            {
                Debug.LogError("There is no Core on the parent.");
            }

        }
    }
}