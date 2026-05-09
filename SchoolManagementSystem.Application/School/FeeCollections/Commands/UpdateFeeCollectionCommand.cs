using SchoolManagementSystem.Application.School.FeeCollections.Models;

namespace SchoolManagementSystem.Application.School.FeeCollections.Commands;

public class UpdateFeeCollectionCommand : IHttpRequest
{
    public FeeCollectionRequest FeeCollection { get; set; }
}
