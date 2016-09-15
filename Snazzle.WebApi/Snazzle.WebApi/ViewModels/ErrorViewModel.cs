using System.ComponentModel.DataAnnotations;

namespace Snazzle.WebApi.ViewModels
{
  public class ErrorViewModel
  {
    [Display(Name = "Error")]
    public string Error { get; set; }

    [Display(Name = "Description")]
    public string ErrorDescription { get; set; }
  }
}
