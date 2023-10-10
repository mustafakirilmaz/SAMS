export enum RegexType {
  TCKN = 1,
  Phone = 2,
  Date = 3,
  Letters = 4,
  Numbers = 5,
  Only3Digits = 6,
  SysPvd = 7,
  Float = 8
}

export let regexTypeDescriptions: Record<keyof typeof RegexType, string> = {
  TCKN: '^[1-9]{1}[0-9]{9}[0,2,4,6,8]{1}$',
  Phone: '^\(?([0-9]{3})\)?[ ]?([0-9]{3})[ ]?([0-9]{2})[ ]?([0-9]{2})$',
  Date: '^(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$',
  Letters: '^[A-Za-z-9ğüşöçıİĞÜŞÖÇ ]+$',
  Numbers: '^[0-9]*$',
  Only3Digits: '^[0-9]{1,3}$',
  SysPvd: '^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$',
  Float: '^[+-]?\\d+(\\.\\d+)?$'
};


export let regexTypeMessageDescriptions: Record<keyof typeof RegexType, string> = {
  TCKN: 'T.C. Kimlik numarasına sadece rakam ve 11 karakter girilebilir',
  Phone: 'Lütfen geçerli bir telefon numarası giriniz.(999) 999 99 99 formatında olmalıdır',
  Date: 'Tarihi yalnızca GG.AA.YYYY şeklinde girebilirsiniz.',
  Letters: 'Yalnızca harf girebilirsiniz.',
  Numbers: 'Yalnızca rakam girebilirsiniz.',
  Only3Digits: 'Yalnızca rakam girebilirsiniz.',
  SysPvd: 'Şifreniz en az 1 büyük harf, 1 küçük harf, 1 rakam içermeli ve en az 6 haneli olmalıdır.',
  Float: 'Girilen değer sayı ya da ondalık sayı olmalıdır.'
};