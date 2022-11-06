using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public GameObject player;
    public float attackRange = 4.5f;
    public IdleState idleState;

    public AudioSource aggroMusic;
    public AudioSource gameOverMusic;

    public StateMachine StateMachine;

    public override State Tick()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        if (distanceToPlayer > attackRange)
        {
            return idleState;
        }

        StartCoroutine(FadeSource(aggroMusic));
        if (!gameOverMusic.isPlaying)
        {
            gameOverMusic.Play();
        }

        StateMachine.enabled = false;

        if (NetworkSyncer.Get())
        {
            NetworkSyncer.Get().SetGameToLostServerRpc();
        }

        return this;
    }

    public override string Description()
    {
        var distanceToPlayer = (transform.position - player.transform.position).magnitude;
        return distanceToPlayer.ToString();
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