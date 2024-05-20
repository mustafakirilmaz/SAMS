import { LOCALE_ID, NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HashLocationStrategy, registerLocaleData } from '@angular/common';
import { AppRoutingModule } from '../app-routing.module';

import { AccordionModule } from 'primeng/accordion';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CardModule } from 'primeng/card';
import { CarouselModule } from 'primeng/carousel';
import { ChartModule } from 'primeng/chart';
import { CheckboxModule } from 'primeng/checkbox';
import { ChipsModule } from 'primeng/chips';
import { CodeHighlighterModule } from 'primeng/codehighlighter';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ColorPickerModule } from 'primeng/colorpicker';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DataViewModule } from 'primeng/dataview';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { FieldsetModule } from 'primeng/fieldset';
import { FileUploadModule } from 'primeng/fileupload';
import { GalleriaModule } from 'primeng/galleria';
import { InplaceModule } from 'primeng/inplace';
import { InputMaskModule } from 'primeng/inputmask';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { LightboxModule } from 'primeng/lightbox';
import { ListboxModule } from 'primeng/listbox';
import { MegaMenuModule } from 'primeng/megamenu';
import { MenuModule } from 'primeng/menu';
import { MenubarModule } from 'primeng/menubar';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { MultiSelectModule } from 'primeng/multiselect';
import { OrderListModule } from 'primeng/orderlist';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { OverlayPanelModule } from 'primeng/overlaypanel';
import { PaginatorModule } from 'primeng/paginator';
import { PanelModule } from 'primeng/panel';
import { PanelMenuModule } from 'primeng/panelmenu';
import { PasswordModule } from 'primeng/password';
import { PickListModule } from 'primeng/picklist';
import { ProgressBarModule } from 'primeng/progressbar';
import { RadioButtonModule } from 'primeng/radiobutton';
import { RatingModule } from 'primeng/rating';
import { RippleModule } from 'primeng/ripple';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { SelectButtonModule } from 'primeng/selectbutton';
import { SidebarModule } from 'primeng/sidebar';
import { SlideMenuModule } from 'primeng/slidemenu';
import { SliderModule } from 'primeng/slider';
import { SpinnerModule } from 'primeng/spinner';
import { SplitButtonModule } from 'primeng/splitbutton';
import { StepsModule } from 'primeng/steps';
import { TabMenuModule } from 'primeng/tabmenu';
import { TableModule } from 'primeng/table';
import { TabViewModule } from 'primeng/tabview';
import { TerminalModule } from 'primeng/terminal';
import { TieredMenuModule } from 'primeng/tieredmenu';
import { ToastModule } from 'primeng/toast';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ToolbarModule } from 'primeng/toolbar';
import { TooltipModule } from 'primeng/tooltip';
import { TreeModule } from 'primeng/tree';
import { TreeTableModule } from 'primeng/treetable';
import { VirtualScrollerModule } from 'primeng/virtualscroller';
import { BlockUIModule } from 'primeng/blockui';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { DividerModule } from 'primeng/divider';

import { AppCodeModule } from './components/app-code/app.code.component'
import { AppMenuComponent } from './components/app-menu/app.menu.component';
import { AppMenuitemComponent } from './components/app-menu/app.menuitem.component';
import { AppRightMenuComponent } from './components/app-menu/app.right-menu.component';
import { AppConfigComponent } from './components/app-config/app.config.component';
import { AppTopBarComponent } from './components/app-topbar/app.topbar.component';
import { AppFooterComponent } from './components/app-footer/app.footer.component';

import { MenuLayoutComponent } from './components/layout/menu-layout/menu-layout.component';
import { EmptyLayoutComponent } from './components/layout/empty-layout/empty-layout.component';

import { MenuService } from './components/app-menu/app.menu.service';

import { FileUploadComponent } from './components/app-file-upload/app-file-upload.component';
import { GridComponent } from './components/app-grid/app-grid.component';
import { AutoCompleteComponent } from './components/auto-complete/auto-complete.component';
import { AutoCompleteDirective } from './directives/auto-complete.directive';
import { CalendarDirective } from './directives/calendar.directive';
import { CellTemplateDirective } from './directives/cell-template.directive';
import { DialogDirective } from './directives/dialog.directive';
import { PageHeaderComponent } from './components/app-page-header/app-page-header.component';
import { PromptComponent } from './components/dialogs/prompt/prompt.component';
import { RemovewhitespacesPipe } from './pipes/remove-whitespaces.pipe';
import { ReportComponent } from './components/app-report/app-report.component';
import { RolesDirective } from './directives/roles.directive';
import { SafeHtmlPipe } from './pipes/safe-html.pipe';
import { ValidatorComponent } from './components/validator/validator.component';

import localeTr from '@angular/common/locales/tr';
import { Constants } from './constants/constants';
import { GlobalVariables } from './constants/global-variables';
import { DialogService } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { MenuRoutes } from '../app-menu-routes';
import { SplitButtonDirective } from './directives/split-button.directive';
import { MultiSelectDirective } from './directives/multi-select.directive';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BaseComponent } from './bases/base.component';
import { DropdownDirective } from './directives/dropdown.directive';
import { CityComponent } from './components/city-town-district/city/city.component';
import { TownComponent } from './components/city-town-district/town/town.component';
import { DistrictComponent } from './components/city-town-district/district/district.component';


import { SplitterModule } from 'primeng/splitter';

