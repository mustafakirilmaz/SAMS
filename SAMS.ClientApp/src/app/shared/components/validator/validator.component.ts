import { Component, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { CommonHelper } from '../../helpers/common-helper';

@Component({
  selector: 'app-validator',
  templateUrl: './validator.component.html',
  styleUrls: ['./validator.component.css']
})

export class ValidatorComponent implements OnInit {
  @Input() control = FormControl;

  constructor(private ch: CommonHelper) { }

  ngOnInit() {
  }

  getMessage(control: FormControl) {
    if (control.dirty && !control.valid) {
      const errorObj = control.errors;
      if (!!errorObj) {
        const hataAdi = Object.keys(errorObj)[0];
        let hataMesaji;
        if (hataAdi === 'required') {
          hataMesaji = 'Bu alan zorunludur.';
        } else if (hataAdi === 'minlength') {
          const requiredLength = errorObj[hataAdi]['requiredLength'];
          hataMesaji = 'En az ' + requiredLength + ' karakter girmelisiniz.';
        } else if (hataAdi === 'maxlength') {
          const requiredLength = errorObj[hataAdi]['requiredLength'];
          hataMesaji = 'En fazla ' + requiredLength + ' karakter girebilirsiniz.';
        } else if (hataAdi === 'min') {
          const min = errorObj[hataAdi]['min'];
          hataMesaji = 'Girilen sayı en az ' + min + ' olabilir.';
        } else if (hataAdi === 'max') {
          const max = errorObj[hataAdi]['max'];
          hataMesaji = 'Girilen sayı en fazla ' + max + ' olabilir.';
        } else if (hataAdi === 'email') {
          hataMesaji = 'Geçersiz bir email girdiniz.';
        } else if (hataAdi === 'regex') {
          hataMesaji = errorObj[hataAdi]['hataMesaji'];
        } else if (hataAdi === 'validUrl') {
          hataMesaji = errorObj[hataAdi]['hataMesaji'];
        }
        return hataMesaji;
      }
    } else {
      return '';
    }
  }

  validationControl() {
    if (!this.ch.isNullOrUndefined(this.control)) {
      return !this.control['valid'] && this.control['dirty'];
    } else {
      return false;
    }
  }
}
