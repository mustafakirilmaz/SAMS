
import { Directive, Input, ViewContainerRef, TemplateRef } from '@angular/core';
import { CommonHelper } from '../helpers/common-helper';

@Directive({
    selector: '[roles]'
  })

  export class RolesDirective {
    @Input() set roles(value: string) {
        let isVisible = false;
        const rolesArr = value.match(/[^&|)(]+/g);
        for (let i = 0; i < rolesArr.length; i++) {
           const role = rolesArr[i];
           if(this.commonHelper.hasRole(role.trim(), true)){
            isVisible = true;
            break;
           }
        }
        if (isVisible) {
            this.viewContainerRef.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainerRef.clear();
        }
    }
  
    constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>, private commonHelper: CommonHelper) { }
  }
  