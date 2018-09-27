using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Towers.Placement;
using Core.Utilities;

public class TowerPlacement : MonoBehaviour
{
    public Ray camRay;
    public LayerMask hitLayers;

    private void Start()
    {
        
    }
    public void OnDrawGizmos()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(camRay.origin, camRay.origin + camRay.direction * 1000f);
    }

    public void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(camRay, out hit, 1000f, hitLayers))
            {
                //Check if hitting grid
                IPlacementArea placement = hit.collider.GetComponent<IPlacementArea>();
                if (placement != null)
                {
                    //Snap position tower to Grid element
                    transform.position = placement.Snap(hit.point, new IntVector2(1, 1));
                }
            }
        }
    }
}
