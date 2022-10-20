using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using RichnessSoft.Entity.Model;


namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class Model
    {
        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        List<Models> ListData = new List<Models>();
        private string _searchString { get; set; }
        private Models _models { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _loaded = true;
            await CheckState();
            Task.Delay(1000);
            await LoadData();
            _loaded = false;
        }

        async Task LoadData()
        {

            if (store.CurentCompany == null)
                return;

            var res = await Task.Run(() => modelService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<Models>)res.Data;
        }

        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }

        async void AddNewAsync()
        {
            string URL = $"/Database/ModelEdit/0/{ParrentMenu}";
            NavigationManager.NavigateTo(URL);
        }

        async void ReloadAsync()
        {
            _loaded = true;
            await LoadData();
            _loaded = false;
            StateHasChanged();
        }

        async void OnEdit(int id)
        {
            string URL = $"/Database/ModelEdit/{id}/{ParrentMenu}";
            NavigationManager.NavigateTo(URL);
        }

        private bool Search(Models model)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (model.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (model.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (model.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        async void OnDelete(int id)
        {
            bool? result = await Dialog.ShowMessageBox(
           "Deleted", Lng["CONFIRM_MSG_DEL"],
           yesText: "Yes", cancelText: "Cancel");

            if (result == true)
            {
                _loaded = true;
                var r = modelService.GetById(id);
                Models Md = (Models)r.Data;
                var res = modelService.Delete(Md);
                if (res.Success)
                {
                    await Dialog.ShowMessageBox("info", Lng["CONFIRM_MSG_DEL_SUCCESS"], "OK");
                    await LoadData();
                }
                else
                {
                    await Dialog.ShowMessageBox("info", Lng["CONFIRM_MSG_DEL_FAIL"], "OK");
                    await Dialog.ShowMessageBox("info", res.Message, "OK");
                }
                _loaded = false;
                StateHasChanged();
            }
        }
    }
}