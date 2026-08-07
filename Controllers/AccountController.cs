using System.Security.Claims;
using InsurancePortal.Migrations;
using InsurancePortal.Models;
using InsurancePortal.Services;
using InsurancePortal.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace InsurancePortal.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<Users> signInManager;
		private readonly UserManager<Users> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IEmailService emailService;

		public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
		{
			this.signInManager = signInManager;
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.emailService = emailService;
		}

		[HttpGet]

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			
			if (!ModelState.IsValid)
			{
				return View(model);
			}

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
			{
				var user = await userManager.FindByEmailAsync(model.Email);
				if (await userManager.IsInRoleAsync(user, "Admin"))
				{
					return RedirectToAction("Index", "Admin");
				}

				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
			return View(model);

		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
			if (!ModelState.IsValid)
			{
				return View(model);

			}
			var user = new Users
			{
				FullName = model.Name,
				UserName = model.Email,
				NormalizedUserName = model.Email.ToUpper(),
				Email = model.Email,
				NormalizedEmail = model.Email.ToUpper()
			};
			var result = await userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				var roleExits = await roleManager.RoleExistsAsync("User");

				if (!roleExits)
				{
					var role = new IdentityRole("User");
					await roleManager.CreateAsync(role);
				}
				await userManager.AddToRoleAsync(user, "User");

				await signInManager.SignInAsync(user, isPersistent: false);

				return RedirectToAction("Login", "Account");
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}

		[HttpGet]

		public IActionResult VerifyEmail()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "User Not Found");
				return View(model);
			}

			var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
			var resetLink = Url.Action("ChangedPassword", "Account", new { email = model.Email, token = resetToken }, Request.Scheme);

			var subject = "Reset Your Password";
            //var body = $"Please reset your password by clicking here: <a href=`{resetLink}`>Reset Password</a>";
            var body = $@"
