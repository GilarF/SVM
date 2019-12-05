using System.Collections;
using System.ComponentModel;

namespace ModSettingsTab
{
    public class StaticConfig : INotifyPropertyChanged, IEnumerable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}