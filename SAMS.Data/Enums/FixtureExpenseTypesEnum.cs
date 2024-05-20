using crypto;
using System.ComponentModel;

namespace SAMS.Data
{
    public enum FixtureExpenseTypesEnum
    {
        [Description("Asansör Ağır Bakımı")]
        ElevatorHeavyMaintenance = 0,

		[Description("Çatı Tamiratı")]
        RoofRenovation = 1,

		[Description("Bina Yalıtımı")]
        BuildingInsulation = 2,

        [Description("Bina Dış Cephe Boyama")]
        BuildingExteriorPainting = 3,

		[Description("Ortak Alan Düzenlemeleri")]
        CommonAreaArrangements = 4,

        [Description("Diğer")]
        Other = 5,
    }
}