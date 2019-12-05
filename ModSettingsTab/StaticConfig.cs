using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ModSettingsTab
{
    public class StaticConfig : INotifyPropertyChanged, IEnumerable
    {
        private readonly Dictionary<string, JToken> _properties = new Dictionary<string, JToken>();
        private readonly JObject _config;

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return _config.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public StaticConfig(JObject config)
        {
            _config = config;
            ParseProperties(_config);
        }

        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

        public void Remove(string key)
        {
            _properties.Remove(key);
        }

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