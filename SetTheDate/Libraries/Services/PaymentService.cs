using Microsoft.EntityFrameworkCore;
using SetTheDate.Libraries.Dtos;
using SetTheDate.Libraries.Repositories;

namespace SetTheDate.Libraries.Services
{
    public class PaymentService
    {
        public readonly PaymentInformationRepository _paymentInformationRepository;
        private readonly ApplicationDbContext _context;

        public PaymentService(PaymentInformationRepository paymentInformationRepository, ApplicationDbContext context)
        {
            _paymentInformationRepository = paymentInformationRepository;
            _context = context;
        }

        public async Task<PaymentInformation> GetPaymentByIdAsync(int id)
        {
            return await _paymentInformationRepository.GetByIdAsync(id);
        }

        public async Task<PaymentInformation> InsertPayment(PaymentInformation paymentInformation)
        {
            _paymentInformationRepository.Add(paymentInformation);
            await _context.SaveChangesAsync();

            return paymentInformation;
        }
        public async Task<PaymentInformation> UpdatePayment(PaymentInformation paymentInformation)
        {
            _paymentInformationRepository.Update(paymentInformation);
            await _context.SaveChangesAsync();

            return paymentInformation;
        }
        public async Task DeletePayment(PaymentInformation paymentInformation)
        {
            _paymentInformationRepository.Delete(paymentInformation);
            await _context.SaveChangesAsync();
        }
    }
}
