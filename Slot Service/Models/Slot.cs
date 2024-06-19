namespace Slot_Service.Models
{
    public class Slot
    {
        public int SlotID { get; set; }       
        public string availability{ get; set; }
        public string SlotCodeName { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }



    }
}
