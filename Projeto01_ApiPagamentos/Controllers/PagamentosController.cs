using Projeto01_ApiPagamentos.Dados;
using Projeto01_ApiPagamentos.Enumeracoes;
using Projeto01_ApiPagamentos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projeto01_ApiPagamentos.Controllers
{
    public class PagamentosController : ApiController
    {
        static readonly PagamentosDao dao = new PagamentosDao();

        #region HTTP GET - Lista todos os pagamentos

        public IEnumerable<Pagamento> GetPagamentos()
        {
            return dao.BuscarTodos();
        }
        #endregion

        #region HTTP GET - Retornar um pagamento
        public Pagamento GetPagamento(int id)
        {
            return dao.BuscarPagamento(id);
        }
        #endregion

        #region HTTP POST - Inclusão de pagamento
        public HttpResponseMessage PostPagamento(Pagamento pagamento)
        {
            ResultadoPagamento resultado = dao.IncluirPagamento(pagamento);

            if (resultado == ResultadoPagamento.PAGAMENTO_OK)
            {
                var response = Request.CreateResponse<Pagamento>(HttpStatusCode.Created, pagamento);
                string uri = Url.Link("DefaultApi", new { id = pagamento.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                string mensagem;

                switch (resultado)
                {
                    case ResultadoPagamento.PAGAMENTO_JA_REALIZADO:
                        mensagem = "Pagamento já realizado";
                        break;
                    case ResultadoPagamento.CARTAO_INVALIDO:
                        mensagem = "O cartão informado não existe";
                        break;
                    case ResultadoPagamento.LIMITE_INDISPONIVEL:
                        mensagem = "Limite indisponível no cartão";
                        break;
                    default:
                        mensagem = "Ocorreu um erro inesperado";
                        break;
                }

                var erro = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro no servidor"),
                    ReasonPhrase = mensagem
                };

                throw new HttpResponseException(erro);
            }
        }
        #endregion

        #region HTTP PUT - Alteração de pagamento
        public HttpResponseMessage PutPagamento(Pagamento Id)
        {
            ResultadoPagamento resultado = dao.AlterarPagamento(Id);

            if (resultado == ResultadoPagamento.PAGAMENTO_ALTERADO)
            {
                var response = Request.CreateResponse<Pagamento>(HttpStatusCode.Created, Id);
                string uri = Url.Link("DefaultApi", new { id = Id.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                string mensagem;

                switch (resultado)
                {
                    case ResultadoPagamento.PAGAMENTO_NAO_EXISTE:
                        mensagem = "Pagamento não existe";
                        break;
                    case ResultadoPagamento.FATURA_PAGA:
                        mensagem = "A fatura já foi paga";
                        break;
                    case ResultadoPagamento.CARTAO_INVALIDO:
                        mensagem = "O cartão informado não existe";
                        break;
                    case ResultadoPagamento.LIMITE_INDISPONIVEL:
                        mensagem = "Limite indisponível no cartão";
                        break;
                    default:
                        mensagem = "Ocorreu um erro inesperado";
                        break;
                }

                var erro = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro no servidor"),
                    ReasonPhrase = mensagem
                };

                throw new HttpResponseException(erro);
            }
        }
        #endregion

        #region HTTP DELETE - Remoção de pagamento
        public HttpResponseMessage DeletePagamento(Pagamento Id)
        {
            ResultadoPagamento resultado = dao.RemoverPagamento(Id);

            if (resultado == ResultadoPagamento.PAGAMENTO_REMOVIDO)
            {
                var response = Request.CreateResponse<Pagamento>(HttpStatusCode.Created, Id);
                string uri = Url.Link("DefaultApi", new { id = Id.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                string mensagem;

                switch (resultado)
                {
                    case ResultadoPagamento.PAGAMENTO_NAO_EXISTE:
                        mensagem = "Pagamento não existe";
                        break;
                    case ResultadoPagamento.FATURA_PAGA:
                        mensagem = "A fatura já foi paga";
                        break;
                    default:
                        mensagem = "Ocorreu um erro inesperado";
                        break;
                }

                var erro = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Erro no servidor"),
                    ReasonPhrase = mensagem
                };

                throw new HttpResponseException(erro);
            }
        }
        #endregion
    }
}
