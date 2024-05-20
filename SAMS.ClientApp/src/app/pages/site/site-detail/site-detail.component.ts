import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup, Validators } from '@angular/forms';
import { SiteService } from 'src/app/services/site-service';
import { Router } from '@angular/router';
import ServiceResult from 'src/app/shared/models/service-result';
import { BaseComponent } from 'src/app/shared/bases/base.component';

@Component({
  selector: 'app-site-detail',
  templateUrl: './site-detail.component.html',
  styleUrls: ['./site-detail.component.css'],

})
export class SiteDetailComponent extends BaseComponent implements OnInit {
  @Input() siteId;
  @Output() onSiteSaved = new EventEmitter<string>();
  siteForm: FormGroup;

  constructor(private siteService: SiteService, public router: Router) { super(); }

  ngOnInit() {
    this.createSiteForm();
    if (this.siteId > 0) {
      this.getSiteById(this.siteId);
    }
  }

  createSiteForm() {
    this.siteForm = this.ch.formBuilder.group({
      id: [null],
      name: [null, Validators.required],
    });
  }

  saveSite() {
    if (!this.ch.validateAllFormFields(this.siteForm, true)) {
      return;
    }
    const siteId = this.ch.getControlValue(this.siteForm, 'id');
    if (this.ch.isNullOrUndefined(siteId)) {
      this.siteService.createSite(this.siteForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onSiteSaved.emit(this.siteForm.value);
        }
      });
    }
    else {
      this.siteService.updateSite(this.siteForm.value.id, this.siteForm.value).subscribe(result => {
        if (this.ch.checkResult(result)) {
          this.ch.messageHelper.showSuccessMessage(result.messages[0]);
          this.onSiteSaved.emit(this.siteForm.value);
        }
      });
    }
  }

  getSiteById(siteId) {
    this.siteService.getSiteById(siteId).subscribe((result: ServiceResult<object>) => {
      if (this.ch.checkResult(result)) {
        this.ch.mapToFormGroup(result.data, this.siteForm);
      }
    });
  }

  get siteFormControls() {
    return this.siteForm.controls;
  }
}
