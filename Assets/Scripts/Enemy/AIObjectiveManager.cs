using System.Collections.Generic;
using UnityEngine;

public class AIObjectiveManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private string PlayerCastleTagName = "PlayerCastle";
    [SerializeField] private string PlayerTagName = "Player";
    [SerializeField] private int MaxAmmountOfEnemiesAttackingPlayer = 5;
#pragma warning restore 0649

    private Transform PlayerTransform
    {
        get
        {
            if (cachedPlayerTransform == null)
                cachedPlayerTransform = GameObject.FindGameObjectWithTag(PlayerTagName)?.GetComponent<Transform>();
            return cachedPlayerTransform;
        }
    }
    
    private Transform CastleTransform
    {
        get
        {
            if (cachedCastleTransform == null)
                cachedCastleTransform = GameObject.FindGameObjectWithTag(PlayerCastleTagName)?.GetComponent<Transform>();
            return cachedCastleTransform;
        }
    }

    private Transform cachedPlayerTransform;
    private Transform cachedCastleTransform;

    private List<EnemyMovement> EnemiesAttackingPlayer = new List<EnemyMovement>();
    private List<EnemyMovement> EnemiesAttackingCastle = new List<EnemyMovement>();

    public Transform GetValidTarget(EnemyMovement enemy)
    {
        RemoveEnemyFromManager(enemy);

        if (EnemiesAttackingPlayer.Count <= MaxAmmountOfEnemiesAttackingPlayer)
        {
            EnemiesAttackingPlayer.Add(enemy);
            return PlayerTransform;
        }
        else
        {
            EnemiesAttackingCastle.Add(enemy);
            return CastleTransform;
        }
    }

    public void RemoveEnemyFromManager(EnemyMovement enemy)
    {
        if (EnemiesAttackingCastle.Contains(enemy))
            EnemiesAttackingCastle.Remove(enemy);
        else if (EnemiesAttackingPlayer.Contains(enemy))
            EnemiesAttackingPlayer.Remove(enemy);
    }
}
