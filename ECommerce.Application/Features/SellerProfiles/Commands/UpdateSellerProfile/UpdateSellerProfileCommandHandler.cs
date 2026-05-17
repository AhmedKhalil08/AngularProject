using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;

public class UpdateSellerProfileCommandHandler : IRequestHandler<UpdateSellerProfileCommand, SellerProfileDto>
{
    private readonly ISellerProfileRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IFileService _fileService;

    public UpdateSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, IFileService fileService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _fileService = fileService;
    }

    public async Task<SellerProfileDto> Handle(UpdateSellerProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _repository.GetByIdAsync(request.Id);
        if (profile == null) throw new NotFoundException("Seller profile not found");
        if (profile.UserId != _currentUser.UserId) throw new ForbiddenAccessException("this is not your profile");

        profile.StoreName = request.StoreName;
        profile.StoreDescription = request.StoreDescription;

        if (request.Logo != null)
            profile.LogoUrl = await _fileService.UploadFileAsync(request.Logo, "logos");
        else if (request.LogoUrl != null)
            profile.LogoUrl = request.LogoUrl;

        await _repository.UpdateAsync(profile);
        await _unitOfWork.SaveChangesAsync();

        return new SellerProfileDto
        {
            Id = profile.Id,
            StoreName = profile.StoreName,
            StoreDescription = profile.StoreDescription,
            LogoUrl = profile.LogoUrl,
            IsApproved = profile.IsApproved,
            TotalEarnings = profile.TotalEarnings,
            UserId = profile.UserId
        };
    }
}