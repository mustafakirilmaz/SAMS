import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../../../bases/base.component';
import { SelectItem } from 'primeng/api';

@Component({
  selector: 'app-district',
  templateUrl: './district.component.html',
  styleUrls: ['./district.component.css']
})

export class DistrictComponent extends BaseComponent implements OnInit {
  @Input() id: string;
  @Input() townCode: string;
  @Input() startSelectType = 1;
  @Input() alwaysDisable = false;
  @Input()
  set parentForm(value) {
    this._parentForm = value;
    this._parentForm.get(this.townCode).valueChanges.subscribe(ik => {
      this._parentForm.controls[this.id].value = null;
      if (!this.ch.isNullOrWhiteSpace(ik)) {
        this.getDistricts();
      } else {
        this.districts =[];
        this._parentForm.controls[this.id].value = null;
        this._parentForm.controls[this.id].disable();
      }
    });
  }
  districts: SelectItem[];
  _parentForm;

  ngOnInit() {
    this.districts = [];
    this._parentForm.controls[this.id].disable();
    if (!this.ch.isNullOrWhiteSpace(this._parentForm.controls[this.id].value)) {
      this.getDistricts();
    }
  }

  getDistricts() {
    const townCode = this._parentForm.controls[this.townCode].value;

    if (this.ch.isNullOrWhiteSpace(townCode)) {
      this.districts = [];
      this._parentForm.controls[this.id].disable();
    } else {
      if (!this.alwaysDisable) {
        this._parentForm.controls[this.id].enable();
      }
    }
    this._parentForm.get(this.id).setErrors(null);
    if (!this.ch.isNullOrWhiteSpace(townCode)) {
      const selectedTownCode = this._parentForm.controls[this.townCode].value;
      this.cs.getDistricts(selectedTownCode).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.districts = result.data;
        }
      });
    }
  }

  onDistrictChange(districtCode) {
    if (this.ch.isNullOrWhiteSpace(districtCode.value)) {
      this.parentForm.get(this.id).setValue(null);
    }
  }
}