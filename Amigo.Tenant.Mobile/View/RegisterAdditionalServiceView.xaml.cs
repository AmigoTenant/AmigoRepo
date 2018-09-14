using System;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using XPO.ShuttleTracking.Application.DTOs.Responses.Tracking;
using XPO.ShuttleTracking.Mobile.Common.Constants;
using XPO.ShuttleTracking.Mobile.Domain.Util;
using XPO.ShuttleTracking.Mobile.Helpers.Util;
using XPO.ShuttleTracking.Mobile.Resource;
using XPO.ShuttleTracking.Mobile.View.Abstract;
using XPO.ShuttleTracking.Mobile.ViewModel;

namespace XPO.ShuttleTracking.Mobile.View
{
    public partial class RegisterAdditionalServiceView : IPersistentView
    {
        public RegisterAdditionalServiceView()
        {
            InitializeComponent();
            InitCalendar();
            CleanControls();
            LoadListener();
            LoadLabel();
        }
        #region LoadMethods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, true);
        }
        protected override bool OnBackButtonPressed()
        {
            // btnBack.Command.Execute(null);
            return true;
        }

        private void InitCalendar()
        {
            var DtDp5yrTestDateTapRecognizer = new TapGestureRecognizer
            {
                TappedCallback = (v, o) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DtDp5yrTestDate.Unfocus();
                        DtDp5yrTestDate.Focus();
                    });
                },
                NumberOfTapsRequired = 1
            };
            var DtDp25yrTestDateTapRecognizer = new TapGestureRecognizer
            {
                TappedCallback = (v, o) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DtDp25yrTestDate.Unfocus();
                        DtDp25yrTestDate.Focus();
                    });
                },
                NumberOfTapsRequired = 1
            };
            ImgDp5yrTestDate.GestureRecognizers.Add(DtDp5yrTestDateTapRecognizer);
            ImgDp25yrTestDate.GestureRecognizers.Add(DtDp25yrTestDateTapRecognizer);
        }

#if DEBUG
        ~RegisterAdditionalServiceView()
        {
            System.Diagnostics.Debug.WriteLine("********* RegisterAdditionalServiceView is about to be destroyed *********");
        }
#endif
        public void Dispose()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(" ******** RegisterAdditionalServiceView Dispose() *********");
