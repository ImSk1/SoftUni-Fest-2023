namespace SoftwareFest.Services
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SoftwareFest.Infrastructure.Exceptions;
    using SoftwareFest.Models;
    using SoftwareFest.Services.Contracts;
    using SoftwareFest.ViewModels;
    using SofwareFest.Infrastructure;

    public class BusinessService : IBusinessService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BusinessService> _logger;
        private readonly IMapper _mapper;

        public BusinessService(ApplicationDbContext dbContext, ILogger<BusinessService> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task CreateBusiness(BusinessViewModel business)
        {
            var businessDbo = _mapper.Map<Business>(business);
            var user = await _userManager.FindByEmailAsync(business.Email);
            if (user == null)
            {
                throw new NotFoundException(
                    $"Cannot create business for user with email {business.Email}. User does not exist.");
            }
            var userId = user.Id;
            businessDbo.UserId = userId;
            await _dbContext.AddAsync(businessDbo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully created business with name {businessDbo.BusinessName}");
        }

        public async Task<BusinessViewModel> GetBusinessById(int id) 
            => _mapper.Map<BusinessViewModel>(await _dbContext.Businesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id));

        public async Task<BusinessViewModel> GetBusinessByUserId(string userId)
        {
            var user = await _userManager
                .Users
                .AsNoTracking()
                .Include(x => x.Business)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return _mapper.Map<BusinessViewModel>(user.Business);
        }

        public async Task UpdateBusiness(BusinessViewModel business)
        {
            var businessDbo = _mapper.Map<Business>(business);
            _dbContext.Update(businessDbo);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated business with name {business.Id}");
        }

        public async Task<bool> DeleteBusiness(int id)
        {
            var business = await _dbContext.Businesses.FindAsync(id);

            if (business == null)
            {
                _logger.LogWarning($"No business found with ID {id}");
                return false;
            }

            _dbContext.Businesses.Remove(business);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted business with ID {id}");
            return true;
        }
    }
}
