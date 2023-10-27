using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoftwareFest.Infrastructure.Exceptions;
using SoftwareFest.Models;
using SoftwareFest.Services.Contracts;
using SoftwareFest.ViewModels;
using SofwareFest.Infrastructure;

namespace SoftwareFest.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BusinessService> _logger;
        private readonly IMapper _mapper;

        public ClientService(ApplicationDbContext dbContext, ILogger<BusinessService> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task CreateClient(ClientViewModel client)
        {
            var clientDbo = _mapper.Map<Client>(client);
            var user = await _userManager.FindByEmailAsync(client.Email);
            if (user == null)
            {
                throw new NotFoundException(
                    $"Cannot create client profile for user with email {client.Email}. User does not exist.");
            }
            var userId = user.Id;
            clientDbo.UserId = userId;
            await _dbContext.AddAsync(clientDbo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully created client profile with name {client.FirstName + " " + client.LastName}");
        }
    }
}
