﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [SerializeField]
    private string gameId = "";
    [SerializeField]
    private int totalTurns = 0;
    [SerializeField]
    private List<Turn> turns;

    private Vector2 currentTargetPosition;

    [System.Serializable]
    private class Turn
    {
        [SerializeField]
        private string id;
        [SerializeField]
        private int turnNumber;
        [SerializeField]
        private Vector2 targetPosition;

        public Vector2 error;

        public Turn(int turnNumber, Vector2 targetPosition)
        {
            this.id = Guid.NewGuid().ToString();
            this.turnNumber = turnNumber;
            this.targetPosition = targetPosition;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

    }

    void Start()
    {
        gameId = Guid.NewGuid().ToString();
        totalTurns = 0;
        turns = new List<Turn>();
    }

    public void TurnEnded(Collision2D collision, GameObject pig)
    {
        totalTurns++;

        Turn turn = new Turn(totalTurns, currentTargetPosition);
        //if player failed to hit the pig
        if (pig != null && collision.gameObject != pig)
        {
            //Get distance between bird hit point and pig position
            turn.error = collision.GetContact(0).point - (Vector2)pig.transform.position;
        }
        else
        {
            turn.error = Vector2.zero;
        }

        turns.Add(turn);
    }

    public string ToJson(bool prettyPrint)
    {
        return JsonUtility.ToJson(this,prettyPrint);
    }

    public Vector3 GenerateTargetPosition()
    {
        currentTargetPosition.x = UnityEngine.Random.Range(0.5f, 6.5f);
        currentTargetPosition.y = -3.5f;

        return (Vector3)currentTargetPosition;
    }

    private void OnGUI()
    {
        if(GUILayout.Button("Print"))
        {
            Debug.Log(ToJson(true));
        }
    }
}
