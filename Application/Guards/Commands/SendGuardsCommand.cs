using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Guards.Commands
{
    public class SendGuardsCommand : IRequest<Unit>
    {
        public SendGuardsCommand(List<GuardDto> guards)
        {
            Guards = guards;
        }

        public List<GuardDto> Guards { get; set; }
    }

    public class SendGuardsCommandHandler : IRequestHandler<SendGuardsCommand, Unit>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IEmailService _emailService;

        public SendGuardsCommandHandler(IAppDbContext appDbContext, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(SendGuardsCommand request, CancellationToken cancellationToken)
        {
            var workers = _appDbContext.Workers.ToList();
            foreach (Worker worker in workers) 
            {
                if (worker != null)
                {
                    var workerGuards = request.Guards.Where(g => g.WorkerId == worker.Id).ToList();
                    if (workerGuards.Any())
                    {
                        var subject = "Lista de gărzi";
                        var guardListHtml = string.Join("", workerGuards.Select(guard => $"<li>{guard.Date.Value.ToString("dd MMM yyyy")}</li>"));
                        var body = $@"
                                    <!DOCTYPE html>
                                    <html lang='ro'>
                                    <head>
                                        <meta charset='UTF-8'>
                                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                                        <title>Lista de Gărzi</title>
                                    </head>
                                    <body style='font-family: Arial, sans-serif; color: #333; margin: 0; padding: 0; background-color: #f4f4f4;'>
                                        <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #fff; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
                                            <h2 style='color: #007bff;'>Lista de Gărzi pentru Aplicația Guard Planner</h2>
                                            <p>Stimate {worker.Name} {worker.FirstName},</p>
                                            <p>Aceasta este lista de gărzi alocate:</p>
                                            <ul>{guardListHtml}</ul>
                                            <p>Cu stimă, echipa Guard Planner</p>
                                        </div>
                                    </body>
                                    </html>";

                        try
                        {
                            await _emailService.SendEmailAsync(worker.Email, subject, body);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to send email: {ex.Message}");
                            continue;
                        }
                    }
                }
            }
            return Unit.Value;
        }
    }
}
