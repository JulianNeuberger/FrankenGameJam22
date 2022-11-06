using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleState : State
{
    public Vector3 moveAnchor;
    public float maxDistanceToMoveAnchor = 15f;
    public float aggroRange = 15f;
    public GameObject player;
    public float stalkDistance = 15f;

    public AttackState attackState;
    public MoveState moveState;

    public AudioSource stalkingSound;
    public AudioSource aggroSound;
    public AudioSource particleSound;
    public ToggleFlashlight flashlight;
    
    private Vector3 target;
    [SerializeField] private bool isStalking;
    private Vector3 spawnPos;

    public float stalkAmountGain = .01f;
    [SerializeField] private float stalkChance = 0.0f;
    private ToggleFlashlight _flashlight;

    // Start is called before the first frame update
    void Start()
    {
        _flashlight = player.GetComponentInChildren<ToggleFlashlight>();
        moveAnchor = transform.position;
        spawnPos = transform.position;
        ChooseAnchoredTarget();
    }

    private void Update()
    {
        UpdateStalkChance();
    }

    public override State Tick()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        if (distanceToPlayer < attackState.attackRange)
        {
            return attackState;
        }

        if (distanceToPlayer < aggroRange)
        {
            StartCoroutine(FadeSource(particleSound));
            StartCoroutine(FadeSource(stalkingSound));
            if (!aggroSound.isPlaying)
            {
                Debug.Log("start aggro sound");
                aggroSound.Play();
            }

            moveState.target = player.transform.position;
            moveAnchor = player.transform.position;
            moveState.canCharge = true;
            moveState.arriveRadius = attackState.attackRange;
            return moveState;
        }

        moveState.arriveRadius = 5f;
        moveState.canCharge = false;

        if (isStalking)
        {
            var shouldStopStalking = Random.value > stalkChance;
            if (shouldStopStalking)
            {
                StopStalking();
            }
            else
            {
                MoveToNextPosition();
            }
        }
        else
        {
            // should we start stalking?
            var shouldStalk = Random.value < stalkChance;
            if (shouldStalk)
            {
                StartStalking();
            }
            else
            {
                if (!particleSound.isPlaying && flashlight.IsFlashlightOn())
                {
                    particleSound.Play();
                }
                MoveToNextPosition();
            }
        }

        return moveState;
    }

    private void StopStalking()
    {
        moveState.target = ChooseFarTarget();
        moveAnchor = moveState.target;
        isStalking = false;
    }

    private void StartStalking()
    {
        StartCoroutine(FadeSource(particleSound));
        if (!stalkingSound.isPlaying)
        {
            stalkingSound.Play();
        }
        moveState.target = ChooseStalkTarget();
        moveAnchor = moveState.target;
        isStalking = true;
    }

    private void MoveToNextPosition()
    {
        moveState.target = ChooseAnchoredTarget();
    }

    private void UpdateStalkChance()
    {
        var oldStalkChance = stalkChance;
        if (_flashlight.IsFlashlightOn())
        {
            stalkChance += stalkAmountGain * Time.deltaTime;
        }
        else
        {
            stalkChance -= stalkAmountGain * Time.deltaTime;
        }
    }

    private Vector3 ChooseStalkTarget()
    {
        var newTarget = player.transform.position;
        newTarget += player.transform.forward * stalkDistance;

        var terrain = GetClosestCurrentTerrain(newTarget);
        var terrainHeight = terrain.SampleHeight(newTarget);
        // offset so we dont crawl at the bottom
        terrainHeight += 2f;
        newTarget = new Vector3(newTarget.x, Mathf.Max(terrainHeight, newTarget.y), newTarget.z);
        return newTarget;
    }

    private Vector3 ChooseFarTarget()
    {
        var newTarget = player.transform.position;
        newTarget += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * 25f;
        newTarget.y = player.transform.position.y + Random.Range(-1f, 1f) * 2f;

        var terrain = GetClosestCurrentTerrain(newTarget);
        var terrainHeight = terrain.SampleHeight(newTarget);
        // offset so we dont crawl at the bottom
        terrainHeight += 2f;
        newTarget = new Vector3(newTarget.x, Mathf.Max(terrainHeight, newTarget.y), newTarget.z);
        return newTarget;
    }

    private Vector3 ChooseAnchoredTarget()
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
            newTarget = transform.position + forward * 5f;
        }

        var turnRadius = 2f;
        var targetDir = Random.insideUnitCircle.normalized * turnRadius;
        newTarget += new Vector3(targetDir.x, 0, targetDir.y) * 5f;

        var terrain = GetClosestCurrentTerrain(newTarget);
        var terrainHeight = terrain.SampleHeight(newTarget);
        // offset so we dont crawl at the bottom
        terrainHeight += 2f;
        newTarget = new Vector3(newTarget.x, Mathf.Max(terrainHeight, newTarget.y), newTarget.z);

        return newTarget;
    }

    public override string Description()
    {
        if (isStalking)
        {
            return "stalking";
        }

        return "not stalking";
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
        float lowDist = GetDistanceToTerrain(terrains[0], pos);
        var terrainIndex = 0;

        for (int i = 1; i < terrains.Length; i++)
        {
            //Find the distance and check if it is lower than the last one then store it
            var dist = GetDistanceToTerrain(terrains[i], pos);
            if (dist < lowDist)
            {
                lowDist = dist;
                terrainIndex = i;
            }
        }

        return terrains[terrainIndex];
    }

    private float GetDistanceToTerrain(Terrain terrain, Vector3 pos)
    {
        var distances = new List<float>();
        var terrainPos = terrain.GetPosition();
        var terrainSize = terrain.terrainData.size;

        distances.Add((new Vector3(terrainPos.x, terrainPos.y, terrainPos.z) - pos).sqrMagnitude);
        distances.Add((new Vector3(terrainPos.x + terrainSize.x, terrainPos.y, terrainPos.z) - pos).sqrMagnitude);
        distances.Add((new Vector3(terrainPos.x, terrainPos.y, terrainPos.z + terrainSize.z) - pos).sqrMagnitude);
        distances.Add((new Vector3(terrainPos.x + terrainSize.x, terrainPos.y, terrainPos.z + terrainSize.z) - pos)
            .sqrMagnitude);

        return distances.Min();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    private IEnumerator FadeSource(AudioSource source)
    {
        var startVol = source.volume;
        while (source.volume > 0)
        {
            source.volume -= Time.deltaTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVol;
    }
}