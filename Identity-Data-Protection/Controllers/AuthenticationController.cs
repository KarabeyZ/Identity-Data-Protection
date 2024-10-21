using Identity_Data_Protection.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity_Data_Protection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, AppDbContext appDbContext, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _signInManager = signInManager;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]User user)
        {
            if(ModelState.IsValid)
            {
                var IdUser = new IdentityUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };
                var rgPassword = await _userManager.CreateAsync(IdUser, user.Password);

                if (rgPassword.Succeeded)
                {
                    user.Password = IdUser.PasswordHash;
                    _appDbContext.Users.Add(user);
                    await _appDbContext.SaveChangesAsync();
                    return Ok(new { Message = "Kayıt başarılı bir şekilde oluşturuldu." });
                }
            }

            return BadRequest(new { Errors = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });

        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var signIn = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, false, false);
                if(signIn.Succeeded)
                {
                    return Ok(new { Message = "Giriş başarılı" });
                }
                else
                {
                    return Unauthorized(new {Message = "Mail ya da şifre yanlış."});
                }
                
            }
            return BadRequest(new { Messages = ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage) });
        }
    }
}
