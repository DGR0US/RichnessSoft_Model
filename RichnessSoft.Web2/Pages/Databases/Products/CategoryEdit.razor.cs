using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class CategoryEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        Category category { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Category/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = cateService.GetById(Id);
                category = (Category)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                category = new Category();
                category.companyid = store.CurentCompany.id;
                category.active = ConstUtil.ACTIVE.YES;
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
                        results = cateService.Add(category);
                    }
                    else if (Mode == gbVar.ModeEdit)
                    {
                        results = cateService.Edit(category);
                    }
                    _loaded = false;
                    if (results.Success)
                    {
                        await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                        if (Mode == gbVar.ModeInsert)
                        {
                            category = new Category();
                        }
                        else
                        {
                            NavigationManager.NavigateTo($"/Database/CategoryEdit/{ParrentMenu}");
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
            var res = cateService.GetByCode(category.companyid, category.code);
            if (res.Data != null && !string.IsNullOrEmpty(((Category)res.Data)?.code))
            {
                Category OldData = (Category)res.Data;
                if (Mode == gbVar.ModeInsert)
                {
                    bSucc = false;
                }
                else if (Mode == gbVar.ModeEdit && OldData.id != category.id)
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
                category.inactivedate = null;
                _picker.Clear();
            }
            StateHasChanged();
        }
    }
}