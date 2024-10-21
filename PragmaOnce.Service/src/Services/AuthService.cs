using AutoMapper;
using PragmaOnce.Core.src.Common;
using PragmaOnce.Core.src.Entities;
using PragmaOnce.Core.src.Interfaces;
using PragmaOnce.Service.src.DTOs;
using PragmaOnce.Service.src.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace PragmaOnce.Service.src.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IPasswordService passwordService, IMapper mapper)
        {
            _userRepo = userRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _mapper = mapper;
        }
        public async Task<UserReadDto> AuthenticateUserAsync(string token)
        {
            var userId = _tokenService.VerifyToken(token);
            var user = await _userRepo.GetByIdAsync(userId);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<string> LogInAsync(UserCredential userCredential)
        {
            var foundByEmail = await _userRepo.GetByEmailAsync(userCredential.Email) ?? throw AppException.NotFound();
            var isPasswordMatch = _passwordService.VerifyPassword(foundByEmail, foundByEmail.Password!, userCredential.Password);
            if (isPasswordMatch == PasswordVerificationResult.Failed)
            {
                throw AppException.InvalidLoginCredentialsException();
            }
            return _tokenService.GetToken(foundByEmail);
        }

        public async Task<string> LogoutAsync()
        {
            return await _tokenService.InvalidateTokenAsync();
        }
    }
}