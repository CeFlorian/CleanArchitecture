using System;

namespace NorthWind.Sales.BusinessObjects.POCOEntities.Events
{
    public abstract class Event
    {
        public DateTime TimeStamps { get; protected set; }
        protected Event()
        {
            TimeStamps = DateTime.Now;
        }
    }
}