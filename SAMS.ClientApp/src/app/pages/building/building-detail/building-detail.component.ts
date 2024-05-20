import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { BuildingService } from 'src/app/services/building-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';
import { SelectItem } from 'primeng/api/selectitem';


@Component({
  selector: 'app-building-detail',
  templateUrl: './building-detail.component.html',
  styleUrls: ['./building-detail.component.css'],

})
export class BuildingDetailComponent extends BaseComponent implements OnInit {
  @Input() buildingId;
  @Output() onBuildingSaved = new EventEmitter<string>();
  buildingForm: FormGroup;
  sites: SelectItem[];

  constructor(private buildingService: BuildingService, public router: Router) { super(); }

  ngOnInit() {
    this.createBuildingForm();
    if (this.buildingId > 0) {
      this.getBuildingById(this.buildingId);
    };
    this.cs.getSites().subscribe(result => {
      if (this.ch.checkResult(result)) {
        this.sites = result.data;
      }
    });
  }

  createBuildingForm() {
    this.buildingForm = this.ch.formBuilder.group({
      id: [null],
      isSite: [false],      
      siteId: [null],
      name: [null, Validators.required],
      address: [null, Validators.required],
      cityCode: [null, Validators.required],
      townCode: [null, Validators.required],
      districtCode: [null, Validators.required],
    });
  }

  saveBuilding() {
    if (!this.ch.validateAllFormFields(this.buildingForm, true)) {
      return;
    }
    const buildingId = this.ch.getControlValue(this.buildingForm, 'id');
    const data = this.buildingForm.getRawValue();
    if (data.isSite) {
      // isSite true ise, siteId boş olmamalıdır
      if (!data.siteId) {
        // Eğer siteId boşsa, kullanıcıya bir hata mesajı gösterilebilir veya işlem engellenebilir
        this.ch.messageHelper.showErrorMessage("Site seçimi zorunludur.");
        return;
      }
    } else {
      // isSite false ise, siteId'yi null olarak ayarla
      data.siteId = null;
    }
    if (this.ch.isNullOrUndefined(buildingId)) {
      this.buildingService.createBuilding(data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onBuildingSaved.emit(data);
        }
      });
    }
    else {
      this.buildingService.updateBuilding(buildingId, data).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onBuildingSaved.emit(data);
        }
      });
    }
  }

  getBuildingById(buildingId) {
    this.buildingService.getBuildingById(buildingId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.buildingForm);
      }
    });
  }

  get buildingFormControls() {
    return this.buildingForm.controls;
  }
}