<!DOCTYPE html>
<html><head><meta charset='UTF-8'><meta name='viewport' content='width=device-width, initial-scale=1.0'></head>
<body style='margin:0;padding:0;background-color:#e8edf2;font-family:Arial,sans-serif;'>
<table width='100%' cellpadding='0' cellspacing='0' style='background:#e8edf2;padding:30px 20px;'>
<tr><td align='center'>
<table width='800' cellpadding='0' cellspacing='0' style='max-width:800px;width:100%;border-radius:12px;overflow:hidden;box-shadow:0 8px 40px rgba(0,0,0,0.25);'>

  <!-- HEADER -->
  <tr>
    <td style='background:#0f2744;padding:22px 40px;'>
      <table width='100%' cellpadding='0' cellspacing='0'>
        <tr>
          <td>
            <table cellpadding='0' cellspacing='0'>
              <tr>
                <td style='background:#c8a84b;width:44px;height:44px;border-radius:50%;text-align:center;vertical-align:middle;font-size:22px;'>&#x1F6E1;&#xFE0F;</td>
                <td style='padding-left:14px;'>
                  <div style='color:#c8a84b;font-size:10px;letter-spacing:3px;text-transform:uppercase;'>Trusted Vehicle Coverage</div>
                  <div style='color:#ffffff;font-size:20px;font-weight:bold;letter-spacing:1px;'>Vehicle Insurance</div>
                </td>
              </tr>
            </table>
          </td>
          <td align='right'>
            <div style='color:#c8a84b;font-size:11px;letter-spacing:2px;text-transform:uppercase;'>Account Security</div>
            <div style='color:#7ba7d4;font-size:12px;margin-top:2px;'>no-reply@driveshield.com</div>
          </td>
        </tr>
      </table>
    </td>
  </tr>

  <!-- HERO LANDSCAPE BANNER -->
  <tr>
    <td style='background:#0f2744;padding:0;'>
      <table width='100%' cellpadding='0' cellspacing='0'>
        <tr>
          <!-- Left Car Section -->
          <td width='42%' style='background:#0a1f3d;padding:35px 30px;text-align:center;vertical-align:middle;'>
            <div style='font-size:72px;'>&#x1F697;</div>
            <div style='background:#c8a84b;height:3px;width:120px;margin:12px auto 6px;border-radius:2px;'></div>
            <div style='color:#7ba7d4;font-size:10px;letter-spacing:3px;text-transform:uppercase;'>Protected &amp; Secured</div>
          </td>
          <!-- Divider -->
          <td width='3' style='background:#c8a84b;opacity:0.7;'></td>
          <!-- Right Text Section -->
          <td style='padding:35px 40px;vertical-align:middle;'>
            <div style='color:#c8a84b;font-size:10px;letter-spacing:4px;text-transform:uppercase;margin-bottom:8px;'>Security Notice</div>
            <div style='color:#ffffff;font-size:30px;font-weight:bold;line-height:1.15;'>Reset Your</div>
            <div style='color:#c8a84b;font-size:30px;font-weight:bold;line-height:1.15;margin-bottom:16px;'>Password</div>
            <table cellpadding='0' cellspacing='0' style='border-left:3px solid #c8a84b;background:rgba(200,168,75,0.15);border-radius:0 6px 6px 0;'>
              <tr>
                <td style='padding:10px 14px;color:#d4c17a;font-size:12px;line-height:1.6;'>
                  A reset was requested for your account.<br>This link expires in <strong style='color:#c8a84b;'>24 hours</strong>.
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </td>
  </tr>

  <!-- MAIN BODY -->
  <tr>
    <td style='background:#ffffff;padding:36px 40px;'>
      <p style='margin:0 0 14px;color:#1e3a5f;font-size:16px;font-weight:bold;'>Dear Valued Member,</p>
      <p style='margin:0 0 20px;color:#374151;font-size:14px;line-height:1.8;'>We received a request to reset the password for your <strong style='color:#0f2744;'>DriveShield Insurance</strong> account. Your vehicle coverage and account security are our top priorities.</p>
      <p style='margin:0 0 28px;color:#374151;font-size:14px;line-height:1.8;'>Click the secure button below to create a new password and regain full access to your policies, claims, and coverage details.</p>

      <!-- CTA BUTTON -->
      <table width='100%' cellpadding='0' cellspacing='0'>
        <tr>
          <td align='center' style='padding-bottom:8px;'>
            <a href='{resetLink}' style='display:inline-block;background:#0f2744;color:#ffffff;font-size:14px;font-weight:bold;letter-spacing:2px;text-transform:uppercase;text-decoration:none;padding:16px 52px;border-radius:50px;border:2px solid #c8a84b;'>
              &#x1F513; &nbsp; Reset My Password
            </a>
          </td>
        </tr>
        <tr>
          <td align='center' style='padding-bottom:28px;color:#9ca3af;font-size:11px;'>Button valid for 24 hours only</td>
        </tr>
      </table>

      <!-- 3 FEATURES -->
      <table width='100%' cellpadding='0' cellspacing='0'>
        <tr>
          <td width='32%' style='background:#f0f4ff;border-top:3px solid #0f2744;border-radius:6px;padding:16px;text-align:center;'>
            <div style='font-size:26px;margin-bottom:6px;'>&#x1F698;</div>
            <div style='color:#0f2744;font-size:11px;font-weight:bold;letter-spacing:1px;text-transform:uppercase;'>Auto Coverage</div>
            <div style='color:#6b7280;font-size:11px;margin-top:4px;'>Full vehicle protection</div>
          </td>
          <td width='2%'></td>
          <td width='32%' style='background:#fffbf0;border-top:3px solid #c8a84b;border-radius:6px;padding:16px;text-align:center;'>
            <div style='font-size:26px;margin-bottom:6px;'>&#x1F4CB;</div>
            <div style='color:#0f2744;font-size:11px;font-weight:bold;letter-spacing:1px;text-transform:uppercase;'>Easy Claims</div>
            <div style='color:#6b7280;font-size:11px;margin-top:4px;'>Fast &amp; hassle-free process</div>
          </td>
          <td width='2%'></td>
          <td width='32%' style='background:#f0f7ff;border-top:3px solid #1e6bbf;border-radius:6px;padding:16px;text-align:center;'>
            <div style='font-size:26px;margin-bottom:6px;'>&#x1F4DE;</div>
            <div style='color:#0f2744;font-size:11px;font-weight:bold;letter-spacing:1px;text-transform:uppercase;'>24/7 Support</div>
            <div style='color:#6b7280;font-size:11px;margin-top:4px;'>Always here to help you</div>
          </td>
        </tr>
      </table>

      <!-- COVERAGE PLANS -->
      <table width='100%' cellpadding='0' cellspacing='0' style='margin-top:24px;border-top:1px solid #e5e7eb;padding-top:20px;'>
        <tr><td align='center' style='color:#0f2744;font-size:10px;letter-spacing:3px;text-transform:uppercase;padding-bottom:12px;'>Our Coverage Plans</td></tr>
        <tr>
          <td width='32%' style='background:#0f2744;border-radius:8px;padding:18px 12px;text-align:center;'>
            <div style='font-size:22px;margin-bottom:6px;'>&#x1F699;</div>
            <div style='color:#c8a84b;font-size:11px;font-weight:bold;'>Comprehensive</div>
            <div style='color:#9bb5d4;font-size:10px;margin-top:3px;'>Full Protection</div>
          </td>
          <td width='2%'></td>
          <td width='32%' style='background:#c8a84b;border-radius:8px;padding:18px 12px;text-align:center;'>
            <div style='font-size:22px;margin-bottom:6px;'>&#x1F6E1;&#xFE0F;</div>
            <div style='color:#fff;font-size:11px;font-weight:bold;'>Third Party</div>
            <div style='color:#0f2744;font-size:10px;margin-top:3px;font-weight:bold;'>Liability Cover</div>
          </td>
          <td width='2%'></td>
          <td width='32%' style='background:#1e4d8c;border-radius:8px;padding:18px 12px;text-align:center;'>
            <div style='font-size:22px;margin-bottom:6px;'>&#x2B50;</div>
            <div style='color:#fff;font-size:11px;font-weight:bold;'>Premium Plus</div>
            <div style='color:#b0c9e8;font-size:10px;margin-top:3px;'>All-in-One Deal</div>
          </td>
        </tr>
      </table>
    </td>
  </tr>

  <!-- FOOTER -->
  <tr>
    <td style='background:#0f2744;padding:24px 40px;'>
      <table width='100%' cellpadding='0' cellspacing='0'>
        <tr>
          <td>
            <div style='color:#c8a84b;font-size:13px;font-weight:bold;margin-bottom:3px;'>DriveShield Insurance Co.</div>
            <div style='color:#6b8aad;font-size:11px;'>123 Insurance Plaza &bull; support@driveshield.com &bull; 1-800-DRIVE-ON</div>
          </td>
          <td align='right'>
            <div style='color:#6b8aad;font-size:11px;margin-bottom:3px;'>If you didn&apos;t request this, ignore this email.</div>
            <a href='#' style='color:#c8a84b;font-size:11px;text-decoration:none;'>Unsubscribe</a>
            <span style='color:#4b5563;margin:0 6px;'>|</span>
            <a href='#' style='color:#c8a84b;font-size:11px;text-decoration:none;'>Privacy Policy</a>
          </td>
        </tr>
      </table>
    </td>
  </tr>

</table>
</td></tr>
</table>
</body></html>";

            await emailService.SendEmailAsync(model.Email, subject, body);

			return RedirectToAction("EmailSent", "Account");




		}

		[HttpGet]

		public IActionResult ChangedPassword(string email, string token)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
			{
				return RedirectToAction("VerifyEmail", "Account");
			}

			var model = new ChangePasswordViewModel
			{
				Email = email,
				Token = token
			};
			return View(model);

		}

		[HttpPost]

		public async Task<IActionResult> ChangedPassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError("", "Somethig went wrong");
				return View(model);
			}

			var user = await userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "User not Found");
				return View(model);
			}
			var resetResult = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

			if (!resetResult.Succeeded)
			{
				foreach (var error in resetResult.Errors)
				{
					ModelState.AddModelError("", error.Description);

				}

			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
			return View(model);

		}


		public IActionResult EmailSent()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
