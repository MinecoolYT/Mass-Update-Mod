using HarmonyLib;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Unity.Towers.Upgrades;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using MelonLoader;
using System;
using UnityEngine;

[assembly: MelonInfo(typeof(MassUpdate.Main), "Mass Update", "1.0.0", "Minecool")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace MassUpdate
{
    public class Main : MelonMod
    {
        [HarmonyPatch(typeof(Tower), nameof(Tower.OnSold))]
        public class SellPatch
        {
            [HarmonyPostfix]
            public static void Postfix(Tower __instance)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    foreach (var tower in InGame.instance.bridge.GetAllTowers())
                    {
                        if (tower.Def.name == __instance.model.name)
                        {
                            InGame.instance.SellTower(tower);
                        }
                    }
                }
            }
        }
        static bool allowUpgrade = true;
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Unity.Bridge.TowerToSimulation), nameof(Upgrade))]
        public class TestPatch
        {
            [HarmonyPostfix]
            public static void Postfix(int pathIndex, bool isParagon, Action<bool> callback)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (!allowUpgrade) return;
                    allowUpgrade = false;
                    foreach (var tower in InGame.instance.bridge.GetAllTowers())
                    {
                        if (tower.Def.name == TowerSelectionMenu.instance.selectedTower.tower.model.name && tower.id != TowerSelectionMenu.instance.selectedTower.id)
                        {
                            tower.Upgrade(pathIndex, isParagon, callback);
                        }
                    }
                    allowUpgrade = true;
                }
            }
        }
    }
}