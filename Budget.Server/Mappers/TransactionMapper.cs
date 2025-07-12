using Budget.Server.Dbos;
using Budget.Server.Dtos;
using Budget.Server.Entities;
using Riok.Mapperly.Abstractions;

namespace Budget.Server.Mappers
{
    [Mapper]
    public partial class TransactionMapper
    {
        public TransactionDTO.GetAll.Response ToGetAllResponse(List<TransactionDBO> transactionsDBO) => new TransactionDTO.GetAll.Response { Transactions = ToGetResponseList(transactionsDBO) };

        private partial List<TransactionDTO.Get.Response> ToGetResponseList(List<TransactionDBO> transactionsDBO);

        public partial TransactionDTO.Get.Response? ToGetResponse(TransactionDBO? transactionDBO);

        [MapperIgnoreTarget(nameof(Transaction.Id))]
        public partial Transaction ToTransaction(TransactionDTO.Create.Request request);

        public TransactionDTO.Create.Response ToCreateResponse(Transaction transaction) => new TransactionDTO.Create.Response { Id = transaction.Id };

        [MapperIgnoreTarget(nameof(Transaction.Id))]
        public partial Transaction ToTransaction(TransactionDTO.Update.Request request);
    }
}
