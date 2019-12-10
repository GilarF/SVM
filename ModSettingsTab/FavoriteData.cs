using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;


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
        private static Queue<string> _favorite;

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
            {
                ModEntry.Helper.Data.WriteJsonFile("data/favorite.json", _favorite);
                ModData.UpdateFavoriteOptionsAsync();
            };

            _favorite =
                ModEntry.Helper.Data.ReadJsonFile<Queue<string>>("data/favorite.json")
                ?? new Queue<string>(5);
            if (_favorite.Count > 5)
                _favorite = new Queue<string>(_favorite.Take(5));
        }

        /// <summary>
        /// Checks if the mod is bookmarked
        /// </summary>
        /// <param name="uniqueId">
        /// unique mod identifier
        /// </param>
        /// <returns></returns>
        public static bool IsFavorite(string uniqueId) => _favorite.Contains(uniqueId);

        /// <summary>
        /// changes bookmark state
        /// </summary>
        /// <param name="uniqueId">
        /// unique mod identifier
        /// </param>
        public static void ChangeStatus(string uniqueId)
        {
            if (IsFavorite(uniqueId))
            {
                var newFavorite = _favorite.Where(id => id != uniqueId);
                _favorite = new Queue<string>(newFavorite);
            }
            else
            {
                _favorite.Enqueue(uniqueId);
                if (_favorite.Count > 5)
                {
                    _favorite.Dequeue();
                    var t = new Queue<Rectangle>(ModData.FavoriteTabSource);
                    t.Enqueue(t.Dequeue());
                    ModData.FavoriteTabSource = t.ToList();
                }
            }

            SaveTimer.Stop();
            SaveTimer.Start();
        }
    }
}