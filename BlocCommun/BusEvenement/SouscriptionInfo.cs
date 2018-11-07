using System;
using System.Collections.Generic;
using System.Text;

namespace BusEvenement
{
    public class SouscriptionInfo
    {
        public bool EstDynamique { get; }

        public Type TypeHandler { get; }

        private SouscriptionInfo(bool estDynamique, Type typeHandler)
        {
            EstDynamique = estDynamique;
            TypeHandler = typeHandler;
        }

        public static SouscriptionInfo Dynamique(Type handlerType)
        {
            return new SouscriptionInfo(true, handlerType);
        }

        public static SouscriptionInfo Typed(Type handlerType)
        {
            return new SouscriptionInfo(false, handlerType);
        }
    }
}
