using Microsoft.JSInterop;

namespace HiddenVilla_Server.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToastrSuccess(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("ShowToastr", "success", message);

        }
        public static async ValueTask ToastrError(this IJSRuntime JsRuntime, string message)
        {
            await JsRuntime.InvokeVoidAsync("ShowToastr", "error", message);

        }

        public static async ValueTask SweetSuccess(this IJSRuntime JsRuntime, string message)
        {

        }
    }
}
