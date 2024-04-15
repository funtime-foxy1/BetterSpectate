
using UnityEngine.UI;
using Steamworks;
using UnityEngine;

namespace BetterSpectate.SteamHelper
{
    internal class Utils
    {
        public static Sprite GetAvatar(Photon.Realtime.Player plr)
        {
            SteamAvatarHandler.TryGetAvatarForPlayer(plr, out var sprite);
            return sprite;
        }
    }
}
