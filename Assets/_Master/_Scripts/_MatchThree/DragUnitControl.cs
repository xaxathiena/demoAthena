using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DragUnitControl : MonoBehaviour
{
    [SerializeField] private float timeToDesicionDirection = 0.2f;
    [SerializeField] private float dragSensitive;
    private Vector3 currentPosition;
    private GameObject target;
    private bool isMouseDragging;
    private Vector3 screenPosition, offset;
    private float currentTimeToDesicion;
    private Vector3 beginPoisition;
    private Vector3 dragDirection;
    private BlockUIControl currentBlockChoose;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentTimeToDesicion = 0f;
            RaycastHit hitInfo;
            target = ReturnClickedObject(out hitInfo);
            beginPoisition = Input.mousePosition;
            if (target != null)
            {
                currentBlockChoose = target.transform.parent.GetComponent<BlockUIControl>();
                isMouseDragging = true;
                //Here we Convert world position to screen position.
                screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDragging = false;
        }

        if (isMouseDragging)
        {
            currentTimeToDesicion += Time.deltaTime;
            if (currentTimeToDesicion >= timeToDesicionDirection)
            {
                isMouseDragging = false;
                dragDirection = Input.mousePosition - beginPoisition;
                if (dragDirection.magnitude > dragSensitive)
                {
                    
                    // Debug.Log(dragDirection + " dragDirection: " + DirectionFromVector(dragDirection) );
                    BoardUIManager.instance.Swap(currentBlockChoose,DirectionFromVector(dragDirection));
                    return;
                    //tracking mouse position.
                    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);

                    //convert screen position to world position with offset changes.
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

                    //It will update target gameobject's current postion.
                    target.transform.position = currentPosition;
                }
            }
        }

    }

    private string DirectionFromVector(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "r" : "l";
        }
        else
        {
            return direction.y > 0 ? "u" : "d";
        }
    }
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject targetObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            targetObject = hit.collider.gameObject;
        }
        return targetObject;
    }
}