#endif
        }
        private void CleanControls()
        {
            stk25Yr.IsVisible = false;
            //StackLayoutlbl25yrTest.IsVisible = false;
            TxtDp25yrTestDate.IsVisible = false;
            ImgDp25yrTestDate.IsVisible = false;
            LblDp25yrTestDate.IsVisible = false;

            stk5yr.IsVisible = false;
            TxtDp5yrTestDate.IsVisible = false;
            ImgDp5yrTestDate.IsVisible = false;
            LblDp5yrTestDate.IsVisible = false;

            TxtDp5yrTestDate.Text = string.Empty;
            TxtDp25yrTestDate.Text = string.Empty;
            TxtShipmentId.Text = string.Empty;

            CleanAllLabels();
        }

        private void CleanAllLabels()
        {
            CleanLabel(LblShipmentId);
            CleanLabel(LblCostCenter);
            CleanLabel(LblMoveType);
            CleanLabel(LblFromBlock);
            CleanLabel(LblToDispatching);
            CleanLabel(LblEquipmentType);
            CleanLabel(LblEquipmentSize);
            CleanLabel(LblEquipmentNo);
            CleanLabel(LblEquipmentStatus);
            CleanLabel(LblChassisNo);
            CleanLabel(LblProduct);
        }

        private void LoadLabel()
        {
            Title = AppString.titleAdditionalService;
            PkrEquipmentSize.Title = string.Format(AppString.lblDefaultSelection, AppString.lblSize);
        }
        #endregion
        #region Listeners
        private void LoadListener()
        {
            TabShip.ItemSelected += TabShipOnItemSelected;
            TxtShipmentId.TextChanged += TxtShipmentIdOnTextChanged;
            TxtCostCenter.PropertyChanged += TxtCostCenter_PropertyChanged;
            PkrMoveType.SelectedIndexChanged += PkrMoveTypeOnSelectedIndexChanged;
            TxtFromBlock.PropertyChanged += TxtFromBlockOnPropertyChanged;
            TxtFromBlock.PropertyChanged += TxtFromBlock_PropertyChanged;
            PkrToDispatching.SelectedIndexChanged += PkrToDispatchingOnSelectedIndexChanged;
            PkrEquipmentType.SelectedIndexChanged += PkrEquipmentTypeOnSelectedIndexChanged;
            PkrEquipmentSize.SelectedIndexChanged += PkrEquipmentSizeOnSelectedIndexChanged;
            PkrEquipmentNo.TextChanged += PkrEquipmentNoOnTextChanged;
            PkrEquipmentStatus.SelectedIndexChanged += PkrEquipmentStatusOnSelectedIndexChanged;
            PkrChassisNo.TextChanged += PkrChassisNoOnTextChanged;
            TxtDp25yrTestDate.TextChanged += TxtDp25yrTestDate_TextChanged;
            DtDp25yrTestDate.DateSelected += DtDp25yrTestDateOnDateSelected;
            TxtDp5yrTestDate.TextChanged += TxtDp5yrTestDate_TextChanged;
            DtDp5yrTestDate.DateSelected += DtDp5yrTestDateOnDateSelected;
            TxtProduct.PropertyChanged += TxtProduct_PropertyChanged;
            btnNext.Clicked += BtnNextOnClicked;
        }

        private void TxtFromBlock_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtFromBlock.Text)) CleanLabel(LblFromBlock);
        }

        private void TxtCostCenter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtCostCenter.Text)) CleanLabel(LblCostCenter);
        }

        private void TxtProduct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtProduct.Text)) CleanLabel(LblProduct);
        }

        private void TxtFromBlockOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!string.IsNullOrEmpty(TxtFromBlock.Text)) CleanLabel(LblFromBlock);
        }

        private void TabShipOnItemSelected(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            switch ((int)TabShip.SelectedItem)
            {
                case 0:
                    CleanLabel(LblCostCenter);
                    break;
                case 1:
                    CleanLabel(LblShipmentId);
                    break;
            }
        }
        private void TxtShipmentIdOnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue)) CleanLabel(LblShipmentId);
        }
        private void PkrMoveTypeOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var dto = (ServiceDTO)PkrMoveType.SelectedItem;
            if (string.IsNullOrEmpty(dto.Code)) return;
            if (ViewModel.EquipmentRequired == FieldRequirementCode.Value.Hidden)
            {
                stk25Yr.IsVisible = false;
                stk5yr.IsVisible = false;
            }
            else
            {
                stk25Yr.IsVisible = true;
                stk5yr.IsVisible = true;
            }
            CleanAllLabels();
        }
        private void PkrToDispatchingOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var dto = (DispatchingPartyDTO)PkrToDispatching.SelectedItem;
            if (dto.DispatchingPartyId != MoveCode.DefaultValue.ValuePickerDefault)
            {
                CleanLabel(LblToDispatching);
            }
        }
        private void PkrEquipmentTypeOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var dto = (EquipmentTypeDTO)PkrEquipmentType.SelectedItem;
            txtIdEqType.Text = dto.Code;

            stk25Yr.IsVisible = false;
            lbl25yr.IsVisible = false;
            TxtDp25yrTestDate.IsVisible = false;
            ImgDp25yrTestDate.IsVisible = false;

            stk5yr.IsVisible = false;
            lbl5yr.IsVisible = false;
            TxtDp5yrTestDate.IsVisible = false;
            ImgDp5yrTestDate.IsVisible = false;
            CleanAllLabels();

            if (string.IsNullOrEmpty(dto.Code)) return;
            if (!dto.Code.ToUpper().Contains(MoveCode.EquipmentType.TankCode)) return;
            TxtDp25yrTestDate.Text = string.IsNullOrEmpty(ViewModel.TxtDp25yrTestDate) ? string.Empty : ViewModel.TxtDp25yrTestDate;
            stk25Yr.IsVisible = true;
            lbl25yr.IsVisible = true;
            TxtDp25yrTestDate.IsVisible = true;
            ImgDp25yrTestDate.IsVisible = true;

            TxtDp5yrTestDate.Text = string.IsNullOrEmpty(ViewModel.TxtDp5yrTestDate) ? string.Empty : ViewModel.TxtDp5yrTestDate;
            stk5yr.IsVisible = true;
            lbl5yr.IsVisible = true;
            TxtDp5yrTestDate.IsVisible = true;
            ImgDp5yrTestDate.IsVisible = true;
        }
        private void PkrEquipmentSizeOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var dto = (EquipmentSizeDTO)PkrEquipmentSize.SelectedItem;
            txtIdEqSize.Text = dto?.Code;
            if (dto?.EquipmentSizeId != MoveCode.DefaultValue.ValuePickerDefault)
            {
                CleanLabel(LblEquipmentSize);
            }
        }
        private void PkrEquipmentNoOnTextChanged(object sender, TextChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(PkrEquipmentNo.Text)) return;
            //string _text = PkrEquipmentNo.Text;      //Get Current Text
            //if (_text.Length > 11) //If it is more than your character restriction
            //{
            //    _text = _text.Remove(_text.Length - 1); // Remove Last character
            //    PkrEquipmentNo.Text = _text; //Set the Old value
            //}
            //else PkrEquipmentNo.Text = _text.ToUpper(); //Set the Old value
            //if (!string.IsNullOrEmpty(e.NewTextValue)) CleanLabel(LblEquipmentNo);
            if (!string.IsNullOrEmpty(e.NewTextValue)) CleanLabel(LblEquipmentNo);
        }
        private void PkrEquipmentStatusOnSelectedIndexChanged(object sender, EventArgs e)
        {
            var dto = (EquipmentStatusDTO)PkrEquipmentStatus.SelectedItem;
            if (dto?.EquipmentStatusId != MoveCode.DefaultValue.ValuePickerDefault)
            {
                CleanLabel(LblEquipmentStatus);
            }
        }
        private void PkrChassisNoOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            //if (string.IsNullOrEmpty(PkrChassisNo.Text)) return;
            //string _text = PkrChassisNo.Text;      //Get Current Text
            //if (_text.Length > 11) //If it is more than your character restriction
            //{
            //    _text = _text.Remove(_text.Length - 1); // Remove Last character
            //    PkrChassisNo.Text = _text; //Set the Old value
            //}
            //else PkrChassisNo.Text = _text.ToUpper();
            //CleanLabel(LblChassisNo);
            if (!string.IsNullOrEmpty(PkrChassisNo.Text)) CleanLabel(LblChassisNo);
        }
        private void TxtDp25yrTestDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                CleanLabel(LblDp25yrTestDate);
            else
            {
                var msg = DateValidator(e.NewTextValue, 0);
                if (!string.IsNullOrEmpty(msg))
                    ShowLabel(LblDp25yrTestDate, msg);
                else
                    CleanLabel(LblDp25yrTestDate);
            }
        }
        private void DtDp25yrTestDateOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            TxtDp25yrTestDate.Text = dateChangedEventArgs.NewDate.ToString(DateFormats.TankDateFormat);
            CleanLabel(LblDp25yrTestDate);
        }
        private void TxtDp5yrTestDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                CleanLabel(LblDp5yrTestDate);
            else
            {
                var msg = DateValidator(e.NewTextValue, 1);
                if (!string.IsNullOrEmpty(msg))
                    ShowLabel(LblDp5yrTestDate, msg);
                else
                    CleanLabel(LblDp5yrTestDate);
            }
        }
        private void DtDp5yrTestDateOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            TxtDp5yrTestDate.Text = dateChangedEventArgs.NewDate.ToString(DateFormats.TankDateFormat);
            CleanLabel(LblDp5yrTestDate);
        }
        private void TxtProductTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue)) CleanLabel(LblProduct);
        }
        private async void BtnNextOnClicked(object sender, EventArgs eventArgs)
        {
            await FieldValidator();
        }

        #endregion
        #region LoadToolBar

        private RegisterAdditionalServiceViewModel ViewModel => BindingContext as RegisterAdditionalServiceViewModel;

        private static readonly string[] MenuOptions = new[]
        {
            AppString.btnToolbarSettings,
            AppString.btnToolbarTos,
            AppString.btnToolbarHome,
            AppString.btnToolbarSearchCharge
        };
        private async void ShowActionSheet(object sender, EventArgs e)
        {
            var menuClicked = await DisplayActionSheet(null, AppString.btnToolbarCancel, null, MenuOptions);

            if (menuClicked == AppString.btnToolbarSettings)
            {
                ViewModel.SettingsCommand.Execute(null);
            }
            else if (menuClicked == AppString.btnToolbarTos)
            {
                ViewModel.TosCommand.Execute(null);
            }
            else if (menuClicked == AppString.btnToolbarHome)
            {
                ViewModel.HomeCommand.Execute(null);
            }
            else if (menuClicked == AppString.btnToolbarSearchCharge)
            {
                ViewModel.SearchChargeNumberCommand.Execute(null);
            }
        }

        #endregion
        #region UIValidation
        private async System.Threading.Tasks.Task<bool> FieldValidator()
        {
            var result = true;
            var remainingFields = new StringBuilder();
            switch ((int)TabShip.SelectedItem)
            {
                case ChargeNumber.ByShipmentId:
                    if (string.IsNullOrEmpty(TxtShipmentId.Text))
                    {
                        ShowLabel(LblShipmentId, string.Format(AppString.lblMandatoryField, AppString.lblShipmentId));
                        remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblShipmentId));
                        if (result) sclView.ScrollToAsync(TxtShipmentId.X, GetAbsoluteY(TxtShipmentId), true);
                        result = false;
                    }
                    else
                    {
                        string[] limit;
                        var resultValidation = new UtilMove().validateMessageShipmentId(TxtShipmentId.Text, out limit);
                        var isResultValid = false;
                        switch (resultValidation)
                        {
                            case ShipmentCode.Ok:
                                isResultValid = true;
                                break;
                            case ShipmentCode.BadInitNumbers:
                                ShowLabel(LblShipmentId, string.Format(AppString.lblStartWithFormat, AppString.lblShipmentId, limit[0], limit[1]));
                                remainingFields.AppendLine(string.Format(AppString.lblStartWithFormat, AppString.lblShipmentId, limit[0], limit[1]));
                                break;
                            case ShipmentCode.WithABC:
                                ShowLabel(LblShipmentId, string.Format(AppString.lblNumericOnly, AppString.lblShipmentId));
                                remainingFields.AppendLine(string.Format(AppString.lblNumericOnly, AppString.lblShipmentId));
                                break;
                            case ShipmentCode.TooLarge:
                            case ShipmentCode.TooShort:
                                ShowLabel(LblShipmentId, string.Format(AppString.lblMaxLength, AppString.lblShipmentId, UtilMove.MINLENGTH));
                                remainingFields.AppendLine(string.Format(AppString.lblMaxLength, AppString.lblShipmentId, UtilMove.MINLENGTH));
                                break;
                        }
                        if (!isResultValid)
                        {
                            if (result) sclView.ScrollToAsync(TxtShipmentId.X, GetAbsoluteY(TxtShipmentId), true);
                            result = isResultValid;
                        }
                        TxtCostCenter.Text = string.Empty;
                    }
                    break;
                case ChargeNumber.ByCostCenter:
                    if (string.IsNullOrEmpty(TxtCostCenter.Text))
                    {
                        ShowLabel(LblCostCenter, string.Format(AppString.lblMandatoryField, AppString.lblCostCenter));
                        remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblCostCenter));
                        if (result) sclView.ScrollToAsync(TxtCostCenter.X, GetAbsoluteY(TxtCostCenter), true);
                        result = false;
                    }
                    TxtShipmentId.Text = string.Empty;
                    break;
            }
            var dtoService = (ServiceDTO)PkrMoveType.SelectedItem;
            if (dtoService.ServiceId == MoveCode.DefaultValue.ValuePickerDefault)
            {
                if (result) sclView.ScrollToAsync(PkrMoveType.X, GetAbsoluteY(PkrMoveType), true);
                ShowLabel(LblMoveType, string.Format(AppString.lblMandatoryField, AppString.lblMoveType));
                remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblMoveType));
                result = false;
            }
            //Location Validation
            if (ViewModel.BlockRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(TxtFromBlock.Text))
                {
                    if (result) sclView.ScrollToAsync(TxtFromBlock.X, GetAbsoluteY(TxtFromBlock), true);
                    ShowLabel(LblFromBlock, string.Format(AppString.lblMandatoryField, AppString.lblFromBlock));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblFromBlock));
                    result = false;
                }
            }

            var disp = (DispatchingPartyDTO)PkrToDispatching.SelectedItem;
            if (disp.DispatchingPartyId == MoveCode.DefaultValue.ValuePickerDefault)
            {
                if (result) sclView.ScrollToAsync(PkrToDispatching.X, GetAbsoluteY(PkrToDispatching), true);
                ShowLabel(LblToDispatching, string.Format(AppString.lblMandatoryField, AppString.lblDispatching));
                remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblDispatching));
                result = false;
            }

            //Product Validation
            if (ViewModel.ProductRequired == FieldRequirementCode.Value.Required)
            {
                if (string.IsNullOrEmpty(TxtProduct.Text))
                {
                    if (result) sclView.ScrollToAsync(TxtProduct.X, GetAbsoluteY(TxtProduct), true);
                    ShowLabel(LblProduct, string.Format(AppString.lblMandatoryField, AppString.lblProduct));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblProduct));
                    result = false;
                }
            }

            var type = (EquipmentTypeDTO)PkrEquipmentType.SelectedItem;
            if (ViewModel.EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (type == null || type.EquipmentTypeId == MoveCode.DefaultValue.ValuePickerDefault)
                {
                    if (result) sclView.ScrollToAsync(PkrEquipmentType.X, GetAbsoluteY(PkrEquipmentType), true);
                    ShowLabel(LblEquipmentType, string.Format(AppString.lblMandatoryField, AppString.lblEquipmentType));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblEquipmentType));
                    result = false;
                }
                var size = (EquipmentSizeDTO)PkrEquipmentSize.SelectedItem;
                if (size?.EquipmentSizeId == MoveCode.DefaultValue.ValuePickerDefault || PkrEquipmentSize.SelectedIndex == -1)
                {
                    if (result) sclView.ScrollToAsync(PkrEquipmentSize.X, GetAbsoluteY(PkrEquipmentSize), true);
                    ShowLabel(LblEquipmentSize, string.Format(AppString.lblMandatoryField, AppString.lblEquipmentSize));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblEquipmentSize));
                    result = false;
                }
            }

            if (ViewModel.EquipmentNumberRequired == FieldRequirementCode.Value.Required)
            {
                var containerValidatorParams = new ContainerValidatorParams()
                {
                    ContainerValue = ViewModel.GeneralService.EquipmentNumber,
                    Size = ViewModel.TxtIdEqSize,
                    Type = ViewModel.TxtIdEqType,

                    EqpNoPrefixLength = ViewModel.EqpNoPrefixLength,
                    EqSizeCalcEqChkDigit = ViewModel.EqSizeCalcEqChkDigit,
                    EqSizeValEqChkDigit = ViewModel.EqSizeValEqChkDigit,
                    EqpNoMaxLength = ViewModel.EqpNoMaxLength,
                    EqpNoMinLength = ViewModel.EqpNoMinLength,
                    EqpNoValidFormat = ViewModel.EqpNoValidFormat,
                };
                var containerResult = string.Empty;
                var toValidate = ContainerChassisValidator.ContainerValidation(containerValidatorParams, out containerResult);
                switch (toValidate)
                {
                    case ContainerChassisValidator.ContainerValidationCode.AppendCheckDigit:
                        PkrEquipmentNo.Text = containerResult;
                        break;

                    case ContainerChassisValidator.ContainerValidationCode.NoContainerValue: //Label de error
                        if (result) sclView.ScrollToAsync(PkrEquipmentNo.X, GetAbsoluteY(PkrEquipmentNo), true);
                        LblEquipmentNo.Text = string.Format(AppString.lblMandatoryField, AppString.lblEqupNo);
                        LblEquipmentNo.IsVisible = true;
                        remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblEqupNo));
                        result = false;
                        break;
                    case ContainerChassisValidator.ContainerValidationCode.IncorrectCheckDigit: //Label de error
                        if (result) sclView.ScrollToAsync(PkrEquipmentNo.X, GetAbsoluteY(PkrEquipmentNo), true);
                        LblEquipmentNo.Text = string.Format(AppString.lblContainerCheckDigit);
                        LblEquipmentNo.IsVisible = true;
                        remainingFields.AppendLine(string.Format(AppString.lblContainerCheckDigit));
                        result = false;
                        break;
                    case ContainerChassisValidator.ContainerValidationCode.InvalidLength: //Label de error
                        if (result) sclView.ScrollToAsync(PkrEquipmentNo.X, GetAbsoluteY(PkrEquipmentNo), true);
                        LblEquipmentNo.Text = string.Format(AppString.lblContainerLength);
                        LblEquipmentNo.IsVisible = true;
                        remainingFields.AppendLine(string.Format(AppString.lblContainerLength));
                        result = false;
                        break;
                    case ContainerChassisValidator.ContainerValidationCode.WrongFormat: //Label de error
                        if (result) sclView.ScrollToAsync(PkrEquipmentNo.X, GetAbsoluteY(PkrEquipmentNo), true);
                        LblEquipmentNo.Text = string.Format(AppString.lblContainerNumberInvalid);
                        LblEquipmentNo.IsVisible = true;
                        remainingFields.AppendLine(string.Format(AppString.lblContainerNumberInvalid));
                        result = false;
                        break;
                }
            }

            if (ViewModel.StatusRequired == FieldRequirementCode.Value.Required)
            {
                var status = (EquipmentStatusDTO)PkrEquipmentStatus.SelectedItem;
                if (status.EquipmentStatusId == MoveCode.DefaultValue.ValuePickerDefault)
                {
                    if (result) sclView.ScrollToAsync(PkrEquipmentStatus.X, GetAbsoluteY(PkrEquipmentStatus), true);
                    ShowLabel(LblEquipmentStatus, string.Format(AppString.lblMandatoryField, AppString.lblStatus));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblStatus));
                    result = false;
                }
            }

            if (ViewModel.EquipmentRequired == FieldRequirementCode.Value.Required)
            {
                if (type != null && type.Code == MoveCode.EquipmentType.TankCode)
                {
                    var yr25Message = DateValidator(TxtDp25yrTestDate.Text, 0);
                    if (!string.IsNullOrEmpty(yr25Message))
                    {
                        if (result) sclView.ScrollToAsync(TxtDp25yrTestDate.X, GetAbsoluteY(TxtDp25yrTestDate), true);
                        ShowLabel(LblDp25yrTestDate, yr25Message);
                        remainingFields.AppendLine(string.Format(yr25Message));
                        result = false;
                    }
                    var yr5Message = DateValidator(TxtDp5yrTestDate.Text, 1);
                    if (!string.IsNullOrEmpty(yr5Message))
                    {
                        if (result) sclView.ScrollToAsync(TxtDp5yrTestDate.X, GetAbsoluteY(TxtDp5yrTestDate), true);
                        ShowLabel(LblDp5yrTestDate, yr5Message);
                        remainingFields.AppendLine(string.Format(yr5Message));
                        result = false;
                    }
                }
            }

            if (ViewModel.ChassisRequired == FieldRequirementCode.Value.Required)
            {
                if (!string.IsNullOrEmpty(PkrChassisNo.Text))
                {
                    var chassisValidatorParams = new ChassisValidatorParams()
                    {
                        ChassisNoValidFormat = ViewModel.ChassisNoValidFormat,
                        Size = ViewModel.TxtIdEqSize,
                        Type = ViewModel.TxtIdEqType,
                        chassisValue = PkrChassisNo.Text,
                        EqSizeTypeDisChassis = ViewModel.EqSizeTypeDisChassis
                    };

                    switch (ContainerChassisValidator.ChassisValidation(chassisValidatorParams))
                    {
                        case ContainerChassisValidator.ChassisValidationCode.NoChassisValue:
                            if (result) sclView.ScrollToAsync(PkrChassisNo.X, GetAbsoluteY(PkrChassisNo), true);
                            ShowLabel(LblChassisNo, string.Format(AppString.lblMandatoryField, AppString.lblChassis));
                            remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblChassis));
                            result = false;
                            break;
                        case ContainerChassisValidator.ChassisValidationCode.WrongFormat:
                            if (result) sclView.ScrollToAsync(PkrChassisNo.X, GetAbsoluteY(PkrChassisNo), true);
                            ShowLabel(LblChassisNo, string.Format(AppString.lblFieldLengthMin, AppString.lblChassis, 6));
                            remainingFields.AppendLine(string.Format(AppString.lblFieldLengthMin, AppString.lblChassis, 6));
                            result = false;
                            break;
                    }
                }
                else
                {
                    if (result) sclView.ScrollToAsync(PkrChassisNo.X, GetAbsoluteY(PkrChassisNo), true);
                    ShowLabel(LblChassisNo, string.Format(AppString.lblFieldLengthMin, AppString.lblChassis, 6));
                    remainingFields.AppendLine(string.Format(AppString.lblFieldLengthMin, AppString.lblChassis, 6));
                    result = false;
                }
            }

            if (!string.IsNullOrEmpty(remainingFields.ToString()))
                await DisplayAlert(AppString.lblErrorValidation, remainingFields.ToString(), AppString.btnDialogOk);
            return result;
        }
        private string DateValidator(string isCorrect, int showLabel)
        {
            var message = string.Empty;
            LblDp25yrTestDate.Text = string.Empty;
            LblDp5yrTestDate.Text = string.Empty;
            switch (Domain.Util.DateValidator.FormatDate(isCorrect))
            {
                case Domain.Util.DateValidator.FormatDateCode.Empty:
                    message = AppString.lblErrorDateRequired; break;
                case Domain.Util.DateValidator.FormatDateCode.TooShort:
                    message = AppString.lblErrorShortFormat; break;
                case Domain.Util.DateValidator.FormatDateCode.BadFormat:
                    message = AppString.lblErrorBadFormat; break;
            }
            return message;
        }
        private void CleanLabel(Label toModify)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                toModify.Text = string.Empty;
                toModify.IsVisible = false;
            });
        }
        private void ShowLabel(Label toModify, string toShow)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                toModify.Text = toShow;
                toModify.IsVisible = true;
                toModify.Focus();

            });
        }

        private double GetAbsoluteY(Xamarin.Forms.View view)
        {
            var y = view.Y;
            var parent = view.Parent as VisualElement;
            while (parent != null)
            {
                if (parent.GetType() == typeof(ScrollView)) break;
                y += parent.Y;
                parent = parent.Parent as VisualElement;
            }
            return y - view.Height;
        }
        #endregion
    }
}