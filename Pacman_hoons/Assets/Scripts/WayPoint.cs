using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

	public List<WayPoint> neighborsList = new List<WayPoint>();
	public List<Vector2> validDirections = new List<Vector2>();

	public bool isPortal;
	public WayPoint connectPortal;
    public bool isEntranceToGhostHouse;
    public bool isGhostHouse;


    private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, 0.3f);

		if (neighborsList.Count > 0)
		{
			foreach(WayPoint _point in neighborsList)
			{
				Gizmos.DrawLine(transform.position, _point.transform.position);
			}
		}
	}

	void Start()
    {
		RaycastHit2D upHit = Physics2D.Raycast(transform.position + Vector3.up * .5f, Vector2.up, 50, 3 << 8);
		if (upHit != false)
		{
			if (upHit.transform.CompareTag("WayPoint"))
			{
				neighborsList.Add(upHit.transform.gameObject.GetComponent<WayPoint>());
				validDirections.Add(Vector2.up);
			}
		}
		RaycastHit2D downHit = Physics2D.Raycast(transform.position + Vector3.down * .5f, Vector2.down, 50, 3 << 8);
		if (downHit != false)
		{
			if (downHit.transform.CompareTag("WayPoint"))
			{
				neighborsList.Add(downHit.transform.gameObject.GetComponent<WayPoint>());
				validDirections.Add(Vector2.down);
			}
		}
		RaycastHit2D leftHit = Physics2D.Raycast(transform.position + Vector3.left * .5f, Vector2.left, 50, 3 << 8);
		if (leftHit != false)
		{
			if (leftHit.transform.CompareTag("WayPoint"))
			{
				neighborsList.Add(leftHit.transform.gameObject.GetComponent<WayPoint>());
				validDirections.Add(Vector2.left);
			}
		}
		RaycastHit2D rightHit = Physics2D.Raycast(transform.position + Vector3.right * .5f, Vector2.right, 50, 3 << 8);
		if (rightHit != false)
		{
			if (rightHit.transform.CompareTag("WayPoint"))
			{
				neighborsList.Add(rightHit.transform.gameObject.GetComponent<WayPoint>());
				validDirections.Add(Vector2.right);
			}
		}
	}
}
