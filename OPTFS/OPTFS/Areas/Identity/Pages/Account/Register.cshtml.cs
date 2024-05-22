// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using OPTFS.Data;
using OPTFS.Models;

namespace OPTFS.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext db;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.db = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            #nullable enable
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            /// 
            [Required]            
            [Display(Name = "Full Name")]
            public string? FullName { get; set; }


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string? Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string? Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string? ConfirmPassword { get; set; }

            public string? UserTypeId { get; set; }

            [MaxLength(100)]
            public string? ContactInfo { get; set; }

            [Display(Name = "Specialty")]
            public int? SpecialtyId { get; set; }

            public virtual Specialty? Specialty { get; set; }

            [MaxLength(255)]
            public string? Qualifications { get; set; }

            [MaxLength(255)]
            public string? Experience { get; set; }

            [MaxLength(255)]
            public string? Style { get; set; }

            public virtual List<Course>? Courses { get; set; }

            [Display(Name = "Country")]
            public int? CountryId { get; set; }

            public virtual Country? Country { get; set; }

            [Display(Name = "City")]
            public int? CityId { get; set; }

            public virtual City? City { get; set; }
        }


        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ViewData["SpecialtyId"] = new SelectList(db.Specialty, "Id", "Name");
            ViewData["CountryId"] = new SelectList(db.Country, "Id", "Name",1);
            //ViewData["CityId"] = new SelectList(db.City, "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            string? UserTypeId = Request.Form["UserTypeId"];
            var FullName = Request.Form["Input.FullName"];
            var CountryId = Convert.ToInt32(Request.Form["Input.CountryId"]);
            var CityId = Convert.ToInt32(Request.Form["Input.CityId"]);
            var SpecialtyId = Convert.ToInt32(Request.Form["Input.SpecialtyId"]);
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FullName = FullName;
                user.UserTypeId = UserTypeId;
                user.CountryId = CountryId;
                user.CityId = CityId;
                user.SpecialtyId = SpecialtyId;
                user.Qualifications = Request.Form["Qualifications"];
                user.Style = Request.Form["Style"];                
                user.Experience = Request.Form["Experience"];

                if (Request.Form.Files?.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string url = @"uploads/" + DateTime.Now.Ticks + "_" + file.FileName.GetHashCode() + System.IO.Path.GetExtension(file.FileName);
                    string newPath = @"wwwroot\" + url.Replace('/', '\\');
                    string dir = System.IO.Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), newPath);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        user.PersonalImageUrl = url;
                    }
                }

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, UserTypeId);
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);                    
                    //await OPTFS.EmailSender.Send("Confirm your email", $"Please confirm your email by <a href='"+callbackUrl+"'>clicking here</a>.", Input.Email);
                    await OPTFS.EmailSender.Send("Confirm your email",$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl).Replace("&amp;code=", "&code=")}'>clicking here</a>.", Input.Email);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, DisplayConfirmAccountLink=false, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
