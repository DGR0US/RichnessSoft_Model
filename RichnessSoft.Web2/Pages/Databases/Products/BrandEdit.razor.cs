using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class BrandEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        Brand brand { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Brand/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = brandService.GetById(Id);
                brand = (Brand)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                brand = new Brand();
                brand.companyid = store.CurentCompany.id;
                brand.active = ConstUtil.ACTIVE.YES;
            }
        }
        async void SaveAsync()
        {
            ResultModel results = new ResultModel();
            try
            {
                _loaded = true;
                string strErrMsg = "";
                string strErrFocus = "";
                if (Validated && CheckDupCode())
                {
                    if (Mode == gbVar.ModeInsert)
                    {
                        results = brandService.Add(brand);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = brandService.Edit(brand);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            brand = new Brand();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/Brand/{ParrentMenu}");
                        }
                    }
                    else
                    {
                        await Dialog.ShowMessageBox("error", Lng["SAVE_MSG_FAIL"], "OK");
                        await Dialog.ShowMessageBox("info", results.Message, "OK");
                    }
                    StateHasChanged();
                }
                _loaded = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckDupCode()
        {
            bool bSucc = true;
            var res = brandService.GetByCode(brand.companyid, brand.code);
            if (res.Data != null && !string.IsNullOrEmpty(((Brand)res.Data)?.code))
            {
                Brand OldData = (Brand)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != brand.id)
                {
                    bSucc = false;
                }
            }
            if (bSucc == false)
            {
                _snackBar.Add(Lng["DUPLICATED_CODE"], Severity.Error);
            }
            return bSucc;
        }

        async void activeChange(IEnumerable<string> values)
        {
            var checkActive = values.ToArray();
            if (checkActive[0] == ConstUtil.ACTIVE.YES)
            {
                brand.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }
    }

}