registerLocaleData(localeTr, 'tr');

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        BrowserModule,
        AppRoutingModule,
        AppCodeModule,
        HttpClientModule,
        BrowserAnimationsModule,
        AccordionModule,
        AutoCompleteModule,
        BreadcrumbModule,
        ButtonModule,
        CalendarModule,
        CardModule,
        CarouselModule,
        ChartModule,
        CheckboxModule,
        ChipsModule,
        CodeHighlighterModule,
        ConfirmDialogModule,
        ColorPickerModule,
        ContextMenuModule,
        DataViewModule,
        DialogModule,
        DropdownModule,
        FieldsetModule,
        FileUploadModule,
        GalleriaModule,
        InplaceModule,
        InputMaskModule,
        InputNumberModule,
        InputSwitchModule,
        InputTextModule,
        InputTextareaModule,
        LightboxModule,
        ListboxModule,
        MegaMenuModule,
        MenuModule,
        MenubarModule,
        MessageModule,
        MessagesModule,
        MultiSelectModule,
        OrderListModule,
        OrganizationChartModule,
        OverlayPanelModule,
        PaginatorModule,
        PanelModule,
        PanelMenuModule,
        PasswordModule,
        PickListModule,
        ProgressBarModule,
        RadioButtonModule,
        RatingModule,
        RippleModule,
        ScrollPanelModule,
        SelectButtonModule,
        SidebarModule,
        SlideMenuModule,
        SliderModule,
        SpinnerModule,
        SplitButtonModule,
        StepsModule,
        TableModule,
        TabMenuModule,
        TabViewModule,
        TerminalModule,
        TieredMenuModule,
        ToastModule,
        ToggleButtonModule,
        ToolbarModule,
        TooltipModule,
        TreeModule,
        TreeTableModule,
        VirtualScrollerModule,
        BlockUIModule,
        ProgressSpinnerModule,
        DividerModule,
        SplitterModule,
    ],
    exports: [
        FormsModule,
        ReactiveFormsModule,
        AccordionModule,
        AutoCompleteModule,
        BreadcrumbModule,
        ButtonModule,
        CalendarModule,
        CardModule,
        CarouselModule,
        ChartModule,
        CheckboxModule,
        ChipsModule,
        CodeHighlighterModule,
        ConfirmDialogModule,
        ColorPickerModule,
        ContextMenuModule,
        DataViewModule,
        DialogModule,
        DropdownModule,
        FieldsetModule,
        FileUploadModule,
        GalleriaModule,
        InplaceModule,
        InputMaskModule,
        InputNumberModule,
        InputSwitchModule,
        InputTextModule,
        InputTextareaModule,
        LightboxModule,
        ListboxModule,
        MegaMenuModule,
        MenuModule,
        MenubarModule,
        MessageModule,
        MessagesModule,
        MultiSelectModule,
        OrderListModule,
        OrganizationChartModule,
        OverlayPanelModule,
        PaginatorModule,
        PanelModule,
        PanelMenuModule,
        PasswordModule,
        PickListModule,
        ProgressBarModule,
        RadioButtonModule,
        RatingModule,
        RippleModule,
        ScrollPanelModule,
        SelectButtonModule,
        SidebarModule,
        SlideMenuModule,
        SliderModule,
        SpinnerModule,
        SplitButtonModule,
        StepsModule,
        TableModule,
        TabMenuModule,
        TabViewModule,
        TerminalModule,
        TieredMenuModule,
        ToastModule,
        ToggleButtonModule,
        ToolbarModule,
        TooltipModule,
        TreeModule,
        TreeTableModule,
        VirtualScrollerModule,
        BlockUIModule,
        ProgressSpinnerModule,
        DividerModule,
        BaseComponent,

        FileUploadComponent,
        GridComponent,
        CellTemplateDirective,
        ReportComponent,
        PageHeaderComponent,
        ValidatorComponent,
        AutoCompleteComponent,
        PromptComponent,

        CalendarDirective,
        DialogDirective,
        AutoCompleteDirective,
        SplitButtonDirective,
        RolesDirective,
        MultiSelectDirective,
        DropdownDirective,

        RemovewhitespacesPipe,
        SafeHtmlPipe,
        
        CityComponent,
        TownComponent,
        DistrictComponent,
        SplitterModule,
    ],
    declarations: [
        MenuLayoutComponent,
        EmptyLayoutComponent,
        AppMenuComponent,
        AppMenuitemComponent,
        AppRightMenuComponent,
        AppConfigComponent,
        AppTopBarComponent,
        AppFooterComponent,
        BaseComponent,

        FileUploadComponent,
        GridComponent,
        CellTemplateDirective,
        ReportComponent,
        PageHeaderComponent,
        ValidatorComponent,
        AutoCompleteComponent,
        PromptComponent,

        CalendarDirective,
        DialogDirective,
        AutoCompleteDirective,
        SplitButtonDirective,
        RolesDirective,
        MultiSelectDirective,
        DropdownDirective,

        RemovewhitespacesPipe,
        SafeHtmlPipe,
        
        CityComponent,
        TownComponent,
        DistrictComponent,
    ],
    providers: [
        { provide: LOCALE_ID, useValue: 'tr-TR', useClass: HashLocationStrategy },
        MenuService,
        MenuRoutes,
        MessageService,
        GlobalVariables,
        Constants,
        DialogService,
    ]
})
export class ShareddModule { }
