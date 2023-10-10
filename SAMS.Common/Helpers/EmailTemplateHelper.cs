namespace SAMS.Common.Helpers
{
    public static class EmailTemplateHelper
    {
        public static string GenerateCreateUserMailContent(string name, string surname, string guid)
        {
            return string.Format(@"Merhaba {0} {1}, <br />
                                <p>Kullanıcı kaydınız başarıyla tamamlandı.</p>
                                <a href='{2}'>Sisteme giriş yapmak için tıklayınız.</a>", name, surname, AppSettingsHelper.Current.CurrentSiteUrl + "/change-password?guid=" + guid);
        }

        public static string RegisterUserMailContent(string name, string surname, string otpCode)
        {
            return string.Format(@"Merhaba {0} {1}, <br />
                                <p>Kullanıcı kaydınız oluşturuldu.</p>
                                <p>Aktivasyon kodunuz: <b>{2}</b></p>", name, surname, otpCode);
        }

        public static string GenerateChangePasswordSuccessMailContent(string name, string surname)
        {
            return string.Format(@"Merhaba {0} {1}, <br />
                                <p>Şifreniz başarıyla değiştirildi.</p>
                                <a href='{2}'>Sisteme giriş yapmak için tıklayınız.</a>", name, surname, AppSettingsHelper.Current.CurrentSiteUrl + "/login");
        }

        public static string GenerateChangePasswordMobileSuccessMailContent(string name, string surname)
        {
            return string.Format(@"Merhaba {0} {1}, <br />
                                <p>Şifreniz başarıyla değiştirildi.</p>", name, surname);
        }

        public static string GenerateForgotPasswordMailContent(string name, string surname, string guid)
        {
            return string.Format(@"
                                Merhaba {0} {1}, <br />
                                <a href='{2}'>Şifrenizi sıfırlamak için tıklayınız.</a>", name, surname, AppSettingsHelper.Current.CurrentSiteUrl + "/change-password?guid=" + guid);
        }

        public static string GenerateForgotPasswordMobileMailContent(string name, string surname, string otpCode)
        {
            return string.Format(@"
                                Merhaba {0} {1}, <br />
                                Şifre sıfırlama kodunuz: <b>{2}</b>", name, surname, otpCode);
        }

        public static string GenerateContactFormMailContent(string name, string lastname, string email, string contentType, string subject, string message)
        {
            return string.Format(@"
                                Ad: {0} <br/>
                                Soyad: {1} <br/>
                                Eposta: {2} <br/>
                                Tip: {3} <br/>
                                Başlık: {4} <br/><br/><br/>
                                Mesaj: <br/>
                                {5}", name, lastname, email, contentType, subject, message);
        }
    }
}
