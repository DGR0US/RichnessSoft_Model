using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RichnessSoft.Common;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class ModelxEdit
    {
        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        string Mode { get; set; }
        Models models { get; set; }
        MudDatePicker _picker;

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        protected override async Task OnInitializedAsync()
        {
            backURL = "/Database/Model/" + ParrentMenu;
            if (Id > 0)
            {
                Mode = gbVar.ModeEdit;
                var r = modelService.GetById(Id);
                models = (Models)r.Data;
            }
            else
            {
                Mode = gbVar.ModeInsert;
                models = new Models();
                models.companyid = store.CurentCompany.id;
                models.active = ConstUtil.ACTIVE.YES;
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
                            results = modelService.Add(models);
                        }
                        else if (Mode == gbVar.ModeEdit)
                        {
                            results = modelService.Edit(models);
                        }
                        _loaded = false;
                        if (results.Success)
                        {
                            await Dialog.ShowMessageBox("info", Lng["SAVE_MSG_SUCCESS"], "OK");
                            if (Mode == gbVar.ModeInsert)
                            {
                                models = new Models();
                            }
                            else
                            {
                                NavigationManager.NavigateTo($"/Database/Model/{ParrentMenu}");
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
                var res = modelService.GetByCode(models.companyid, models.code);
                if (res.Data != null && !string.IsNullOrEmpty(((Models)res.Data)?.code))
                {
                    Models OldData = (Models)res.Data;
                    if (Mode == gbVar.ModeInsert)
                    {
                        bSucc = false;
                    }
                    else if (Mode == gbVar.ModeEdit && OldData.id != models.id)
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
                var sss = values.ToArray();
                if (sss[0] == ConstUtil.ACTIVE.YES)
                {
                    models.inactivedate = null;
                    _picker.Clear();
                }
                StateHasChanged();
            }
        
    }
}