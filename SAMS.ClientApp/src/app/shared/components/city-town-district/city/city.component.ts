import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../../../bases/base.component';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-city',
  templateUrl: './city.component.html',
  styleUrls: ['./city.component.css']
})

export class CityComponent extends BaseComponent implements OnInit {
  @Input() parentForm;
  cityFilter: SelectItem[];
  @Input() id: string;
  @Input() startSelectType = 1; // 1:Lütfen Seçiniz, 2: Tümü
  @Input() defaultLabelType = 1; // 1:Lütfen Seçiniz, 2: Tümü
  defaultLabel: string;

  constructor() { super(); }

  ngOnInit() {
    this.getCities();
  }

  getCities() {
    this.cs.getCities().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.cityFilter = result.data;
      }
    });
  }

  onCityChange(cityCode) {
    if (this.ch.isNullOrWhiteSpace(cityCode.value)) {
      this.parentForm.get(this.id).setValue(null);
    }
  }
}