using System;

namespace ThermostatScheduler.WebApp.Exceptions
{
    public class EntityReferencedException : Exception
    {
        public EntityReferencedException()
            : base("Entity cannot be deleted because it is referenced in some other entity(ies).")
        {
        }
    }
}
