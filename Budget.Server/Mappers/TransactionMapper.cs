using Budget.Server.Dtos;
using Budget.Server.Entities;
using Budget.Server.Enums;
using Budget.Server.Queries;

namespace Budget.Server.Mappers
{
    public class TransactionMapper
    {
        public TransactionDTO.GetAll.Response ToGetAllResponse(IReadOnlyList<TransactionQuery> queries)
        {
            return new TransactionDTO.GetAll.Response
            {
                Transactions = queries.Select(x => new TransactionDTO.TransactionBase
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    Date = x.Date,
                    PaymentMethod = x.PaymentMethod,
                    Comment = x.Comment
                }).ToList()
            };
        }

        public TransactionDTO.Get.Response ToGetResponse(TransactionQuery query)
        {
            return new TransactionDTO.Get.Response
            {
                Id = query.Id,
                Amount = query.Amount,
                Date = query.Date,
                PaymentMethod = query.PaymentMethod,
                Comment = query.Comment
            };
        }

        public Transaction ToEntity(TransactionDTO.Create.Command command)
        {
            return new Transaction
            {
                Amount = command.Amount,
                Date = command.Date,
                PaymentMethod = command.PaymentMethod,
                Comment = command.Comment
            };
        }

        public TransactionDTO.Create.Response ToCreateResponse(Transaction entity)
        {
            return new TransactionDTO.Create.Response
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Date = entity.Date,
                PaymentMethod = entity.PaymentMethod,
                Comment = entity.Comment
            };
        }

        public Transaction ToEntity(TransactionDTO.Update.Command command)
        {
            return new Transaction
            {
                Amount = command.Amount,
                Date = command.Date,
                PaymentMethod = command.PaymentMethod,
                Comment = command.Comment
            };
        }

        public TransactionDTO.Update.Response ToUpdateResponse(Transaction entity)
        {
            return new TransactionDTO.Update.Response
            {
                Amount = entity.Amount,
                Date = entity.Date,
                PaymentMethod = entity.PaymentMethod,
                Comment = entity.Comment
            };
        }

        public double ToAmount(TransactionDTO.UpdateAmount.Command command)
        {
            return command.Amount;
        }

        public TransactionDTO.UpdateAmount.Response ToUpdateAmountResponse(double amount)
        {
            return new TransactionDTO.UpdateAmount.Response
            {
                Amount = amount
            };
        }

        public DateOnly? ToDate(TransactionDTO.UpdateDate.Command command)
        {
            return command.Date;
        }

        public TransactionDTO.UpdateDate.Response ToUpdateDateResponse(DateOnly? date)
        {
            return new TransactionDTO.UpdateDate.Response
            {
                Date = date
            };
        }

        public PaymentMethod? ToPaymentMethod(TransactionDTO.UpdatePaymentMethod.Command command)
        {
            return command.PaymentMethod;
        }

        public TransactionDTO.UpdatePaymentMethod.Response ToUpdatePaymentMethodResponse(PaymentMethod? paymentMethod)
        {
            return new TransactionDTO.UpdatePaymentMethod.Response
            {
                PaymentMethod = paymentMethod
            };
        }

        public string? ToComment(TransactionDTO.UpdateComment.Command command)
        {
            return command.Comment;
        }

        public TransactionDTO.UpdateComment.Response ToUpdateCommentResponse(string? comment)
        {
            return new TransactionDTO.UpdateComment.Response
            {
                Comment = comment
            };
        }
    }
}
