namespace Station.dto
{
    public class SubscriberDetailsDto
    {
        public string SubscriberName { get; set; }
        public int MeterNumber { get; set; }
        public int MeterId { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public string SubscriptionType { get; set; }                        
        public float PreviousReading { get; set; }        
    }
}
