using Budget.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Dtos
{
    public class TransactionDTO
    {

        #region GetAll

        public class GetAll
        {
            public class Response
            {
                public required List<Get.Response> Transactions { get; set; }
            }
        }

        #endregion GetAll

        #region Get

        public class Get
        {
            public class Response
            {
                public required int Id { get; set; }
                public required TransactionType Type { get; set; }
                public required double Amount { get; set; }
                public required string Title { get; set; }
                public required DateOnly? Date { get; set; }
                public required PaymentMethod PaymentMethod { get; set; }
                public required string Comment { get; set; }
            }
        }

        #endregion Get

        #region Create

        public class Create
        {
            public class Request
            {
                [Required]
                public TransactionType Type { get; init; } = TransactionType.None;

                [Required]
                public double Amount { get; init; } = 0;

                [Required]
                public string Title { get; set; } = string.Empty;

                public DateOnly? Date { get; init; }

                [Required]
                public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

                [Required(AllowEmptyStrings = true)]
                public string Comment { get; init; } = string.Empty;
            }
        }

        #endregion Create

        #region Update

        public class Update
        {
            public class Request
            {
                [Required]
                public TransactionType Type { get; init; } = TransactionType.None;

                [Required]
                public double Amount { get; init; } = 0;

                [Required]
                public string Title { get; set; } = string.Empty;

                public DateOnly? Date { get; init; }

                [Required]
                public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

                [Required(AllowEmptyStrings = true)]
                public string Comment { get; init; } = string.Empty;
            }
        }

        #endregion Update
    }
}