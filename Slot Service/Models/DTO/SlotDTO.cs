namespace Slot_Service.Models.DTO
{
    public class SlotDTO
    {
        public int SlotId { get; set; }
        public string SlotCodeName { get; set; }
        public DateTime DateTime { get; set; }

        public string availability { get; set; }
        
    }
}
