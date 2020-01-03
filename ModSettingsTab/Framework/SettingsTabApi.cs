using System;
using System.Collections.Generic;
using ModSettingsTabApi.Events;
using ModSettingsTabApi.Framework.Interfaces;

namespace ModSettingsTab.Framework
{
    public class SettingsTabApi : ISettingsTabApi
    {
        public event EventHandler<OptionsChangedEventArgs> OptionsChanged;

        public void Send(object sender,Dictionary<string, Value> options)
        {
            OptionsChanged?.Invoke(sender, new OptionsChangedEventArgs(options));
        }
    }
}