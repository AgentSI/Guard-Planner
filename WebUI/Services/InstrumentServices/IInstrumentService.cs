using Application.DTOs;
using Application.Interfaces.Pagination;
using MediatR;

namespace WebUI.Services.InstrumentServices
{
    public interface IInstrumentService
    {
        Task<PaginationResult<InstrumentDto>> GetInstruments(PaginationParameter queryModel, Guid guardId);
        Task<Guid> InstrumentCreate(InstrumentDto request);
        Task<Unit> InstrumentDelete(Guid id);
        Task<Unit> InstrumentEdit(InstrumentDto request);
        Task<InstrumentDto> GetInstrumentById(Guid id);
    }
}
