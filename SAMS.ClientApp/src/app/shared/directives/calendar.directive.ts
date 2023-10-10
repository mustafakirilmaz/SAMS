import { Directive } from '@angular/core';
import { Calendar } from 'primeng/calendar';

@Directive({
  selector: 'p-calendar'
})

export class CalendarDirective {
  constructor(calendar: Calendar) {
    calendar.dateFormat = 'dd-mm-yy';
    // calendar.dataType = 'string';
    // calendar.locale = {
    //   firstDayOfWeek: 1,
    //   dayNames: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
    //   dayNamesShort: ['Paz', 'Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt'],
    //   dayNamesMin: ['Pz', 'Pt', 'Sa', 'Ça', 'Pe', 'Cu', 'Ct'],
    //   monthNames: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
    //   monthNamesShort: ['Oca', 'Şub', 'Mar', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Eki', 'Kas', 'Ara'],
    //   today: 'Bugün',
    //   clear: 'Temizle'
    // };
    calendar.autoZIndex = true;
    calendar.baseZIndex = 100000;
    calendar.appendTo = 'body';
    calendar.readonlyInput = true;
    calendar.showIcon = true;
    calendar.yearNavigator = true;
    calendar.yearRange = '1900:2050';
    calendar.monthNavigator = true;
    calendar.showButtonBar = true;
  }
}
