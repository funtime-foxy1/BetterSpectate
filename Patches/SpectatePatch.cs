using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterSpectate.SteamHelper;
using HarmonyLib;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Object = UnityEngine.GameObject;
using UnityEngine;

namespace BetterSpectate.Patches
{
    [HarmonyPatch(typeof(Spectate))]
    internal class SpectatePatch
    {
        static Object panel;

        private static void AddPlayerToDeadPanel(Player player)
        {
            var deadPlrs = panel.transform.Find("allDead");
            var template = deadPlrs.Find("template");
            var clone = Object.Instantiate(template, deadPlrs);
            clone.gameObject.SetActive(true);
            clone.GetComponent<Image>().sprite = Utils.GetAvatar(player.refs.view.Owner);
            if (player.data.microphoneValue >= .275)
            {
                var speak = clone.Find("speak");
                speak.gameObject.SetActive(true);
            }
            //Plugin.Log.LogWarning("MIC VAL: " + player.data.microphoneValue);
        }

        private static List<Player> getDeadPlayers()
        {
            var allDead = new List<Player>();
            var allPlayers = PlayerHandler.instance.players;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                var plr = allPlayers[i];
                if (plr.data.dead)
                {
                    allDead.Add(plr);
                }
            }
            return allDead;
        }
        private static void RefreshPlrList()
        {
            var deadPlrs = panel.transform.Find("allDead");
            foreach (Transform transform in deadPlrs.transform)
            {
                if (transform.gameObject.name == "template") { continue; }
                UnityEngine.Object.Destroy(transform.gameObject);
            }
            for (int i = 0; i < getDeadPlayers().Count; i++)
            {
                var plr = getDeadPlayers()[i];
                AddPlayerToDeadPanel(plr);
            }
        }

        [HarmonyPatch("StartSpectate")]
        [HarmonyPostfix]
        private static void StartPatch()
        {
            var _panel = Plugin.assets.LoadAsset<Object>("SpectateUI");
            if (_panel == null)
            {
                Plugin.Log.LogFatal("SpectateUI Panel is non-existant.");
                return;
            }
            //        TODO: Fill the parent in ↓↓↓↓
            panel = Object.Instantiate(_panel, Object.Find("GAME").transform);
            panel = panel.transform.Find("Panel").gameObject;

            for (int i = 0; i < getDeadPlayers().Count; i++)
            {
                var plr = getDeadPlayers()[i];
                AddPlayerToDeadPanel(plr);
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        private static void UpdatePatch(Spectate __instance)
        {
            if (panel == null || !Spectate.spectating) { return; }
            RefreshPlrList();


            var cam = __instance.gameObject.GetComponent<Camera>();
            if (!cam)
            {
                cam = __instance.gameObject.GetComponentInChildren<Camera>();
            }

            /*if (Input.mouseScrollDelta.y > 0)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView -= 5, 5f, 500f);
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView += 10, 5f, 500f);
            }*/
        }
    }
}
