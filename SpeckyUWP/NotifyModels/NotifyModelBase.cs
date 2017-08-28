using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpeckyUWP.NotifyModels
{
    public class NotifyModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Automatically calls the property changed event passing the method name used to make the call unless propertyName is specified.
        /// Note: If used inside a property propertyName can only be blank when Notify is called from directly within the property.
        /// If the logic is passed to anoher method then propertyName must still be specified in order fire PropertyChanged with the correct property name.
        /// </summary>
        /// <param name="propertyName">The name of the property invoking PropertyChanged.</param>
        public void Notify([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
