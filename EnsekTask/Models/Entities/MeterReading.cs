namespace EnsekTask.Models.Entities
{
    public class MeterReading
    {
        public int AccountNumber { get; set; }
        public DateTime Date { get; set; }
        //  it's unclear whether the database should store a textual representation
        //  of a 5-digit reading or whether that's the incoming valid format. I am
        //  presuming the latter because of the wording so storing them in the
        //  database as numbers
        public int Reading { get; set; }
    }
}
