using System;
namespace Model
{
    public class Car : IEquipment
    {
        public Car()
        {
        }

        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }
    }
}
