using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using RichnessSoft.Entity.Model;

namespace RichnessSoft.Web2.Pages.Databases.Products
{
    public partial class BrandP
    {
        [Parameter]
        public string ParrentMenu { get; set; }
        private bool _loaded;
        string backURL = "";
        List<Brand> ListData = new List<Brand>();
        private string _searchString { get; set; }
        private Brand _brand { get; set; }

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

            var res = await Task.Run(() => brandService.GetAllAsync(store.CurentCompany.id));
            ListData = (List<Brand>)res.Data;
        }

        protected override void OnParametersSet()
        {
            backURL = "SubMenu/" + ParrentMenu;
        }

        async void AddNewAsync()
        {
            string URL = $"/Database/BrandEdit/0/{ParrentMenu}";
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
            string URL = $"/Database/BrandEdit/{id}/{ParrentMenu}";
            NavigationManager.NavigateTo(URL);
        }

        private bool Search(Brand brand)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (brand.code?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (brand.name1?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (brand.name2?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
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
                var r = brandService.GetById(id);
                Brand brand = (Brand)r.Data;
                var res = brandService.Delete(brand);
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