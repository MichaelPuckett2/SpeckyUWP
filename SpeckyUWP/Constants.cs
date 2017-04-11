using System;
using System.Linq;
using System.Reflection;

namespace Specky
{
    public static class Constants
    {
        public static readonly BindingFlags AllBindingFlags = (BindingFlags)Enum.GetValues(typeof(BindingFlags)).Cast<int>().Aggregate((flags, nextFlag) => flags | nextFlag);

    }
}
