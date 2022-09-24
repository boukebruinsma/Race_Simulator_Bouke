using System;
namespace Model
{
    public interface IParticipant
    {
        public String Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
    }
}
