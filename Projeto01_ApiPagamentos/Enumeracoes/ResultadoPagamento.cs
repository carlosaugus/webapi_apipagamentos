using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto01_ApiPagamentos.Enumeracoes
{
    public enum ResultadoPagamento
    {
        PAGAMENTO_OK, //Pagamento realizado com sucesso
        PAGAMENTO_JA_REALIZADO, //Pagamento já realizado
        CARTAO_INVALIDO, //Não existe o cartão informado
        LIMITE_INDISPONIVEL, //Saldo insuficiente no cartão
        FATURA_PAGA,
        PAGAMENTO_NAO_EXISTE,
        PAGAMENTO_ALTERADO,
        PAGAMENTO_REMOVIDO
    }
}