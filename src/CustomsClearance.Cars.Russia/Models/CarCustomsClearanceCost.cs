namespace CustomsClearance.Cars.Russia.Models
{
    /// <summary>
    /// Car customs clearance cost (rubles)
    /// </summary>
    public class CarCustomsClearanceCost
    {
        /// <summary>
        /// Customs fee (rubles)
        /// </summary>
        public decimal CustomsFee { get; set; }

        /// <summary>
        /// CustomsDuty (rubles)
        /// </summary>
        public decimal CustomsDuty { get; set; }

        /// <summary>
        /// ExciseTax (rubles)
        /// </summary>
        public decimal ExciseTax { get; set; }

        /// <summary>
        /// Vat (rubles)
        /// </summary>
        public decimal Vat { get; set; }

        /// <summary>
        /// Recycling fee (rubles)
        /// </summary>
        public decimal RecyclingFee { get; set; }

        /// <summary>
        /// Total customs clearance cost (rubles)
        /// </summary>
        public decimal Total => CustomsFee + CustomsDuty + ExciseTax + Vat + RecyclingFee;
    }
}