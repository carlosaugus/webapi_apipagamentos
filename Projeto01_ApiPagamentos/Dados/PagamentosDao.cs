using Projeto01_ApiPagamentos.Enumeracoes;
using Projeto01_ApiPagamentos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projeto01_ApiPagamentos.Dados
{
    public class PagamentosDao
    {
        public IEnumerable<Pagamento> BuscarTodos()
        {
            using (var cn = new PagServiceEntities())
            {
                return cn.Pagamentos.ToList();
            }
        }

        public Pagamento BuscarPagamento(int Id)
        {
            using (var cn = new PagServiceEntities())
            {
                return cn.Pagamentos.FirstOrDefault(p => p.Id == Id);
            }
        }

        public ResultadoPagamento IncluirPagamento(Pagamento pagamento)
        {
            using (var cn = new PagServiceEntities())
            {
                try
                {
                    //Verifica se o cartão existe
                    var cartao = cn.Cartoes.FirstOrDefault(p => p.NumeroCartao == pagamento.NumeroCartao);
                    if (cartao == null)
                    {
                        return ResultadoPagamento.CARTAO_INVALIDO;
                    }

                    //Verifica se o pagamento já foi realizado
                    var pagto = cn.Pagamentos.Where(p => p.NumeroPedido == pagamento.NumeroPedido);
                    if (pagto.Count() > 0)
                    {
                        return ResultadoPagamento.PAGAMENTO_JA_REALIZADO;
                    }

                    //Verifica se há saldo disponível
                    var listaPagamentos = cn.Pagamentos.Where(l => l.NumeroCartao.Equals(pagamento.NumeroCartao) && l.Status == 1);
                    double total = 0;
                    if (listaPagamentos.Count() > 0)
                    {
                        total = listaPagamentos.Sum(t => t.Valor);
                    }

                    if ((total + pagamento.Valor) > cartao.Limite)
                    {
                        return ResultadoPagamento.LIMITE_INDISPONIVEL;
                    }

                    cn.Pagamentos.Add(pagamento);
                    cn.SaveChanges();
                    return ResultadoPagamento.PAGAMENTO_OK;
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }

        public ResultadoPagamento AlterarPagamento(Pagamento pagamento)
        {
            using (var cn = new PagServiceEntities())
            {
                try
                {
                    //Verifica se o pagamento existe
                    var pag = cn.Pagamentos.FirstOrDefault(p => p.Id == pagamento.Id);
                    if (pag == null)
                    {
                        return ResultadoPagamento.PAGAMENTO_NAO_EXISTE;
                    }

                    //Verifica se a fatura foi paga
                    if (pag.Status != 1)
                    {
                        return ResultadoPagamento.FATURA_PAGA;
                    }

                    //Verifica se o cartão é diferente
                    if (pag.NumeroCartao != pagamento.NumeroCartao)
                    {
                        //Verifica se o cartão existe
                        var cartao = cn.Cartoes.FirstOrDefault(p => p.NumeroCartao == pagamento.NumeroCartao);
                        if (cartao == null)
                        {
                            return ResultadoPagamento.CARTAO_INVALIDO;
                        }
                    }

                    var cartaoLimite = cn.Cartoes.FirstOrDefault(p => p.NumeroCartao == pagamento.NumeroCartao);

                    //Verifica se há saldo disponível no cartão
                    var listaPagamentos = cn.Pagamentos.Where(l => l.NumeroCartao.Equals(pagamento.NumeroCartao) && l.Status == 1 && !l.NumeroPedido.Equals(pagamento.NumeroPedido));
                    double total = 0;
                    if (listaPagamentos.Count() > 0)
                    {
                        total = listaPagamentos.Sum(t => t.Valor);
                    }

                    if ((total + pagamento.Valor) > cartaoLimite.Limite)
                    {
                        return ResultadoPagamento.LIMITE_INDISPONIVEL;
                    }

                    cn.Entry<Pagamento>(pagamento).State = EntityState.Modified;
                    cn.SaveChanges();
                    return ResultadoPagamento.PAGAMENTO_ALTERADO;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public ResultadoPagamento RemoverPagamento(Pagamento pagamento)
        {
            using (var cn = new PagServiceEntities())
            {
                try
                {
                    //Verifica se o pagamento existe
                    var pag = cn.Pagamentos.FirstOrDefault(p => p.Id == pagamento.Id);
                    if (pag == null)
                    {
                        return ResultadoPagamento.PAGAMENTO_NAO_EXISTE;
                    }

                    //Verifica se a fatura foi paga
                    if (pag.Status != 1)
                    {
                        return ResultadoPagamento.FATURA_PAGA;
                    }

                    cn.Entry<Pagamento>(pag).State = EntityState.Deleted;
                    cn.SaveChanges();
                    return ResultadoPagamento.PAGAMENTO_REMOVIDO;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}