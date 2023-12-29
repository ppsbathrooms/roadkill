using System.Collections.Generic;
using UnityEngine;

namespace Vehicles.Controllers
{
    public class CombineController : VehicleController
    {
        [Header("Vehicle Specific Settings")]
        [SerializeField] public GameObject attachmentPoint;
        [SerializeField] public Dictionary<GameObject, GameObject> attachments = new Dictionary<GameObject, GameObject>();
        [SerializeField] public GameObject _activeAttachment;


    }
}
