using Budget.Server.Api.Transactions.Requests;
using Budget.Server.Api.Transactions.Responses;
using Budget.Server.Core.Helpers.Pagination;
using Budget.Server.Data.Transactions;
using Riok.Mapperly.Abstractions;

namespace Budget.Server.Core.Transactions
{
    [Mapper]
    public partial class TransactionMapper
    {
        public Pagination<GetTransactionResponse> ToGetListResponse(Pagination<TransactionQuery> transactionsDBOs) => new() { Page = ToGetListResponse(transactionsDBOs.Page), IsLastPage = transactionsDBOs.IsLastPage };

        public partial List<GetTransactionResponse> ToGetListResponse(List<TransactionQuery> transactionsDBOs);

        public partial GetTransactionResponse? ToGetResponse(TransactionQuery? transactionDBO);

        [MapperIgnoreTarget(nameof(Transaction.Id))]
        public partial Transaction ToTransaction(CreateTransactionRequest request);

        [MapperIgnoreTarget(nameof(Transaction.Id))]
        public partial Transaction ToTransaction(UpdateTransactionRequest request);
    }
}
