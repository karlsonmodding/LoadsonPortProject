using System;
using System.Collections.Generic;
using Loadson;
using LoadsonAPI;
using UnityEngine;

namespace EnemyCounter
{
    public class Main : Mod
    {
        public override void OnEnable()
        {
            TimerText.AddText(() =>
            {
                enemies.Clear();
                foreach(RagdollController ragdollController in UnityEngine.Object.FindObjectsOfType<RagdollController>())
                    if (!ragdollController.IsRagdoll() && !enemies.Contains(ragdollController.gameObject))
                        enemies.Add(ragdollController.gameObject);
                if (UIManger.Instance.winUI.activeSelf)
                {
                    if(enemies.Count > 0) return "<size=14>[INVALID RUN]</size>";
                    return "";
                }
                return "<size=14>Enemies: " + enemies.Count + "</size>";
            });
        }
        private static readonly List<GameObject> enemies = new List<GameObject>();
    }
}
