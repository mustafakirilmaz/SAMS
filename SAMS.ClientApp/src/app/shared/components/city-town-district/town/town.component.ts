import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../../../bases/base.component';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-town',
  templateUrl: './town.component.html',
  styleUrls: ['./town.component.css']
})

export class TownComponent extends BaseComponent implements OnInit {
  @Input() id: string;
  @Input() cityCode: string;
  @Input() startSelectType = 1;
  @Input() alwaysDisable = false;
  @Input()
  set parentForm(value) {
    this._parentForm = value;
    this._parentForm.get(this.cityCode).valueChanges.subscribe(ik => {
      this._parentForm.controls[this.id].value = null;
      if (!this.ch.isNullOrWhiteSpace(ik)) {
        this.getTowns();
      } else {
        this.towns = [];
        this._parentForm.controls[this.id].disable();
      }
    });
  }
  towns: SelectItem[];
  _parentForm;

  ngOnInit() {
      this.towns = [];
    this._parentForm.controls[this.id].disable();
    if (!this.ch.isNullOrWhiteSpace(this._parentForm.controls[this.id].value)) {
      this.getTowns();
    }
  }

  getTowns() {
    const cityCode = this._parentForm.controls[this.cityCode].value;

    if (this.ch.isNullOrWhiteSpace(cityCode)) {
      this.towns = [];
      this._parentForm.controls[this.id].disable();
    } else {
      if (!this.alwaysDisable) {
        this._parentForm.controls[this.id].enable();
      }
    }
    this._parentForm.get(this.id).setErrors(null);
    if (!this.ch.isNullOrWhiteSpace(cityCode)) {
      const selectedCityCode = this._parentForm.controls[this.cityCode].value;
      this.cs.getTowns(selectedCityCode).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.towns = result.data;
        }
      });
    }
  }

  onTownChange(townCode) {
    if (this.ch.isNullOrWhiteSpace(townCode.value)) {
      this.parentForm.get(this.id).setValue(null);
    }
  }
}