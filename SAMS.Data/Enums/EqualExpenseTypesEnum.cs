using crypto;
using System.ComponentModel;

namespace SAMS.Data
{
    public enum EqualExpenseTypesEnum
    {
        [Description("Güvenlik")]
        SecurityService = 0,

		[Description("Çalışan Maaşı")]
        StaffSalary = 1,

        [Description("Çalışan Kıdemi")]
        StaffSeverancePay = 2,

		[Description("Çalışan Kıyafeti")]
        StaffUniform = 3,

        [Description("Site Yönetimi Danışmanlık")]
        SiteManagementConsultancyFee = 4,

		[Description("Hukuki Danışmanlık")]
        LegalConsultancyFee = 5,
    }
}