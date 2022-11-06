using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;
using UnityEngine.UI;

public class ElectroShock : UseableButton
{
    public float cooldownSeconds = 10f;
    public float energyCost = 0f;

    public LightningBoltScript lightning;
    public int numBolts = 5;

    public Light flash;

    public Image icon;

    public Color activeColor;
    public Color inactiveColor;

    private float lastShock = float.MinValue;

    private List<SharkController> inRange = new();
    private AudioSource _thunderAudio;

    private void Start()
    {
        flash.gameObject.SetActive(false);
        _thunderAudio = GetComponentInChildren<AudioSource>();
    }

    public override void Use()
    {
        DoShock();
    }

    private void Update()
    {
        if (Input.GetKeyUp("e"))
        {
            DoShock();
        }

        icon.color = CanShock() ? activeColor : inactiveColor;
    }

    public void DoShock()
    {
        if (!CanShock())
        {
            return;
        }

        lastShock = Time.time;
        _thunderAudio.Play();
        SpawnEffect();

        foreach (var shark in inRange)
        {
            shark.Scare(transform.position);
        }
    }

    private bool CanShock()
    {
        return Time.time - lastShock > cooldownSeconds;
    }

    private void SpawnEffect()
    {
        flash.gameObject.SetActive(true);
        var bolts = new List<GameObject>();
        for (var i = 0; i < numBolts; i++)
        {
            var instance = Instantiate(lightning);
            var start = transform.position + transform.forward * 3f;
            var shootDir = Quaternion.AngleAxis(Random.Range(-25f, 25f), Vector3.up) *
                           Quaternion.AngleAxis(Random.Range(-25f, 25f), Vector3.right) * transform.forward;
            var end = start + shootDir * 10f;
            instance.StartObject.transform.position = start;
            instance.EndObject.transform.position = end;
            bolts.Add(instance.gameObject);
        }

        var coroutine = TriggerRemoveEffects(bolts, .3f);
        StartCoroutine(coroutine);
    }

    private IEnumerator TriggerRemoveEffects(List<GameObject> bolts, float delay)
    {
        var startedAt = Time.time;
        
        yield return new WaitForSeconds(.1f);
        
        while (Time.time - startedAt < delay)
        {
            flash.intensity = Mathf.Lerp(flash.intensity, 0.0f, Time.deltaTime);
            yield return null;
        }

        foreach (var bolt in bolts)
        {
            Destroy(bolt.gameObject);
        }
        flash.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null)
        {
            return;
        }
        
        var shark = other.transform.parent.GetComponent<SharkController>();
        if (shark == null)
        {
            return;
        }

        inRange.Add(shark);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null)
        {
            return;
        }

        var shark = other.transform.parent.GetComponent<SharkController>();
        if (shark == null)
        {
            return;
        }

        inRange.Remove(shark);
    }
}