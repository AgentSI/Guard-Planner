using Application.DTOs;
using MediatR;

namespace WebUI.Services.ReceiptServices
{
    public interface IReceiptService
    {
        Task<Guid> ReceiptCreate(ReceiptDto request);
        Task<Unit> ReceiptDelete(Guid id);
        Task<Unit> ReceiptEdit(ReceiptDto request);
        Task<ReceiptDto> GetReceiptById(Guid id);
    }
}
