using crypto;
using System.ComponentModel;

namespace SAMS.Data
{
    public enum ProportionalExpenseTypesEnum
    {
        [Description("Asansör Bakımı")]
        ElevatorPeriodicMaintenance = 0,

		[Description("Asansör Elektiği")]
        ElevatorElectricity = 1,

		[Description("Asansör Acil Kurtarma İletişimi")]
        ElevatorCommunication = 2,

        [Description("Jeneratör Bakımı")]
        GeneratorMaintenance = 3,

		[Description("Jeneratör Yakıtı")]
        GeneratorFuel = 4,

        [Description("Temizlik Malzemesi")]
        CleaningMaterials = 5,
        
        [Description("İyileştirme Malzemesi")]
        HealingMaterial = 6,
        
        [Description("İletişim")]
        Communication = 7,

		[Description("Posta")]
        Mail = 8,

        [Description("Noter")]
        Notary = 9,

		[Description("Kırtasiye")]
        Stationery = 10,

        [Description("İçme Suyu")]
        DrinkingWater = 11,

		[Description("Ortak Alan Suyu")]
        CommonAreaWater = 12,

		[Description("Ortak Alan Elektriği")]
        CommonAreaElectricity = 13,

        [Description("Yangın Söndürme Sistemi Elektiriği")]
        FireFightingElectricity = 14,

		[Description("Kazan Bakımı")]
        BoilerMaintenance = 15,

        [Description("Hidrofor Bakımı")]
        HydrophoreMaintenance = 16,

		[Description("Temizlik Hizmeti")]
        CleaningService = 17,

		[Description("Öngörülemeyen Giderler")]
        UnforeseenExpenses = 18,
    }
}