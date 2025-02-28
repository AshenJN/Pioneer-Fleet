﻿//#define SELF_DROP

using System.Collections;
using System.Collections.Generic;
using MatchThree.BConfig;
using MatchThree.Utilities;
using UnityEngine;

namespace MatchThree.Board
{
    /*
     * Animation for Block GameObject
     */
    public class BlockActionBehaviour : MonoBehaviour
    {
        [SerializeField] BlockConfig m_BlockConfig;
        public bool isMoving { get; set; }
        Queue<Vector3> m_MovementQueue = new Queue<Vector3>(); 

        public void MoveDrop(Vector2 vtDropDistance)
        {
            m_MovementQueue.Enqueue(new Vector3(vtDropDistance.x, vtDropDistance.y, 1));

            if (!isMoving)
            {
                StartCoroutine(moveDrop());
            }
        }

        IEnumerator moveDrop(float acc = 1.0f)
        {
            isMoving = true;

            while (m_MovementQueue.Count > 0)
            {
                Vector2 vtDestination = m_MovementQueue.Dequeue();

                int dropIndex = System.Math.Min(9, System.Math.Max(1, (int)Mathf.Abs(vtDestination.y)));
                float dur = m_BlockConfig.dropSpeed[dropIndex - 1];
                yield return dropSmooth(vtDestination, dur * acc);
            }

            isMoving = false;
            yield break;
        }

        IEnumerator dropSmooth(Vector2 vtDropDistance, float duration)
        {
            Vector2 to = new Vector3(transform.position.x + vtDropDistance.x, transform.position.y - vtDropDistance.y);
            yield return Action2D.MoveTo(transform, to, duration);
        }
    }
}