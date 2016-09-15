using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Snazzle.WebApi.ViewModels
{
  public class LogoutViewModel
  {
    [BindNever]
    public string RequestId { get; set; }
  }
}
