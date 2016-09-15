using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Snazzle.WebApi.ViewModels
{
  public class AuthorizeViewModel
  {
    [Display(Name = "Application")]
    public string ApplicationName { get; set; }

    [BindNever]
    public string RequestId { get; set; }

    [Display(Name = "Scope")]
    public string Scope { get; set; }
  }
}
