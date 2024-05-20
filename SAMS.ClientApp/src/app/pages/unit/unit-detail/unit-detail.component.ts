import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { UnitService } from 'src/app/services/unit-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';


@Component({
  selector: 'app-unit-detail',
  templateUrl: './unit-detail.component.html',
  styleUrls: ['./unit-detail.component.css'],

})
export class UnitDetailComponent extends BaseComponent implements OnInit {
  @Input() unitId;
  @Output() onUnitSaved = new EventEmitter<string>();
  unitForm: FormGroup;
  buildings: SelectItem[];

  constructor(private unitService: UnitService, public router: Router) { super(); }

  ngOnInit() {
    this.createUnitForm();
    if (this.unitId > 0) {
      this.getUnitById(this.unitId);
    };
    this.cs.getBuildings().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.buildings = result.data;
      }
    });
  }

  createUnitForm() {
    this.unitForm = this.ch.formBuilder.group({
      id: [null],
      buildingId: [null],
      name: [null, Validators.required],
      floor: [null, Validators.required],
      netSquareMeter: [null, Validators.required],
      grossSquareMeter: [null, Validators.required],
      shareOfLand: [null, Validators.required]
    });
  }

  saveUnit() {
    if (!this.ch.validateAllFormFields(this.unitForm, true)) {
      return;
    }
    const unitId = this.ch.getControlValue(this.unitForm, 'id');
    const data = this.unitForm.getRawValue();
    if (this.ch.isNullOrUndefined(unitId)) {
      this.unitService.createUnit(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onUnitSaved.emit(data);
        }
      });
    }
    else {
      this.unitService.updateUnit(unitId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onUnitSaved.emit(data);
        }
      });
    }
  }

  getUnitById(unitId) {
    this.unitService.getUnitById(unitId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.unitForm);
      }
    });
  }

  get unitFormControls() {
    return this.unitForm.controls;
  }
}
