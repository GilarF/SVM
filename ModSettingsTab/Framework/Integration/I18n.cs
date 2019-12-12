using System;
using StardewValley;

namespace ModSettingsTab.Framework.Integration
{
    public class I18n
    {
        public string en { get; set; } = null;
        public string zh { get; set; } = null;
        public string fr { get; set; } = null;
        public string de { get; set; } = null;
        public string hu { get; set; } = null;
        public string it { get; set; } = null;
        public string ja { get; set; } = null;
        public string ko { get; set; } = null;
        public string pt { get; set; } = null;
        public string ru { get; set; } = null;
        public string es { get; set; } = null;
        public string tr { get; set; } = null;

        public string this[LocalizedContentManager.LanguageCode code]
        {
            get
            {
                switch (code)
                {
                    case LocalizedContentManager.LanguageCode.en:
                        return en;
                    case LocalizedContentManager.LanguageCode.ja:
                        return ja;
                    case LocalizedContentManager.LanguageCode.ru:
                        return ru;
                    case LocalizedContentManager.LanguageCode.zh:
                        return zh;
                    case LocalizedContentManager.LanguageCode.pt:
                        return pt;
                    case LocalizedContentManager.LanguageCode.es:
                        return es;
                    case LocalizedContentManager.LanguageCode.de:
                        return de;
                    case LocalizedContentManager.LanguageCode.th:
                        return zh;
                    case LocalizedContentManager.LanguageCode.fr:
                        return fr;
                    case LocalizedContentManager.LanguageCode.ko:
                        return ko;
                    case LocalizedContentManager.LanguageCode.it:
                        return it;
                    case LocalizedContentManager.LanguageCode.tr:
                        return tr;
                    case LocalizedContentManager.LanguageCode.hu:
                        return hu;
                    default:
                        return en;
                }
            }
        }
    }
}