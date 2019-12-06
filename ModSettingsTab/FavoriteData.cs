using System.Collections.Generic;
using System.Timers;

namespace ModSettingsTab
{
    /// <summary>
    /// saves the mod to favorites
    /// </summary>
    public static class FavoriteData
    {
        /// <summary>
        /// collection of selected mods
        /// </summary>
        private static readonly Dictionary<string, bool> Favorite;

        /// <summary>
        /// save timer, anti-click protection
        /// </summary>
        private static readonly Timer SaveTimer;

        static FavoriteData()
        {
            SaveTimer = new Timer(2000.0)
            {
                Enabled = false,
                AutoReset = false
            };
            SaveTimer.Elapsed += (t, e) =>
                ModEntry.Helper.Data.WriteJsonFile("favorite.json", Favorite);

            Favorite =
                ModEntry.Helper.Data.ReadJsonFile<Dictionary<string, bool>>("favorite.json")
                ?? new Dictionary<string, bool>();
        }

        /// <summary>
        /// Checks if the mod is bookmarked
        /// </summary>
        /// <param name="uniqueId">
        /// unique mod identifier
        /// </param>
        /// <returns></returns>
        public static bool IsFavorite(string uniqueId)
        {
            return Favorite.ContainsKey(uniqueId) && Favorite[uniqueId];
        }

        /// <summary>
        /// changes bookmark state
        /// </summary>
        /// <param name="uniqueId">
        /// unique mod identifier
        /// </param>
        /// <param name="status"></param>
        public static void ChangeStatus(string uniqueId, bool status)
        {
            if (Favorite.ContainsKey(uniqueId))
                Favorite[uniqueId] = status;
            else
                Favorite.Add(uniqueId, status);
            SaveTimer.Stop();
            SaveTimer.Start();
        }
    }
}