using System;
using System.Collections;
using System.Collections.Generic;
using FlyBattle.Utils;
using UnityEngine;

[Serializable]
public class PlayerLanding : MonoBehaviour
{
    public GameObject Parachutist;
    public event Action DisLandingPlayer = delegate { };

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    void Landing()
    {
        GameObject newCharacted = ObjectPool.Spawn(Parachutist);
    }
}