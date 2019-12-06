using System.Collections.Generic;
using System.Timers;

namespace ModSettingsTab
{
    public static class FavoriteData
    {
        private static Dictionary<string, bool> _favorite;
        private static readonly Timer _saveTimer;

        static FavoriteData()
        {
            _saveTimer = new Timer(2000.0)
            {
                Enabled = false,
                AutoReset = false
            };
            _saveTimer.Elapsed += (t, e) =>
                ModEntry.Helper.Data.WriteJsonFile("favorite.json", _favorite);

            _favorite =
                ModEntry.Helper.Data.ReadJsonFile<Dictionary<string, bool>>("favorite.json")
                ?? new Dictionary<string, bool>();
        }

        public static bool IsFavorite(string uniqueId)
        {
            return _favorite.ContainsKey(uniqueId) && _favorite[uniqueId];
        }

        public static void ChangeStatus(string uniqueId, bool status)
        {
            if (_favorite.ContainsKey(uniqueId))
                _favorite[uniqueId] = status;
            else
                _favorite.Add(uniqueId, status);
            _saveTimer.Stop();
            _saveTimer.Start();
        }
    }
}