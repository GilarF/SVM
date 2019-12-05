using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ModSettingsTab
{
    /// <summary>
    /// modification settings (config.json)
    /// </summary>
    public class StaticConfig : INotifyPropertyChanged, IEnumerable
    {
        /// <summary>
        /// mod static parameter dictionary
        /// </summary>
        private readonly Dictionary<string, JToken> _properties = new Dictionary<string, JToken>();

        /// <summary>
        /// json representation of parameters
        /// (save to config.json)
        /// </summary>
        private readonly JObject _config;

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return _config.ToString();
        }

        public IEnumerator GetEnumerator() => _properties.GetEnumerator();

        public StaticConfig(JObject config)
        {
            _config = config;
            ParseProperties(_config);
        }

        /// <summary>
        /// checks for a parameter
        /// </summary>
        /// <param name="key">
        /// JToken.Path
        /// The key is made up of property names and array indexes separated by periods, e.g. Manufacturers[0].Name.
        /// </param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        /// <summary>
        /// hides the parameter from the page
        /// </summary>
        /// <param name="key">
        /// JToken.Path
        /// The key is made up of property names and array indexes separated by periods, e.g. Manufacturers[0].Name.
        /// </param>
        public void Remove(string key)
        {
            _properties.Remove(key);
        }

        /// <summary>
        /// get or set a parameter by key
        /// </summary>
        /// <param name="key">
        /// JToken.Path
        /// The key is made up of property names and array indexes separated by periods, e.g. Manufacturers[0].Name.
        /// </param>
        public JToken this[string key]
        {
            get => !_properties.ContainsKey(key) ? null : _properties[key];
            set
            {
                if (!_properties.ContainsKey(key))
                    return;
                _config.SelectToken(key).Replace(value);
                _properties[key] = _config.SelectToken(key);
                OnPropertyChanged(key);
            }
        }

        private void OnPropertyChanged(string name)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ParseProperties(JToken obj)
        {
            switch (obj.Type)
            {
                case JTokenType.Object:
                    ((JObject) obj).Properties().ToList()
                        .ForEach(p => ParseProperties(p.Value));
                    break;
                case JTokenType.Array:
                    obj.Children().ToList().ForEach(ParseProperties);
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.String:
                case JTokenType.Boolean:
                    _properties.Add(obj.Path, obj);
                    break;
            }
        }
    }
}