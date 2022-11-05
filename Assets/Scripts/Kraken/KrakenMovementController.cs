using UnityEngine;

public class KrakenMovementController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float huntSpeed = 6f;
    
    public float maxHeight = 10f;
    
    public float turnSpeedDegrees = 25f; 
    
    public Vector3 moveAnchor;
    public float maxDistanceToMoveAnchor = 25f;
    
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        moveAnchor = transform.position;
        ChooseTarget();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
        UpdatePosition();
        if (IsAtTarget())
        {
            ChooseTarget();
        }
    }

    private void ChooseTarget()
    {
        Vector3 newTarget;
        var distanceToMoveAnchor = (moveAnchor - transform.position).magnitude;
        if (distanceToMoveAnchor > maxDistanceToMoveAnchor)
        {
            newTarget = moveAnchor;
        }
        else
        {
            var forward = transform.forward;
            newTarget = transform.position + forward * 15f;
        }
        
        var turnRadius = 2f;
        var targetDir = Random.insideUnitCircle.normalized * turnRadius;
        newTarget += new Vector3(targetDir.x, targetDir.y, 0) * 5f;

        var terrain = GetClosestCurrentTerrain(newTarget);
        var terrainHeight = terrain.SampleHeight(newTarget);
        // offset so we dont crawl at the bottom
        terrainHeight += 2f;
        newTarget = new Vector3(newTarget.x, Mathf.Max(terrainHeight, newTarget.y), newTarget.z);
        
        target = newTarget;
    }

    private void UpdatePosition()
    {
        var vectorToTarget = target - transform.position;
        var angleToTarget = Vector3.SignedAngle(transform.forward, vectorToTarget, Vector3.up);
        angleToTarget *= Mathf.Deg2Rad;
        var moveAmount = Mathf.Clamp(Mathf.Cos(angleToTarget), 0f, 1f);
        transform.position += transform.forward * (moveSpeed * moveAmount * Time.deltaTime);
    }
    
    private void UpdateRotation()
    {
        var targetRot = Quaternion.LookRotation(target - transform.position, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime);
    }

    private bool IsAtTarget()
    {
        var diff = (target - transform.position).magnitude;
        var radius = 6f;
        return diff < radius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target, 1f);
    }
    
    private Terrain GetClosestCurrentTerrain(Vector3 pos)
    {
        //Get all terrain
        Terrain[] terrains = Terrain.activeTerrains;

        //Make sure that terrains length is ok
        if (terrains.Length == 0)
            return null;

        //If just one, return that one terrain
        if (terrains.Length == 1)
            return terrains[0];

        //Get the closest one to the player
        float lowDist = (terrains[0].GetPosition() - pos).sqrMagnitude;
        var terrainIndex = 0;

        for (int i = 1; i < terrains.Length; i++)
        {
            Terrain terrain = terrains[i];
            Vector3 terrainPos = terrain.GetPosition();

            //Find the distance and check if it is lower than the last one then store it
            var dist = (terrainPos - pos).sqrMagnitude;
            if (dist < lowDist)
            {
                lowDist = dist;
                terrainIndex = i;
            }
        }
        return terrains[terrainIndex];
    }
}
