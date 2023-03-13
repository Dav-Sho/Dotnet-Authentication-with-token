using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_authentication.Services
{
    public class AuthService : AuthRepo
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            
        }
        public async Task<ServiceResponse<string>> Login(string emai, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(emai.ToLower()));

            if(user is null) {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "User not found";
                return response;
            }else if(!comparePassword(password, user.PasswordHash, user.PasswordSalt)) {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Unvalid Credentials";
                return response;
            }else{
                response.Data = CreateToken(user);
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "User Login";
                return response;
            }
        }

        public async Task<ServiceResponse<string>> Register(User user, string password)
        {
            var response = new ServiceResponse<string>();
            if(await UserExist(user.Email)) {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "User already exist";
                return response;
            }

            CreateHashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            _context.Add(user);
            await _context.SaveChangesAsync();
            response.Data = CreateToken(user);
            response.StatusCode = HttpStatusCode.Created;
            response.Message = "User created";
            return response;

        }

        public async Task<bool> UserExist(string emai)
        {
            if(await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(emai.ToLower()))){
                return true;
            }

            return false;
        }


        private void CreateHashPassword(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using(var hmac =  new System.Security.Cryptography.HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool comparePassword(string password, byte[] passwordHash, byte[] passwordSalt) {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user) {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var appSetting = _configuration.GetSection("AppSettings:Token").Value;

            if(appSetting is null) {
                throw new Exception("App setting not found");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSetting));

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}