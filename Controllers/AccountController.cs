using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using DelusiveVortexAspApi.Models;
using DelusiveVortexAspApi.ViewModels;
using DelusiveVortexAspApi.JWT;

namespace DelusiveVortexAspApi.Controllers
{
    [Route("api/Аккаунт")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <remarks>
        /// Создает нового пользователя и возвращает JWT-токен для него.
        /// </remarks>
        [HttpPost("Регистрация")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel { UserName = model.Username, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { токен = _jwtTokenGenerator.GenerateToken(user) });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Авторизует пользователя.
        /// </summary>
        /// <remarks>
        /// Проверяет данные пользователя и возвращает JWT-токен в случае успеха.
        /// </remarks>
        [HttpPost("Авторизация")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return Ok(new { токен = _jwtTokenGenerator.GenerateToken(user) });
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Обновляет данные пользователя по логину.
        /// </summary>
        /// <remarks>
        /// Полное обновление всех данных пользователя.
        /// </remarks>
        [HttpPut("ОбновитьПоЛогину/{username}")]
        public async Task<IActionResult> UpdateUserByUsername(string username, [FromBody] UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(user);
                }

                return BadRequest(result.Errors);
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Обновляет данные пользователя по логину, но только email и логин!.
        /// </summary>
        /// <remarks>
        /// Обновляет логин или email.
        /// </remarks>
        [HttpPatch("ОбновитьЧастичноПоЛогину/{username}")]
        public async Task<IActionResult> PatchUserByUsername(string username, [FromBody] UserModel model)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.UserName))
            {
                user.UserName = model.UserName;
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                user.Email = model.Email;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(user);
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Удаляет пользователя по логину.
        /// </summary>
        /// <remarks>
        /// Аннигилирует всего пользователя по логину.
        /// </remarks>
        [HttpDelete("УдалитьПоЛогину/{username}")]
        public async Task<IActionResult> DeleteUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}
