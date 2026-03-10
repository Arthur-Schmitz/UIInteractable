///-----------------------------------------------------------------
///   Author : Arthur Schmitz                    
///   Date   : 10/03/2025 09:16
///-----------------------------------------------------------------

namespace Com.ArthurSchmitz.UIInteractable 
{
    public struct StateData
    {
        public string   callerName;
        public bool     available;

        public StateData(string callerName, bool available)
        {
            this.callerName = callerName;
            this.available  = available;
        }

        public override string ToString() => "{" 
            + $"{nameof(callerName)} = {callerName}, "
            + $"{nameof(available )} = {available }"
            + "}";

    }
}