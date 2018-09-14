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
    public partial class RegisterMoveView: IPersistentView
    {
        public RegisterMoveView()
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
        ~RegisterMoveView()
        {
            System.Diagnostics.Debug.WriteLine("********* RegisterView is about to be destroyed *********");
        }
#endif
        public void Dispose()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(" ******** RegisterMoveView Dispose() *********");
#endif
        }
        private void CleanControls()
        {
            stk25Yr.IsVisible = false;
            StackLayoutlbl25yrTest.IsVisible = false;
            TxtDp25yrTestDate.IsVisible = false;
            ImgDp25yrTestDate.IsVisible = false;
            LblDp25yrTestDate.IsVisible = false;

            stk5yr.IsVisible = false;
            StackLayoutlbl5yrTest.IsVisible = false;
            TxtDp5yrTestDate.IsVisible = false;
            ImgDp5yrTestDate.IsVisible = false;
            LblDp5yrTestDate.IsVisible = false;

            TxtDp5yrTestDate.Text = string.Empty;
            TxtDp25yrTestDate.Text = string.Empty;
            LayoutBob.IsVisible = false;
            StkOther.IsVisible = false;
            TxtBobtail.IsVisible = false;
            TxtBobtail.Text = string.Empty;
            TxtShipmentId.Text = string.Empty;
            CleanAllLabels();
        }

        private void CleanAllLabels()
        {
            CleanLabel(LblShipmentId);
            CleanLabel(LblCostCenter);
            CleanLabel(LblMoveType);
            CleanLabel(LblFromBlock);
            CleanLabel(LblToBlock);
            CleanLabel(LblToDispatching);
            CleanLabel(LblEquipmentType);
            CleanLabel(LblEquipmentSize);
            CleanLabel(LblEquipmentNo);
            CleanLabel(LblEquipmentStatus);
            CleanLabel(LblChassisNo);
            CleanLabel(LblProduct);
            CleanLabel(LblBobTail);
        }
        
        
        private void LoadLabel()
        {
            Title = AppString.titleRegisterMove;
            LblHeaderTimeMsg.Text = AppString.lblHeaderTimeMsg;
            LblDetails.Text = AppString.lblDetails;
            LblRegMoveChargeNo.Text = AppString.lblRegMoveChargeNo;
            LblMoveTypeT.Text = AppString.lblMoveType;
            PkrMoveType.Title = AppString.lblMoveType;
            LblFromBlockT.Text = AppString.lblFromBlock;
            LblToBlockT.Text = AppString.lblToBlock;
            LblDispatching.Text = AppString.lblDispatching;
            PkrToDispatching.Title = AppString.lblDispatching;
            LblEquipment.Text = AppString.lblEquipment;
            LblType.Text = AppString.lblType;
            PkrEquipmentType.Title = AppString.lblType;
            LblSize.Text = AppString.lblSize;
            PkrEquipmentSize.Title = string.Format(AppString.lblDefaultSelection, AppString.lblSize);
            LblNumber.Text = AppString.lblNumber;
            LblStatus.Text = AppString.lblStatus;
            PkrEquipmentStatus.Title = AppString.lblStatus;
            StackLayoutlbl25yrTest.Text = AppString.lbl25yrTest;
            StackLayoutlbl5yrTest.Text = AppString.lbl5yrTest;
            LblChassis.Text = AppString.lblChassis;
            LblRegMoveOther.Text = AppString.lblRegMoveOther;
            LblProductT.Text = AppString.lblProduct;
            LayoutBob.Text = AppString.lblBobTail;
            btnNext.Text = AppString.lblNext;
            TxtShipmentId.Placeholder = AppString.lblShipmentId;
        }
        #endregion
        #region Listeners
        private void LoadListener()
        {
            TabShip.ItemSelected += TabShipOnItemSelected;
            TxtShipmentId.TextChanged += TxtShipmentIdOnTextChanged;
            PkrMoveType.SelectedIndexChanged += PkrMoveTypeOnSelectedIndexChanged;
            TxtToBlock.PropertyChanged += TxtToBlock_PropertyChanged;
            TxtFromBlock.PropertyChanged += TxtFromBlockOnPropertyChanged;
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
            TxtBobtail.TextChanged += TxtBobtailTextChanged;
            btnNext.Clicked += BtnNextOnClicked;
        }

        private void TxtProduct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtProduct.Text)) CleanLabel(LblProduct);
        }

        private void TxtToBlock_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtToBlock.Text)) CleanLabel(LblToBlock);
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
            if (PkrMoveType?.SelectedItem == null) return;

            var dto = (ServiceDTO)PkrMoveType.SelectedItem;
            if (string.IsNullOrEmpty(dto.Code)) return;
            LayoutBob.IsVisible = false;
            StkOther.IsVisible = false;
            TxtBobtail.IsVisible = false;
            TxtBobtail.Text = string.Empty;
            if (dto.Code.ToUpper().Contains(MoveCode.MoveType.BobTailCode))
            {
                LayoutBob.IsVisible = true;
                StkOther.IsVisible = true;
                TxtBobtail.IsVisible = true;
                TxtBobtail.Text = string.Empty;
            }
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
            var dto = PkrToDispatching.SelectedItem as DispatchingPartyDTO;
            if (dto != null && dto.DispatchingPartyId != MoveCode.DefaultValue.ValuePickerDefault)
            {
                CleanLabel(LblToDispatching);
            }
        }
        private void PkrEquipmentTypeOnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            var dto = PkrEquipmentType.SelectedItem as EquipmentTypeDTO;
            if (dto == null) return;
            txtIdEqType.Text = dto.Code;

            stk25Yr.IsVisible = false;
            StackLayoutlbl25yrTest.IsVisible = false;
            TxtDp25yrTestDate.IsVisible = false;
            ImgDp25yrTestDate.IsVisible = false;

            stk5yr.IsVisible = false;
            StackLayoutlbl5yrTest.IsVisible = false;
            TxtDp5yrTestDate.IsVisible = false;
            ImgDp5yrTestDate.IsVisible = false;
            CleanAllLabels();

            if (string.IsNullOrEmpty(dto.Code)) return;
            if (!dto.Code.ToUpper().Contains(MoveCode.EquipmentType.TankCode)) return;
            TxtDp25yrTestDate.Text = string.IsNullOrEmpty(ViewModel.TxtDp25yrTestDate) ? string.Empty : ViewModel.TxtDp25yrTestDate;
            stk25Yr.IsVisible = true;
            StackLayoutlbl25yrTest.IsVisible = true;
            TxtDp25yrTestDate.IsVisible = true;
            ImgDp25yrTestDate.IsVisible = true;

            TxtDp5yrTestDate.Text = string.IsNullOrEmpty(ViewModel.TxtDp5yrTestDate) ? string.Empty : ViewModel.TxtDp5yrTestDate;
            stk5yr.IsVisible = true;
            StackLayoutlbl5yrTest.IsVisible = true;
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
        private void TxtBobtailTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue)) CleanLabel(LblBobTail);
        }
        private async void BtnNextOnClicked(object sender, EventArgs eventArgs)
        {
            await FieldValidator();
        }

        #endregion

        #region LoadToolBar

        private RegisterMoveViewModel ViewModel => BindingContext as RegisterMoveViewModel;

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
                            sclView.ScrollToAsync(TxtShipmentId.X, GetAbsoluteY(TxtShipmentId), true);
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
            //Service Validation
            var dtoService = (ServiceDTO)PkrMoveType.SelectedItem;
            if (dtoService.ServiceId == MoveCode.DefaultValue.ValuePickerDefault)
            {
                ShowLabel(LblMoveType, string.Format(AppString.lblMandatoryField, AppString.lblMoveType));
                remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblMoveType));
                if(result) sclView.ScrollToAsync(PkrMoveType.X, GetAbsoluteY(PkrMoveType), true);
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
                if (string.IsNullOrEmpty(TxtToBlock.Text))
                {
                    if (result) sclView.ScrollToAsync(TxtToBlock.X, GetAbsoluteY(TxtToBlock), true);
                    ShowLabel(LblToBlock, string.Format(AppString.lblMandatoryField, AppString.lblToBlock));
                    remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblToBlock));
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
                    ContainerValue = ViewModel.GeneralMove.EquipmentNumber,
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

            if (string.IsNullOrWhiteSpace(TxtBobtail.Text) && dtoService.Code == MoveCode.MoveType.BobTailCode)
            {
                if (result) sclView.ScrollToAsync(TxtBobtail.X, GetAbsoluteY(TxtBobtail), true);
                ShowLabel(LblBobTail, string.Format(AppString.lblMandatoryField, AppString.lblBobTail));
                remainingFields.AppendLine(string.Format(AppString.lblMandatoryField, AppString.lblBobTail));
                result = false;
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
        private void ShowLabel(Label toModify,string toShow)
        {
            Device.BeginInvokeOnMainThread(() => {
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
