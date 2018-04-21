using System.Data.Linq.Mapping;

namespace usis.Middleware.Sage.OfficeLine.Data
{
    [Table(Name = "KHKMandanten")]
    internal class ClientProperty
    {
        [Column(Name = "Eigenschaft")]
        public int Id { get; set; }

        [Column(Name = "Mandant")]
        public short Client { get; set; }

        [Column(Name = "Wert")]
        public string Value { get; set; }
    }

    [Table(Name = "KHKBilanzdefinition")]
    internal class BalanceSheetDefinition
    {
        [Column(Name = "BilanzID")]
        public string Id { get; set; }

        [Column(Name = "Mandant")]
        public short Client { get; set; }

        [Column(Name = "Bezeichnung", CanBeNull = true)]
        public string Description { get; set; }

        [Column(CanBeNull = true)]
        public short Saldo { get; set; }

        [Column(CanBeNull = true)]
        public string Definition { get; set; }
    }
}
