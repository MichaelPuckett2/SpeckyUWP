using System;
using System.Linq;
using System.Reflection;
using static Specky.Constants;

namespace Specky.NotifyModels.SingleInstance
{
    public class SingleInstanceNotifyModel<T> : NotifyModelBase where T : class
    {
        protected SingleInstanceNotifyModel()
        {
            if (typeof(T).GetConstructors(AllBindingFlags).Where(contructorInfo => !contructorInfo.IsPrivate).Any())
                throw new FormatException($"{typeof(T).Name} can only have a parameterless private constuctor in order to inherit from {nameof(SingleInstanceNotifyModel<T>)}");
        }

        static public T Instance { get; } = (T)typeof(T).GetConstructors(AllBindingFlags)
                                                        .Where(contructorInfo => contructorInfo.IsPrivate)
                                                        .FirstOrDefault()?
                                                        .Invoke(null);
    }
}