namespace CountriesProject.DAL.Models
{
    public class UserCountryListEntry
    {
        public int ListEntryId { get; set; }
        public int CountryId { get; set; }
        public string NameCommon { get; set; }
        public string FlagUrl { get; set; }
        public string Region { get; set; }
        public string ListType { get; set; }
        public DateTime AddedAt { get; set; }
    }
}